﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentNPC : Agent
{ 
    // Este será el steering final que se aplique al personaje.
    [SerializeField] protected Steering steer;
    // Todos los steering que tiene que calcular el agente.
    private List<SteeringBehaviour> listSteerings;
    private bool ModoDep;

    protected int vida;
    protected int atq;
    protected float range;
    


    protected  void Awake()
    {
        this.steer = new Steering();


        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        // La llamaremos listSteerings
        // Puedes usar GetComponents<>()
        SteeringBehaviour [] steers = GetComponents<SteeringBehaviour>();
        listSteerings = new List<SteeringBehaviour>(steers);
    }

    // Use this for initialization
    protected virtual void Start()
    {
        this.Velocity = Vector3.zero;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // En cada frame se actualiza el movimiento
        ApplySteering(Time.deltaTime);
        if(ModoDep){
            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), new Vector3(this.transform.position.x + Acceleration.x, this.transform.position.y + Acceleration.y + 3f , this.transform.position.z + Acceleration.z), Color.blue);
        }

        // En cada frame podría ejecutar otras componentes IA
    }

    private void ApplySteering(float deltaTime)
    {   

        Acceleration = this.steer.linear;
        // Actualizar las propiedades para Time.deltaTime según NewtonEuler
        // La actualización de las propiedades se puede hacer en LateUpdate()
        Velocity += Acceleration * deltaTime;
        Rotation = this.steer.angular;
        
        Orientation += Rotation * deltaTime;
        //transform.rotation = Quaternion.Euler(0,Orientation, 0);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, Orientation);
        
        
        Position += (new Vector3(Mathf.Min(Velocity.x, MaxSpeed),0 ,Mathf.Min(Velocity.z, MaxSpeed))) * deltaTime;

        
        // Rotation
        // Position
        // Orientation
    }

    public virtual void LateUpdate()
    {
        Steering kinematicFinal = new Steering();
        
        // Reseteamos el steering final.
        this.steer = new Steering();

        // Recorremos cada steering
        //Habrá que modificarlo cuando se tenga el actuador
        /*
        foreach(SteeringBehaviour b in listSteerings){
            
            kinematicFinal = b.GetSteering(this);
            
        }
        */
    
        List<SteeringBehaviour> auxList = new List<SteeringBehaviour>();
        foreach (SteeringBehaviour b in listSteerings)
        {
            if (GetComponent<StateMachineManager>() != null){
                if (GetComponent<StateMachineManager>().CurrentState == StateMachineManager.wanderState)
                {
                    if (b.NameSteering == "Wander" || b.NameSteering == "WallAvoidance") { auxList.Add(b); }
                }
                else 
                {
                    if (b.NameSteering != "Wander") { auxList.Add(b); }
                } 
            } else {
                auxList.Add(b);
            }
        }
        kinematicFinal = Arbitro.getKinematicFinal(auxList, this);
        
        //foreach (SteeringBehaviour behavior in listSteerings)
        //    Steering kinematic = behavior.GetSteering(this);
        //// La cinemática de este SteeringBehaviour se tiene que combinar
        //// con las cinemáticas de los demás SteeringBehaviour.
        //// Debes usar kinematic con el árbitro desesado para combinar todos
        //// los SteeringBehaviour.
        //// Llamaremos kinematicFinal a la aceleraciones finales de esas combinaciones.

        // A continuación debería entrar a funcionar el actuador para comprobar
        // si la propuesta de movimiento es factible:
        // kinematicFinal = Actuador(kinematicFinal, self)


        // El resultado final se guarda para ser aplicado en el siguiente frame.
        this.steer = kinematicFinal;
    }

    public void ActivarDep(){
        this.ModoDep = true;
    }

    public void DeactivarDep(){
        this.ModoDep = false;
    }

    public void attackEnemy(AgentNPC target){
        target.vida -= this.atq;

    }
}
