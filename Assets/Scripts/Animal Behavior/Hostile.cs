using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine), typeof(NavMeshAgent))]
public class Hostile : MonoBehaviour
{
    private StateMachine sm;
    private HostileIdle idleState;
    private HostileWander wanderState;
    private HostilePursue pursueState;
    private HostileAttack attackState;
    private HostileFlee fleeState;
    [HideInInspector] public bool herderWithinDistance = false;
    public Dictionary<Herdable, Herdable> herdablesWithinDistance;
    [HideInInspector] public float height;

    public bool shouldIdle = true;
    public NavMeshAgent navAgent;

    public float idleDurationMin;
    public float idleDurationMax;
    public float wanderSpeed;
    public float wanderAccel;
    public float wanderRange;
    public float fleeDistance;
    public float fleeAccel;
    public float fleeSpeed;
    public float pursueAccel;
    public float pursueSpeed;
    public float calcNewDestFrequency;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        herdablesWithinDistance = new Dictionary<Herdable, Herdable>();
        height = GetComponentInChildren<Collider>().bounds.extents.y;
        sm = GetComponent<StateMachine>();
        idleState = new HostileIdle(this);
        wanderState = new HostileWander(this);
        pursueState = new HostilePursue(this);
        attackState = new HostileAttack(this);
        fleeState = new HostileFlee(this);
        sm.SetInitialState(idleState);
    }

    private void Start()
    {
        sm.AddTransition(idleState, () => shouldIdle == false, wanderState);
        sm.AddTransition(wanderState, () => shouldIdle == true, idleState);
        sm.AddTransition(fleeState, () => herderWithinDistance == false, idleState);

        sm.AddTransition(wanderState, () => herdablesWithinDistance.Count > 0, pursueState);

        sm.AddGlobalTransition(() => herderWithinDistance == true, fleeState);
    }
}
