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
    //target == null 
    public override bool isComplete()
    {
        AgentNPC npcActual = GetComponent<AgentNPC>();
        if (comprobarDistancia(npcActual) || 
           (target.GetComponent<AgentNPC>() && (target.GetComponent<AgentNPC>().Position - npcActual.Position).magnitude >= 12)) 
        {
            //Debug.Log("cortar camino");
            GetComponent<ComponenteIA>().pararMovimiento();
            return true;
        }
        return false;
    }
    public override void execute()
    {
        if (target != null) 
        {
            if (!GetComponent<PathFinding>().hayCamino() && !comprobarDistancia(GetComponent<AgentNPC>())) GetComponent<PathFinding>().CalcularCamino(target.transform.position);
            else 
            {
                if (!comprobarDistancia(GetComponent<AgentNPC>()) && (GetComponent<PathFinding>().GetDestino().getPosition() - target.transform.position).magnitude > 3)
                    GetComponent<PathFinding>().CalcularCamino(target.transform.position);
            }

            switch (GetComponent<StateMachineIA>().CurrentState)
            {   case StateCapture:
                    GetComponent<AgentNPC>().changeColorCapture();
                    break;
                case StateDefend:
                    GetComponent<AgentNPC>().changeColorDefensa(); 
                    break;
                case StateFlee:
                    GetComponent<AgentNPC>().changeColorFlee();
                    break;
                default:
                    GetComponent<AgentNPC>().changeColorMovimiento();
                    break;
            }

            
        } 

    }

    public void setTarget(GameObject target)
    {
        this.target = target;
    }

    public GameObject getTarget()
    {
        return target;
    }

    private bool comprobarDistancia(AgentNPC npcActual) 
    {
        //AgentNPC npcActual = GetComponent<AgentNPC>();
        if (target == null) return true;
        if (target.GetComponent<AgentNPC>())
        {
            return (npcActual.Position - target.transform.position).magnitude < npcActual.getRange();

        }
        else if (target.GetComponent<KeypointBase>())
        {
            return (target.GetComponent<KeypointBase>().transform.position - npcActual.Position).magnitude < 4;
        }
        else 
        {
            return GetComponent<ComponenteIA>().distanciaHeal();
        }
    }
}
