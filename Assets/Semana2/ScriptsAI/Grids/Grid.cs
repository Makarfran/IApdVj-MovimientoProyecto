using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid : MonoBehaviour
{
    [SerializeField] public int a;
    [SerializeField] public int b;
    [SerializeField] protected float lado;
    [SerializeField] public Tile[,] posiciones;
    public List<Tile> lista;
    public bool estaInicializado = false;
    // Start is called before the first frame update
    void Start()
    {  

        // crea una matriz axb con las casillas
        posiciones = new Tile[a,b];
        for(int i = 0; i < a ; i++){
            for(int j = 0; j < b; j++){
                GameObject a = GameObject.Find("Tile " + i + " " + j);
                posiciones[i,j] = a.GetComponent<Tile>();
                //Debug.Log(posiciones[i,j]);
                posiciones[i,j].fila = i;
                posiciones[i,j].columna = j;
                //esto se asegura que las posiciones de las casillas esten bien
                posiciones[i,j].setPos(a.transform.position);
                //Debug.Log(posiciones[i,j]);
                
            }
        }
        lista = posiciones.Cast<Tile>().ToList();
        
        estaInicializado = true;
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
        Vector3 posIni = posiciones[0,0].getPosition();
        int x = (int) ((position.x - (posIni.x-(lado/2))) / lado);
        //Debug.Log(x);
        if(x==getAncho()){
            x=x-1;
        }
        int z = (int) ((position.z - (posIni.z-(lado/2))) / lado);
        //Debug.Log(z);
        if(z==getAlto()){
            z=z-1;
        }
        Tile tile = posiciones[x,z];
        if(tile.isPasable()){
            return tile;
        }

        tile = lista
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

    public void setA(int a){
        this.a = a;
    }
    public void setB(int b){
        this.b = b;
    }
    public void setLado(float b){
        this.lado = b;
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

    public bool IsPositionInsideRectangle(Vector3 position)
    {   
        Vector3 a = getTilePosition(0,0);
        Vector3 b = getTilePosition(0,getAlto()-1);
        Vector3 c = getTilePosition(getAncho()-1,getAlto()-1);
        Vector3 d = getTilePosition(getAncho()-1,0);
        // Calculate vectors
        Vector3 BC = c - b;
        Vector3 BM = position - b;
        Vector3 DA = a - d;
        Vector3 DM = position - d;

        // Calculate dot products
        float BCBC = Vector3.Dot(BC, BC);
        float BCBM = Vector3.Dot(BC, BM);
        float DADA = Vector3.Dot(DA, DA);
        float DADM = Vector3.Dot(DA, DM);

        // Check if position is inside rectangle
        return 0 <= BCBM && BCBM <= BCBC && 0 <= DADM && DADM <= DADA;
    }


    

}
