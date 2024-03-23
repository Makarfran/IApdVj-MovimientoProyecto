using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding 
{
    [SerializeField] int costeMovimientoLineal;
    [SerializeField] Grid gird;

    Queue<Tile> cerrados = new Queue<Tile>();
    // Matriz para marcar los nodos visitados
    List <Tile> visitados = new List<Tile>();  
    Queue<int> depthQueue = new Queue<int>();
    List<Tile> espacioLocal = new List<Tile>();
    public PathFinding(){}


    public void BFS(Tile startTile, int depth)
    {

        espacioLocal.Clear();
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
    }

    private void getVecinosNoVisitados( Tile tile, int depth)
    {
        comprobarVecino( depth, tile.fila - 1, tile.columna); // Vecino izquierdo
        comprobarVecino( depth, tile.fila + 1, tile.columna); // Vecino derecho
        comprobarVecino( depth, tile.fila, tile.columna - 1); // Vecino arriba
        comprobarVecino( depth, tile.fila, tile.columna + 1); // Vecino abajo
    }

    private void comprobarVecino( int depth, int x, int y)
    {
        if (!(x >= 0 && x < gird.getAncho() && y >= 0 && y < gird.getAlto())){
            return;
        }
        Tile vecino = gird.getTile(x,y);
        if (!visitados.Contains(vecino))
        {
            cerrados.Enqueue(vecino);
            depthQueue.Enqueue(depth);
            visitados.Add(vecino); // Marcar como visitado
            espacioLocal.Add(vecino);
        }
    }


}
