using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Grid : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] protected int a;
    [SerializeField] protected int b;
    [SerializeField] protected float longLado;
    protected Tile[,] posiciones;
    [SerializeField] protected List<Vector2> impasables;
    [SerializeField] protected List<Vector2>  coste2;
    [SerializeField] protected List<Vector2>  coste3;

    void Start(){
        posiciones = new Tile[a,b];
        for(int i = 0; i < a; i++ ){ 
            for(int j = 0; j < b ; j++ ){
                
                
                Vector3 pos = new Vector3(this.transform.position.x + longLado/2f + i*longLado, 0, this.transform.position.z + longLado/2f + j*longLado);
                posiciones[i,j] = new Tile(pos);
                
            }
        }
    }  

    void Update(){
        
    }
}