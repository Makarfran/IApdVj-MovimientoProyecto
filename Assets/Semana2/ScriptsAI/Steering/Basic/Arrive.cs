using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour

    public float radio;
    public float slowRadio;
    public float targetSpeed;
    public Vector3 targetVelocity;
    public float timeToTarget;

    [SerializeField] public float distance; 
    void Start()
    {
        this.nameSteering = "Arrive";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        Vector3 direction = target.transform.position - agent.transform.position;
        distance = direction.magnitude;

        // ojo ¿que significa return null -- dejar de moverse?
        if(distance < radio){
            
            steer.linear = new Vector3(0,0,0);
            steer.angular = 0;
            return steer;
            //return null;
        }

        if (distance > slowRadio){
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