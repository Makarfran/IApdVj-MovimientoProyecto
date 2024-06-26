using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid : MonoBehaviour
{
    [SerializeField] protected int a;
    [SerializeField] protected int b;
    [SerializeField] protected float lado;
    [SerializeField] public Tile[,] posiciones;

    // Start is called before the first frame update
    void Start()
    {  

        // crea una matriz axb con las casillas
        posiciones = new Tile[a,b];
        for(int i = 0; i < a ; i++){
            for(int j = 0; j < b; j++){
                GameObject a = GameObject.Find("Tile " + i + " " + j);
                posiciones[i,j] = a.GetComponent<Tile>();
                
                posiciones[i,j].fila = i;
                posiciones[i,j].columna = j;
                //esto se asegura que las posiciones de las casillas esten bien
                posiciones[i,j].setPos(a.transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   

    }

    public Vector3 getTilePosition(int x, int y){
        return posiciones[x,y].getPosition();
    }

    public Tile getTile(int x, int y){
        return posiciones[x,y];
    }

    public Tile getTileByVector(Vector3 position){

        List<Tile> lista = posiciones.Cast<Tile>().ToList();

        Tile tile = lista
            .Where(o => o.pasable)
            .Aggregate((o1, o2) => Vector3.Distance(o1.getPosition(), position) < Vector3.Distance(o2.getPosition(), position) ? o1 : o2);
        return tile;

    }

    public int getAlto(){
        return b;
    }

    public int getAncho(){
        return a;
    }

    public Tile[,] getTiles(){
        return posiciones;
    }

    public void ActivateGrid(){
        for(int i = 0; i < a ; i++){
            for(int j = 0; j < b; j++){
                
                posiciones[i,j].GetComponent<MeshRenderer>().enabled = true;
                
            }
        }
    }

    public void DeactivateGrid(){
        for(int i = 0; i < a ; i++){
            for(int j = 0; j < b; j++){
                
                posiciones[i,j].GetComponent<MeshRenderer>().enabled = false;
                
            }
        }
    }

    

}
