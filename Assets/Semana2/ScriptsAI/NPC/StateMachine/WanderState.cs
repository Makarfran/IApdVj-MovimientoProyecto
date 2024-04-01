using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : BaseState
{
    float time = 0f;
    public override void EnterState(StateMachineManager stateMachine) 
    {
        time = 0;
        
    }

    public override void UpdateState(StateMachineManager stateMachine) 
    {
        if ( time > 10) 
        {
            ExitState(stateMachine);
        }
        time += Time.deltaTime;
    }

    public override void ExitState(StateMachineManager stateMachine) 
    {
        if (GameObject.Find("FormationManager") != null && GameObject.Find("FormationManager").GetComponent<FormationManager>().slotAssignments[0].Npc == stateMachine.gameObject)
        {
            stateMachine.SwitchState(StateMachineManager.formationState);
            GameObject.Find("FormationManager").GetComponent<FormationManager>().UpdateSlots();
        }
        else { stateMachine.SwitchState(StateMachineManager.idleState); }
    }

    public override void OnCollisionEnter(StateMachineManager stateMachine) { }
}
