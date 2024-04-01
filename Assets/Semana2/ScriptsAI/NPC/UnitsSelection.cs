using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * L. Daniel Hern�ndez. 2018. Copyleft
 * 
 * Una propuesta para dar �rdenes a un grupo de agentes sin formaci�n.
 * 
 * Recursos:
 * Los rayos de C�mara: https://docs.unity3d.com/es/current/Manual/CameraRays.html
 * "Percepci�n" mediante Physics.Raycast: https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
 * SendMessage to external functions: https://www.youtube.com/watch?v=4j-lh3C_w1Q
 * 
 * */

public class UnitsSelection : MonoBehaviour
{
    public static List<GameObject> npcsSelected = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {

        // Damos una orden cuando levantemos el bot�n del rat�n.
        //Usamos bot�n izquierdo del rat�n para seleccionar y deseleccionar npcs
        if (Input.GetMouseButtonUp(0))
        {
            

            // Comprobamos si el rat�n golpea a algo en el escenario.
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
        
<<<<<<< HEAD
        //Utilizamos el bot�n derecho del rat�n para dar �rdenes a los npcs seleccionados
        if (Input.GetMouseButtonUp(1)) 
        {
            // Comprobamos si el rat�n golpea a algo en el escenario.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo)) 
            {
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Terrain"))
                {
                    Vector3 newTarget = new Vector3 (hitInfo.point.x, 0, hitInfo.point.z);

                    //GameObject[] listNPC = GameObject.FindGameObjectsWithTag("NPC");

                    /*
                     * Otra alternativa es recurrir a una lista p�blica de todas las unidades seleccionadas
                     *              public List<GameObject> selectedUnits;
                     * En este caso se cambiar�a "listNPC" por "selectedUnits"
                     * 
                     * Dicha lista pertenecer�a a una clase encarga de controlar eventos generales del juego,
                     * como por ejemplo la selecci�n de unidades. La ventaja de mantener una lista en tiempo
                     * de ejecuci�n es obvia: Si el n�mero de unidades es peque�o (p.e. dos) en relaci�n con
                     * el n�mero total de NPC (p.e. miles), pues no ser�a necesario que Unity busque en todos los
                     * objetos del escenario con la marca de haber sido seleccionado, 
                     * lo que facilita y agiliza algunas tareas. P.e. para realizar formaciones.
                     */
        
                    foreach (var npc in npcsSelected)
                    {
                        // Llama al m�todo denominado "NewTarget" en TODOS y cada uno de los MonoBehaviour de este game object (npc)
                        //npc.SendMessage("NewTarget", newTarget);

                        // Se asume que cada NPC tiene varias componentes scripts (es decir, varios MonoBehaviour).
                        // En algunos de esos scripts est� la funci�n "NewTarget(Vector3 target)"
                        // Dicha funci�n contendr� las instrucciones necesarias para ir o no al nuevo destino.
                        // P.e. Dejar lo que est� haciendo y  disparar a target.
                        // P.e. Si no tengo vida suficiente huir de target.
                        // P.e. Si fui seleccionado en una acci�n anterio y estoy a la espera de nuevas �rdenes, entonces hacer un Arrive a target.

                        // Nota1: En el caso de que tu objeto tenga una estructura jer�rquica, 
                        // y se quiera invocar a NewTarget de todos sus hijos, deber�s usar BroadcastMessage.

                        // Nota 2: En el caso de que solo se tenga una funci�n "NewTarget" para cada NPC, entonces 
                        // puede ser m�s eficiente algo como:
                        //if (npc.GetComponent<Arrive>() != null) npc.GetComponent<Arrive>().NewTarget(newTarget);
                        npc.SendMessage("NewTarget", newTarget);
                        // que obtiene la componente del NPC que yo s� que contiene a la funci�n NewTarget(), y la invoca.
                    }
                }
            }       
        }
        
=======
>>>>>>> 76d19a085c92211bf2d51cb1bd551463f07b4bf5
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
