using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinding : MonoBehaviour
{
    [SerializeField] public int costeMovimientoLineal;
    [SerializeField] public Grid gird;
    private int maxDepth;
    private LRTAStart lrta;
    private List<Tile> camino;
    private int posCamino;

    public void setGrid(Grid g){
        gird = g;
    }

    void Start(){
        camino = new List<Tile>();
        lrta = new LRTAStart();
        this.generateGrid();
        lrta.setGrid(gird);
        lrta.costeMovimientoLineal = costeMovimientoLineal;
        lrta.maxDepth = 1;
    }

    public void generateGrid(){
        float tam = this.GetComponent<AgentNPC>().getTam();
        string name = $"Grid {tam}";
        GameObject obj = GameObject.Find(name);
        if(obj ){
            gird = obj.GetComponent<Grid>();
        } else{
            GameObject gridgen = GameObject.Find("GridGenerator");
            GameObject gridA = gridgen.GetComponent<GeneraGrid>().generameGrid(this.GetComponent<AgentNPC>());
            gird = gridA.GetComponent<Grid>();

        }
        if(this.GetComponent<AgentNPC>()){
            this.GetComponent<AgentNPC>().setGrid(gird);
        }
    }

    void Update()
    {
        
        if (camino.Count > 0)
        {
            /*
            if (posCamino == 0)
            {
                posCamino += 1;
                if (posCamino < camino.Count) { SendMessage("NewTarget", camino[posCamino].getPosition()); }
                
            }
            */
            if ((camino[posCamino].getPosition() - GetComponent<AgentNPC>().Position).magnitude < 2)
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