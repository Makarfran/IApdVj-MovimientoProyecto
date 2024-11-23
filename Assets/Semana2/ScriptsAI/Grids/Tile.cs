using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Tile : MonoBehaviour
{
    public int fila, columna;
    public bool pasable = true;
    public Vector3 pos;
    public float gCoste;
    public int hCoste;
    public int tempHCoste;
    public float fCoste;
    public Tile tilePadre;
    public String tipo;

    public Text textComponent;



    void Start()
    {
        
        // Obtener la capa del objeto actual
        int layerIndex = gameObject.layer;
        if (layerIndex != 5)
        {
            // Define el tamaño de la caja de colisión
            Vector3 boxSize = GetComponent<Collider>().bounds.size;
            pasable = true;
            // Verifica si este objeto está en contacto con un obstáculo
            Collider[] colliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Obstaculos"))
                {
                    // Si este objeto está en contacto con un obstáculo, invoca setImpasable()
                    // Debug.Log("Tile: "+fila +" "+columna+" choca");
                    pasable = false;
                    CambiarColorARojo();
                    break;
                }
                else if (collider.gameObject.CompareTag("Camino"))
                {
                    tipo = "Camino";

                }
                else if (collider.gameObject.CompareTag("Desierto") && tipo != "Camino")
                {
                    tipo = "Desierto";
                }
                else 
                {
                    tipo = "Hierba";
                }
            }
        }
        else
        {
            Renderer renderer = GetComponent<Renderer>();
            // Asignar un color basado en la influencia
            Color color = new Color(0f, 0f, 0f, 0.5f);  // El verde es 0 porque no se usa
            renderer.material.color = color;
        }


    }

    public void setPasable(bool pasable)
    {
        this.pasable = pasable;
    }

    public String getTipo()
    {
        return tipo;
    }

    public void calcularFCoste()
    {
        fCoste = gCoste + hCoste;

    }
    public void setPos(Vector3 pos)
    {
        this.pos = pos;
    }

    public Vector3 getPosition()
    {
        return pos;
    }

    public void setText(int text)
    {
        textComponent.text = text.ToString();
    }

    public void CambiarColorARojo()
    {
        // Obtén el Renderer del objeto para acceder a su material
        Renderer renderer = GetComponent<Renderer>();

        // Cambia el color del material a rojo
        renderer.material.color = Color.red;
    }

    public void CambiarColorVerde()
    {
        // Obtén el Renderer del objeto para acceder a su material
        Renderer renderer = GetComponent<Renderer>();

        // Cambia el color del material a rojo
        renderer.material.color = Color.green;
    }

    public void setColor(Color color)
    {
        Renderer renderer = GetComponent<Renderer>();
        // Asignar un color basado en la influencia
        renderer.material.color = color;
    }
}
