using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFlee : MonoBehaviour, IState
{
    public List<Action> action = new List<Action>();
    public List<Action> entryAction = new List<Action>();
    public List<Action> exitAction = new List<Action>();
    public List<ITransition> transitions = new List<ITransition>();

    // Start is called before the first frame update
    void Start()
    {
        //Metemos todas las transiciones asociadas al gameObject
        transitions.Add(GetComponent<Tidle>());
        //transitions.Add(GetComponent<THeal>());

        //Metemos Acciones
        //ACCI�N ENTRADA fijar objetivo y huir
        entryAction.Add(GetComponent<FijarObjetivoHeal>());
        //if (!Elite())
        //entryAction.Add(GetComponent<Flee>());
        //ACCI�N movimiento
        action.Add(GetComponent<Movimiento>());
        
        //ACCI�N SALIDA soltar objetivo
        exitAction.Add(GetComponent<SoltarObjetivoBase>());
    }

    //Devuelve la acci�n o lista de acciones a realizar mientras se esta en el estado
    public List<Action> getAction()
    {
        return action;
    }

    //Devuelve la acci�n o lista de acciones a realizar cuando se entra en el estado
    public List<Action> getEntryAction() { 
        GetComponent<AgentNPC>().changeColorFlee();
        return entryAction; 
        }

    //Devuelve la acci�n o lista de acciones a realizar cuando se sale del estado
    public List<Action> getExitAction() { return exitAction; }

    //Devuelve las transiciones que se comprobar�n
    public List<ITransition> getTransitions() { return transitions; }

    public bool condicionIdle() 
    {
        if (GetComponent<AgentNPC>().getVida() == 0) return true;
        ComponenteIA ia = GetComponent<ComponenteIA>();
        // Transicion a heal
        return ia.fullVida() || (ia.distanciaHeal() && ia.enemigoAgresivo());
    }
}
