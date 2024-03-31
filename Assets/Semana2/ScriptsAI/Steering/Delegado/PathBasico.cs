using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBasico : MonoBehaviour
{
    public List<Agent> objetivos;

    // Update is called once per frame
    public Agent getObjetivoInicial(){
        return objetivos[0];
    }

    public Agent getSiguiente(Agent obj){
        int i;
        int length = objetivos.Count;
        for(i = 0 ; i < length; i++){
            if(objetivos[i] == obj && i != length - 1){
                return objetivos[i+1];
            }
        }
        return obj;
    }

    public Agent getSigJP(Agent obj){
        
        if(obj.GetComponent("JumpPoint") == null){
            return obj;
        }
        int i;
        int length = objetivos.Count;
        int temp = 9999;
        for(i = 0 ; i < length; i++){
            if(objetivos[i] == obj){
                
                temp = i;
            }
            if(objetivos[i].GetComponent("JumpPoint") != null && i > temp){
                
                return objetivos[i];
            }
        }
        return null;
    }

    public void setObjetvosFromTiles(List<Tile> camino){
        objetivos = new List<Agent>();
        foreach (Tile tile in camino)
        {
            GameObject fakeAgent = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            fakeAgent.AddComponent<AgentNPC>();
            fakeAgent.GetComponent<AgentNPC>().Position = tile.getPosition();
            objetivos.Add(fakeAgent.GetComponent<AgentNPC>());
        }
    }
}
