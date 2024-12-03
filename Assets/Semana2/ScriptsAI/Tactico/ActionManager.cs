using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField]private List<Action> queue = new List<Action>();
    [SerializeField]private List<Action> active = new List<Action>();
    public int currTime;

    public void scheduleAction( List<Action> actions){
        foreach (Action action in actions) 
        {
            if (!queue.Contains(action) && !active.Contains(action)) { queue.Add(action); }
        }
        //queue.AddRange(actions);
    }

    void Update(){
        currTime += 1;
        List<Action> temporary = new List<Action>(queue);
        foreach (Action a in temporary){
            if(a.priority <= getHighestPriority(active)){
                break;
            }
            if(a.canInterrupt()){
                active.Clear();
                active.Add(a);
                queue.Remove(a);
            }
        }
        temporary = new List<Action>(queue);
        foreach(Action a in temporary){
            bool cond = false;
            //if (a.expiryTime < currTime){
            //    queue.Remove(a);
            //}
            foreach(Action activ in active){
                if(!a.canDoBoth(activ)){
                    cond = true;
                    break;
                }
            }
            if (cond) { continue; }
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
