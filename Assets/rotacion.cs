using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotacion : MonoBehaviour
{
    public float valor = 0;

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.AngleAxis(valor, Vector3.up); // crece de x a z o z a x según referencia

        //transform.rotation = Quaternion.Euler(0, valor, 0); // de x a z o z a x según referencia

         transform.rotation = new Quaternion(); //Quaternion.identity; o z a x según referencia
         transform.Rotate(Vector3.up, valor);
        valor++;
    }
}
