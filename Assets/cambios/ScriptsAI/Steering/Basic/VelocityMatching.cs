using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatching : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent pers;
    protected float timeToTarget = 0.1f;

    
    void Start()
    {
        this.nameSteering = "VelocityMatching";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        steer.linear = agent.Velocity - pers.Velocity;
        steer.linear /= timeToTarget;

        if(steer.linear.magnitude > pers.MaxAcceleration){
            steer.linear.Normalize();
            steer.linear *= pers.MaxAcceleration;
        }

        steer.angular = 0;

        // Calcula el steering.

        // Retornamos el resultado final.
        return steer;
    }
}