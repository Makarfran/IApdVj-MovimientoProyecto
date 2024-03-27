using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frontal : FormationPattern
{

    public static FormationManager.Location[] locations = new FormationManager.Location[] { new FormationManager.Location(Vector3.zero, 0),
                                                                                            new FormationManager.Location(new Vector3(-2, 0, 0), -45),
                                                                                            new FormationManager.Location(new Vector3(2, 0, 0), 45),
                                                                                            new FormationManager.Location(new Vector3(-1, 0, -3), 0),
                                                                                            new FormationManager.Location(new Vector3(1, 0, -3), 0),
                                                                                            new FormationManager.Location(new Vector3(0, 0, -6), 0),
                                                                                            new FormationManager.Location(new Vector3(-3, 0, -3), -90),
                                                                                            new FormationManager.Location(new Vector3(3, 0, -3), 90),
                                                                                            new FormationManager.Location(new Vector3(-2, 0, -6), 0),
                                                                                            new FormationManager.Location(new Vector3(2, 0, -6), 0),
                                                                                          };

    //Devuelve el drifft offset
    public FormationManager.Location GetDriftOffset(List<SlotAssignment> slotAssignments) 
    {
        //Centro de masas
        Vector3 centerPosition = Vector3.zero;
        float centerOrientation = 0f;

        //Para cada slot añadimos su contribución al centro de masas
        foreach (SlotAssignment slot in slotAssignments) 
        {
            FormationManager.Location location = GetSlotLocation(slot.SlotNumber);
            centerPosition += location.Position;
            centerOrientation += location.Orientation;
        }

        centerPosition = centerPosition / slotAssignments.Count;
        centerOrientation = centerOrientation / slotAssignments.Count;
        return new FormationManager.Location(centerPosition, centerOrientation);

    }

    //Devuelve la localización del slot
    public FormationManager.Location GetSlotLocation(int slotNumber) 
    {
        return locations[slotNumber];
    }

    //Devuelve verdadero si el patrón soporta el número de slots
    public bool SupportsSlots(int slotCount) 
    { 
        return slotCount < locations.Length; 
    }
}
