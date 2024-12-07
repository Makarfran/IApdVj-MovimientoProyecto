using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypointCura : Keypoint
{
    public InfluenceManager infman;
    //public InfluenceMap infmap;
    // Start is called before the first frame update
    void Start()
    {
        infman = FindObjectOfType<InfluenceManager>();
        //infmap = FindObjectOfType<InfluenceMap>();
        //Bando = "None";
    }

    // Update is called once per frame
    void Update()
    {
        
        if (infman != null)
        {

            int inf = getInfluenceValue(this.transform.position);
            
            if (inf == 1)
            {
                Bando = "A";
            }
            else if (inf == -1)
            {
                Bando = "R";
            }
            else
            {
                Bando = "None";
            }
        }
    }

    public int getInfluenceValue(Vector3 vec)
    {

        float maxinf = infman.getMaxInf();

        float inf = infman.getInfluenceTile(vec, InfluenceMap.Faccion.Azul);
        
        if (inf < 0)
        {
            if (inf < (3 / 4) * maxinf)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else if (inf == 0)
        {
            return 0;
        }
        else
        {
            if (inf > -(3 / 4) * maxinf)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}