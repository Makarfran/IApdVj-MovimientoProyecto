using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AgentNPCElite : AgentNPC
{


    protected override void Start()
    {
        base.Start();
        this.vida = 12;
        this.maxVida = 12;
        this.atq = 3;
        this.range = 2f;

    }
    // Start is called before the first frame update


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (grid != null)
        {
            Tile tile = grid.getTileByVector(this.transform.position);
            String tipo = tile.getTipo();
            switch (tipo)
            {
                case "Hierba":
                    this.MaxSpeed = 4f;
                    this.MaxAcceleration = 3f;
                    break;
                case "Desierto":
                    this.MaxSpeed = 4f;
                    this.MaxAcceleration = 3f;
                    break;
                case "Camino":
                    this.MaxSpeed = 7f;
                    this.MaxAcceleration = 4f;
                    break;
                case "Agua":
                    this.MaxSpeed = 1f;
                    this.MaxAcceleration = 0.1f;
                    break;
            }
        }

    }


    public override float getGCosteWeightCamino(Tile tile)
    {
        switch (tile.getTipo())
        {
            case "Hierba":
            case "Desierto":
                return 1f;

            case "Camino":
                return 0.7f;
                
            case "Agua":
                return 1.5f;
                
            default:
                return 1f;
        }
    }
}
