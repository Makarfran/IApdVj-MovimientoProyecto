using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public int expiryTime;
    public int priority;

    public abstract bool canInterrupt();
    public abstract bool canDoBoth(Action other);
    public abstract bool isComplete();
    public abstract void execute();
}
