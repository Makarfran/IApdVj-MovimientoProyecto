using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCapture : MonoBehaviour, ITransition
{
    //Comprueba si se debe lanzar la transición
    public bool isTriggered()
    {
        //Condicion de Captura -- 
        return GetComponent<ComponenteIA>().conditionCapture();
    }

    //Devuelve el estado objetivo de la transición
    public IState getTargetState()
    {
        return GetComponent<StateCapture>();
    }

    //Devuelve una lista de acciones a ejecutar cuando la transición se dispara
    public List<Action> getAction()
    {
        //¿Accion que introduzca un texto de ataque?
        //Otra posibilidad acción de animación
        List<Action> actions = new List<Action>();
        return actions;
    }

}
