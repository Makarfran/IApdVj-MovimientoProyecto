using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathFinding 
{
    [SerializeField] public int costeMovimientoLineal;
    [SerializeField] Grid gird;
    [SerializeField] public  int maxDepth;
    
    private BusquedaAnchura busaAnch = new BusquedaAnchura();
    
    private Dictionary<Tile, int> hValues;
    private Dictionary<Tile, int> tempValues;
    private List<Tile> S;
    private Tile u;
    private List<Tile> T;    
    [SerializeField ]public List<Tile> camino;
    public PathFinding(){
        hValues = new Dictionary<Tile, int>();
        tempValues = new Dictionary<Tile, int>();
        S = new List<Tile>();
        T = new List<Tile>();
        this.costeMovimientoLineal = 10;
        this.maxDepth = 2;
        camino = new List<Tile>();
    }

    public List<Tile> LRTA(int startFila, int startColumna, int endFila, int endColumna){
        u = gird.getTile(startFila,startColumna);
        Tile goal = gird.getTile(endFila,endColumna); 
        T.Clear();
        T.Add(goal);
        camino.Add(u);
        inicializarHeuristicas(goal);

        while (!T.Contains(u))
        {
            S = busaAnch.getEspacioLocal(gird,u,maxDepth);
            ValueUpdateStep();

            do
            {
                Tile a = minSuccessor(u);
                u = a;
                camino.Add(u);
            }
            while (!S.Contains(u));
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
            Tile minSucc = minSuccessorInS(u);
            Tile v = (tempValues[u] >= (w(u,minSucc) + hValues[minSucc])) ? u : minSucc;

            hValues[v] = Mathf.Max(tempValues[v], ( w(v,minSuccessor(v)) + hValues[minSuccessor(v)])  );
            v.setText(hValues[v]);
            if (hValues[v] == int.MaxValue)
                return;
        }
    }

    private List<Tile> minSuccessors(Tile tile){

       List<Tile> neighbors = busaAnch.getNeighbors(tile);

       /* neighbors.Sort((vecino1,vecino2) =>{
            int cost1 = w(u, vecino1) + hValues[vecino1];
            int cost2 = w(u, vecino2) + hValues[vecino2];
            return cost1.CompareTo(cost2);
        });*/

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
            temp = vecino;
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
        
        return costeMovimientoLineal;
    }

    private void inicializarHeuristicas(Tile goal){
        Tile[,] tiles = gird.getTiles();
        foreach (Tile tile in tiles)
        {   //hValues[tile] = calcularHCoste(tile,goal);
            int coste = calcularHCoste(tile,goal);
            hValues[tile] = coste;
            tile.setText(coste);
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

}
