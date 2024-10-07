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
    [SerializeField]
    protected float updateTime;
    protected float lastUpdate;
    public bool ModoDep;

    void Start()
    {
        delegatedAgent = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        if(!ModoDep){
            delegatedAgent.GetComponent<MeshRenderer>().enabled = false;
        }
        //delegatedAgent.hideFlags = HideFlags.HideInHierarchy; // Ocultar en la jerarquía

        // Agregar el componente Agent al objeto invisible
        delegatedAgent.AddComponent<Agent>();
        this.nameSteering = "Wander";
        this.Weight = 1f;
        /*
        if((gameObject.GetComponent("WallAvoidance") != null)){
                this.GetComponent<WallAvoidance>().target = delegatedAgent.GetComponent<Agent>();
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override Steering GetSteering(Agent agent)
    {
        
        
        // % de wanderRate
        wanderOrientation = RandomBinomial() * wanderRate;
        //Debug.Log("rate: " +rate);
        
        //Debug.Log("WanderOrientation: "+wanderOrientation);
        float targetOrientation = wanderOrientation + agent.Orientation;

        // center of the wander circle
        Vector3 target = agent.Position + wanderOffSet * agent.OrientationToVector();

        // target location
        target += (wanderRadius * OrientationToVector(targetOrientation));
        
        // delegation to Face steering
        this.Rtarget =  delegatedAgent.GetComponent<Agent>();
        this.Rtarget.Position = target;
        this.Rtarget.Orientation = targetOrientation;
        /*
        if((gameObject.GetComponent("Face") != null) && (this.target != this.GetComponent<Face>().Target)){
                Face cara = this.GetComponent<Face>();
                cara.Target = delegatedAgent.GetComponent<Agent>();
        }
        */

        Steering steering = base.GetSteering(agent); 
        // Set a full aceleration
        steering.linear = maxAcceleration * agent.OrientationToVector();
        return steering;

    }

    // Método auxiliar para generar un valor binomial aleatorio
    private float RandomBinomial()
    {
        return Random.value - Random.value;
    }    


    // Método auxiliar para convertir una orientación en un vector
    private Vector3 OrientationToVector(float orientation)
    {
        return new Vector3(Mathf.Cos(Bodi.MapToRangePi(orientation)), 0, Mathf.Sin(Bodi.MapToRangePi(orientation)));
    }

}
