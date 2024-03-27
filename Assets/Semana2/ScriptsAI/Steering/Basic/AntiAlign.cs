using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiAlign : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour

    float timeToTarget = 0.1f;
    void Start()
    {
        this.nameSteering = "Anti-Align";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        float angularAcceleration;
        float targetRotation;
        float rotation = agent.Orientation - target.Orientation;
        

        rotation = Bodi.MapToRangePi(rotation);
        
        
        float rotationSize = Mathf.Abs(rotation);

        
        if (Mathf.Abs(rotationSize - Mathf.PI) < (target.InteriorRadius)*Mathf.PI/180f){
            steer.linear = Vector3.zero;
            steer.angular = 0;
            return steer;
        }

        
        if (Mathf.Abs(rotationSize - Mathf.PI) > (target.ArrivalRadius)*Mathf.PI/180f){
            targetRotation = agent.MaxRotation;
        } else {
            targetRotation = agent.MaxRotation * rotationSize / target.ArrivalRadius;
        }

        targetRotation *= rotation / rotationSize;

        steer.angular = targetRotation - agent.Rotation;
        steer.angular /= timeToTarget;

        angularAcceleration = Mathf.Abs(steer.angular);
        if(angularAcceleration > agent.MaxAngularAcc){
            steer.angular /= angularAcceleration;
            steer.angular *= agent.MaxAngularAcc;
        }
        
        // Calcula el steering.
        steer.linear = Vector3.zero;
        // Retornamos el resultado final.
        
        return steer;
    }
}