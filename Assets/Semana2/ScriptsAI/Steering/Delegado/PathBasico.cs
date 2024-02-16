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
}
