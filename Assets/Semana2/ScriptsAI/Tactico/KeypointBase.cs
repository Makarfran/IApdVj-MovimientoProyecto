using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypointBase : Keypoint
{
    
    [SerializeField]private bool winCondition;
    Controlador controladorJuego;
    
    public Material Amat;
    public Material Rmat;
    public float lifeP;
    private float lifePMax= 3000;
    private BoxCollider range; //tengo que ver como controlar como pierde puntos por ser capturados
    // Start is called before the first frame update
    void Start()
    {
        lifeP = lifePMax;
        controladorJuego = GameObject.Find("ControladorJuego").GetComponent<Controlador>();

    }

    // Update is called once per frame
    void Update()
    {
        captureCheck();
        if(lifeP <= 0){
            switchBando();
            lifeP = lifePMax;
            
        }
    }

    public float getLifeP() 
    {
        return lifeP;
    }

    public float getLifePMax() 
    {
        return lifePMax;
    }

    public void switchBando(){
        if(Bando == "A"){
            Bando = "R";
            controladorJuego.basesTeamA.Remove(gameObject);
            controladorJuego.basesTeamR.Add(gameObject);
            GameObject mychild = this.transform.GetChild(0).gameObject;
            GameObject plane = mychild.transform.GetChild(1).gameObject;
            
            Material[] mats = plane.GetComponent<Renderer>().materials;
            mats[0] = Rmat;
            plane.GetComponent<Renderer>().materials = mats;
            this.GetComponent<InfluenceMap>().setFaccion(InfluenceMap.Faccion.Rojo);
            
        } else{
            Bando = "A";
            controladorJuego.basesTeamR.Remove(gameObject);
            controladorJuego.basesTeamA.Add(gameObject);
            GameObject mychild = this.transform.GetChild(0).gameObject;
            GameObject plane = mychild.transform.GetChild(1).gameObject;
            Material[] mats = plane.GetComponent<Renderer>().materials;
            mats[0] = Amat;
            plane.GetComponent<Renderer>().materials = mats;
            this.GetComponent<InfluenceMap>().setFaccion(InfluenceMap.Faccion.Azul);
            
        }
    }

    public void captureCheck(){
        int countA = 0;
        int countR = 0;
        foreach(AgentNPC npc in nepeces){
            if(npc.getBando() == "A"){
                countA++;
            } else {
                countR++;
            }
        }

        int diff = countA-countR;
        if(Bando == "A"){
            if(diff < 0){
                lifeP = lifeP - 1f*countR;
                if (lifeP < 0) lifeP = 0;
            } else if (diff > 0 && lifeP < lifePMax){
                lifeP = lifeP + 1*countA;
                if (lifeP > lifePMax) lifeP = lifePMax;
            }
        } else{
            if(diff < 0 && lifeP < lifePMax){
                lifeP = lifeP + 1f*countR;
                if (lifeP > lifePMax) lifeP = lifePMax;
            } else if (diff > 0 ){
                lifeP = lifeP - 1*countA;
                if (lifeP < 0) lifeP = 0;
            }
        }
        //hacer sumatorio de agentnpcs en area de un bando vs otro
        // contador a vs contador r
        //si el contador mayor es del defensor, se recupera vida, si es el otro se pierde
        //quitar vida en funcion del valor
    }

    private List<AgentNPC> nepeces = new List<AgentNPC>();
    public List<AgentNPC> GetNPCS () { return nepeces; }


    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Npc" ){
            AgentNPC npc = collision.gameObject.GetComponent<AgentNPC>();
            nepeces.Add(npc);
            

        }
    }
    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Npc" ){
            AgentNPC npc = collision.gameObject.GetComponent<AgentNPC>();
            nepeces.Remove(npc);
            

        }
    }
        
    

    
}
