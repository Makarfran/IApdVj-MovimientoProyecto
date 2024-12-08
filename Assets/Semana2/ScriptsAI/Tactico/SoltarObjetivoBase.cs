using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoltarObjetivoBase : Action
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        priority = 1;
    }


    public override bool canInterrupt()
    {
        return true;
    }
    public override bool canDoBoth(Action other)
    {
        return false;
    }

    public override bool isComplete()
    {
        if (GetComponent<Movimiento>().getTarget() == null)
            return true;
        else return false;
    }
    public override void execute()
    {
        GetComponent<Movimiento>().setTarget(null);
    }

}
