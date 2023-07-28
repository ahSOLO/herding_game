using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine))]
public class Herdable : MonoBehaviour
{
    [SerializeField] private float herderDistanceInnerThreshold;
    
    private StateMachine sm;
    private HerdableIdle idleState;
    private HerdableWander wanderState;
    private HerdableRunning runningState;
    private bool herderWithinDistance = false;
    public Dictionary<Herdable, Herdable> herdablesWithinDistance;
    [HideInInspector] public Vector3 currentDestination;
    [HideInInspector] public float height;

    public bool shouldIdle = true;
    public NavMeshAgent navAgent;

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
        sm.AddTransition(runningState, () => herderWithinDistance == false, idleState);

        sm.AddGlobalTransition(() => herderWithinDistance == true, runningState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            herderWithinDistance = true;
        }
        else if (other.CompareTag("Herdable"))
        {
            var herdable = other.transform.parent.GetComponent<Herdable>();
            if (!herdablesWithinDistance.ContainsKey(herdable))
            {
                herdablesWithinDistance.Add(herdable, herdable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            herderWithinDistance = false;
        }
        else if (other.CompareTag("Herdable"))
        {
            herdablesWithinDistance.Remove(other.transform.parent.GetComponent<Herdable>());
        }
    }

    public bool HerderWithinInnerDistance()
    {
        return (PlayerController.Instance.transform.position - transform.position).magnitude < herderDistanceInnerThreshold;
    }
}
