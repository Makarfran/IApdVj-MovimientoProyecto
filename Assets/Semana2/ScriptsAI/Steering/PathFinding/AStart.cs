using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStart
{
    [SerializeField] int costeMovimientoLineal;
    [SerializeField] Grid gird;
    private List<Tile> abiertos;
    private List<Tile> cerrados;
    public AStart(){

    }


    public List<Tile> buscarCamino(int startFila, int startColumna, int endFila, int endColumna){

        // inicialización de valores g y h de Tiles
        for(int i = 0; i < gird.getAlto(); i++){
            for (int j = 0; j < gird.getAncho(); j++){
                Tile tile = gird.getTile(i,j);

                tile.tilePadre = null;
                tile.gCoste = int.MaxValue;
                tile.calcularFCoste();
                
            }
        }


        // inicialización de conjuntos abiertos y cerrados
        abiertos = new List<Tile>();
        cerrados= new List<Tile>();

        Tile startTile = gird.getTile(startFila,startColumna);
        Tile endTile = gird.getTile(endFila, endColumna);
        startTile.gCoste = 0;
        startTile.hCoste = calcularHCoste(startTile, endTile);
        startTile.calcularFCoste();
        abiertos.Add(startTile);


        //bucle A*
        while(abiertos.Count > 0){
            Tile currentTile = getTileMenorF(abiertos);
            // si hemos alcanzado al objetivo
            if(currentTile == endTile){
                return getSolucion(currentTile);
            }

            // si no, Expandir nodo 
            abiertos.Remove(currentTile);
            cerrados.Add(currentTile);
            
            foreach (Tile vecino in getVecinos(currentTile))
            {
                if(cerrados.Contains(vecino)){
                    continue;
                }

                if(!vecino.pasable){
                    cerrados.Add(vecino);
                }

                //poda
                int gCoste = currentTile.gCoste + calcularHCoste(currentTile,vecino);
                if( gCoste < vecino.gCoste){
                    vecino.tilePadre = currentTile;
                    vecino.gCoste = gCoste;
                    vecino.hCoste = calcularHCoste(currentTile,vecino);
                    vecino.calcularFCoste();

                    if(!abiertos.Contains(vecino)){
                        abiertos.Add(vecino);
                    }
                }
            }
        }

        // explorado todo el espacio y no se ha encontrado el objetivo
        return null;
    }

    private List<Tile> getVecinos(Tile tile){
        List<Tile> vecinos = new List<Tile>();

        // izquierda
        if(tile.columna -1 >= 0){
            vecinos.Add(gird.getTile(tile.fila, tile.columna -1));
        }

        // derecha
        if(tile.columna + 1 < gird.getAncho()){
            vecinos.Add(gird.getTile(tile.fila, tile.columna + 1));
        }

        // arriba

        if(tile.fila - 1 >= 0){
            vecinos.Add(gird.getTile(tile.fila -1, tile.columna));
        }

        //abajo
        if(tile.fila +  1 < gird.getAlto()){
            vecinos.Add(gird.getTile(tile.fila + 1,tile.columna));
        }

        return vecinos;
    }

    private List<Tile> getSolucion(Tile tile){
        List<Tile> solucion = new List<Tile>();
        Tile currentTile = tile;
        
        while(currentTile.tilePadre != null){
            solucion.Add(currentTile);
            currentTile = currentTile.tilePadre;
        }
        // debería ser startTile
        solucion.Add(currentTile);
        solucion.Reverse();
        return solucion;
    }

    // Busca el estado de menor costo entre lista de abiertos.
    private Tile getTileMenorF(List<Tile> tileList){
        Tile tileMenorCosto = tileList[0];
        for(int i = 1; i < tileList.Count;i++){
            if(tileList[i].fCoste < tileMenorCosto.fCoste){
                tileMenorCosto = tileList[i];
            }
        }
        return tileMenorCosto;
    }

    // distancia Manhattan 
    private int calcularHCoste(Tile current, Tile goal){
        int distanciaX = Mathf.Abs(current.columna -  goal.columna);
        int distanciaY = Mathf.Abs(current.fila - goal.fila);

        return (costeMovimientoLineal * (distanciaX + distanciaY));
    }

}
