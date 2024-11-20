using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypointBase : Keypoint
{
    [SerializeField]private string Bando;
    [SerializeField]private bool winCondition;
    
    public Material Amat;
    public Material Rmat;
    public float lifeP;
    private float lifePMax= 1500;
    private BoxCollider range; //tengo que ver como controlar como pierde puntos por ser capturados
    // Start is called before the first frame update
    void Start()
    {
        lifeP = lifePMax;
        
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

    public string getBando(){
        return Bando;
    }

    public void switchBando(){
        if(Bando == "A"){
            Bando = "R";
            GameObject mychild = this.transform.GetChild(0).gameObject;
            GameObject plane = mychild.transform.GetChild(1).gameObject;
            
            Material[] mats = plane.GetComponent<Renderer>().materials;
            mats[0] = Rmat;
            plane.GetComponent<Renderer>().materials = mats;
            this.GetComponent<InfluenceMap>().setFaccion(InfluenceMap.Faccion.Rojo);
            
        } else{
            Bando = "A";
            GameObject mychild = this.transform.GetChild(0).gameObject;
            GameObject plane = mychild.transform.GetChild(1).gameObject;
            Material[] mats = plane.GetComponent<Renderer>().materials;
            mats[0] = Amat;
            plane.GetComponent<Renderer>().materials = mats;
            this.GetComponent<InfluenceMap>().setFaccion(InfluenceMap.Faccion.Azul);
            
        }
    }

    public void captureCheck(){
        nepeces.Clear();
        colliderCheck();
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
                lifeP = lifeP - 5*countR;
            } else if (diff > 0 && lifeP < lifePMax){
                lifeP = lifeP + 1*countA;
            }
        } else{
            if(diff < 0 && lifeP < lifePMax){
                lifeP = lifeP + 1*countR;
            } else if (diff > 0 ){
                lifeP = lifeP - 5*countA;
            }
        }
        //hacer sumatorio de agentnpcs en area de un bando vs otro
        // contador a vs contador r
        //si el contador mayor es del defensor, se recupera vida, si es el otro se pierde
        //quitar vida en funcion del valor
    }

    private List<AgentNPC> nepeces = new List<AgentNPC>();
    public List<AgentNPC> GetNPCS () { return nepeces; }

    private void colliderCheck () {
        Vector3 boxSize = GetComponent<Collider>().bounds.size;
        
        // Verifica si este objeto está en contacto con un obstáculo
        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<AgentNPC>())
                {
                AgentNPC npc = collider.gameObject.GetComponent<AgentNPC>();
                 nepeces.Add(npc);
                 
            }
        }
    }
        
    

    
}
