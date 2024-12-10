using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MStartPatrulla : Action
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
        AgentNPC npcActual = GetComponent<AgentNPC>();
        if ((GetComponent<ActivarPatrulla>().camino.getObjetivoInicial().transform.position - npcActual.Position).magnitude <= 3)
        {
            //Debug.Log("cortar camino");
            GetComponent<ComponenteIA>().pararMovimiento();
            return true;
        }
        return false;
    }
    public override void execute()
    {
        //Debug.Log("LLegando al punto de patrulla");
        
        Vector3 objetivo = GetComponent<ActivarPatrulla>().camino.getObjetivoInicial().transform.position;
        if (!GetComponent<PathFinding>().hayCamino())
            GetComponent<PathFinding>().CalcularCamino(objetivo);
    }

}
