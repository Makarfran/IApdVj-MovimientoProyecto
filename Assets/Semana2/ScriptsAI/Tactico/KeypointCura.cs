using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypointCura : Keypoint
{
    Controlador controladorJuego;
    
    // Start is called before the first frame update
    void Start()
    {
        controladorJuego = GameObject.Find("ControladorJuego").GetComponent<Controlador>();
        Bando = "None";
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(FindObjectOfType<InfluenceManager>()){
            int inf = getInfluenceValue(this.transform.position);
            if(inf == 1){
                Bando = "A";
               
            } else if (inf == -1){
                Bando = "R";
                
            } else {
                Bando = "None";
            }
        }
        */
    }

    public int getInfluenceValue(Vector3 vec){
        InfluenceManager influenceManager = FindObjectOfType<InfluenceManager>();
        InfluenceMap influenceMap = FindObjectOfType<InfluenceMap>();
        float maxinf = influenceManager.getMaxInf();
        
        float inf = influenceManager.getInfluenceTile(vec, InfluenceMap.Faccion.Azul);
        if(inf < 0){
            if(inf < (3/4)*maxinf){
                return 0;
            } else{
                return 1;
            }
        } else if(inf == 0){
            return 0;
        } else {
            if(inf > -(3/4)*maxinf){
                return 0;
            } else {
                return -1;
            }
        }
    }
}
