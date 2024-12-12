using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentNPCScout : AgentNPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        this.vida = 3000;
        this.maxVida = 3000;
        this.atq = 75;
        this.range = 8f;
        this.tipoUnidad = "Scout";
        this.respawnTime = 15;

    }
    

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


    public override float getGCosteWeightCamino(Tile tile){
        switch (tile.getTipo())
        {
            case "Hierba":
                return 0.8f;

            case "Desierto":
                return 1f;

            case "Camino":
                return 0.5f;
                
            case "Agua":
                return 1.5f;
                
            default:
                return 1f;
        }
    }

    public override (float,float,float,float ) getFactorInfluencia(){
        // comportamiento evidentemente evasivo, siempre elige el camino
        // gobernado por su equipo!
        return (0.1f, 0.3f, 1.20f, 3f);
    }

    public override bool vidaBaja()
    {
        if (vida / maxVida <= 0.3f)
        {
            return true;
        }
        return false;
    }
}
