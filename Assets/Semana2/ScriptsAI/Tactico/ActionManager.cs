using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private List<Action> queue = new List<Action>();
    private List<Action> active = new List<Action>();
    public int currTime;

    public void scheduleAction( List<Action> action){
        queue.AddRange(action);
    }

    void Update(){
        
        currTime += 1;
        foreach(Action a in queue){
            if(a.priority <= getHighestPriority(active)){
                break;
            }
            if(a.canInterrupt()){
                active.Clear();
                active.Add(a);
            }
        }
        List<Action> temporary = new List<Action>(queue);
        foreach(Action a in temporary){
            if(a.expiryTime < currTime){
                queue.Remove(a);
            }
            foreach(Action activ in active){
                if(!a.canDoBoth(activ)){
                    break;
                }
            }
            queue.Remove(a);
            active.Add(a);
        }
         temporary = new List<Action>(active);
        foreach(Action a in temporary){
            if(a.isComplete()){
                active.Remove(a);
            } else {
                a.execute();
            }
        }
    }


    private int getHighestPriority(List<Action> actions){
        int highest = 0;
        foreach(Action a in actions){
            if(a.priority > highest){
                highest = a.priority;
            }
        }
        return highest;
    }
}
