using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine), typeof(NavMeshAgent))]
public class Herdable : MonoBehaviour
{
    [SerializeField] private float herderDistanceInnerThreshold;
    
    private StateMachine sm;
    private HerdableIdle idleState;
    private HerdableWander wanderState;
    private HerdableRunning runningState;
    [HideInInspector] public bool herderWithinDistance = false;
    public Dictionary<Hostile, Hostile> hostilesWithinDistance;
    public Dictionary<Herdable, Herdable> herdablesWithinDistance;
    [HideInInspector] public Vector3 currentDestination;
    [HideInInspector] public float height;

    public bool shouldIdle = true;
    public NavMeshAgent navAgent;

    public int maxHP;
    public float idleDurationMin;
    public float idleDurationMax;
    public float wanderSpeed;
    public float wanderAccel;
    public float wanderRange;
    public float runDistance;
    public float runAccel1;
    public float runAccel2;
    public float runSpeed1;
    public float runSpeed2;
    public float calcNewRunDestFrequency;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        herdablesWithinDistance = new Dictionary<Herdable, Herdable>();
        hostilesWithinDistance = new Dictionary<Hostile, Hostile>();
        height = GetComponentInChildren<Collider>().bounds.extents.y;
        currentDestination = transform.position;
        sm = GetComponent<StateMachine>();
        idleState = new HerdableIdle(this);
        wanderState = new HerdableWander(this);
        runningState = new HerdableRunning(this);
        sm.SetInitialState(idleState);
    }

    private void Start()
    {
        sm.AddTransition(idleState, () => shouldIdle == false, wanderState);
        sm.AddTransition(wanderState, () => shouldIdle == true, idleState);
        sm.AddTransition(runningState, () => herderWithinDistance == false && hostilesWithinDistance.Count == 0, idleState);

        sm.AddGlobalTransition(() => herderWithinDistance == true, runningState);
        sm.AddGlobalTransition(() => hostilesWithinDistance.Count > 0, runningState);
    }

    public bool HerderWithinInnerDistance()
    {
        return (PlayerController.Instance.transform.position - transform.position).magnitude < herderDistanceInnerThreshold;
    }
}
