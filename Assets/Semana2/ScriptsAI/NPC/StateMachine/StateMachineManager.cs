using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    BaseState currentState;
    public static WanderState wanderState = new WanderState();
    public static IdleState idleState = new IdleState();
    public static FormationState formationState = new FormationState();

    public BaseState CurrentState 
    {
        get { return currentState; }
        set { currentState = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(BaseState state) 
    {
        currentState = state;
        state.EnterState(this);
    }
    
}
