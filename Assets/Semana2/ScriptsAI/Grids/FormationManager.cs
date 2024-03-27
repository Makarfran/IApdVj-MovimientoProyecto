using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    
    //Estructura de datos que permite almacenar una posici�n y una orientaci�n
    public struct Location
    {
        private Vector3 position;
        private float orientation;

        public Location(Vector3 pos, float or)
        {
            position = pos;
            orientation = or;
        }

        public Vector3 Position
        { 
            get { return position; }
        }
        public float Orientation 
        { 
            get { return orientation; }
        }
    }
    

    //Lista de asignaciones �mejor array?
    public List<SlotAssignment> slotAssignments;

    //Posici�n y orientaci�n para evitar derrapes
    //De momento uso l�der
    public Location driftOffset;

    //Patr�n de la formaci�n
    public FormationPattern pattern;

    // Start is called before the first frame update
    void Start()
    {
        slotAssignments = new List<SlotAssignment>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
       
    //Actualiza las asignaciones de los npcs a los slots
    public void UpdateSlotAssignments() 
    {
        for (int i = 0; i < slotAssignments.Count; i++) 
        {
            slotAssignments[i].SlotNumber = i;
        }

        //De momento, uso l�der como punto de anclaje
        driftOffset = pattern.GetDriftOffset(slotAssignments);
    }

    //A�ade un nuevo personaje al primer slot disponible. Devuelve falso si no hay m�s slots disponibles
    public bool AddCharacter(GameObject character)
    {
        //Obtenemos el n�mero de slots ocupados
        int occupiedSlots = slotAssignments.Count;

        //Comprobamos si el patr�n soporta m�s slots
        if (pattern.SupportsSlots(occupiedSlots + 1))
        {
            //A�adimos un nuevo slot
            SlotAssignment slotAssignment = new SlotAssignment(character, occupiedSlots+1);
            slotAssignments.Add(slotAssignment);

            //Actualizamos la asignaci�n de slots y devolvemos verdadero
            UpdateSlotAssignments();
            return true;
        }

        //Si el patr�n no soporta m�s slots devolvemos falso
        return false;
    }

    public bool AddCharacters(List<GameObject> characters) 
    {
        //Obtenemos el n�mero de slots ocupados
        int occupiedSlots = slotAssignments.Count;
        //Comprobamos si el patr�n soporta m�s slots
        if (pattern.SupportsSlots(occupiedSlots + characters.Count))
        {
            foreach (GameObject character in characters) 
            {
                AddCharacter(character);
            }
            return true;
        }

        return false;
    }

    public void RemoveCharacter(GameObject character) 
    {
        //Encontramos el slot del personaje
        SlotAssignment slot = slotAssignments.Find(x => x.Npc == character);
        //Eliminamos el slot de la lista �Es necesario asegurarnos de que el resultado sea v�lido?
        slotAssignments.Remove(slot);
        //Actualizamos las asignaciones
        UpdateSlotAssignments();
    }

    public void UpdateSlots() 
    {
        //Encuentra el punto de anclaje, en este momento uso al l�der
        Location anchorLoc = new (slotAssignments[0].Npc.GetComponent<AgentNPC>().Position, slotAssignments[0].Npc.GetComponent<AgentNPC>().Orientation);
        /*
        //en este momento orientaci�n del l�der
        float orientation =  Mathf.Atan2(slotAssignments[0].Npc.Position.x, slotAssignments[0].Npc.Position.y);
        */
        foreach (SlotAssignment slot in slotAssignments) 
        {
            //Obtenemos la localizaci�n relativa del slot al l�der
            Location relativeLoc = pattern.GetSlotLocation(slot.SlotNumber);

            //Obtener la localizaci�n del slot
            Vector3 locationPosition = CalcularPosition(relativeLoc.Position, Bodi.MapToRangePi(anchorLoc.Orientation), anchorLoc.Position);
            float locationOrientation = anchorLoc.Orientation + relativeLoc.Orientation;
            //Location location = new Location(locationPosition - driftOffset.Position, locationOrientation - driftOffset.Orientation);
            Location location = new Location(locationPosition, locationOrientation );
            //Establecemos la localizaci�n como objetivo del npc
            slot.Npc.SendMessage("NewTarget", location.Position);
            slot.Npc.SendMessage("NewTargetOr", location.Orientation);
        }

    }

    //Calcula la posici�n real del slot
    //  eje z   eje x

    //( Cos 0   sin 0)  ( x )
    //( -sin 0  cos 0)  ( z )
    public Vector3 CalcularPosition(Vector3 relativeLoc, float orientation, Vector3 anchorLoc) 
    {
        //Aplicamos el cambio de base a la posici�n relativa
        float relativeLocX = Mathf.Cos(orientation) * relativeLoc.x + Mathf.Sin(orientation) * relativeLoc.z;
        float relativeLocZ = -Mathf.Sin(orientation) * relativeLoc.x + Mathf.Cos(orientation) * relativeLoc.z;
        //Sumamos la posici�n del l�der o punto de anclaje
        relativeLocX = anchorLoc.x + relativeLocX;
        relativeLocZ = anchorLoc.z + relativeLocZ;
        return new Vector3(relativeLocX, 0, relativeLocZ); 
    }
}
