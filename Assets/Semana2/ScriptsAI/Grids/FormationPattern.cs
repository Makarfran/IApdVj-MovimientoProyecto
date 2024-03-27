using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface FormationPattern
{
    //N�mero de slots m�ximos que acepta el patr
/*
    public int NumberOfSlots
    {
        get;
        set;
    }
*/
    //Devuelve el drifft offset
    FormationManager.Location GetDriftOffset(List<SlotAssignment> slotAssignments);

    //Devuelve la localizaci�n del slot
    FormationManager.Location GetSlotLocation(int slotNumber);

    //Devuelve verdadero si el patr�n soporta el n�mero de slots
    bool SupportsSlots(int slotCount);

}
