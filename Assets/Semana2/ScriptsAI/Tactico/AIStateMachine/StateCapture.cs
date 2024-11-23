using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCapture : MonoBehaviour, IState
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
        //transitions.Add(GetComponent<THuir>());

        //Metemos Acciones
        //ACCI�N ENTRADA fijar objetivo
        entryAction.Add(GetComponent<FijarObjetivo>());
        //ACCI�N movimiento, capturar
        action.Add(GetComponent<Movimiento>());
        //action.Add(GetComponent<AtacarBase>());
        //ACCI�N SALIDA soltar objetivo
        exitAction.Add(GetComponent<SoltarObjetivo>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void fijarObjetivo() { }

    public void soltarObjetivo() { }

    public bool condicionIdle() { return false; }
}
