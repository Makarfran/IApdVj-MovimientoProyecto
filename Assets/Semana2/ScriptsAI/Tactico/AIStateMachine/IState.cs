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

    //Fija el objetivo de la acci�n correspondiente al estado
    //p.e: en el estado de ataque establece como objetivo al enemigo correspondiente
    //p.e : en el estado de captura establece como objetivo la base a capturar
    void fijarObjetivo();

    void soltarObjetivo();

    bool condicionIdle();

}
