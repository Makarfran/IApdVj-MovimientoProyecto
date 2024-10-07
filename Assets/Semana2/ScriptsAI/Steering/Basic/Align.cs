using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    
    [SerializeField]
    float timeToTarget = 1f;
    void Start()
    {
        this.nameSteering = "Align";
        this.Weight = 1f;
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        float rotation = target.Orientation - agent.Orientation;
        

        rotation = Bodi.MapToRangePi(rotation);
        
        
        float rotationSize = Mathf.Abs(rotation);
        

        if (rotationSize < Bodi.MapToRangePi(target.InteriorRadius)){
            steer.linear = Vector3.zero;
            steer.angular = 0;
            return steer;
        }

        float targetRotation;
        if (rotationSize > Bodi.MapToRangePi(target.ArrivalRadius)){
            targetRotation = agent.MaxRotation;
        } else {
            targetRotation = agent.MaxRotation * rotationSize / target.ArrivalRadius;
        }

        targetRotation *= rotation / rotationSize;

        steer.angular = targetRotation - agent.Rotation;
        steer.angular /= timeToTarget;

        float angularAcceleration = Mathf.Abs(steer.angular);
        if(angularAcceleration > agent.MaxAngularAcc){
            steer.angular /= angularAcceleration;
            steer.angular *= agent.MaxAngularAcc;
        }
        
        // Calcula el steering.
        steer.linear = Vector3.zero;
        // Retornamos el resultado final.
        
        return steer;
    }

    public void NewTargetOr(float newTarget)
    {
        target = GetNewTargetOr(newTarget);
    }
}