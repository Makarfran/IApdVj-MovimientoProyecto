using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAttack: MonoBehaviour, ITransition
{
    //Comprueba si se debe lanzar la transición
    public bool isTriggered() 
    {
        //Condicion de Ataque -- enemigo cerca y enemigo visible
        Controlador controlador = GameObject.Find("ControladorJuego").GetComponent<Controlador>();
        if (transform.parent.name == "A")
        {
            return AttackCondition(controlador.teamB);
        }
        if (transform.parent.name == "B")
        {
            return AttackCondition(controlador.teamA);
        }

        return false;
    }

    //Devuelve el estado objetivo de la transición
    public IState getTargetState() 
    {
        return GetComponent<StateAttack>();
    }

    //Devuelve una lista de acciones a ejecutar cuando la transición se dispara
    public List<Action> getAction() 
    {
        //¿Accion que introduzca un texto de ataque?
        //Otra posibilidad acción de animación
        List<Action> actions = new List<Action>();
        return actions;
    }

    private bool AttackCondition(List<GameObject> enemigos)  
    {
        foreach (GameObject enemigo in enemigos)
        {
            //Falta condicion de visibilidad
            if ( ((enemigo.GetComponent<AgentNPC>().Position - GetComponent<AgentNPC>().Position).magnitude) < 20) 
            {
                Debug.Log("condicion activa");
               return true; 
            }
        }
        return false;
    }
}
