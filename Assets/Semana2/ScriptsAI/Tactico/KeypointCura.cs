using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypointCura : Keypoint
{
    public InfluenceManager infman;
    public InfluenceMap infmap;
    private List<AgentNPC> agentes = new List<AgentNPC>();
    public List<AgentNPC> GetAgentes () { return agentes; }
    // Start is called before the first frame update
    void Start()
    {
        infman = FindObjectOfType<InfluenceManager>();
        infmap = FindObjectOfType<InfluenceMap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (infman != null)
        {
            float inf = getInfluenceValue(this.transform.position);
            if (inf >= 3)
            {
                Bando = "A";
            }
            else if (inf <= -3)
            {
                Bando = "R";
            }
            else
            {
                Bando = "None";
            }
        }
        foreach(AgentNPC a in agentes){
            if(a.getBando() == getBando()) a.recuperarVida();
        }
        
    }

    public float getInfluenceValue(Vector3 vec)
    {

        float maxinf = infman.getMaxInf();

        float inf = infman.getInfluenceTile(vec, InfluenceMap.Faccion.Azul);
        return inf;
        /*
        if (inf < 0)
        {
            if (inf < (3 / 4) * maxinf)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else if (inf == 0)
        {
            return 0;
        }
        else
        {
            if (inf > -(3 / 4) * maxinf)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }*/
    }


    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Npc" ){
            AgentNPC npc = collision.gameObject.GetComponent<AgentNPC>();
            agentes.Add(npc);
            

        }
    }
    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Npc" ){
            AgentNPC npc = collision.gameObject.GetComponent<AgentNPC>();
            agentes.Remove(npc);
            

        }
    }
}
