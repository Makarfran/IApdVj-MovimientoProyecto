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
    

    //Lista de asignaciones 
    public List<SlotAssignment> slotAssignments;

    //Posici�n y orientaci�n para evitar derrapes
    //De momento uso l�der
    public Location driftOffset;

    //Patr�n de la formaci�n
    public FormationPattern pattern;

    public float time = 0f;

    //Verdadero = criterio1, falso = criterio2
    [SerializeField] public bool criterio = true;

    public Vector3 pathDestination = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        slotAssignments = new List<SlotAssignment>();
    }

    // Update is called once per frame
    void Update()
    {

        if (slotAssignments.Count > 0 )
        {
            if (criterio) 
            {
                Criterio1();
            }
            else
            {

                Criterio2();
            }  

        }
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
            character.GetComponent<StateMachineManager>().SwitchState(StateMachineManager.formationState);
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
        character.GetComponent<StateMachineManager>().SwitchState(StateMachineManager.idleState);
    }

    public void RemoveAllCharacters() 
    {
        foreach (SlotAssignment slot in slotAssignments) 
        {
            slot.Npc.GetComponent<StateMachineManager>().SwitchState(StateMachineManager.idleState);
        }

        slotAssignments.Clear();
    }

    public void UpdateSlots() 
    {
        if (slotAssignments.Count == 0) { return; }
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

    public void PathfindingCriterio(Vector3 newTarget) 
    {
        
        foreach (SlotAssignment slot in slotAssignments)
        {
            slot.Npc.GetComponent<PathFinding>().CalcularCamino(newTarget);
        }

    }

    //Calcula la posici�n real del slot
    //  eje z   eje x

    //( Cos 0   sin 0)  ( x )        
    //(              )  (   )    =    (relativeLocX, relativeLocZ)
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

    //Comprobamos si hay que romper la formaci�n

    public bool BreakFormation()
    {
        //Lista auxiliar con los npcs de la formaci�n
        List<GameObject> npcFormation = new List<GameObject>();
        foreach (SlotAssignment slot in slotAssignments)
        {
            npcFormation.Add(slot.Npc);
        }

        //Si el npc seleccionado esta en la formaci�n lo eliminamos de la lista auxiliar, si no lo esta se rompe la formaci�n
        foreach (GameObject npc in UnitsSelection.npcsSelected)
        {
            if (npcFormation.Contains(npc)) { npcFormation.Remove(npc); }
            else
            {
                RemoveAllCharacters();
                return true;
            }
        }

        //Eliminamos de la formaci�n aquellos npcs que estan en la formaci�n pero que no han sido seleccionados
        foreach (GameObject npc in npcFormation) { RemoveCharacter(npc); }

        //La formaci�n se mantiene
        return false;
    }

    private void Criterio1() 
    {
        Vector3 distancia = slotAssignments[0].Npc.GetComponent<order>().arrivalPoint.Position - slotAssignments[0].Npc.GetComponent<AgentNPC>().Position;
        if (distancia.magnitude > 1f || slotAssignments[0].Npc.GetComponent<StateMachineManager>().CurrentState == StateMachineManager.wanderState)
        {
            time = 0;
            Vector3 vel = slotAssignments[0].Npc.GetComponent<AgentNPC>().Velocity.normalized;
            vel = vel * 2;
            Vector3 leaderFollowing = slotAssignments[0].Npc.GetComponent<AgentNPC>().Position - vel;
            for (int i = 1; i < slotAssignments.Count; i++)
            {
                slotAssignments[i].Npc.SendMessage("NewTarget", leaderFollowing);
            }
        }
        else
        {
            if (time < Time.deltaTime)
            {
                UpdateSlots();
            }
            if (time > 10)
            {
                slotAssignments[0].Npc.GetComponent<StateMachineManager>().SwitchState(StateMachineManager.wanderState);
            }
            time += Time.deltaTime;

        }
    }

    private void Criterio2() 
    {
        if (pathDestination != null && (pathDestination - slotAssignments[0].Npc.GetComponent<AgentNPC>().Position).magnitude > 2f)
        {
            time = 0;
            if (slotAssignments[0].Npc.GetComponent<StateMachineManager>().CurrentState == StateMachineManager.wanderState)
            {
                pathDestination = slotAssignments[0].Npc.GetComponent<Wander>().Target.Position;
                PathfindingCriterio(slotAssignments[0].Npc.GetComponent<AgentNPC>().Position);
            }
        }
        else
        {

            UpdateSlots();

            if (time > 10)
            {
                slotAssignments[0].Npc.GetComponent<StateMachineManager>().SwitchState(StateMachineManager.wanderState);

            }
            time += Time.deltaTime;
        }
    }
}
