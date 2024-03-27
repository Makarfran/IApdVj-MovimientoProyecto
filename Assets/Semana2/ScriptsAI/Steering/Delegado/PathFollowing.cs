using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : Seek
{

    public PathBasico camino;
    // Start is called before the first frame update
    void Start()
    {
        this.nameSteering = "Path Following";
        this.target = camino.getObjetivoInicial();
    }


    public override Steering GetSteering(Agent agent)
    {   
        float distancia = Vector3.Distance(agent.transform.position, target.transform.position);
        if(Mathf.Abs(distancia) < 2f){
            this.target = camino.getSiguiente(this.target);
            if(gameObject.GetComponent("Face") != null){
                this.GetComponent<Face>().NewTarget(this.target.Position);
            }
        }

        
        return base.GetSteering(agent);
    }
}
