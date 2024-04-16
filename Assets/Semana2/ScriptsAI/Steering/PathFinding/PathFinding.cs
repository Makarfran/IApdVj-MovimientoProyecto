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
    private List<Tile> camino;
    private int posCamino;

    void Start(){
        camino = new List<Tile>();
        lrta = new LRTAStart();
        lrta.setGrid(gird);
        lrta.costeMovimientoLineal = costeMovimientoLineal;
        lrta.maxDepth = 1;
    }

    void Update()
    {
        
        if (camino.Count > 0)
        {
            if (posCamino == 0)
            {
                posCamino += 1;
                if (posCamino < camino.Count) { SendMessage("NewTarget", camino[posCamino].getPosition()); }
                
            }
            else if ((camino[posCamino].getPosition() - GetComponent<AgentNPC>().Position).magnitude < 2)
            {
                posCamino += 1;
                if (posCamino < camino.Count) { SendMessage("NewTarget", camino[posCamino].getPosition()); }
            }
            if (posCamino >= camino.Count - 1)
            {
                posCamino = 0;
                camino.Clear();
            }
        }
    }

    public void CalcularCamino(Vector3 newTarget)
    {
        Tile goal = gird.getTileByVector(newTarget);
        Tile start = gird.getTileByVector(transform.position);

        camino = new List<Tile>(lrta.run(start, goal));
        posCamino = 0;
    }

    /*
    public void NewTarget(Vector3 newTarget){
        Tile goal = gird.getTileByVector(newTarget);
        Tile start = gird.getTileByVector(transform.position);

        List<Tile> camino = lrta.run(start, goal);

        PathBasico path = GetComponent<PathBasico>();
        path.setObjetvosFromTiles(camino);
        PathFollowing pathFollowing = GetComponent<PathFollowing>();
        pathFollowing.setObjetivoInicial();

    }
    */
    
    }