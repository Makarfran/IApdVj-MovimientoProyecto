using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour

    
    void Start()
    {
        this.nameSteering = "Flee";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        steer.angular = 0;
        steer.linear = agent.transform.position - target.transform.position;
        steer.linear = steer.linear.normalized * agent.MaxAcceleration;

        // Retornamos el resultado final.
        return steer;
    }
}