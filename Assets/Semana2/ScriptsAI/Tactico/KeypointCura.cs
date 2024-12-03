using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypointCura : Keypoint
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<InfluenceManager>()){
            int inf = getInfluenceValue(this.transform.position);
            if(inf == 1){
                Bando = "A";
            } else if (inf = -1){
                Bando = "R";
            } else {
                Bando = "None";
            }
        }
        
    }

    public int getInfluenceValue(Vector3 vec){
        InfluenceManager influenceManager = FindObjectOfType<InfluenceManager>();
        InfluenceMap influenceMap = FindObjectOfType<InfluenceMap>();
        Grid infGrid = influenceManager.getGrid();

        Tile tile = infGrid.getTileByVector(vec);
        float maxinf = influenceManager.getMaxInf();
        
        float inf = influenceManager.getInfluenceTile(tilePosition, InfluenceMap.Azul);
        if(inf < 0){
            if(inf < (3/4)*maxinf){
                return 0;
            } else{
                return 1;
            }
        } else if(inf == 0){
            return 0;
        } else {
            if(inf > -(3/4)*maxinf){
                return 0;
            } else {
                return -1;
            }
        }
    }
}
