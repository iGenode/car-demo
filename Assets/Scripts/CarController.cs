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

    private float _horizontalInput;
    private float _verticalInput;
    private float _currentSteerAngle;
    private float _currentbrakeForce;
    private bool _isBraking;

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



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CarController : MonoBehaviour
//{
//    [SerializeField]
//    private float _speed = 5.0f;
//    [SerializeField]
//    private float _turnSpeed = 10.0f;
//    [SerializeField]
//    private Transform _turningPoint;

//    private Rigidbody _rigidbody;
//    private float _moveVertical = 0.0f;
//    //private Vector3 _moveHorizontal = default;
//    private float _moveHorizontal = 0.0f;

//    private void Awake()
//    {
//        _rigidbody = GetComponent<Rigidbody>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        _moveVertical = Input.GetAxis("Vertical");
//        _moveHorizontal = Input.GetAxis("Horizontal");
//        //if (_moveHorizontal != 0.0f)
//        //{
//        //    transform.Rotate(0.0f, _moveHorizontal * _turnSpeed * Time.deltaTime, 0.0f);
//        //}
//    }

//    private void FixedUpdate()
//    {
//        if (_moveVertical != 0.0f)
//        {
//            _rigidbody.AddRelativeForce(0.0f, 0.0f, _moveVertical * _speed * Time.deltaTime, ForceMode.Force);
//        }
//        if (_rigidbody.velocity != default)
//        {
//            if (_moveHorizontal != 0.0f)
//            {
//                //Debug.Log($"Turning position is {_turningPoint.position}");
//                //Debug.Log($"Force applied is {_moveHorizontal * _turnSpeed * Time.fixedDeltaTime * transform.right}");
//                _rigidbody.AddForceAtPosition(_moveHorizontal * _turnSpeed * Time.fixedDeltaTime * transform.right, _turningPoint.position, ForceMode.Force);
//            }
//        }

//    }
//}
