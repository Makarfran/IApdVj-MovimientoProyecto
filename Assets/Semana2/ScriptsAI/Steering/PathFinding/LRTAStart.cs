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
    private List<Tile> camino;

   public LRTAStart(){

  }

    private void resetEstructures(){
        hValues = new Dictionary<Tile, int>();
        tempValues = new Dictionary<Tile, int>();
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
        u.CambiarColorARojo();

        inicializarHeuristicas(goal);

        while (!T.Contains(u))
        {
            
            S = busaAnch.getEspacioLocal(gird,u,goal,maxDepth);
            ValueUpdateStep();
            bool bucleDectectado = false;
            do
            {
                List<Tile> successors = minSuccessors(u);
                u = successors[0];
                foreach (Tile successor in successors)
                {
                    if(camino.Contains(successor) || hValues[successor] == int.MaxValue){
                        bucleDectectado = true;
                        continue;
                    }
                    bucleDectectado = false;
                    u = successor;
                    break;
                }
                
                camino.Add(u);
                u.CambiarColorARojo();
                
                // bucle detectado, hay varios hijos pero el padre es el
                // mejor nodo del hijo y del hijo el mejor nodo el padre
                // se opta por volver a actualizar los valores
                // para que sus heuristicas aumenten y salga del bucle
                // en el caso de que la unica posibilidad del hijo sea 
                // volver al padre, no se fuerza a volver a actualizar hvalues.
                if(bucleDectectado && successors.Count >1){
                    break;
                }            
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
            updateSuccessors(u);
            Tile minSucc = minSuccessor(u);
            hValues[u] = Mathf.Max(tempValues[u], hValues[minSucc]);
            
            if (hValues[u] == int.MaxValue)
                return;
        }
    }


    private void updateSuccessors(Tile tile){
        List<Tile> successors = busaAnch.getNeighbors(tile);
        foreach (Tile vecino in successors)
        {
            hValues[vecino] = getFCoste(tile,vecino);
        } 
    }

    private List<Tile> minSuccessors(Tile tile){

       List<Tile> neighbors = busaAnch.getNeighbors(tile);
       
        neighbors.Sort((vecino1,vecino2) =>{            
            return hValues[vecino1].CompareTo(hValues[vecino2]);
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
        {   
            if (tile.pasable){
                int coste = calcularHCoste(tile,goal);
                hValues[tile] = coste;                
            }else{
                hValues[tile] = int.MaxValue;
            }

            
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
        if (coste == int.MaxValue && tempValues.ContainsKey(vecino)){
            coste = tempValues[vecino];
        }
        
        coste += w(tile,vecino);
        
        if (coste < 0){
            coste = int.MaxValue;
        }
        return coste;
    }
}