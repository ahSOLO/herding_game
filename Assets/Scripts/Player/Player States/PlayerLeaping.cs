using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaping : IState
{
    PlayerController pC;
    public PlayerLeaping(PlayerController pC)
    {
        this.pC = pC;
    }

    public void FixedTick()
    {
        
    }

    public void LateTick()
    {
        
    }

    public void OnEnter()
    {
        pC.SetMovementProperties(pC.leapingRotSpeed, pC.leapingAccel, pC.leapingMaxSpd);
        pC.Leap();
        pC.hasLeaped = true;
    }

    public void OnExit()
    {
        pC.hasLeaped = false;
    }

    public void Tick()
    {
        pC.MoveWithInput();
    }
}
