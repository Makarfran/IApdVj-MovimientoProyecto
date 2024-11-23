using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    public GameObject A;
    public GameObject B;
    public List<GameObject> teamA = new List<GameObject>();
    public List<GameObject> teamB = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < A.transform.childCount; i++)
        {
            teamA.Add(A.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < B.transform.childCount; i++)
        {
            teamB.Add(B.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
