using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBerserker : MonoBehaviour, ITransition
{
    //Comprueba si se debe lanzar la transición
    public bool isTriggered()
    {
        ComponenteIA ia = GetComponent<ComponenteIA>();
        //Al ser una mecanica antisuicida la condicion es la misma que para la huida. vida baja. Además comprobamos si hay enemigos cerca
        return ia.elite() && ia.conditionFlee() && ia.conditionAttack();
    }

    //Devuelve el estado objetivo de la transición
    public IState getTargetState()
    {
        return GetComponent<StateBerserker>();
    }

    //Devuelve una lista de acciones a ejecutar cuando la transición se dispara
    public List<Action> getAction()
    {

        List<Action> actions = new List<Action>();
        return actions;
    }

}
