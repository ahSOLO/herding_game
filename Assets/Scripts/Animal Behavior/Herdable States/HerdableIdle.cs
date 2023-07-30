using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdableIdle : IState
{
    private Herdable herdable;
    private float idleTime;

    public HerdableIdle(Herdable herdable)
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
        idleTime = Random.Range(herdable.idleDurationMin, herdable.idleDurationMax);
        herdable.shouldIdle = true;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        idleTime -= Time.deltaTime;

        if (idleTime <= 0)
        {
            herdable.shouldIdle = false;
        }
    }
}
