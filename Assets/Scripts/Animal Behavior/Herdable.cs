using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine))]
public class Herdable : MonoBehaviour
{
    [SerializeField] private float herderDistanceInnerThreshold;
    [SerializeField] private float herderDistanceOuterThreshold;
    [SerializeField] private bool debugState;
    [SerializeField] string currentStateLog;
    
    private StateMachine sm;
    private HerdableIdle idleState;
    private HerdableWander wanderState;
    private HerdableRunning runningState;

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
        sm.AddTransition(runningState, () => !HerderWithinOuterDistance(), idleState);

        sm.AddGlobalTransition(HerderWithinOuterDistance, runningState);
    }

    private bool HerderWithinOuterDistance()
    {
        return (PlayerController.Instance.transform.position - transform.position).magnitude < herderDistanceOuterThreshold;
    }

    public bool HerderWithinInnerDistance()
    {
        return (PlayerController.Instance.transform.position - transform.position).magnitude < herderDistanceInnerThreshold;
    }

    private void Update()
    {
        if (debugState)
        {
            currentStateLog = sm.state.ToString();
        }
    }
}
