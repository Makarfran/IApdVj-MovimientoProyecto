using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransition
{
    //Comprueba si se debe lanzar la transición
    bool isTriggered();
    
    //Devuelve el estado objetivo de la transición
    IState getTargetState();

    //Devuelve una lista de acciones a ejecutar cuando la transición se dispara
    List<Action> getAction();
}
