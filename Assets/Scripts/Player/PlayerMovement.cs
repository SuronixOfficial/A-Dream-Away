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

    private Vector2 _moveDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    private void OnMove(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        _rigidbody.linearVelocity = _moveDirection * _speed * Time.fixedDeltaTime;
    }
    private void Update()
    {
        SetHeight();
        SetSprite();
    }

    private void SetHeight()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up;

        if (Physics.Raycast(origin, -transform.up, out hit, Mathf.Infinity, _groundLayer))
        {
            if (hit.collider == null) return;

            transform.position = new Vector3(transform.position.x,
                hit.point.y + _groundDistance,
                transform.position.z);
        }
    }
    private void SetSprite()
    {
        if (_moveDirection.x != 0 &&
            _moveDirection.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_moveDirection.x != 0 &&
            _moveDirection.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }
}
