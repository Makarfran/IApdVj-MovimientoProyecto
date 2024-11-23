using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tidle : MonoBehaviour, ITransition
{
    //Comprueba si se debe lanzar la transici�n
    public bool isTriggered()
    {
        IState state = GetComponent<StateMachineIA>().CurrentState;
        return state.condicionIdle();
        /*
        if (ReferenceEquals(state, GetComponent<StateAttack>())) 
        {
            //Condicion de p�rdida de objetivo
            //El objetivo ha sido eliminado o ha huido
        }
        
        if (ReferenceEquals(state, GetComponent<StateCapture>())) 
        {
            //Condicion de p�rdida de objetivo
            //El objetivo ha sido capturado
        }

        if (ReferenceEquals(state, GetComponent<StateDefend>())) 
        {
            //Condicion de p�rdida de objetivo
            //El objetivo ya ha sido capturado o el enemigo ya no lo esta intentando capturar
        }

        if (ReferenceEquals(state, GetComponent<StateFlee>())) 
        {
                //Condicion de curaci�n
                //El personaje se ha curado
        }
        */
    }

    //Devuelve el estado objetivo de la transici�n
    public IState getTargetState()
    {
        return GetComponent<StateIdle>();
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
