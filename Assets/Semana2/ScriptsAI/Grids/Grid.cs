using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Grid : MonoBehaviour
{
    

    [SerializeField] protected int GridWidth; //cuadrados eje x
    [SerializeField] protected int GridHeight; //eje y
    protected float cellW; //ancho de la celda
    protected float cellH; //Alto de la celda
    protected Vector2[,] array2;

    // Start is called before the first frame update
    void Start()
    {
        

        if(GridWidth != 0){
            this.cellW = 100 / (float)GridWidth;
        }

        if(GridHeight != 0){
            this.cellH =  100 / (float)GridWidth;
        }
        array2 = new Vector2[GridWidth, GridHeight];
        float x = 0f;
        float y = 0f;
        for(int j = 0; j < GridHeight-1; j++){
            for(int i = 0; i < GridWidth-1; i++){
                array2[i,j] = new Vector2(x,y);
                x += cellW; 
            }
            x = 0f;
            y += cellH;
        }

        Debug.Log(array2[3,2].x);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
