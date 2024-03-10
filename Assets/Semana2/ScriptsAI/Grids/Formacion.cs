using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formacion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected int a;
    [SerializeField] protected int b;
    [SerializeField] protected Vector2 LeaderSlot;
    protected Agent[,] posiciones;
    [SerializeField] protected AgentNPC leader;
    [SerializeField] protected AgentNPC[] soldados;

    void Start(){
        for(int i = 0; i < a; i++ ){
            for(int j=0; j<b; j++){
                posiciones[i,j] = new Agent();
                posiciones[i,j].transform.position = new Vector3(this.transform.position.x + 1.5f + i*2, 0, this.transform.position.z + 1.5f + j*2);
            }
        }
    }
}
