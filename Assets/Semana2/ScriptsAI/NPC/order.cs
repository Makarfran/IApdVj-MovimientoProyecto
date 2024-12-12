using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class order : MonoBehaviour
{
    public Agent arrivalPoint;
    public Agent alignPoint;

    public void destroy()
    {
        arrivalPoint = null;
        alignPoint = null;
    }
}
