using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Representa la propuesta de movimiento.
[RequireComponent(typeof(AgentNPC))]
public class SteeringBehaviour : MonoBehaviour
{

    protected string nameSteering = "no steering";

    public string NameSteering
    {
        set { nameSteering = value; }
        get { return nameSteering; }
    }

    // Objetivo
    public Agent target;

    //Peso o prioridad para árbitro
    float weight;
    public float Weight 
    {
        set { weight = value; }
        get { return weight; }
    }

    /// <summary>
    /// Cada SteerinBehaviour retornará un Steering=(vector, escalar)
    /// acorde a su propósito: llegar, huir, pasear, ...
    /// Sobreescribie siempre este método en todas las clases hijas.
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public virtual Steering GetSteering(Agent agent)
    {
        return null;
    }


    protected virtual void OnGUI()
    {
        // Para la depuración te puede interesar que se muestre el nombre
        // del steeringbehaviour sobre el personaje.
        // Te puede ser util Rect() y GUI.TextField()
        // https://docs.unity3d.com/ScriptReference/GUI.TextField.html
    }

    protected Agent GetNewTarget(Vector3 newTarget) 
    {
        Agent agentNewTarget;
        if (GetComponent<order>().arrivalPoint == null)
        {
            agentNewTarget = Agent.AgentCreator().GetComponent<Agent>();
            agentNewTarget.gameObject.name = "arrivePoint";
            GetComponent<order>().arrivalPoint = agentNewTarget;
        }
        else { agentNewTarget = GetComponent<order>().arrivalPoint; }

        agentNewTarget.Position = newTarget;
        GetComponent<order>().arrivalPoint = agentNewTarget;
        return agentNewTarget;
    }

    protected Agent GetNewTargetOr(float newTarget)
    {
        Agent agentNewTarget;
        if (GetComponent<order>().alignPoint == null)
        {
            agentNewTarget = Agent.AgentCreator().GetComponent<Agent>();
            agentNewTarget.gameObject.name = "alignPoint";
            GetComponent<order>().alignPoint = agentNewTarget;
        }
        else { agentNewTarget = GetComponent<order>().alignPoint; }

        agentNewTarget.Orientation = newTarget;
        GetComponent<order>().alignPoint = agentNewTarget;
        return agentNewTarget;
    }
}
