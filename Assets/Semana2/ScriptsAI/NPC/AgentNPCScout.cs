using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentNPCScout : AgentNPC
{
    [SerializeField] Grid grid;
    
    // Start is called before the first frame update
    

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        
        Tile tile = grid.getTileByVector(this.transform.position);
        String tipo = tile.getTipo();
        switch (tipo)
        {
        case "Hierba":
            this.MaxSpeed = 6f;
            break;
        case "Desierto":
            this.MaxSpeed = 4f;
            break;
        case "Camino":
            this.MaxSpeed = 12f;
            break;
        case "Agua":
            this.MaxSpeed = 1f;
            break;
        }
    }
}
