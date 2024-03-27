using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esto es mas para generar grids que otra cosa
public class GridGenerator : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] protected int a;
    [SerializeField] protected int b;
    [SerializeField] protected float longLado;
    [SerializeField] protected GameObject obj;
    protected Tile[,] posiciones;
    [SerializeField] protected List<Vector2> impasables;
    [SerializeField] protected List<Vector2>  coste2;
    [SerializeField] protected List<Vector2>  coste3;

    void Start(){
        posiciones = new Tile[a,b];
        for(int i = 0; i < a; i++ ){ 
            for(int j = 0; j < b ; j++ ){
                Vector3 pos = new Vector3(this.transform.position.x + i*longLado, this.transform.position.y, this.transform.position.z  + j*longLado);
                var spanw = Instantiate(obj, pos, Quaternion.identity);
                spanw.name = $"Tile {i} {j}";
                spanw.GetComponent<Tile>().setPos(pos);
                
            }
        }
    }  

    
}