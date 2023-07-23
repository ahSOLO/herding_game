using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdableWander : IState
{
    private Herdable herdable;

    public HerdableWander(Herdable herdable)
    {
        this.herdable = herdable;
    }

    public void FixedTick()
    {
        
    }

    public void LateTick()
    {
        
    }

    public void OnEnter()
    {
        herdable.navAgent.speed = herdable.wanderSpeed;
        herdable.navAgent.acceleration = herdable.wanderAccel;

        Vector2 randomPointInsideUC = Random.insideUnitCircle;
        Vector3 randomDestination = herdable.transform.position + new Vector3(randomPointInsideUC.x, 0, randomPointInsideUC.y) * herdable.wanderRange;
        herdable.navAgent.SetDestination(randomDestination);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if (!herdable.navAgent.pathPending && herdable.navAgent.remainingDistance <= herdable.navAgent.stoppingDistance && herdable.navAgent.velocity.sqrMagnitude == 0f)
        {
            herdable.shouldIdle = true;
        }
    }
}
