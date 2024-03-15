using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool pasable = true;
    public Vector3 pos;
    public int coste;

    

    public void setPos(Vector3 pos){
        this.pos = pos;
    }

    public Vector3 getPosition(){
        return pos;
    }
}
