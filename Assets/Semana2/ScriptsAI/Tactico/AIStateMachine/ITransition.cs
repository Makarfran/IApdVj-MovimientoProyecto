using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransition
{
    //Comprueba si se debe lanzar la transici�n
    bool isTriggered();
    
    //Devuelve el estado objetivo de la transici�n
    IState getTargetState();

    //Devuelve una lista de acciones a ejecutar cuando la transici�n se dispara
    List<Action> getAction();
}
