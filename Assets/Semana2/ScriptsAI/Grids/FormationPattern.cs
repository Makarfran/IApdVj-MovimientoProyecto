using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface FormationPattern
{
    //Número de slots máximos que acepta el patr
/*
    public int NumberOfSlots
    {
        get;
        set;
    }
*/
    //Devuelve el drifft offset
    FormationManager.Location GetDriftOffset(List<SlotAssignment> slotAssignments);

    //Devuelve la localización del slot
    FormationManager.Location GetSlotLocation(int slotNumber);

    //Devuelve verdadero si el patrón soporta el número de slots
    bool SupportsSlots(int slotCount);

}
