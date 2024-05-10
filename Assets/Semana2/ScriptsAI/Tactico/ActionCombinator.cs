using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCombinator : Action
{
    public List<Action> subactions;
    public override bool canInterrupt(){
        foreach(Action a in subactions){
            if(a.canInterrupt()) return true;
        }
        return false;
    }
    public override bool canDoBoth(Action other){
        foreach(Action a in subactions){
            if(!a.canDoBoth(other)) return false;
        }
        return true;
    }

    public override bool isComplete(){
        foreach(Action a in subactions){
            if(!a.isComplete()) return false;
        }
        return true;
    }

    public override void execute(){
        foreach(Action a in subactions){
            a.execute();
        }
    }
}
