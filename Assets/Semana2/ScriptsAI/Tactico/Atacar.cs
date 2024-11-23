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
        if (Time.time > timeInicio + 2)
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
