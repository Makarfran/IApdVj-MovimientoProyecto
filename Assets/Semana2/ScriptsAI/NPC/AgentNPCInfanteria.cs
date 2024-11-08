using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AgentNPCInfanteria : AgentNPC
{
    

    protected override void Start()
    {
        base.Start();
        this.vida = 8;
        this.maxVida = 8;
        this.atq = 2;
        this.range = 7f;
        
    }
    
    // Start is called before the first frame update

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(grid != null){
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
}
