using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Face // debe heredar de face
{
    [SerializeField]
    protected float wanderOffSet; // separaci�n desde el npc. Centro del circulo
    [SerializeField]
    protected float wanderRadius; // radio del circulo
    [SerializeField]
    protected float wanderRate; // max radians o degree en el que calcular los puntos aleatorios.
    protected float wanderOrientation; // current orientation del punto aleatorio calculado.
    [SerializeField]
    protected float maxAcceleration;
    protected GameObject delegatedAgent;


    void Start()
    {
        delegatedAgent = new GameObject("InvisibleObject");
        delegatedAgent.hideFlags = HideFlags.HideInHierarchy; // Ocultar en la jerarquía

        // Agregar el componente Agent al objeto invisible
        delegatedAgent.AddComponent<Agent>();
        this.nameSteering = "Wander";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override Steering GetSteering(Agent agent)
    {
        
        // % de wanderRate
        float rate = Random.value;
        Debug.Log("rate: " +rate);
        wanderOrientation +=rate * wanderRate;
        float targetOrientation = wanderOrientation + agent.Orientation;

        // center of the wander circle
        Vector3 target = agent.Position + wanderOffSet * agent.OrientationToVector();

        // target location
        float sin = Mathf.Sin(Bodi.MapToRangePi(targetOrientation));
        float cos = Mathf.Cos(Bodi.MapToRangePi(targetOrientation));
        Vector3 targetOrientationAsVector = new Vector3(sin, 0, cos);
        target += (wanderRadius * targetOrientationAsVector);

        
        // delegation to Face steering
        this.Rtarget =  delegatedAgent.GetComponent<Agent>();
        this.Rtarget.Position = target;
        this.Rtarget.Orientation = targetOrientation;
        

        Steering steering = base.GetSteering(agent); 
        // Set a full aceleration
        steering.linear = maxAcceleration * agent.OrientationToVector();
        return steering;

    }

}
