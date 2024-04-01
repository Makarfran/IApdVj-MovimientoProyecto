using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinding : MonoBehaviour
{
    [SerializeField] public int costeMovimientoLineal;
    [SerializeField] Grid gird;
    private int maxDepth;
    private LRTAStart lrta;   
 

    void Start(){
        lrta = new LRTAStart();
        lrta.setGrid(gird);
        lrta.costeMovimientoLineal = costeMovimientoLineal;
        lrta.maxDepth = 1;
    }

    void Update(){



    }

    public void NewTarget(Vector3 newTarget){
        Tile goal = gird.getTileByVector(newTarget);
        Tile start = gird.getTileByVector(transform.position);
        
        
        List<Tile> camino = lrta.run(start, goal);

        PathBasico path = GetComponent<PathBasico>();
        path.setObjetvosFromTiles(camino);
        PathFollowing pathFollowing = GetComponent<PathFollowing>();
        pathFollowing.setObjetivoInicial();

    }



}