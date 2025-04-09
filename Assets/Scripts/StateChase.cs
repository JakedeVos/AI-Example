using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChase : State
{
    public StateChase(AIController ai) : base(ai)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Chase State");
    }

    public override void Update()
    {
        ai.ChasePlayer();
        if (!ai.CanSeePlayer())
        {
            ai.ChangeState(new StatePatrol(ai));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Chase State");
    }
}
