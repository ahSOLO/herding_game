using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileIdle : IState
{
    private Hostile hostile;
    private float idleTime;

    public HostileIdle(Hostile hostile)
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
        idleTime = Random.Range(hostile.idleDurationMin, hostile.idleDurationMax);
        hostile.shouldIdle = true;
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        idleTime -= Time.deltaTime;

        if (idleTime <= 0)
        {
            hostile.shouldIdle = false;
        }
    }
}
