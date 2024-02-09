using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public GameObject markSpecial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //El usuario pulsa el botón que será identificado como el botón virtual "Fire1".
        if (Input.GetButtonDown("Fire1")) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) mark(hit.transform.gameObject);
        }
    }

    private void mark(GameObject thing) 
    {
        GameObject marker = null;

        //Si hay un hijo en el objeto llamado Mark
        if (thing.transform.Find("Mark") != null) //Obtiene la referencia
            marker = thing.transform.Find("Mark").gameObject;

        //Si no hay referencia entonces
        if (marker == null)
        {
            
            //Crea una instancia del prefab

            marker = Instantiate(markSpecial, thing.transform);

            marker.transform.localPosition = Vector3.up * 1; //Cambia la posicion relativa
            marker.name = "Mark"; //Cambia el nombre
        }
        else //Si hay refencia entonces 
            Destroy(marker); // Destroy the object 
    }
}
