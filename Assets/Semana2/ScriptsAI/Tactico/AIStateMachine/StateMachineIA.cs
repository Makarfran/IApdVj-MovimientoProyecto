using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineIA : MonoBehaviour
{
    //Guarda lista de estados para la máquina de estados
    List <IState> states = new List<IState>();

    // Guarda estado inicial
    StateIdle initialState = null;

    //Guarda el estado actual
    public IState CurrentState = null;

    // Start is called before the first frame update
    void Start()
    {
        initialState = this.GetComponent<StateIdle>();
        CurrentState = initialState;
        
        //Metemos todos los estados asociados al gameObject
        states.Add(GetComponent<StateIdle>());
        states.Add(GetComponent<StateAttack>());
        states.Add(GetComponent<StateDefend>());
        states.Add(GetComponent<StateCapture>());
        
    }

    // Update is called once per frame
    //Comprobamos y aplicamos transiciones, devolviendo una lista de acciones
    void Update()
    {
        //Asumimos que ninguna transición ha sido activada
        ITransition triggeredTransition = null;

        //Comprobamos las transiciones y guardamos la primera que se activa

        foreach (ITransition transition in CurrentState.getTransitions())
        {
            if (transition.isTriggered())
            {
                triggeredTransition = transition;
                break;
            }
        }

        //Comprobamos si tenemos una transición para lanzar
        if (triggeredTransition != null) 
        {
            //Encontrar el estado objetivo
            IState targetState = triggeredTransition.getTargetState();

            //Añade la acción de salida del estado anterior, la acción de transición y la acción de entrada del nuevo estado
            List <Action> actions = new List<Action>();
            actions.AddRange(CurrentState.getExitAction());
            actions.AddRange(triggeredTransition.getAction());
            actions.AddRange(targetState.getEntryAction());

            //Completamos la transición y devolvemos la lista de acciones
            CurrentState = targetState;
            Debug.Log(CurrentState);
            //TO-DO hay que cambiar los return probablemente por scheduleAction() del action manager
            GetComponent<ActionManager>().scheduleAction(actions);
            //return actions;
        }
        else { GetComponent<ActionManager>().scheduleAction(CurrentState.getAction()); }

    }

    
}
