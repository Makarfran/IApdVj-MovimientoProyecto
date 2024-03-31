using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinding : MonoBehaviour
{
    [SerializeField] public int costeMovimientoLineal;
    [SerializeField] Grid gird;
    [SerializeField] public  int maxDepth;
    private LRTAStart lrta;   
 

    void Start(){
        lrta = new LRTAStart();
        lrta.setGrid(gird);
        lrta.costeMovimientoLineal = costeMovimientoLineal;
        lrta.maxDepth = maxDepth;
    }

    void Update(){



    }

    public void NewTarget(Vector3 newTarget){
        Tile goal = gird.getTileByVector(newTarget);
        Tile start = gird.getTileByVector(transform.position);
       
        Debug.Log("start: "+start.fila+" "+start.columna);
        Debug.Log("newTarget: " + goal.fila + " " + goal.columna);

        List<Tile> camino = lrta.run(start, goal);
        //List<Tile> camino = lrta.run(8, 9, 6, 9);
        PathBasico path = GetComponent<PathBasico>();
        path.setObjetvosFromTiles(camino);
        PathFollowing pathFollowing = GetComponent<PathFollowing>();
        pathFollowing.setObjetivoInicial();

    }



}