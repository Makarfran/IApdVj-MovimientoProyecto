using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbitro : MonoBehaviour
{

    // Start is called before the first frame update
    public static Steering getKinematicFinal(List<SteeringBehaviour> steers, Agent agente)
    {
        Steering final = new Steering();
        Steering temp = new Steering();

        foreach (SteeringBehaviour b in steers)
        {

            if(b.NameSteering == "Wander"){
                
                temp = b.GetSteering(agente);
                final.linear += b.Weight * temp.linear;
                final.angular += b.Weight * temp.angular;
                
            }
            if(b.enabled != true || b.Target == null){
                continue;
            }

            if (b.NameSteering == "Align" && (agente.Velocity.magnitude > 0.1)) {
             }
            else if (b.NameSteering == "Face") {
                
                temp = b.GetSteering(agente);
                final.linear += b.Weight * temp.linear;
                final.angular += b.Weight * temp.angular;
            } else {
                temp = b.GetSteering(agente);
                final.linear += b.Weight * temp.linear;
                final.angular += b.Weight * temp.angular;
            }
        

            

            
        }
        /*
        foreach(SteeringBehaviour b in steers){
            temp = b.GetSteering(agente);
            if(b.NameSteering == "Seek"){
                if(Vector3.Distance(agente.transform.position, b.target.transform.position) < 20){

                    final.linear.x += 0.8f * temp.linear.x;
                    final.linear.y += 0.8f *temp.linear.y;
                    final.linear.z += 0.8f *temp.linear.z;
                }    
            } else if(b.NameSteering == "Path Following"){

                final.linear.x += 0.3f*temp.linear.x;
                final.linear.y += 0.3f*temp.linear.y;
                final.linear.z += 0.3f*temp.linear.z;
            }

            if(b.NameSteering == "Wall Avoidance"){

                final.linear.x +=  0.2f * temp.linear.x;
                final.linear.y += 0.2f * temp.linear.y;
                final.linear.z += 0.2f * temp.linear.z;
            }
        }
        */

        //Limitamos el resultado y devolvemos 
        if (final.linear.magnitude > agente.MaxAcceleration)
        {
            final.linear = final.linear.normalized * agente.MaxAcceleration;
        }
        final.angular = Mathf.Min(final.angular, agente.MaxRotation);

        return final;

    }
}
