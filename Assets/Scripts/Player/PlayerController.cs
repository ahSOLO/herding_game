using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(StateMachine), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private Rigidbody rb;
    private StateMachine sM;
    private Collider col;

    private PlayerIdle idleState;
    private PlayerWalking walkingState;
    private PlayerRunning runningState;
    private PlayerLeaping leapingState;
    private bool runButtonPressed;
    private bool wantsToMove;
    private bool wantsToLeap;
    private float rotationSpeed;
    private float acceleration;
    private float maxSpeed;

    [HideInInspector]
    public bool hasLeaped;
    public float walkingRotSpeed;
    public float walkingAccel;
    public float walkingMaxSpd;
    public float runningRotSpeed;
    public float runningAccel;
    public float runningMaxSpd;
    public float leapingRotSpeed;
    public float leapingAccel;
    public float leapingMaxSpd;
    public float leapSpdVert;
    public float leapMultHori;

    InputAction moveAction;
    InputAction runAction;
    InputAction leapAction;

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
        sM = GetComponent<StateMachine>();
        col = GetComponentInChildren<Collider>();
        idleState = new PlayerIdle(this);
        walkingState = new PlayerWalking(this);
        runningState = new PlayerRunning(this);
        leapingState = new PlayerLeaping(this);
        sM.SetInitialState(idleState);
    }

    private void Start()
    {
        moveAction = InputManager.Instance.GetInputAction("Move");
        moveAction.Enable();
        runAction = InputManager.Instance.GetInputAction("Run");
        runAction.Enable();
        leapAction = InputManager.Instance.GetInputAction("Leap");
        leapAction.Enable();

        sM.AddTransition(idleState, () => wantsToMove && !runButtonPressed, walkingState);
        sM.AddTransition(idleState, () => wantsToMove && runButtonPressed, runningState);
        sM.AddTransition(walkingState, () => !wantsToMove, idleState);
        sM.AddTransition(runningState, () => !wantsToMove, idleState);
        sM.AddTransition(walkingState, () => wantsToMove && runButtonPressed, runningState);
        sM.AddTransition(runningState, () => wantsToMove && !runButtonPressed, walkingState);
        sM.AddTransition(leapingState, IsGrounded, idleState);
        sM.AddTransition(idleState, () => wantsToLeap, leapingState);
        sM.AddTransition(walkingState, () => wantsToLeap, leapingState);
        sM.AddTransition(runningState, () => wantsToLeap, leapingState);
    }

    private void Update()
    {
        runButtonPressed = runAction.ReadValue<float>() == 1f;
        wantsToMove = moveAction.ReadValue<Vector2>() != Vector2.zero;
        wantsToLeap = leapAction.WasPressedThisFrame();
    }

    public void Leap()
    {
        rb.velocity = new Vector3(transform.forward.x * leapMultHori, transform.forward.y + leapSpdVert, transform.forward.z * leapMultHori);
    }

    private bool IsGrounded()
    {
        if (hasLeaped && rb.velocity.y <= 0)
        {
            if (Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f))
            {
                return true;
            }
        }

        return false;
    }

    public void SetMovementProperties(float rotSpeed, float accel, float maxSpeed)
    {
        rotationSpeed = rotSpeed;
        acceleration = accel;
        this.maxSpeed = maxSpeed;
    }

    public void MoveWithInput()
    {
        Move(moveAction.ReadValue<Vector2>());
    }

    private void Move(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            Vector3 dir3 = new Vector3(dir.x, 0, dir.y);
            SmoothRotateTowards(dir3);
            Accelerate(dir3, acceleration * dir.magnitude, maxSpeed);
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
