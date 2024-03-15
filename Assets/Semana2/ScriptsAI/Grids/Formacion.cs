using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formacion : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] protected Vector2 LeaderSlot;
    protected Tile[,] posiciones;
    [SerializeField] protected AgentNPC leader;
    [SerializeField] protected AgentNPC[] soldados;

    
}
