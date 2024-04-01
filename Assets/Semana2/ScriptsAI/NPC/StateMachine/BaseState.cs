using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState 
{
    public abstract void EnterState(StateMachineManager stateMachine);

    public abstract void UpdateState(StateMachineManager stateMachine);

    public abstract void ExitState(StateMachineManager stateMachine);

    public abstract void OnCollisionEnter(StateMachineManager stateMachine);
}
