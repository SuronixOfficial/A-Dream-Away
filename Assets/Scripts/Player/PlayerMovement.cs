using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _groundDistance;

    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private CharacterController _controller;

    private Vector2 _moveInput;
    private Vector3 _velocity;
    private const float GRAVITY = 9.81f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        }
        else
        {
            InputReader.Instance.MoveEvent -= OnMove;
        }
    }

    private void OnMove(Vector2 moveInput)
    {
        _moveInput = moveInput;
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

        Vector3 moveDirection = new Vector3(_moveInput.x * _speed, _velocity.y, 0) * Time.deltaTime;
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
