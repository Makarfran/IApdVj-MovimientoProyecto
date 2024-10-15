using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LRTAStart : MonoBehaviour 
{
    [SerializeField] public int costeMovimientoLineal;
    [SerializeField] public Grid gird;
    [SerializeField] public int  maxDepth;
    
    private BusquedaAnchura busaAnch = new BusquedaAnchura();
    
    private Dictionary<Tile, int> hValues;
    private Dictionary<Tile, int> tempValues;
    private Dictionary<Tile, int> AuxtempValues;
    private List<Tile> S;
    private Tile u;
    private List<Tile> T;   
    private List<Tile> camino;

   public LRTAStart(){
    //resetEstructures();
  }

    private void resetEstructures(){
        hValues = new Dictionary<Tile, int>();
        tempValues = new Dictionary<Tile, int>();
        AuxtempValues = new Dictionary<Tile, int>();
        S = new List<Tile>();
        T = new List<Tile>();
        camino = new List<Tile>();        
    }

    public List<Tile> run(Tile startTile, Tile endTile){
        
        resetEstructures();
        u = startTile;
        Tile goal = endTile; 
        T.Clear();
        T.Add(goal);
        camino.Add(u);
        u.CambiarColorVerde();

        inicializarHeuristicas(goal);

       while (!T.Contains(u) )
        {
            S = busaAnch.getEspacioLocal(gird,u,goal,maxDepth);
            ValueUpdateStep();

            do
            {   
                if (T.Contains(u)){
                    Debug.Log("mal! se ha detectado goal dentro del espacio de bsuqueda");
                    break;
                }
                Tile a = minSuccessor(u);
                a.CambiarColorVerde();
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
            calcularMejorVecinoLocal();
            Tile u = getBestVecino();
            if (u == null){ // not improvement possible
                continue;
            }

            hValues[u] = AuxtempValues[u];
        }
    }

    private Tile getBestVecino(){
        int cost = int.MaxValue;
        Tile bestVecino = null;
        foreach (var vecino in AuxtempValues){
            if (hValues[vecino.Key] == int.MaxValue && vecino.Value < cost){
                cost = vecino.Value;
                bestVecino = vecino.Key;
            }
        }
        return bestVecino;
    }
    private void calcularMejorVecinoLocal(){
        AuxtempValues.Clear();
        foreach(Tile tile in S){
            List<Tile> neighbors = busaAnch.getNeighbors(tile);
            int bestCost = int.MaxValue;
            
            foreach(Tile neighbor in neighbors){
                if (hValues[neighbor] == int.MaxValue){
                    continue;
                }
                int realCost = w(tile,neighbor) + hValues[neighbor];
                if (realCost < bestCost){
                    bestCost = realCost;
                }
            }
            
            AuxtempValues[tile] = Mathf.Max(tempValues[tile],bestCost);
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
        if (u == v){
            return 0;
        }
        return costeMovimientoLineal;
    }

    public void inicializarHeuristicas(Tile goal){
        Tile[,] tiles = gird.getTiles();
        foreach (Tile tile in tiles)
        {   
            
            int coste = calcularHCoste(tile,goal);
            hValues[tile] = coste;                
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
