using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusquedaAnchura
{
    

    private Queue<Tile> cerrados = new Queue<Tile>();
    // Matriz para marcar los nodos visitados
    private List <Tile> visitados = new List<Tile>();      
    private Queue<int> depthQueue = new Queue<int>();
    private List<Tile> vecinos = new List<Tile>();
    private List<Tile> espacioLocal = new List<Tile>();

    private Grid gird;
    private Tile endTile;
    /*
    Obtiene el espacio local de busqueda por busqueda en anchura.
    */

    public BusquedaAnchura(){
    }

    public List<Tile> getEspacioLocal(Grid gird, Tile startTile, Tile endTile, int depth)
    {
        this.gird = gird;
        this.endTile = endTile;
        espacioLocal.Clear();
        cerrados.Clear();
        depthQueue.Clear();
        visitados.Clear();
        // Marcar el nodo actual como visitado y encolarlo con su nivel
        cerrados.Enqueue(startTile);
        depthQueue.Enqueue(0);
        visitados.Add(startTile);
        espacioLocal.Add(startTile);

        while (cerrados.Count != 0)
        {
            // Sacar un nodo de la cola e imprimirlo si está dentro del límite de profundidad
            Tile currentTile = cerrados.Dequeue();
            int currentDepth = depthQueue.Dequeue();
            if (currentDepth >= depth){
                continue;
            }
            // Obtener los vecinos del nodo actual y encolarlos si no han sido visitados
            getVecinosNoVisitados(currentTile, currentDepth +1);
        }
        return espacioLocal;
    }


    private void getVecinosNoVisitados( Tile tile, int depth)
    {
        getVecinos(tile);
        foreach(Tile vecino in vecinos){
            comprobarVecino(depth,vecino);
        }
    }

    public List<Tile> getNeighbors(Tile tile){
        getVecinos(tile);
        return vecinos;
    }
    private void getVecinos(Tile tile){
        vecinos.Clear();
        getVecino(tile.fila - 1, tile.columna); // vecino izquierdo
        getVecino(tile.fila + 1, tile.columna); // Vecino derecho
        getVecino(tile.fila, tile.columna - 1); // Vecino arriba
        getVecino(tile.fila, tile.columna + 1); // Vecino abajo   
    }    

    private void getVecino(int fila, int columna){
        
        
        if(isCeldaValida(fila,columna) ){
            
            Tile vecino = gird.getTile(fila,columna);    
            if (vecino.pasable){
                vecinos.Add(vecino);
            }
            
        }
        
    }

    private bool isCeldaValida(int fila, int columna){
        return (fila >= 0 && fila < gird.a && columna >= 0 && columna < gird.b);
    }

    private void comprobarVecino( int depth, Tile vecino)
    {
        if ( vecino == endTile || !isCeldaValida(vecino.fila,vecino.columna)){
            return;
        }

        if (!visitados.Contains(vecino))
        {
            cerrados.Enqueue(vecino);
            depthQueue.Enqueue(depth);
            visitados.Add(vecino); // Marcar como visitado
            espacioLocal.Add(vecino);
        }
    }    


}
