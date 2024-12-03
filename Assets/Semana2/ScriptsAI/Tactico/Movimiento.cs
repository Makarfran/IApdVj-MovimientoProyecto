using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : Action
{
    [SerializeField]private GameObject target;
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
        if (target == null || comprobarDistancia() || 
           (target.GetComponent<AgentNPC>() && (target.GetComponent<AgentNPC>().Position - GetComponent<Agent>().Position).magnitude >= 12)) 
        {
            //Debug.Log("cortar camino");
            GetComponent<PathFinding>().cortarCamino();
            return true;
        }
        return false;
    }
    public override void execute()
    {
        GetComponent<PathFinding>().CalcularCamino(target.transform.position);
    }

    public void setTarget(GameObject target)
    {
        this.target = target;
    }

    public GameObject getTarget()
    {
        return target;
    }

    private bool comprobarDistancia() 
    {
        AgentNPC npcActual = GetComponent<AgentNPC>();
        if (target.GetComponent<AgentNPC>())
        {
            return (npcActual.Position - target.transform.position).magnitude < npcActual.getRange();

        }
        else 
        {
            return target.GetComponent<KeypointBase>().GetNPCS().Contains(npcActual);
        }
    }
}
