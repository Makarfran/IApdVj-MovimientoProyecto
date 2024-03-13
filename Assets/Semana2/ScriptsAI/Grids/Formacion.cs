using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formacion : Grid
{
    // Start is called before the first frame update
    
    [SerializeField] protected Vector2 LeaderSlot;
    protected Tile[,] posiciones;
    [SerializeField] protected AgentNPC leader;
    [SerializeField] protected AgentNPC[] soldados;

    
}
