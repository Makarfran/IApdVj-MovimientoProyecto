using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : MonoBehaviour, IState
{
    public List<Action> action = new List<Action>();
    public List<Action> entryAction = new List<Action>();
    public List<Action> exitAction = new List<Action>();
    public List<ITransition> transitions = new List<ITransition>();

    // Start is called before the first frame update
    void Start()
    {
        //Metemos todas las transiciones asociadas al gameObject
        transitions.Add(GetComponent<TPatrol>());
        transitions.Add(GetComponent<TAttack>());
        transitions.Add(GetComponent<TDefend>());
        transitions.Add(GetComponent<TCapture>());
        
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

    public bool condicionIdle() { return false; }
}
