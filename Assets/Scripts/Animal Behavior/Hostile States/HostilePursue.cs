using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class HostilePursue : IState
{
    private Hostile hostile;
    private float calcNewDestTimer;

    public HostilePursue(Hostile hostile)
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
        calcNewDestTimer = 0;
        hostile.navAgent.acceleration = hostile.pursueAccel;
        hostile.navAgent.speed = hostile.pursueSpeed;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if (calcNewDestTimer <= 0)
        {
            var targetHerdable = GetClosestHerdable(hostile.herdablesWithinDistance);
            if (targetHerdable != null)
            {
                hostile.navAgent.SetDestination(targetHerdable.transform.position);
            }
        }
    }

    private Herdable GetClosestHerdable(Dictionary<Herdable, Herdable> herdablesWithinDistance)
    {
        var closestDistance = Mathf.Infinity;
        Herdable closestHerdable = null;

        foreach (Herdable herdable in herdablesWithinDistance.Keys)
        {
            if ((herdable.transform.position - hostile.transform.position).sqrMagnitude < closestDistance)
            {
                closestHerdable = herdable;
            }
        }

        return closestHerdable;
    }
}
