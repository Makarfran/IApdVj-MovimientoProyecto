using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTraceScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Devuelve un rayo que va desde la cámara hasta el punto en la pantalla.
        //Camera.main devuelve la camara principal

        //ScreenPointToRay es un método de la class camera que recibe 2 parámetros.
        //1. El punto 3D cuyas coordenadas contienen un punto de espacio de la pantalla en píxeles
        //2. (OPCIONAL) Permite especificar que ojo utilizar. predeterminado Mono(no estereoscópico). (right, left, mono)

        //mousePosition es una propiedad estática de la clase Input que permite devolver la posición del ratón en
        //coordenadas píxel

        //Ray es una línea infinita que empieza en el origen y tiene una direccion. 2 parametros: direction y origin.

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //RaycastHit es una estructura Unity con información sobre el lanzamiento de un ray.
        RaycastHit hit;
        
        // Otro: .Linecast() .BoxCast() .SphereCast() .CapsuleCast()
        if (Physics.Raycast(ray, out hit))
            draw(ray, hit);
    }

    void draw(Ray ray, RaycastHit hit) 
    {
        // El objeto golpeado no es el plano
        string str = hit.transform.gameObject.name;
        if (!(str.Equals("Plane") || str.Equals("Quad"))) 
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            Debug.DrawLine(hit.point, hit.point + 20 * hit.normal, Color.blue);
        }

        // Cambiamos el color, si es posible
        changeColor(hit);
    }

    private GameObject firstThing = null;
    private GameObject secondThing = null;
    private bool firstTime = true;

    MeshRenderer m_Renderer = null;
    Color m_OriginalColor = Color.green;

    void changeColor(RaycastHit hit) 
    {
        string str = hit.transform.gameObject.name;
        if (firstTime && !(str.Equals("Plane") || str.Equals("Quad")))
        {
            firstThing = hit.transform.gameObject;
            m_Renderer = firstThing.GetComponent<MeshRenderer>();
            m_OriginalColor = m_Renderer.material.color;
            m_Renderer.material.color = Color.gray;
            firstTime = false;
            return;
        }

        if (firstThing == null) return;

        secondThing = hit.transform.gameObject;
        if (firstThing == secondThing) return;

        m_Renderer.material.color = m_OriginalColor;

        firstTime = true;
    }
}
