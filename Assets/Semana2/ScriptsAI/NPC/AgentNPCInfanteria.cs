using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AgentNPCInfanteria : AgentNPC
{
    

    protected override void Start()
    {
        base.Start();
        this.vida = 7500;
        this.maxVida = 7500;
        this.atq = 100;
        this.range = 2f;
        this.tipoUnidad = "Infanteria";
        this.respawnTime = 15;

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

    public override float getGCosteWeightCamino(Tile tile)
    {
        switch (tile.getTipo())
        {
            case "Hierba":
            return 1f;

            case "Camino":
                return 0.5f;

            case "Desierto":                
            case "Agua":
                return 1.5f;
            default:
                return 1f;
        }
    }    

    public override (float,float,float,float ) getFactorInfluencia(){
        // prefiere el camino más corto a la base, la influencia no se usa.

        return (0f, 0f,0f,0f);
    }

    public override bool vidaBaja()
    {
        if (vida / maxVida <= 0.2f)
        {
            return true;
        }
        return false;
    }
}
