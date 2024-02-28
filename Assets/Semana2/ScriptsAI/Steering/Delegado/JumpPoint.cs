using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{
    //Posici�n del punto de salto
    [SerializeField] protected Vector3 jumpLocation;

    //Posici�n del punto de final de salto
    [SerializeField] protected Vector3 landingLocation;

    //Diferencia entre el punto de salto y el punto de final de salto
    protected Vector3 deltaPosition;

    public Vector3 JumpLocation 
    {
        set { jumpLocation = value; }
        get { return jumpLocation; }
    }

    public Vector3 LandingLocation 
    {
        set { landingLocation = value; }
        get { return landingLocation; }
    }

    public Vector3 DeltaPosition 
    {
        get { return LandingLocation-JumpLocation; }
    }

}
