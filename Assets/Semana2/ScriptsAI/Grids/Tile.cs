using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Tile : MonoBehaviour
{
    public int fila,columna;
    public bool pasable = true;
    public Vector3 pos;
    public int gCoste;
    public int hCoste;
    public int tempHCoste;
    public int fCoste;
    public Tile tilePadre;
    


    public void calcularFCoste(){
        fCoste = gCoste + hCoste;

    }
    public void setPos(Vector3 pos){
        this.pos = pos;
    }

    public Vector3 getPosition(){
        return pos;
    }
}
