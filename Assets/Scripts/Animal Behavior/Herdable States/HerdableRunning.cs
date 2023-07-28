using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HerdableRunning : IState
{
    private Herdable herdable;
    private float calcNewDestTimer;
    private float calcNewFlockingDestTimer;
    
    public HerdableRunning(Herdable herdable)
    {
        this.herdable = herdable;
    }

    public void FixedTick()
    {
        
    }

    public void LateTick()
    {
        calcNewFlockingDestTimer -= Time.deltaTime;
        
        if (calcNewFlockingDestTimer <=0 && herdable.herdablesWithinDistance.Count > 0)
        {
            Vector3 flockingDest = herdable.herdablesWithinDistance.ElementAt(UnityEngine.Random.Range(0, herdable.herdablesWithinDistance.Count)).Value.currentDestination;
            herdable.currentDestination = (herdable.currentDestination * 0.6f) + (flockingDest * 0.4f);
            herdable.navAgent.SetDestination(herdable.currentDestination);

            calcNewFlockingDestTimer = herdable.calcNewRunDestFrequency;
        }
    }

    public void OnEnter()
    {
        herdable.navAgent.acceleration = herdable.runAccel1;
        herdable.shouldIdle = false;
        calcNewDestTimer = 0;
        calcNewFlockingDestTimer = 0;
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
