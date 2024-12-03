using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : MonoBehaviour, IState
{
    public List<Action> action = new List<Action>();
    public List<Action> entryAction = new List<Action>();
    public List<Action> exitAction = new List<Action>();
    public List<ITransition> transitions = new List<ITransition>();

    void Start()
    {
        //Metemos todas las transiciones asociadas al gameObject
        transitions.Add(GetComponent<Tidle>());
        //transitions.Add(GetComponents<TAntiSuicida>());
        //Si tenemos path para patrulla
        //if (GetComponent<PathBasico>() != null)
        //transitions.Add(GetComponents<TPatrulla>());

        //Metemos Acciones
        //ACCIÓN ENTRADA fijar objetivo
        entryAction.Add(GetComponent<FijarObjetivo>());
        //ACCIÓN movimiento, atacar
        action.Add(GetComponent<Movimiento>());
        action.Add(GetComponent<Atacar>());
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

    /* metodo cambiado a componenteIA
    public void fijarObjetivo() 
    {
     
        GameObject objetivo = null;
        Controlador controlador = GameObject.Find("ControladorJuego").GetComponent<Controlador>();
        //Si no funciona probar getBando
        if (transform.parent.name == "A") 
        {
            objetivo = getObjetivo(controlador.teamB);
        }
        if (transform.parent.name == "B") 
        {
            objetivo = getObjetivo(controlador.teamA);
        }
        GetComponent<Atacar>().setTarget(objetivo.GetComponent<AgentNPC>());
        GetComponent<Movimiento>().setTarget(objetivo);
    }
    
    public void soltarObjetivo() 
    { 
        GetComponent<Atacar>().setTarget(null);
        GetComponent<Movimiento>().setTarget(null);
    }
    */
    public bool condicionIdle() 
    {
        if (GetComponent<Atacar>().getTarget() == null) return false;
        AgentNPC enemigo = GetComponent<Atacar>().getTarget();
        //Condicion de pérdida de objetivo
        if (GetComponent<ComponenteIA>().enemigoMuerto(enemigo) || (enemigo.Position - GetComponent<Agent>().Position).magnitude >= 12)
        {
            Debug.Log("de atack a idle");
            return true; 
        }
        else return false;
        //El objetivo ha sido eliminado o ha huido
    }

    /* metodo cambiado a componenteIA
    GameObject getObjetivo(List<GameObject> enemigos) 
    {
        float distancia = float.MaxValue;
        GameObject objetivoActual = null;
        foreach (GameObject enemigo in enemigos)
        {
            if (distancia > (enemigo.GetComponent<Agent>().Position - GetComponent<Agent>().Position).magnitude)
            {
                distancia = (enemigo.GetComponent<Agent>().Position - GetComponent<Agent>().Position).magnitude;
                objetivoActual = enemigo;
            }
        }
        return objetivoActual;
    }
    */
}
