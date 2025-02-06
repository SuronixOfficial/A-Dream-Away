using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
[DefaultExecutionOrder(-20)]
public class InputReader : PersistentSingleton<InputReader>
{
    [SerializeField] private float _movementInputSmoothTime = 0.1f; 

    public static AvailableDevices CurrentDeviceType;

    public void SetDisablePlayerInput(bool status) => _disablePlayerInput = status;

    private PlayerInput _playerInput;

    private bool _disablePlayerInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _rawMovementInput;
    private Vector2 _smoothVelocity;

    protected override void Awake()
    {
        base.Awake();
        _playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _disablePlayerInput = false;
    }

    private void Start()
    {
        GetControlScheme();
    }
    private void GetControlScheme()
    {
        switch (CurrentDeviceType)
        {
            case AvailableDevices.Keyboard:

                InputDevice[] keyboardMouseDevices = new InputDevice[2];
                foreach (var device in InputSystem.devices)
                {
                    if (device is Keyboard) keyboardMouseDevices[0] = device;
                    if (device is Mouse) keyboardMouseDevices[1] = device;
                }

                _playerInput.SwitchCurrentControlScheme("Keyboard&Mouse", keyboardMouseDevices);
                break;

            case AvailableDevices.Gamepad:

                InputDevice gamePadDevice = null;
                foreach (var device in InputSystem.devices)
                {
                    if (device is Gamepad || device is Joystick) gamePadDevice = device;
                }
                _playerInput.SwitchCurrentControlScheme("Gamepad", gamePadDevice);
                break;
        }
        _playerInput.neverAutoSwitchControlSchemes = true;
    }
    public void SetGameplay()
    {
        _playerInput.SwitchCurrentActionMap("Gameplay");
    }
    public void SetUI()
    {
        _playerInput.SwitchCurrentActionMap("UI");
    }

    public event Action ResumeEvent;
    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
        }
    }

    public event Action PauseEvent;
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
        }
    }

    public event Action<Vector2> MoveEvent;
    public void OnMove(InputAction.CallbackContext context)
    {
        _rawMovementInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        CustomMovement();
    }

    private void CustomMovement()
    {
        _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, _rawMovementInput, ref _smoothVelocity, _movementInputSmoothTime * Time.deltaTime);
        MoveEvent?.Invoke(_disablePlayerInput ? Vector2.zero : _smoothedMovementInput);
    }
 
}
public enum AvailableDevices
{
    Keyboard,
    Gamepad,
}
public enum UsedDeviceType
{
    Current,
    Keyboard,
    Gamepad,
}