using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEvitarDerrota : MonoBehaviour, ITransition
{
    //Comprueba si se debe lanzar la transición
    public bool isTriggered()
    {
            return GetComponent<ComponenteIA>().comprobarAtaqueBasePrincipal(GetComponent<AgentNPC>().getBando());
    }

    //Devuelve el estado objetivo de la transición
    public IState getTargetState()
    {
        return GetComponent<StateDefend>();
    }

    //Devuelve una lista de acciones a ejecutar cuando la transición se dispara
    public List<Action> getAction()
    {

        List<Action> actions = new List<Action>();
        return actions;
    }
}
