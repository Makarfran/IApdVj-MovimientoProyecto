using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] protected int a;
    [SerializeField] protected int b;
    [SerializeField] protected float lado;
    protected Tile[,] posiciones;
    
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
                posiciones[i,j].setPos(new Vector3(this.transform.position.x + i *lado, 0, this.transform.position.z + j*lado));

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
        Tile tileInicial = getTile(0,0);
        float diffX = position.x - tileInicial.pos.x;
        float diffY = position.z - tileInicial.pos.z;
        int  coorX = Mathf.FloorToInt(diffX / 3f);
        int coorY = Mathf.FloorToInt(diffY / 3f);

        return getTile(coorX, coorY);

    }

    public int getAlto(){
        return a;
    }

    public int getAncho(){
        return b;
    }
}
