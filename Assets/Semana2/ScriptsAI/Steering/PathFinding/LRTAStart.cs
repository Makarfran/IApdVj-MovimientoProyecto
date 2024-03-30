using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LRTAStart 
{
    public int costeMovimientoLineal;
    public Grid gird;
    public  int maxDepth;
    
    private BusquedaAnchura busaAnch = new BusquedaAnchura();

    
    private Dictionary<Tile, int> hValues;
    private Dictionary<Tile, int> tempValues;
    private List<Tile> S;
    private Tile u;
    private List<Tile> T;    
    [SerializeField ]public List<Tile> camino;

/*
    private bool buscar = false;
    void Start(){

    }

    void Update(){

        if(buscar){
            List<Tile> camino = LRTA(0,0,2,3);
            buscar = false;
        }

    }
*/

   public LRTAStart(){
        hValues = new Dictionary<Tile, int>();
        tempValues = new Dictionary<Tile, int>();
        S = new List<Tile>();
        T = new List<Tile>();
        camino = new List<Tile>();
  }

    public List<Tile> run(int startFila, int startColumna, int endFila, int endColumna){
        u = gird.getTile(startFila,startColumna);
        Tile goal = gird.getTile(endFila,endColumna); 
        T.Clear();
        T.Add(goal);
        camino.Add(u);
        u.CambiarColorARojo();

        inicializarHeuristicas(goal);

        while (!T.Contains(u))
        {
            
            S = busaAnch.getEspacioLocal(gird,u,goal,maxDepth);
            ValueUpdateStep();
            
            do
            {
                foreach (Tile successor in minSuccessors(u))
                {
                    if(camino.Contains(successor)){
                        continue;
                    }
                    u = successor;
                    break;
                }
                camino.Add(u);
                u.CambiarColorARojo();
            }
            while (S.Contains(u));
        }
        return camino;
    }

    private void ValueUpdateStep()
    {
        foreach (Tile u in S)
        {
            tempValues[u] = hValues[u];
            hValues[u] = int.MaxValue;
        }

        while (S.Any(u => hValues[u] == int.MaxValue))
        {
            Tile u = S.First(u => hValues[u] == int.MaxValue);
            Tile minSucc = minSuccessor(u);
            hValues[u] = Mathf.Max(tempValues[u], getFCoste(u,minSucc));
            //u.setText(hValues[u]);
            if (hValues[u] == int.MaxValue)
                return;
        }
    }

    private List<Tile> minSuccessors(Tile tile){

       List<Tile> neighbors = busaAnch.getNeighbors(tile);

        neighbors.Sort((vecino1,vecino2) =>{
            int cost1 = getFCoste(tile,vecino1);
            int cost2 = getFCoste(tile,vecino2);
            
            return cost1.CompareTo(cost2);
        });

        // Devuelve el vecino seleccionado
        return neighbors;

    }

    private Tile minSuccessorInS(Tile tile){
        List<Tile> neighbors = minSuccessors(tile);
        Tile temp = neighbors[0];
        foreach (Tile vecino in neighbors)
        {
            if(!S.Contains(vecino)){
                continue;
            }
            return vecino;
        }
        return temp;
    }
    private Tile minSuccessor(Tile tile)
    {
        List<Tile> neighbors = minSuccessors(tile);
        return neighbors[0]; 
 
    }


    private int w(Tile u, Tile v)
    {
        if (u == v){
            return 0;
        }
        return costeMovimientoLineal;
    }

    public void inicializarHeuristicas(Tile goal){
        Tile[,] tiles = gird.getTiles();
        foreach (Tile tile in tiles)
        {   //hValues[tile] = calcularHCoste(tile,goal);
            int coste = calcularHCoste(tile,goal);
            hValues[tile] = coste;
            //tile.setText(coste);
        }
    }

    // distancia Manhattan 
    private int calcularHCoste(Tile current, Tile goal){
        int distanciaX = Mathf.Abs(current.columna -  goal.columna);
        int distanciaY = Mathf.Abs(current.fila - goal.fila);

        return (costeMovimientoLineal * (distanciaX + distanciaY));
    }

    public void setGrid(Grid gird){
        this.gird = gird;
    }

    private int getFCoste(Tile tile, Tile vecino){

        int coste = hValues[vecino];
        if (coste == int.MaxValue){
            coste = tempValues[vecino];
        }
        
        coste += w(tile,vecino);
        
        if (coste < 0){
            coste = int.MaxValue;
        }
        return coste;
    }
}