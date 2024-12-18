using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrol : MonoBehaviour, IState
{
    public List<Action> action = new List<Action>();
    public List<Action> entryAction = new List<Action>();
    public List<Action> exitAction = new List<Action>();
    public List<ITransition> transitions = new List<ITransition>();

    // Start is called before the first frame update
    void Start()
    {
        //Metemos todas las transiciones asociadas al gameObject
        transitions.Add(GetComponent<TAttack>());
        transitions.Add(GetComponent<TDefend>());
        transitions.Add(GetComponent<Tidle>());

        //Metemos Acciones
        //ACCI�N ENTRADA Nos movemos al primer punto de patrulla y activamos patrulla
        entryAction.Add(GetComponent<MStartPatrulla>());
        entryAction.Add(GetComponent<ActivarPatrulla>());

        //ACCI�N 
        
        //ACCI�N SALIDA Desactivar patrulla
        exitAction.Add(GetComponent<DesactivarPatrulla>());
    }

    //Devuelve la acci�n o lista de acciones a realizar mientras se esta en el estado
    public List<Action> getAction()
    {
        return action;
    }

    //Devuelve la acci�n o lista de acciones a realizar cuando se entra en el estado
    public List<Action> getEntryAction() { return entryAction; }    

    //Devuelve la acci�n o lista de acciones a realizar cuando se sale del estado
    public List<Action> getExitAction() { return exitAction; }

    //Devuelve las transiciones que se comprobar�n
    public List<ITransition> getTransitions() { return transitions; }

    public bool condicionIdle() 
    {
        if (GetComponent<ComponenteIA>().comprobarModoParaPatrulla()) return false;
        else return true; 
    }
}
