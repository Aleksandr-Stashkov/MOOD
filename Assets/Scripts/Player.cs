using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _charaterController;
    private Transform _cameraPivot;
    private float _speed = 3.5f;
    private float _gravity = 9.8f;
    private float _mouseSensitivity = 200f;
    private float _verticalVelocity;

    private void Start()
    {
        _charaterController = GetComponent<CharacterController>();
        if (_charaterController == null)
        {
            Debug.LogError("Player could not locate the Character Controller component.");
        }

        _cameraPivot = transform.GetChild(0);
        if (_cameraPivot == null)
        {
            Debug.LogError("Player could not locate its child.");
        }        
    }

    private void Update()
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
        else
        {
            _verticalVelocity -= _gravity;
        }
        motionVector.y = _verticalVelocity * Time.deltaTime;

        _charaterController.Move(transform.TransformDirection(motionVector));
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.rotation *= Quaternion.AngleAxis(mouseX * _mouseSensitivity * Time.deltaTime, transform.up);
        _cameraPivot.transform.rotation *= Quaternion.AngleAxis(mouseY * _mouseSensitivity/2f * Time.deltaTime, _cameraPivot.transform.right);
    }
}
