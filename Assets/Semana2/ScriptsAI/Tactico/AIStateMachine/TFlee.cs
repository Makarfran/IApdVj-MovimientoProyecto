using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFlee : MonoBehaviour, ITransition
{
    //Comprueba si se debe lanzar la transici�n
    public bool isTriggered()
    {
        ComponenteIA ia = GetComponent<ComponenteIA>();
        return !ia.elite() && ia.conditionFlee();
    }

    //Devuelve el estado objetivo de la transici�n
    public IState getTargetState()
    {
        return GetComponent<StateFlee>();
    }

    //Devuelve una lista de acciones a ejecutar cuando la transici�n se dispara
    public List<Action> getAction()
    {
        
        List<Action> actions = new List<Action>();
        return actions;
    }

}
