using UnityEngine;
using System.Collections.Generic;

public class InfluenceManager : MonoBehaviour
{
    public static InfluenceManager Instance { get; private set; }

    // Prefab para representar la influencia en cada tile
    public GameObject tileVisualPrefab;

    // Diccionarios para almacenar la influencia en cada tile
    private Dictionary<Tile, float> influenciaRojo = new Dictionary<Tile, float>();
    private Dictionary<Tile, float> influenciaAzul = new Dictionary<Tile, float>();

    // Tiempo total en segundos para que la influencia se reduzca a cero
    [SerializeField]
    public float seconds = 10.0f;

    // Tiempo total en segundos para que la influencia se reduzca a cero
    [SerializeField]
    public int maxInfluence = 1000;    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        // Calculamos el decremento de influencia en cada frame
        float decrementoPorFrame = (maxInfluence / seconds) * Time.deltaTime;

        // Actualizar la influencia para cada tile en el equipo rojo
        ActualizarInfluenciaDiccionario(influenciaRojo, decrementoPorFrame,InfluenceMap.Faccion.Rojo);

        // Actualizar la influencia para cada tile en el equipo azul
        ActualizarInfluenciaDiccionario(influenciaAzul, decrementoPorFrame,InfluenceMap.Faccion.Azul);
    }

  private void ActualizarInfluenciaDiccionario(Dictionary<Tile, float> influenciaDiccionario, float decrementoPorSegundo, InfluenceMap.Faccion faccion)
    {
            // Creamos una lista de claves para iterar sin modificar el diccionario directamente
        List<Tile> tiles = new List<Tile>(influenciaDiccionario.Keys);
        foreach (var tileInfluence in tiles)
        {   
            EliminarInfluencia(tileInfluence,decrementoPorSegundo,faccion);
        }
    }    


    // Método para agregar influencia a un tile
    public void AgregarInfluencia(Tile position, float influence, InfluenceMap.Faccion faccion)
    {
        influence = Mathf.Min(maxInfluence, influence);
        if (faccion == InfluenceMap.Faccion.Rojo)
        {
            if (influenciaRojo.ContainsKey(position))
                influenciaRojo[position]  = Mathf.Min(maxInfluence, influenciaRojo[position] + influence);
            else
                influenciaRojo[position] = influence;
        }
        else if (faccion == InfluenceMap.Faccion.Azul)
        {
            if (influenciaAzul.ContainsKey(position))
                influenciaAzul[position] = Mathf.Min(maxInfluence, influenciaAzul[position] + influence);
            else
                influenciaAzul[position] = influence;
        }

        // Actualizar visualización
        ActualizarVisualTile(position);
    }

    // Método para eliminar influencia de un tile
    public void EliminarInfluencia(Tile position, float influence, InfluenceMap.Faccion faccion)
    {
        //Debug.Log("eliminando tile " + position);
        if (faccion == InfluenceMap.Faccion.Rojo && influenciaRojo.ContainsKey(position))
        {
            influenciaRojo[position] -= influence;
            if (influenciaRojo[position] <= 0)
                influenciaRojo.Remove(position);
        }
        else if (faccion == InfluenceMap.Faccion.Azul && influenciaAzul.ContainsKey(position))
        {
            influenciaAzul[position] -= influence;
            if (influenciaAzul[position] <= 0)
                influenciaAzul.Remove(position);
        }

        // Actualizar visualización
        ActualizarVisualTile(position);
    }

    // Método para actualizar el color del tile visual basado en las influencias
    private void ActualizarVisualTile(Tile position)
    {

        // Obtener las influencias de las dos facciones
        float rojo = influenciaRojo.ContainsKey(position) ? influenciaRojo[position]/maxInfluence : 0f;
        float azul = influenciaAzul.ContainsKey(position) ? influenciaAzul[position]/maxInfluence : 0f;

        // Normalizar las influencias para que sumen 1
        //float totalInfluencia = influenciaRojoVal + influenciaAzulVal;
        //float rojo = totalInfluencia > 0 ? influenciaRojoVal  : 0f;
        //float azul = totalInfluencia > 0 ? influenciaAzulVal  : 0f;

        // Asignar un color basado en la influencia
        Color color = new Color(rojo, azul, 0f, 0.5f);  // El verde es 0 porque no se usa
        position.setColor(color);

        // Añadimos un Debug para asegurarnos de que el color se está asignando correctamente
        //Debug.Log($"Tile en {position} con influencia roja: {rojo}, azul: {azul}, color asignado: {color}");
    }
}
