using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 3;
    [SerializeField] private float _sprintSpeed = 5;

    private SpriteRenderer _spriteRenderer;
    private CharacterController _controller;

    private Vector2 _moveInput;
    private Vector3 _velocity;
    private float _currentSpeed;
    private bool _isSprinting;

    private const float GRAVITY = 9.81f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        TriggerActions(true);
    }

    private void OnDestroy()
    {
        TriggerActions(false);
    }

    private void TriggerActions(bool status)
    {
        if (status)
        {
            InputReader.Instance.MoveEvent += OnMove;
            InputReader.Instance.SprintEvent += OnSprint;
            InputReader.Instance.SprintCancelledEvent += OnCancelSprint;
        }
        else
        {
            InputReader.Instance.MoveEvent -= OnMove;
            InputReader.Instance.SprintEvent -= OnSprint;
            InputReader.Instance.SprintCancelledEvent -= OnCancelSprint;
        }
    }

    private void OnMove(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    private void OnSprint()
    {
        _isSprinting = true;
    }

    private void OnCancelSprint()
    {
        _isSprinting = false;
    }

    private void Update()
    {
        Move();
        SetSprite();
    }

    private void Move()
    {
        if (!_controller.isGrounded)
        {
            _velocity.y = -GRAVITY;
        }
        else
        {
            _velocity.y = -0.1f;
        }

        _currentSpeed = _isSprinting ? _sprintSpeed : _walkSpeed;
        Vector3 moveDirection = new Vector3(_moveInput.x * _currentSpeed, _velocity.y, _moveInput.y * _currentSpeed) * Time.deltaTime;
        _controller.Move(moveDirection);
    }
 
    private void SetSprite()
    {
        if (_moveInput.x != 0 &&
            _moveInput.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_moveInput.x != 0 &&
            _moveInput.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }
}
