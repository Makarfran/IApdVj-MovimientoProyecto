using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour

    [SerializeField] protected float radio; // radio pequeño;
    [SerializeField] protected float slowRadio; // radio grande durante el cual se desacelera.
    protected float targetSpeed;
    protected Vector3 targetVelocity;
    [SerializeField] protected float timeToTarget;

    protected float distance; 
    void Start()
    {
        this.nameSteering = "Leave";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        Vector3 direction = agent.transform.position - target.transform.position ;
        distance = direction.magnitude;

        // ojo ¿que significa return null -- dejar de moverse?
        //  si, concretamente hay que despejar una a tal que v = 0
        //  v = v0 + at; para v = 0, at = -v0  a = (-v0/t)
        //  
        if(distance > slowRadio){
            steer.linear = - agent.Velocity;
            steer.angular = 0;      
            return steer;
            //return null;
        }

        if (distance > radio){
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
}