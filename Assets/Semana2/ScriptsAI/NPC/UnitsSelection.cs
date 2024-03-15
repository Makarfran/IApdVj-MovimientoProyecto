using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * L. Daniel Hernández. 2018. Copyleft
 * 
 * Una propuesta para dar órdenes a un grupo de agentes sin formación.
 * 
 * Recursos:
 * Los rayos de Cámara: https://docs.unity3d.com/es/current/Manual/CameraRays.html
 * "Percepción" mediante Physics.Raycast: https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
 * SendMessage to external functions: https://www.youtube.com/watch?v=4j-lh3C_w1Q
 * 
 * */

public class UnitsSelection : MonoBehaviour
{
    public static List<GameObject> npcsSelected = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {

        // Damos una orden cuando levantemos el botón del ratón.
        //Usamos botón izquierdo del ratón para seleccionar y deseleccionar npcs
        if (Input.GetMouseButtonUp(0))
        {

            // Comprobamos si el ratón golpea a algo en el escenario.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {

                // Si golpeamos un npc lo seleccionamos o deseleccionamos
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Npc"))
                {
                    GameObject npc = hitInfo.collider.gameObject;
                    //Si el Shift esta siendo pulsado (shift + click)
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        if (npcsSelected.Contains(npc)) { Deselect(npc); }
                        else { Select(npc); }
                    }
                    else    //Si el Shift no esta siendo pulsado (click)
                    {
                        DeselectAll();
                        Select(npc);
                    }
                }

                // Si lo que golpea es un punto del terreno entonces da la orden a todas las unidades NPC
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Terrain"))
                {
                    DeselectAll();
                }
            }
        }
        
        //Utilizamos el botón derecho del ratón para dar órdenes a los npcs seleccionados
        if (Input.GetMouseButtonUp(1)) 
        {
            // Comprobamos si el ratón golpea a algo en el escenario.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo)) 
            {
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Terrain"))
                {
                    Vector3 newTarget = new Vector3 (hitInfo.point.x, 0, hitInfo.point.z);

                    //GameObject[] listNPC = GameObject.FindGameObjectsWithTag("NPC");

                    /*
                     * Otra alternativa es recurrir a una lista pública de todas las unidades seleccionadas
                     *              public List<GameObject> selectedUnits;
                     * En este caso se cambiaría "listNPC" por "selectedUnits"
                     * 
                     * Dicha lista pertenecería a una clase encarga de controlar eventos generales del juego,
                     * como por ejemplo la selección de unidades. La ventaja de mantener una lista en tiempo
                     * de ejecución es obvia: Si el número de unidades es pequeño (p.e. dos) en relación con
                     * el número total de NPC (p.e. miles), pues no sería necesario que Unity busque en todos los
                     * objetos del escenario con la marca de haber sido seleccionado, 
                     * lo que facilita y agiliza algunas tareas. P.e. para realizar formaciones.
                     */
        
                    foreach (var npc in npcsSelected)
                    {
                        // Llama al método denominado "NewTarget" en TODOS y cada uno de los MonoBehaviour de este game object (npc)
                        //npc.SendMessage("NewTarget", newTarget);

                        // Se asume que cada NPC tiene varias componentes scripts (es decir, varios MonoBehaviour).
                        // En algunos de esos scripts está la función "NewTarget(Vector3 target)"
                        // Dicha función contendrá las instrucciones necesarias para ir o no al nuevo destino.
                        // P.e. Dejar lo que esté haciendo y  disparar a target.
                        // P.e. Si no tengo vida suficiente huir de target.
                        // P.e. Si fui seleccionado en una acción anterio y estoy a la espera de nuevas órdenes, entonces hacer un Arrive a target.

                        // Nota1: En el caso de que tu objeto tenga una estructura jerárquica, 
                        // y se quiera invocar a NewTarget de todos sus hijos, deberás usar BroadcastMessage.

                        // Nota 2: En el caso de que solo se tenga una función "NewTarget" para cada NPC, entonces 
                        // puede ser más eficiente algo como:
                        if (npc.GetComponent<Arrive>() != null) npc.GetComponent<Arrive>().NewTarget(newTarget);
                        // que obtiene la componente del NPC que yo sé que contiene a la función NewTarget(), y la invoca.
                    }
                }
            }       
        }
        
    }

    public void Select(GameObject npc)
    {
        npcsSelected.Add(npc);
        Transform marker = npc.transform.Find("Mark");
        marker.gameObject.SetActive(true);
    }

    public void Deselect(GameObject npc)
    {
        npcsSelected.Remove(npc);
        Transform marker = npc.transform.Find("Mark");
        marker.gameObject.SetActive(false);
    }

    public void DeselectAll()
    {
        foreach (GameObject npc in npcsSelected)
        {
            Transform marker = npc.transform.Find("Mark");
            marker.gameObject.SetActive(false);
        }
        npcsSelected.Clear();
    }

}
