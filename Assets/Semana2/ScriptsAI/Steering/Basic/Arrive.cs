using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    [SerializeField] protected float radio; // radio pequeño; Modificado para usar el radio interior del agente objetivo
    [SerializeField] protected float slowRadio; // radio grande durante el cual se desacelera. Modificado para usar el radio externo del agente objetivo
    protected float targetSpeed;
    protected Vector3 targetVelocity;
    [SerializeField] protected float timeToTarget = 0.1f;

    protected float distance; 
    void Start()
    {
        this.nameSteering = "Arrive";
        this.Weight = 0.7f;
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        if (target == null) 
        {
            return steer;
        }

        // Calcula el steering.
        Vector3 direction = target.transform.position - agent.transform.position;
        distance = direction.magnitude;

        /*
        Dejar de mover el npc al llegar al objetivo
        v = v0 + at; para v = 0, at = -v0  a = (-v0/t)
        */
        if(distance < radio)
        {
            steer.linear = - agent.Velocity;
            steer.angular = 0;      
            return steer;
        }

        if (distance > slowRadio)
        {
            targetSpeed = agent.MaxSpeed;
        } else {
            targetSpeed = agent.MaxSpeed * distance / slowRadio;
        }


        // combininacion de direccion y spee en velocidad:
        targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        // calculo de aceleracion
        steer.linear = targetVelocity - agent.Velocity;
        steer.linear /= timeToTarget;

        if (steer.linear.magnitude > agent.MaxAcceleration){
            steer.linear.Normalize();
            steer.linear *= agent.MaxAcceleration;
        }
        steer.angular = 0;
        // Retornamos el resultado final.
        return steer;
    }

    public void NewTarget(Vector3 newTarget) 
    {
        target = GetNewTarget(newTarget);
    }
}