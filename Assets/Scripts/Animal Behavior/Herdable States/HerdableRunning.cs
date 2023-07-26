using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdableRunning : IState
{
    private Herdable herdable;
    private float calcNewDestTimer;
    
    public HerdableRunning(Herdable herdable)
    {
        this.herdable = herdable;
    }

    public void FixedTick()
    {
        
    }

    public void LateTick()
    {
        if (herdable.herdablesWithinDistance.Count > 2)
        {
            Vector3 flockingDest = herdable.currentDestination;
            foreach (var herdable_ in herdable.herdablesWithinDistance)
            {
                flockingDest += herdable_.currentDestination;
            }
            flockingDest /= (herdable.herdablesWithinDistance.Count);
            herdable.currentDestination = (herdable.currentDestination * 2 / 3) + (flockingDest / 3);
            herdable.navAgent.SetDestination(herdable.currentDestination);
        }
    }

    public void OnEnter()
    {
        herdable.navAgent.acceleration = herdable.runAccel1;
        herdable.shouldIdle = false;
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
            Vector3 targetDest = herdable.transform.position + (herdable.transform.position - PlayerController.Instance.transform.position).normalized * herdable.runDistance;

            herdable.navAgent.SetDestination(targetDest);
            herdable.currentDestination = targetDest;

            calcNewDestTimer = herdable.calcNewRunDestFrequency;
        }

        if (herdable.HerderWithinInnerDistance())
        {
            herdable.navAgent.speed = herdable.runSpeed2;
            herdable.navAgent.acceleration = herdable.runAccel2;
        }
        else
        {
            herdable.navAgent.speed = herdable.runSpeed1;
            herdable.navAgent.acceleration = herdable.runAccel1;
        }
    }
}
