using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{

    [SerializeField] protected Agent Rtarget;
    // Start is called before the first frame update
    void Start()
    {
        this.nameSteering = "Face";
        this.Weight = 1f;
    }


    public override Steering GetSteering(Agent agent)
    {
        Vector3 direction = Rtarget.transform.position - agent.transform.position;

        if(Vector3.Magnitude(direction) == 0f){
            Steering steer = new Steering();
            return steer;
        }
        

        this.target = Rtarget;
<<<<<<< HEAD
        this.target.Orientation = Bodi.sitienesPiyloquieresengradosPeroalreves(Mathf.Atan2(direction.x, direction.z));
        Debug.Log(this.target.Orientation);
=======
        this.target.Orientation = Bodi.sitienesPiyloquieresengradosPeroalreves(Mathf.Atan2(-direction.x, direction.z));
        
>>>>>>> 9219dfa1bda4b8a08f2cc1a1a1c755164ae239c7

        return base.GetSteering(agent);
    }

<<<<<<< HEAD
    public void NewTarget(Vector3 newTarget)
    {
        Rtarget = GetNewTarget(newTarget);
=======
    public void changeRtarget(Agent agente){
        this.Rtarget = agente;
>>>>>>> 9219dfa1bda4b8a08f2cc1a1a1c755164ae239c7
    }
}
