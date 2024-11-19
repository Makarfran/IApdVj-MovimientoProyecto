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

    public override (float,float,float,float ) getFactorInfluencia(){
        // actitud camorrista, más peso a ir por el camino de inlfuencia enemiga

       return ( 1.10f, 2f, 0.2f, 0.6f);
    } 
    
    public override float CalcularFactorModificado(float factorActual, float influenciaNeta)
    {
        var factoresInfluencia = getFactorInfluencia();
        float minMejora = factoresInfluencia.Item1;
        float maxMejora = factoresInfluencia.Item2;

        float minEmpeoramiento = factoresInfluencia.Item3;
        float maxEmpeoramiento = factoresInfluencia.Item4;        

        float maxInfluencia = InfluenceManager.Instance.maxInfluence;
        float minInfluencia = 0f;
        float newFactor = factorActual;

        if (minMejora == 0 && maxMejora == 0 && minEmpeoramiento == 0 && maxEmpeoramiento == 0  ){
            return factorActual;
        } 

        // mejoro el porcentaje de coste
        if (influenciaNeta < 0 )
        {
            
            newFactor = factorActual *  ( 1 - Map( -influenciaNeta, minInfluencia,maxInfluencia, minEmpeoramiento, maxEmpeoramiento));
        }
        // empeoro el porcenaje de coste
        else if (influenciaNeta > 0)
        {
            
            newFactor = (factorActual * Map(influenciaNeta, minInfluencia,maxInfluencia, minMejora, maxMejora));
        }

        return newFactor;
    }  
          
}
