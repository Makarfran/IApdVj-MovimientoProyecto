using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public bool pasable = true;
    public Vector3 pos;
    public int coste;

    public Tile(Vector3 vec){
        this.pos = vec;
    }

    
}
