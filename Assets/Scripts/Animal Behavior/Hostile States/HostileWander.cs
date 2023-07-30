using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileWander : IState
{
    private Hostile hostile;

    public HostileWander(Hostile hostile)
    {
        this.hostile = hostile;
    }

    public void FixedTick()
    {

    }

    public void LateTick()
    {

    }

    public void OnEnter()
    {
        hostile.navAgent.speed = hostile.wanderSpeed;
        hostile.navAgent.acceleration = hostile.wanderAccel;

        Vector2 randomPointInsideUC = Random.insideUnitCircle;
        Vector3 randomDestination = PlayerController.Instance.transform.position + new Vector3(randomPointInsideUC.x, 0, randomPointInsideUC.y) * hostile.wanderRange;
        hostile.navAgent.SetDestination(randomDestination);
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        if (!hostile.navAgent.pathPending && hostile.navAgent.remainingDistance <= hostile.navAgent.stoppingDistance && hostile.navAgent.velocity.sqrMagnitude == 0f)
        {
            hostile.shouldIdle = true;
        }
    }
}
