using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarPatrulla : Action
{
    // Start is called before the first frame update
    void Start()
    {
        priority = 1;
    }

    // Update is called once per frame
    void Update()
    {

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
        if (GetComponent<PathFollowing>().camino == null && GetComponent<PathFollowing>().target == null)
            return true;
        else return false;
    }
    public override void execute()
    {
        GetComponent<PathFollowing>().camino = null;
        GetComponent<PathFollowing>().target = null;
        GetComponent<Movimiento>().setTarget(null);
    }

}
