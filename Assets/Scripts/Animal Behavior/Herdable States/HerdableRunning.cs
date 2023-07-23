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
        
    }

    public void OnEnter()
    {
        herdable.navAgent.acceleration = herdable.runAccel;
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

            calcNewDestTimer = herdable.calcNewRunDestFrequency;
        }

        if (herdable.HerderWithinInnerDistance())
        {
            herdable.navAgent.speed = herdable.runSpeed2;
        }
        else
        {
            herdable.navAgent.speed = herdable.runSpeed1;
        }
    }
}
