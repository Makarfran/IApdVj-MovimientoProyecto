using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouse : MonoBehaviour
{
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();    
    }

    void OnMouseEnter() // el mesh (malla) se vuelve rojo cuando el ratón esta encima ... 
    {
        rend.material.color = new Color(1, 0, 0); //Color.red
    }

    void OnMouseOver() // ...el rojo se intercambia a azul cuando el ratón ya no esta encima
    {
        rend.material.color += new Color(-.5f, 0, .5f) * Time.deltaTime;
    }

    void OnMouseExit() // ... y la malla finalmente se vuelve blanca cuando el ratón se va
    {
        rend.material.color = Color.white;
    }
}
