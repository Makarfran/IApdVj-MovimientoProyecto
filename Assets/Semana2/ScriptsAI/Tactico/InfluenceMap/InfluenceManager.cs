using UnityEngine;
using System.Collections.Generic;

public class InfluenceManager : MonoBehaviour
{
    public static InfluenceManager Instance { get; private set; }

    // Prefab para representar la influencia en cada tile
    public GameObject tileVisualPrefab;
    [SerializeField] public InfluenceGrid gird;

    // Diccionarios para almacenar la influencia en cada tile
    private Dictionary<Tile, float> influenciaRojo = new Dictionary<Tile, float>();
    private Dictionary<Tile, float> influenciaAzul = new Dictionary<Tile, float>();

    // Tiempo total en segundos para que la influencia se reduzca a cero
    [SerializeField]
    public float seconds = 10.0f;
    public bool gridInicializado = false;  // Variable para controlar si el grid está listo

    // Tiempo total en segundos para que la influencia se reduzca a cero
    [SerializeField]
    public int maxInfluence = 10;
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


    void Start()
    {


    }
    void Update()
    {


        // Verificamos si el Grid está inicializado antes de calcular influencias
        if (!gridInicializado)
        {
            VerificarGridInicializado();
            return;  // Si no está inicializado, no calculamos nada
        }

        // Calculamos el decremento de influencia en cada frame
        float decrementoPorFrame = (maxInfluence / seconds) * Time.deltaTime;

        // Actualizar la influencia para cada tile en el equipo rojo
        ActualizarInfluenciaDiccionario(influenciaRojo, decrementoPorFrame, InfluenceMap.Faccion.Rojo);

        // Actualizar la influencia para cada tile en el equipo azul
        ActualizarInfluenciaDiccionario(influenciaAzul, decrementoPorFrame, InfluenceMap.Faccion.Azul);
    }

    private void ActualizarInfluenciaDiccionario(Dictionary<Tile, float> influenciaDiccionario, float decrementoPorSegundo, InfluenceMap.Faccion faccion)
    {
        // Creamos una lista de claves para iterar sin modificar el diccionario directamente
        List<Tile> tiles = new List<Tile>(influenciaDiccionario.Keys);
        foreach (var tileInfluence in tiles)
        {
            EliminarInfluencia(tileInfluence, decrementoPorSegundo, faccion);
        }
    }


    // Método para agregar influencia a un tile
    public void AgregarInfluencia(Tile position, float influence, InfluenceMap.Faccion faccion)
    {
        influence = Mathf.Min(maxInfluence, influence);
        if (faccion == InfluenceMap.Faccion.Rojo)
        {   
            influenciaRojo[position] = Mathf.Min(maxInfluence, influenciaRojo[position] + influence);
        }
        else if (faccion == InfluenceMap.Faccion.Azul)
        {
            influenciaAzul[position] = Mathf.Min(maxInfluence, influenciaAzul[position] + influence);
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
            if (influenciaRojo[position] < 0)
                influenciaRojo[position] = 0;
        }
        else if (faccion == InfluenceMap.Faccion.Azul && influenciaAzul.ContainsKey(position))
        {
            influenciaAzul[position] -= influence;
            if (influenciaAzul[position] < 0)
                influenciaAzul[position] = 0 ;
        }

        // Actualizar visualización
        ActualizarVisualTile(position);
    }

    // Método para actualizar el color del tile visual basado en las influencias
    private void ActualizarVisualTile(Tile position)
    {

        // Obtener las influencias de las dos facciones
        float rojo = influenciaRojo.ContainsKey(position) ? influenciaRojo[position] / maxInfluence : 0f;
        float azul = influenciaAzul.ContainsKey(position) ? influenciaAzul[position] / maxInfluence : 0f;

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

    public Dictionary<Tile, float> getInfluenceMap(InfluenceMap.Faccion faccion)
    {
        Dictionary<Tile, float> influencia = new Dictionary<Tile, float>();

        foreach (var tile in influenciaRojo)
        {
            if (faccion == InfluenceMap.Faccion.Rojo)
            {
                influencia[tile.Key] = tile.Value - influenciaAzul[tile.Key];
            }
            else
            {
                influencia[tile.Key] = influenciaAzul[tile.Key] - tile.Value;
            }

        }
        return influencia;
    }


    private void VerificarGridInicializado()
    {
        if (gird != null && gird.estaInicializado)
        {
            gridInicializado = true;  // El grid ya está listo
            foreach (Tile tile in gird.posiciones)
            {
                influenciaAzul[tile] = 0;
                influenciaRojo[tile] = 0;
            }

        }
    }


}
