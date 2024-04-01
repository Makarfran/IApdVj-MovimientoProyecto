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
