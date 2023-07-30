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
        Vector3 randomDestination = hostile.transform.position + new Vector3(randomPointInsideUC.x, 0, randomPointInsideUC.y) * hostile.wanderRange;
        randomDestination.y = hostile.transform.position.y - hostile.height;
        Ray lineOfSightRay = new Ray(hostile.transform.position, randomDestination - hostile.transform.position);
        if (Physics.Raycast(lineOfSightRay, out RaycastHit hitInfo, hostile.wanderRange + 1f))
        {
            hostile.navAgent.SetDestination(hitInfo.point);
        }
        else
        {
            hostile.shouldIdle = true;
        }
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
