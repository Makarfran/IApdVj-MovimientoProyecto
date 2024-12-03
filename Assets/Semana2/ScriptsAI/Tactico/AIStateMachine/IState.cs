using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //Devuelve la acci�n o lista de acciones a realizar mientras se esta en el estado
    List<Action> getAction();

    //Devuelve la acci�n o lista de acciones a realizar cuando se entra en el estado
    List<Action> getEntryAction();

    //Devuelve la acci�n o lista de acciones a realizar cuando se sale del estado
    List<Action> getExitAction();

    //Devuelve las transiciones que se comprobar�n
    List<ITransition> getTransitions();

    //Comprueba si se debe salir del estado actual para tomar una nueva decisi�n
    bool condicionIdle();

}   
