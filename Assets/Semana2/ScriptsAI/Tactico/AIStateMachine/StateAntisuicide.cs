using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAntisuicide : MonoBehaviour, IState 
{
    List<Action> action;
    List<Action> entryAction;
    List<Action> exitAction;
    List<ITransition> transitions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Devuelve la acción o lista de acciones a realizar mientras se esta en el estado
    public List<Action> getAction() { return action; }

    //Devuelve la acción o lista de acciones a realizar cuando se entra en el estado
    public List<Action> getEntryAction() { return entryAction; }

    //Devuelve la acción o lista de acciones a realizar cuando se sale del estado
    public List<Action> getExitAction() { return exitAction; }

    //Devuelve las transiciones que se comprobarán
    public List<ITransition> getTransitions() 
    {
        return transitions; 
    }


    public void fijarObjetivo() { }

    public void soltarObjetivo() { }

    public bool condicionIdle() { return false; }
}
