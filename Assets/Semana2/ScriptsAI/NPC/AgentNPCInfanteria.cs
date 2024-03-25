using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AgentNPCInfanteria : AgentNPC
{
    [SerializeField] Grid grid;
    
    // Start is called before the first frame update
    void Awake(){
        base.Awake();
    }

    void Start(){
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        Tile tile = grid.getTileByVector(this.transform.position);
        String tipo = tile.getTipo();
        switch (tipo)
        {
        case "Hierba":
            this.MaxSpeed = 5f;
            break;
        case "Desierto":
            this.MaxSpeed = 1f;
            break;
        case "Camino":
            this.MaxSpeed = 7f;
            break;
        case "Agua":
            this.MaxSpeed = 1f;
            break;
        }
    }
}
