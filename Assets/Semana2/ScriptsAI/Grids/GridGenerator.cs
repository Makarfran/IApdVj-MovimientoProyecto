using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Esto es mas para generar grids que otra cosa
public class GridGenerator : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] protected int a;
    [SerializeField] protected int b;
    [SerializeField] protected float longLado;
    [SerializeField] protected GameObject obj;
    protected Tile[,] posiciones;
    [SerializeField] protected List<Vector2> impasables;
    [SerializeField] protected List<Vector2>  coste2;
    [SerializeField] protected List<Vector2>  coste3;
    [SerializeField] public Text numberText;
    [SerializeField] public Canvas canvas;
    void Start(){
        posiciones = new Tile[a,b];
        int count = 0;
        for(int i = 0; i < a; i++ ){ 
            for(int j = 0; j < b ; j++ ){
                Vector3 pos = new Vector3(this.transform.position.x + j*longLado, this.transform.position.y + i*longLado, this.transform.position.z  );
                var spanw = Instantiate(obj, pos, Quaternion.identity);
                spanw.name = $"Tile {i} {j}";
                spanw.GetComponent<Tile>().setPos(pos);
                
                //GameObject obj = new GameObject($"Tile {i} {j}");
               
                                // Agregar un componente de texto al GameObject
                

                // Configurar el componente de texto
                 spanw.GetComponent<Tile>().textComponent = spanw.AddComponent<Text>();
               spanw.GetComponent<Tile>().textComponent.text = (count).ToString();
                count++;
                spanw.GetComponent<Tile>().textComponent.font = numberText.font;
                spanw.GetComponent<Tile>().textComponent.fontSize = numberText.fontSize;
                spanw.GetComponent<Tile>().textComponent.color = numberText.color;
                spanw.GetComponent<Tile>().textComponent.transform.position = pos; 
                spanw.GetComponent<Tile>().textComponent.transform.SetParent(canvas.transform, false);
                
            }
        }
    }  

    
}