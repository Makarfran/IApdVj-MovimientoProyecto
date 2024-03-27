using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathFinding 
{
    [SerializeField] int costeMovimientoLineal;
    [SerializeField] Grid gird;
    [SerializeField] int maxDepth;
    
    private BusquedaAnchura busaAnch = new BusquedaAnchura();
    
    private Dictionary<Tile, int> hValues;
    private Dictionary<Tile, int> tempValues;
    private List<Tile> S;
    private Tile u;
    private List<Tile> T;    
    
    public PathFinding(){
        hValues = new Dictionary<Tile, int>();
        tempValues = new Dictionary<Tile, int>();
        S = new List<Tile>();

    }

    public void LRTA(int startFila, int startColumna, int endFila, int endColumna){
        u = gird.getTile(startFila,startColumna);
        Tile goal = gird.getTile(endFila,endColumna); 
        T.Clear();
        T.Add(goal);
        inicializarHeuristicas(goal);

        while (!T.Contains(u))
        {
            S = busaAnch.getEspacioLocal(gird,u,maxDepth);
            ValueUpdateStep();

            do
            {
                Tile a = minSuccessor(u);
                u = a;
            }
            while (!S.Contains(u));
        }
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
            Tile v = (tempValues[u] >= (w(u,minSucc) + hValues[minSucc])) ? u : minSucc;

            hValues[v] = Mathf.Max(tempValues[v], ( w(v,minSuccessor(v)) + hValues[minSuccessor(v)])  );

            if (hValues[v] == int.MaxValue)
                return;
        }
    }

    private Tile minSuccessor(Tile tile)
    {
        
        List<Tile> neighbors = busaAnch.getNeighbors(tile);

        
        Tile selectedNeighbor = null;
        int minCost = int.MaxValue;

        
        foreach (Tile neighbor in neighbors)
        {
            // Calcula el costo de ir de 'u' al vecino 'a'
            int cost = w(u, neighbor) + hValues[neighbor];

            // Si el costo es menor que el costo mínimo actual, actualiza el vecino seleccionado y el costo mínimo
            if (cost < minCost)
            {
                selectedNeighbor = neighbor;
                minCost = cost;
            }
        }

        // Devuelve el vecino seleccionado
        return selectedNeighbor;
    }


    private int w(Tile u, Tile v)
    {
        
        return costeMovimientoLineal;
    }






    private void inicializarHeuristicas(Tile goal){
        Tile[,] tiles = gird.getTiles();
        foreach (Tile tile in tiles)
        {
            hValues[tile] = calcularHCoste(tile,goal);
        }
    }

    // distancia Manhattan 
    private int calcularHCoste(Tile current, Tile goal){
        int distanciaX = Mathf.Abs(current.columna -  goal.columna);
        int distanciaY = Mathf.Abs(current.fila - goal.fila);

        return (costeMovimientoLineal * (distanciaX + distanciaY));
    }

}
