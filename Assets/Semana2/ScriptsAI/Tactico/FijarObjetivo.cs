using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FijarObjetivo : Action
{
    
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
        if (GetComponent<Atacar>().getTarget() != null && GetComponent<Movimiento>().getTarget() != null )
            return true;
        else return false;
    }
    public override void execute()
    {
        GetComponent<ComponenteIA>().fijarObjetivo();
        Debug.Log("fijar objetivo");
    }

}
