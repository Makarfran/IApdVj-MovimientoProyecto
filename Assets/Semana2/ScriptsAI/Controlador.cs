using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    public GameObject R;
    public GameObject A;
    public List<GameObject> teamR = new List<GameObject>();
    public List<GameObject> teamA = new List<GameObject>();
    public GameObject baseRoja;
    public GameObject baseAzul;
    public List<GameObject> basesInicioTeamR = new List<GameObject>();
    public List<GameObject> basesInicioTeamA = new List<GameObject>();
    public List<GameObject> basesTeamR = new List<GameObject>();
    public List<GameObject> basesTeamA = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < R.transform.childCount; i++)
        {
            teamR.Add(R.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < A.transform.childCount; i++)
        {
            teamA.Add(A.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
