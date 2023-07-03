using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _motorForce;
    [SerializeField]
    private float _brakeForce;
    [SerializeField]
    private float _maxSteerAngle;

    [Header("Wheel Colliders")]
    [SerializeField]
    private WheelCollider _frontLeftWheelCollider;
    [SerializeField]
    private WheelCollider _frontRightWheelCollider;
    [SerializeField]
    private WheelCollider _rearLeftWheelCollider;
    [SerializeField]
    private WheelCollider _rearRightWheelCollider;

    [Header("Wheels")]
    [SerializeField]
    private Transform _frontLeftWheelTransform;
    [SerializeField]
    private Transform _frontRightWheelTransform;
    [SerializeField]
    private Transform _rearLeftWheelTransform;
    [SerializeField]
    private Transform _rearRightWheelTransform;

    [Header("UI Controls")]
    [SerializeField]
    private HoldableButton _brakeButton;
    [SerializeField]
    private Button _uprightButton;

    [Header("Trail Renderer")]
    [SerializeField]
    private TrailRenderer[] _trailRenderers;

    private float _horizontalInput;
    private float _verticalInput;
    private float _currentSteerAngle;
    private float _currentbrakeForce;
    private bool _isBraking;
    private bool _isEmitting = false;

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        _frontLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
        _frontRightWheelCollider.motorTorque = _verticalInput * _motorForce;
        _currentbrakeForce = _isBraking ? _brakeForce : 0f;
        ApplyBreaking();
        HandleTrail();
    }

    private void ApplyBreaking()
    {
        _frontRightWheelCollider.brakeTorque = _currentbrakeForce;
        _frontLeftWheelCollider.brakeTorque = _currentbrakeForce;
        _rearLeftWheelCollider.brakeTorque = _currentbrakeForce;
        _rearRightWheelCollider.brakeTorque = _currentbrakeForce;
    }

    private void HandleSteering()
    {
        _currentSteerAngle = _maxSteerAngle * _horizontalInput;
        _frontLeftWheelCollider.steerAngle = _currentSteerAngle;
        _frontRightWheelCollider.steerAngle = _currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(_frontLeftWheelCollider, _frontLeftWheelTransform);
        UpdateSingleWheel(_frontRightWheelCollider, _frontRightWheelTransform);
        UpdateSingleWheel(_rearRightWheelCollider, _rearRightWheelTransform);
        UpdateSingleWheel(_rearLeftWheelCollider, _rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void HandleTrail()
    {
        // TODO: check if wheel is grounded before emitting
        // TODO: rotate the trail objects with wheels around Y axis to 
        // TODO: calculate proper forces instead of just drawing when breaking
        if (_currentbrakeForce != 0f && !_isEmitting)
        {
            foreach (TrailRenderer trailRenderer in _trailRenderers)
            {
                trailRenderer.emitting = true;
            }
            _isEmitting = true;
        } 
        else if (_currentbrakeForce == 0f && _isEmitting)
        {
            foreach (TrailRenderer trailRenderer in _trailRenderers)
            {
                trailRenderer.emitting = false;
            }
            _isEmitting = false;
        }
    }

    private void UprightCar()
    {
        var angles = transform.rotation.eulerAngles;
        angles.x = 0;
        angles.z = 0;
        transform.rotation = Quaternion.Euler(angles);
    }

    private void SetMovementInput(Vector2 input)
    {
        _horizontalInput = input.x;
        _verticalInput = input.y;
    }

    private void SetBrakingInput(bool input)
    {
        _isBraking = input;
    }

    private void OnEnable()
    {
        JoystickControls.OnMove += SetMovementInput;
        _brakeButton.OnClick += SetBrakingInput;
        _uprightButton.onClick.AddListener(UprightCar);
    }

    private void OnDisable()
    {
        JoystickControls.OnMove -= SetMovementInput;
        _brakeButton.OnClick -= SetBrakingInput;
        _uprightButton.onClick.RemoveListener(UprightCar);
    }
}
