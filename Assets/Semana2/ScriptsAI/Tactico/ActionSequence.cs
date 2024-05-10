using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence : Action
{
    public List<Action> subactions;
    public int activeIndex = 0;
    public override bool canInterrupt(){
        return subactions[0].canInterrupt();
    }
    public override bool canDoBoth(Action other){
        foreach(Action a in subactions){
            if(!a.canDoBoth(other)) return false;
        }
        return true;
    }

    public override bool isComplete(){
        return activeIndex >= subactions.Count;
    }

    public override void execute(){
        subactions[activeIndex].execute();
        if(subactions[activeIndex].isComplete()) activeIndex += 1;
    }
}
