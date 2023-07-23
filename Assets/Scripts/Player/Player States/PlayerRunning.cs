using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunning : IState
{
    PlayerController pC;
    public PlayerRunning(PlayerController pC)
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
        pC.SetMovementProperties(pC.runningRotSpeed, pC.runningAccel, pC.runningMaxSpd);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        pC.MoveWithInput();
    }
}
