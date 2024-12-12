using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDefend : MonoBehaviour, IState
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


        //Metemos Acciones
        //ACCIÓN ENTRADA fijar objetivo
        entryAction.Add(GetComponent<FijarObjetivoDefend>());
        //ACCIÓN movimiento, capturar
        action.Add(GetComponent<Movimiento>());
        //action.Add(GetComponent<Capture>());
        //ACCIÓN SALIDA soltar objetivo
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

    //Devuelve la acción o lista de acciones a realizar cuando se entra en el estado
    public List<Action> getEntryAction() { return entryAction; }

    //Devuelve la acción o lista de acciones a realizar cuando se sale del estado
    public List<Action> getExitAction() { return exitAction; }

    //Devuelve las transiciones que se comprobarán
    public List<ITransition> getTransitions() { return transitions; }

    public bool condicionIdle()
    {
        if (GetComponent<Movimiento>().getTarget() != null)
        {
            KeypointBase objetivo = GetComponent<Movimiento>().getTarget().GetComponent<KeypointBase>();
            if (objetivo != null && (objetivo.getBando() != GetComponent<AgentNPC>().getBando() || objetivo.getLifeP() == objetivo.getLifePMax()))
            {
                Debug.Log("de defensa a idle");
                return true;
            }
        }
        return false;
    }
}
//||
//                !GetComponent<ComponenteIA>().fijarObjetivoDefensa(out GameObject target)