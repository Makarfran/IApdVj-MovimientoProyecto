using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tidle : MonoBehaviour, ITransition
{
    //Comprueba si se debe lanzar la transición
    public bool isTriggered()
    {
        IState state = GetComponent<StateMachineIA>().CurrentState;
        return state.condicionIdle();
        /*
        if (ReferenceEquals(state, GetComponent<StateAttack>())) 
        {
            //Condicion de pérdida de objetivo
            //El objetivo ha sido eliminado o ha huido
        }
        
        if (ReferenceEquals(state, GetComponent<StateCapture>())) 
        {
            //Condicion de pérdida de objetivo
            //El objetivo ha sido capturado
        }

        if (ReferenceEquals(state, GetComponent<StateDefend>())) 
        {
            //Condicion de pérdida de objetivo
            //El objetivo ya ha sido capturado o el enemigo ya no lo esta intentando capturar
        }

        if (ReferenceEquals(state, GetComponent<StateFlee>())) 
        {
                //Condicion de curación
                //El personaje se ha curado
        }
        */
    }

    //Devuelve el estado objetivo de la transición
    public IState getTargetState()
    {
        return GetComponent<StateIdle>();
    }

    //Devuelve una lista de acciones a ejecutar cuando la transición se dispara
    public List<Action> getAction()
    {
        //¿Accion que introduzca un texto informativo?
        //Otra posibilidad acción de animación
        List<Action> actions = new List<Action>();
        return actions;
    }
}
