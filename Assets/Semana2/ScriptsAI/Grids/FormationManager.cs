using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    
    //Estructura de datos que permite almacenar una posición y una orientación
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
    

    //Lista de asignaciones ¿mejor array?
    public List<SlotAssignment> slotAssignments;

    //Posición y orientación para evitar derrapes
    //De momento uso líder
    public Location driftOffset;

    //Patrón de la formación
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

        //De momento, uso líder como punto de anclaje
        driftOffset = pattern.GetDriftOffset(slotAssignments);
    }

    //Añade un nuevo personaje al primer slot disponible. Devuelve falso si no hay más slots disponibles
    public bool AddCharacter(GameObject character)
    {
        //Obtenemos el número de slots ocupados
        int occupiedSlots = slotAssignments.Count;

        //Comprobamos si el patrón soporta más slots
        if (pattern.SupportsSlots(occupiedSlots + 1))
        {
            //Añadimos un nuevo slot
            SlotAssignment slotAssignment = new SlotAssignment(character, occupiedSlots+1);
            slotAssignments.Add(slotAssignment);

            //Actualizamos la asignación de slots y devolvemos verdadero
            UpdateSlotAssignments();
            return true;
        }

        //Si el patrón no soporta más slots devolvemos falso
        return false;
    }

    public bool AddCharacters(List<GameObject> characters) 
    {
        //Obtenemos el número de slots ocupados
        int occupiedSlots = slotAssignments.Count;
        //Comprobamos si el patrón soporta más slots
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
        //Eliminamos el slot de la lista ¿Es necesario asegurarnos de que el resultado sea válido?
        slotAssignments.Remove(slot);
        //Actualizamos las asignaciones
        UpdateSlotAssignments();
    }

    public void UpdateSlots() 
    {
        //Encuentra el punto de anclaje, en este momento uso al líder
        Location anchorLoc = new (slotAssignments[0].Npc.GetComponent<AgentNPC>().Position, slotAssignments[0].Npc.GetComponent<AgentNPC>().Orientation);
        /*
        //en este momento orientación del líder
        float orientation =  Mathf.Atan2(slotAssignments[0].Npc.Position.x, slotAssignments[0].Npc.Position.y);
        */
        foreach (SlotAssignment slot in slotAssignments) 
        {
            //Obtenemos la localización relativa del slot al líder
            Location relativeLoc = pattern.GetSlotLocation(slot.SlotNumber);

            //Obtener la localización del slot
            Vector3 locationPosition = CalcularPosition(relativeLoc.Position, Bodi.MapToRangePi(anchorLoc.Orientation), anchorLoc.Position);
            float locationOrientation = anchorLoc.Orientation + relativeLoc.Orientation;
            //Location location = new Location(locationPosition - driftOffset.Position, locationOrientation - driftOffset.Orientation);
            Location location = new Location(locationPosition, locationOrientation );
            //Establecemos la localización como objetivo del npc
            slot.Npc.SendMessage("NewTarget", location.Position);
            slot.Npc.SendMessage("NewTargetOr", location.Orientation);
        }

    }

    //Calcula la posición real del slot
    //  eje z   eje x

    //( Cos 0   sin 0)  ( x )
    //( -sin 0  cos 0)  ( z )
    public Vector3 CalcularPosition(Vector3 relativeLoc, float orientation, Vector3 anchorLoc) 
    {
        //Aplicamos el cambio de base a la posición relativa
        float relativeLocX = Mathf.Cos(orientation) * relativeLoc.x + Mathf.Sin(orientation) * relativeLoc.z;
        float relativeLocZ = -Mathf.Sin(orientation) * relativeLoc.x + Mathf.Cos(orientation) * relativeLoc.z;
        //Sumamos la posición del líder o punto de anclaje
        relativeLocX = anchorLoc.x + relativeLocX;
        relativeLocZ = anchorLoc.z + relativeLocZ;
        return new Vector3(relativeLocX, 0, relativeLocZ); 
    }
}
