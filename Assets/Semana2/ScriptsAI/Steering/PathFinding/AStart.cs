using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStart
{
    [SerializeField] public int costeMovimientoLineal;
    [SerializeField] public Grid gird;
    private List<Tile> abiertos;
    private List<Tile> cerrados;
    private AgentNPC agent;
    public int costConnection = 100;
    public AStart()
    {

    }


    public List<Tile> buscarCamino(Tile startTile, Tile endTile)
    {

        // inicialización de valores g y h de Tiles
        for (int i = 0; i < gird.a; i++)
        {
            for (int j = 0; j < gird.b; j++)
            {
                Tile tile = gird.getTile(i, j);
                tile.tilePadre = null;
            }
        }


        // inicialización de conjuntos abiertos y cerrados
        abiertos = new List<Tile>();
        cerrados = new List<Tile>();

        startTile.gCoste = 0;
        startTile.hCoste = calcularHCoste(startTile, endTile);
        startTile.calcularFCoste();

        abiertos.Add(startTile);


        //bucle A*
        while (abiertos.Count > 0)
        {
            Tile currentTile = getTileMenorF(abiertos);
            // si hemos alcanzado al objetivo
            if (currentTile == endTile)
            {
                return getSolucion(currentTile);
            }

            // si no, Expandir nodo 
            abiertos.Remove(currentTile);
            cerrados.Add(currentTile);

            foreach (Tile vecino in getVecinos(currentTile))
            {

                if (!vecino.pasable)
                {   
                    cerrados.Add(vecino);
                    continue;
                }                


                float gcoste = currentTile.gCoste + getGCoste(vecino);

                if (cerrados.Contains(vecino))
                {
                    if (vecino.gCoste <= gcoste)
                    {
                        continue;
                    }
                    cerrados.Remove(vecino);
                }
                else if (abiertos.Contains(vecino))
                {
                    if (vecino.gCoste <= gcoste)
                    {
                        continue;
                    }

                }

                vecino.gCoste = gcoste;
                vecino.hCoste = calcularHCoste(vecino, endTile);
                vecino.calcularFCoste();
                vecino.tilePadre = currentTile;

                if (!abiertos.Contains(vecino)){
                    abiertos.Add(vecino);
                }                
            }
        }

        // explorado todo el espacio y no se ha encontrado el objetivo
        return null;
    }

    private List<Tile> getVecinos(Tile tile)
    {
        List<Tile> vecinos = new List<Tile>();

        // izquierda
        if (tile.columna - 1 >= 0)
        {
            vecinos.Add(gird.getTile(tile.fila, tile.columna - 1));
        }

        // derecha
        if (tile.columna + 1 < gird.b)
        {
            vecinos.Add(gird.getTile(tile.fila, tile.columna + 1));
        }

        // arriba

        if (tile.fila - 1 >= 0)
        {
            vecinos.Add(gird.getTile(tile.fila - 1, tile.columna));
        }

        //abajo
        if (tile.fila + 1 < gird.a)
        {
            vecinos.Add(gird.getTile(tile.fila + 1, tile.columna));
        }

        return vecinos.Where(tile => tile.pasable).ToList();
    }

    private List<Tile> getSolucion(Tile tile)
    {
        List<Tile> solucion = new List<Tile>();
        Tile currentTile = tile;

        while (currentTile.tilePadre != null)
        {
            currentTile.CambiarColorVerde();
            solucion.Add(currentTile);
            currentTile = currentTile.tilePadre;
        }
        // debería ser startTile
        currentTile.CambiarColorVerde();
        solucion.Add(currentTile);
        solucion.Reverse();
        return solucion;
    }

    // Busca el estado de menor costo entre lista de abiertos.
    private Tile getTileMenorF(List<Tile> tileList)
    {
        Tile tileMenorCosto = tileList[0];
        for (int i = 1; i < tileList.Count; i++)
        {
            if (tileList[i].fCoste < tileMenorCosto.fCoste)
            {
                tileMenorCosto = tileList[i];
            }
        }
        return tileMenorCosto;
    }

    // distancia Manhattan 
    private int calcularHCoste(Tile current, Tile goal)
    {
        int distanciaX = Mathf.Abs(current.columna - goal.columna);
        int distanciaY = Mathf.Abs(current.fila - goal.fila);

        return (costeMovimientoLineal * (distanciaX + distanciaY));
    }

    public void setGrid(Grid gird)
    {
        this.gird = gird;
    }

    public void setAgent(AgentNPC agent)
    {
        this.agent = agent;
    }

    private float getGCoste(Tile tile){
        return (agent.getGCosteWeight(tile) * costConnection);
    }
}
