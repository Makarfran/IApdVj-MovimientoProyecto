using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationEagle : FormationPattern
{
    public static FormationManager.Location[] locations = new FormationManager.Location[] { new FormationManager.Location(Vector3.zero, 0),
                                                                                            new FormationManager.Location(new Vector3(1, 0, 3), 0),
                                                                                            new FormationManager.Location(new Vector3(-1, 0,3), 0),
                                                                                            new FormationManager.Location(new Vector3(3, 0, 0), 90),
                                                                                            new FormationManager.Location(new Vector3(-3, 0, 0), -90),
                                                                                            new FormationManager.Location(new Vector3(-1, 0, -3), -135),
                                                                                            new FormationManager.Location(new Vector3(1, 0, -3), 135),
                                                                                            new FormationManager.Location(new Vector3(0, 0, 3), 0),
                                                                                            new FormationManager.Location(new Vector3(0, 0, 6), 0),
                                                                                            new FormationManager.Location(new Vector3(0, 0, -3), 180),
                                                                                          };

    //Devuelve el drifft offset
    public FormationManager.Location GetDriftOffset(List<SlotAssignment> slotAssignments)
    {
        //Centro de masas
        Vector3 centerPosition = Vector3.zero;
        float centerOrientation = 0f;

        //Para cada slot a�adimos su contribuci�n al centro de masas
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

    //Devuelve la localizaci�n del slot
    public FormationManager.Location GetSlotLocation(int slotNumber)
    {
        return locations[slotNumber];
    }

    //Devuelve verdadero si el patr�n soporta el n�mero de slots
    public bool SupportsSlots(int slotCount)
    {
        return slotCount < locations.Length;
    }
}
