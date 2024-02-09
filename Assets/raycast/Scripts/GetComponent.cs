using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetComponent : MonoBehaviour
{
    public float mass = 0; //public information
    public Material material = null; // public information

    Rigidbody rb = null; //Reference of the RigidBody
    MeshRenderer mr = null; //Reference of the Mesh Renderer

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //Get rigidbody from this.GameObject
        mr = GetComponent<MeshRenderer>(); // Get MeshRenderer from this.GameObject
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null) rb.mass = mass; // Modify component RigidBody and observe
        if (mr != null) mr.material = material; //Modify component MeshRenderer and observe
    }
}
