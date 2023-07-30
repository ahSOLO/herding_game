using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HostileFlee : IState
{
    private Hostile hostile;
    private float calcNewDestTimer;

    public HostileFlee(Hostile hostile)
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
        hostile.navAgent.acceleration = hostile.fleeAccel;
        hostile.shouldIdle = false;
        calcNewDestTimer = 0;
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        calcNewDestTimer -= Time.deltaTime;

        if (calcNewDestTimer <= 0)
        {
            Vector3 targetDest = hostile.transform.position + (hostile.transform.position - PlayerController.Instance.transform.position).normalized * hostile.fleeDistance;

            hostile.navAgent.SetDestination(targetDest);

            calcNewDestTimer = hostile.calcNewDestFrequency;
        }
    }
}
