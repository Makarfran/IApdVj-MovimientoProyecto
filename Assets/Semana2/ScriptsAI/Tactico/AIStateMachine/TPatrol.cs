using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPatrol : MonoBehaviour, ITransition
{
    //Comprueba si se debe lanzar la transici�n
    public bool isTriggered()
    {
        ComponenteIA ia = GetComponent<ComponenteIA>();
        //solo infanteria
        return ia.infanteria() && ia.condicionPatrol() && !GetComponent<AgentNPC>().vidaBaja();
    }

    //Devuelve el estado objetivo de la transici�n
    public IState getTargetState()
    {
        return GetComponent<StatePatrol>();
    }

    //Devuelve una lista de acciones a ejecutar cuando la transici�n se dispara
    public List<Action> getAction()
    {
        //�Accion que introduzca un texto informativo?
        //Otra posibilidad acci�n de animaci�n
        List<Action> actions = new List<Action>();
        return actions;
    }
}
