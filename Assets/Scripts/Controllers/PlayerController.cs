using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    private Rigidbody rb;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float maxSpeed;

    InputAction moveAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            enabled = false;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        moveAction = InputManager.Instance.GetInputAction("Move");
        moveAction.Enable();
    }

    private void Update()
    {
        Move(moveAction.ReadValue<Vector2>());
    }

    private void Move(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            Vector3 dir3 = new Vector3(dir.x, 0, dir.y);
            SmoothRotateTowards(dir3);
            Accelerate(dir3, accelerationSpeed * dir.magnitude, maxSpeed);
        }
    }

    private void Accelerate(Vector3 dir, float acceleration, float maxSpeed)
    {
        rb.velocity += dir * acceleration * Time.deltaTime;

        float mag = rb.velocity.magnitude;
        if (mag > maxSpeed)
        {
            rb.velocity *= maxSpeed / mag ;
        }
    }

    private void SmoothRotateTowards(Vector3 dir)
    {
        Quaternion targetRot = Quaternion.LookRotation(dir);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.deltaTime));
    }
}
