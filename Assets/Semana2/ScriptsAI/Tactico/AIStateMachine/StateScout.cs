using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScout : MonoBehaviour, IState
{
    public List<Action> action = new List<Action>();
    public List<Action> entryAction = new List<Action>();
    public List<Action> exitAction = new List<Action>();
    public List<ITransition> transitions = new List<ITransition>();

    void Start()
    {
        //Metemos todas las transiciones asociadas al gameObject
        transitions.Add(GetComponent<Tidle>());
        transitions.Add(GetComponent<TAttack>());
        transitions.Add(GetComponent<TCapture>());
        transitions.Add(GetComponent<TDefend>());

        //Metemos Acciones
        //ACCIÓN ENTRADA fijar objetivo
        entryAction.Add(GetComponent<ObjetivoExploracion>());
        //ACCIÓN movimiento, atacar
        action.Add(GetComponent<Exploracion>());
        //action.Add(GetComponent<cambiarObjetivo>());
        //ACCIÓN SALIDA soltar objetivo
        exitAction.Add(GetComponent<SoltarObjetivoExploracion>());
    }

    //Devuelve la acción o lista de acciones a realizar mientras se esta en el estado
    public List<Action> getAction()
    {
        return action;
    }

    //Devuelve la acción o lista de acciones a realizar cuando se entra en el estado
    public List<Action> getEntryAction() { return entryAction; }

    //Devuelve la acción o lista de acciones a realizar cuando se sale del estado
    public List<Action> getExitAction() { return exitAction; }

    //Devuelve las transiciones que se comprobarán
    public List<ITransition> getTransitions() { return transitions; }


    public bool condicionIdle()
    {
        AgentNPC npcActual = GetComponent<AgentNPC>();
        if (GetComponent<Exploracion>().getTarget() != null && (GetComponent<Exploracion>().getTarget().getPosition() - npcActual.Position).magnitude <= 2)
        {
            return true;
        }
        return false;
    }
}
