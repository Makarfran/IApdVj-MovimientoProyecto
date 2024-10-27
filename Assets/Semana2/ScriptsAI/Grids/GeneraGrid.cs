using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Esto es mas para generar grids que otra cosa
public class GeneraGrid : MonoBehaviour {
    // Start is called before the first frame update
    
    [SerializeField] protected Vector3 posIni;
    [SerializeField] protected Vector3 posFin;
    public Material matTrans;

    

    public GameObject generameGrid(AgentNPC npc){
        float lado = npc.getTam();
        
        float disVert = Mathf.Abs(posFin.z - posIni.z);
        
        float disHor = Mathf.Abs(posFin.x - posIni.x);
        
        int a = (int) Mathf.Ceil(disHor/lado);
        int b = (int) Mathf.Ceil(disVert/lado);
        
        GameObject gridT = new GameObject("Grid");
        gridT.name = $"Grid {lado}";
        gridT.transform.position = posIni;
        for(int i = 0; i < a; i++ ){ 
           
            for(int j = 0; j < b ; j++ ){
                GameObject plane  = GameObject.CreatePrimitive(PrimitiveType.Cube);
                plane.transform.parent = npc.transform;
                
                plane.GetComponent<MeshRenderer>().material = matTrans;
                plane.GetComponent<MeshRenderer>().enabled = false;
                
                plane.transform.localScale = new Vector3(plane.transform.localScale.x*lado, 1f ,plane.transform.localScale.x*lado);
                plane.name = $"Tile {i} {j}";
                plane.AddComponent<Tile>();
                plane.transform.parent = gridT.transform;
                BoxCollider boxer = plane.GetComponent<BoxCollider>();
                boxer.center = new Vector3(0f,-1f, 0f);
                Vector3 pos = new Vector3(posIni.x + i*lado + lado/2, -0.5f , posIni.z + j*lado + lado/2  );
                //plane.tag = "Terrain";
                
                
                plane.transform.position = pos;
                
                plane.GetComponent<Tile>().setPos(pos);
                
            }
        }
        gridT.AddComponent<Grid>();
        gridT.GetComponent<Grid>().setA(a);
        gridT.GetComponent<Grid>().setB(b);
        return gridT;
    }  

    
}