using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    

    
    void Start()
    {
        this.nameSteering = "Seek";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        //steer.angular = 0;
        //steer.linear = target.transform.position - agent.transform.position;
        //steer.linear = steer.linear.normalized * agent.MaxAcceleration;
        

        //Craig W. reynolds
        //Vector ditancia
        Vector3 desired_velocity = target.transform.position - agent.transform.position;
        desired_velocity.Normalize();
        //Aplicamos máxima aceleración
        desired_velocity *= agent.MaxAcceleration;
        //Calculamos el steering
        steer.linear = desired_velocity - agent.Velocity;

        // Retornamos el resultado final.
        return steer;
    }
}