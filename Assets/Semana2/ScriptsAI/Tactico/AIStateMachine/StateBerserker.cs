using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBerserker : MonoBehaviour, IState
{
    public List<Action> action = new List<Action>();
    public List<Action> entryAction = new List<Action>();
    public List<Action> exitAction = new List<Action>();
    public List<ITransition> transitions = new List<ITransition>();

    void Start()
    {
        //Metemos todas las transiciones asociadas al gameObject
        transitions.Add(GetComponent<Tidle>());
        
        //Si tenemos path para patrulla
        //if (GetComponent<PathBasico>() != null)
        //transitions.Add(GetComponents<TPatrulla>());

        //Metemos Acciones
        //ACCIÓN ENTRADA fijar objetivo
        entryAction.Add(GetComponent<FijarObjetivoBerserker>());
        //ACCIÓN movimiento, atacar
        action.Add(GetComponent<Movimiento>());
        action.Add(GetComponent<Atacar>());
        //action.Add(GetComponent<cambiarObjetivo>());
        //ACCIÓN SALIDA soltar objetivo
        exitAction.Add(GetComponent<SoltarObjetivo>());
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
        if (GetComponent<AgentNPC>().getVida() == 0) return true;
        ComponenteIA ia = GetComponent<ComponenteIA>();
        AgentNPC target = GetComponent<Atacar>().target;
        //Si no hay enemigos cerca sale de estado agresivo
        return (target != null && (ia.enemigoHuido(target)) || ia.enemigoMuerto(target));
        
    }

}
