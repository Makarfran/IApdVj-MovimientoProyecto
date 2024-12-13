using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploracion : Action
{
    [SerializeField] private Tile target;
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
    //target == null 
    public override bool isComplete()
    {
        AgentNPC npcActual = GetComponent<AgentNPC>();
        if ((target.getPosition() - npcActual.Position).magnitude <= 2)
        {
            return true;
        }
        return false;
    }
    public override void execute()
    {
        if (target != null)
        {
            if (!GetComponent<PathFinding>().hayCamino()) GetComponent<PathFinding>().CalcularCamino(target.transform.position);
            else
            {
                if ((GetComponent<PathFinding>().GetDestino().getPosition() - target.transform.position).magnitude > 3)
                    GetComponent<PathFinding>().CalcularCamino(target.transform.position);
            }
            GetComponent<AgentNPC>().changeColorExploracion();
        }

    }

    public void setTarget(Tile target)
    {
        this.target = target;
    }

    public Tile getTarget()
    {
        return target;
    }

}
