using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estructura para almacenar la asignación de un personaje a un slot
public class SlotAssignment
{
    
    protected GameObject npc;

    protected int slotNumber;

    public SlotAssignment(GameObject character, int slot) 
    {
        npc = character;
        slotNumber = slot;
    }

    public GameObject Npc 
    {
        get { return npc; }
        set { npc = value; }
    }

    public int SlotNumber
    {
        get { return slotNumber; }
        set { slotNumber = value; }
    }

}
