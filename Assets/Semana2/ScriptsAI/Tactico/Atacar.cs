using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Atacar : Action
{
    private bool didyoudoit = false;
    private AgentNPC target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool canInterrupt(){
        return true;
    }
    public override bool canDoBoth(Action other){
        return false;
    }

    public override bool isComplete(){
        if(didyoudoit){
            return true;
        } else {
            return false;
        }
    }
    public override void execute(){
        AgentNPC agente = this.GetComponent<AgentNPC>();
        //if on range do the next
        //hacer una animacion?
        target.pierdeVida(agente.getAtaque());
        didyoudoit = true;
    }

    public void setTarget(AgentNPC target){
        this.target = target;
    }
}
