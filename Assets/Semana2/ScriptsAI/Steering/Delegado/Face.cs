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
            Debug.Log("magnitud igual a 0, steering vacio en face");
            return steer;
        }
        

        this.target = Rtarget;

        this.target.Orientation = Bodi.sitienesPiyloquieresengradosPeroalreves(Mathf.Atan2(direction.x, direction.z));

        return base.GetSteering(agent);
    }


    public void NewTarget(Vector3 newTarget)
    {
        Rtarget = GetNewTarget(newTarget);

    }

    

}
