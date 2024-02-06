using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fle : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent pers;

    
    void Start()
    {
        this.nameSteering = "Flee";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        steer.angular = 0;
        steer.linear = pers.transform.position - agent.transform.position;
        steer.linear.Normalize();
        steer.linear *= pers.MaxAcceleration;

        // Retornamos el resultado final.
        return steer;
    }
}