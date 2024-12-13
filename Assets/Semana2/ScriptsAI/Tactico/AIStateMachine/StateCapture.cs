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
        transitions.Add(GetComponent<Tidle>());
        transitions.Add(GetComponent<TEvitarDerrota>());
        

        //Metemos Acciones
        //ACCI�N ENTRADA fijar objetivo
        entryAction.Add(GetComponent<FijarObjetivoBase>());
        //ACCI�N movimiento, capturar
        action.Add(GetComponent<Movimiento>());
        //action.Add(GetComponent<Capture>());
        //ACCI�N SALIDA soltar objetivo
        exitAction.Add(GetComponent<SoltarObjetivoBase>());
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
    public List<Action> getEntryAction() { 
        GetComponent<AgentNPC>().changeColorCapture();
        return entryAction; 
        }

    //Devuelve la acci�n o lista de acciones a realizar cuando se sale del estado
    public List<Action> getExitAction() { return exitAction; }

    //Devuelve las transiciones que se comprobar�n
    public List<ITransition> getTransitions() { return transitions; }

    public bool condicionIdle() 
    {
        if (GetComponent<Movimiento>().getTarget() != null) 
        {
            KeypointBase objetivo = GetComponent<Movimiento>().getTarget().GetComponent<KeypointBase>();
            if (objetivo != null && objetivo.getBando() == GetComponent<AgentNPC>().getBando())
            {
                Debug.Log("de captura a idle");
                return true;
            }
        }
        return false;
    }
}
