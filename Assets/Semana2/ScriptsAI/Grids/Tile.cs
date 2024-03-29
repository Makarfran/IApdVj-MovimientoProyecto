using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Tile : MonoBehaviour
{
    public int fila,columna;
    public bool pasable;
    public Vector3 pos;
    public int gCoste;
    public int hCoste;
    public int tempHCoste;
    public int fCoste;
    public Tile tilePadre;
    public String tipo;

    public Text textComponent;

    public String getTipo(){
        return tipo;
    }

    public void calcularFCoste(){
        fCoste = gCoste + hCoste;

    }
    public void setPos(Vector3 pos){
        this.pos = pos;
    }

    public Vector3 getPosition(){
        return pos;
    }

    public void setText(int text){
        textComponent.text = text.ToString();
    }
}
