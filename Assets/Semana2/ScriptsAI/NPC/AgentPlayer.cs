﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.VR.WSA.WebCam;

//Agente especial controlado por el jugador

public class AgentPlayer : Agent
{
    // Update is called once per frame
    public virtual void Update()
    {
        // Mientras que no definas las propiedades en Bodi esto seguirá dando error.
        Velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Se mueve a máxima velocidad en la dirección dada por el jugador
        Velocity *= MaxSpeed;  // DESCOMENTA !!
        Vector3 translation = Velocity * Time.deltaTime;
        transform.Translate(translation, Space.World); 

        // Para el jugador usamos el SteeringBehaviour (LookAt)
        // que ya lleva implementado Unity.
        // Notar que al jugador le aplicamos un movimiento no-acelerado.
        transform.LookAt(transform.position + Velocity); 
        Orientation = transform.rotation.eulerAngles.y; // DESCOMENTA !!
    }

}
