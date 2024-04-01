using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separacion : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public List<Agent> targets = new List<Agent>();
    public float decayCoeficient; // k formula k/d^2
    public float treshold;


    
    void Start()
    {
        this.nameSteering = "Separacion";
        this.Weight = 0.7f;
        //Mejor si tenemos una lista con todos los npc en un game manager 
        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("Npc")) 
        {
            if (npc != this.gameObject) { targets.Add(npc.GetComponent<Agent>()); } 
        }
        
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        foreach (Agent target in targets){
            Vector3 direction = agent.transform.position - target.transform.position;
            float distance = direction.magnitude;
            float aceleracion = 0;
            if (distance < treshold) {
                aceleracion = Mathf.Min(decayCoeficient / (distance * distance), agent.MaxAcceleration);
            }

            direction.Normalize();
            steer.linear += aceleracion * direction; 
        }

        // Retornamos el resultado final.
        return steer;
    }
}