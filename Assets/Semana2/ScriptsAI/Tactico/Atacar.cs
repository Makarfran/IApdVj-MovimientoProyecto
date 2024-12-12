using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Atacar : Action
{
    private bool didyoudoit = false;
    public AgentNPC target;
    private float timeInicio = float.MaxValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool canInterrupt()
    {
        return false;
    }
    public override bool canDoBoth(Action other)
    {
        return false;
    }

    public override bool isComplete()
    {
        if (target != null) 
        {
            Vector3 direction = target.Position - GetComponent<AgentNPC>().Position;
            float orientation = this.target.Orientation = Bodi.sitienesPiyloquieresengradosPeroalreves(Mathf.Atan2(direction.x, direction.z));
            GetComponent<Align>().NewTargetOr(orientation);
        }
       
        //GetComponent<Face>().target = target;
        if (target == null || (target.Position - GetComponent<Agent>().Position).magnitude >= 12 || Time.time > timeInicio + 2)
        {
            didyoudoit = false;
            timeInicio = float.MaxValue;
            return true;
        }
        return false;
    }

    public override void execute()
    {
        if (!didyoudoit)
        {
            //if on range do the next
            //hacer una animacion?
            Vector3 direction = target.Position - GetComponent<AgentNPC>().Position;
            float orientation = this.target.Orientation = Bodi.sitienesPiyloquieresengradosPeroalreves(Mathf.Atan2(direction.x, direction.z));
            GetComponent<Align>().NewTargetOr(orientation);
            GetComponent<AgentNPC>().attackEnemy(target);
            didyoudoit = true;
            timeInicio = Time.time;
        }
    }

    public void setTarget(AgentNPC target)
    {
        this.target = target;
    }

    public AgentNPC getTarget()
    {
        return target;
    }

}
