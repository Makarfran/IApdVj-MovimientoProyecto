using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate alrededor del eje y del mundo
        transform.Rotate(Vector3.up * Time.deltaTime * 500, Space.World);
    }
}
