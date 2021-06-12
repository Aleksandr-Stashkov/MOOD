using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _charaterController;
    private Transform _camera;

    private float _speed = 3.5f;
    private float _mouseSensitivity = 200f;
    private float _verticalVelocity;

    void Start()
    {
        _charaterController = GetComponent<CharacterController>();
        Transform child;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            if (child == null)
            {
                Debug.LogError("Player could not locate its child.");
            }
            else
            {
                if (child.CompareTag("MainCamera"))
                {
                    _camera = child;
                }
            }
        }

        if (_camera == null)
        {
            Debug.LogError("Player could not locate Main Camera.");
        }
        if (_charaterController == null)
        {
            Debug.LogError("Player could not locate the Character Controller component.");
        }
    }
        
    void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 motionVector = new Vector3(horizontalInput, 0, verticalInput);
        motionVector *= _speed * Time.deltaTime;

        if (_charaterController.isGrounded && _verticalVelocity != 0)
        {
            _verticalVelocity = 0;
        }
        motionVector.y = _verticalVelocity * Time.deltaTime;

        _charaterController.Move(transform.TransformDirection(motionVector));
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        transform.rotation *= Quaternion.Euler(0, mouseX * Time.deltaTime, 0);

        float initialAngleY = _camera.rotation.eulerAngles.x;
        float angleY = -Mathf.Clamp(Mathf.DeltaAngle(initialAngleY - mouseY * Time.deltaTime, 0), -68f, 60f) + Mathf.DeltaAngle(initialAngleY, 0);
        _camera.rotation *= Quaternion.Euler(angleY, 0, 0);
    }
}
