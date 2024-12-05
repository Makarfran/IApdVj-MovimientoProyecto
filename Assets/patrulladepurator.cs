using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrulladepurator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateDepuration(){
        foreach(Transform child in transform)
        {
	        child.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void deactivateDepuration(){
        foreach(Transform child in transform)
        {
	        child.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
