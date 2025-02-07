using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

public class ViewBobbing : MonoBehaviour
{
    [SerializeField] private CinemachineFollow _cameraFollow;
    [SerializeField] private CharacterController _controller;

    [SerializeField] private float _walkBobbingAmount = 0.1f;
    [SerializeField] private float _walkBobbingSpeed = 6f;
    [SerializeField] private float _idleBobbingAmount = 0.02f;
    [SerializeField] private float _idleBobbingSpeed = 1.5f;
    [SerializeField] private float _smoothTransitionSpeed = 5f;

    private Vector3 originalOffset;
    private Vector3 targetOffset;

    void Start()
    {
        originalOffset = _cameraFollow.FollowOffset;
        targetOffset = originalOffset;
    }

    void Update()
    {
        bool isMoving = _controller != null && _controller.velocity.magnitude > 0.1f;

        if (isMoving)
        {
            float bobbingY = Mathf.Sin(Time.time * _walkBobbingSpeed) * _walkBobbingAmount;
            float bobbingX = Mathf.Cos(Time.time * _walkBobbingSpeed * 0.75f) * (_walkBobbingAmount * 0.5f); 

            targetOffset = originalOffset + new Vector3(bobbingX, bobbingY, 0);
        }
        else
        {
            float breathingY = Mathf.Sin(Time.time * _idleBobbingSpeed) * _idleBobbingAmount;
            float breathingX = Mathf.Cos(Time.time * _idleBobbingSpeed * 0.75f) * (_idleBobbingAmount * 0.5f);

            targetOffset = originalOffset + new Vector3(breathingX, breathingY, 0);
        }

        _cameraFollow.FollowOffset = Vector3.Lerp(_cameraFollow.FollowOffset, targetOffset, Time.deltaTime * _smoothTransitionSpeed);
    }
}
