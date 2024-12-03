using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InfluenceManager : MonoBehaviour
{
    public static InfluenceManager Instance { get; private set; }

    // Prefab para representar la influencia en cada tile
    [SerializeField] public GameObject tileVisualPrefab;
    [SerializeField] public InfluenceGrid gird;

    // Diccionarios para almacenar la influencia en cada tile
    private Dictionary<Tile, float> influenciaRojo = new Dictionary<Tile, float>();
    private Dictionary<Tile, float> influenciaAzul = new Dictionary<Tile, float>();
    private Dictionary<Tile, float> mapaRojo;
    private Dictionary<Tile, float> mapaAzul;
    // Tiempo total en segundos para que la influencia se reduzca a cero
    [SerializeField]
    public float seconds = 10.0f;
    public bool gridInicializado = false;  // Variable para controlar si el grid está listo

    // Tiempo total en segundos para que la influencia se reduzca a cero
    [SerializeField]
    public int maxInfluence = 10;

    [SerializeField] public int updateEach = 10; // Tiempo en segundos para actualizar la influencia    
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
        // Iniciamos la corrutina
        StartCoroutine(UpdateInfluenceMapRoutine());
        StartCoroutine(UpdateInfluenceColorRoutine());
    }

    public InfluenceGrid getGrid(){
        return gird;
    }

    public float getMaxInf(){
        return maxInfluence;
    }
    // Corrutina que llama a updateInfluenceMap cada cierto tiempo
    IEnumerator UpdateInfluenceMapRoutine()
    {
        while (true)
        {
            if (!gridInicializado)
            {
                yield return new WaitForSeconds(1); // Espera el tiempo especificado    
            }

            updateInfluenceMap(); // Llamada a la función principal
            yield return new WaitForSeconds(updateEach); // Espera el tiempo especificado
        }
    }

    IEnumerator UpdateInfluenceColorRoutine()
    {
        while (true)
        {
            Debug.Log("corrutina1");
            // Verificamos si el Grid está inicializado antes de calcular influencias
            if (!gridInicializado)
            {
                VerificarGridInicializado();
                yield return new WaitForSeconds(1);
            }

            // Calculamos el decremento de influencia en cada frame
            float decrementoPorFrame = (maxInfluence / seconds) * updateEach;

            // Actualizar la influencia para cada tile en el equipo rojo
            ActualizarInfluenciaDiccionario(influenciaRojo, decrementoPorFrame, InfluenceMap.Faccion.Rojo);

            // Actualizar la influencia para cada tile en el equipo azul
            ActualizarInfluenciaDiccionario(influenciaAzul, decrementoPorFrame, InfluenceMap.Faccion.Azul);
  
            yield return new WaitForSeconds(3); // Espera el tiempo especificado
        }
    }


    void Update()
    {


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
                influenciaAzul[position] = 0;
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

    private void updateInfluenceMap()
    {
        mapaRojo = new Dictionary<Tile, float>();
        mapaAzul = new Dictionary<Tile, float>();

        foreach (var tile in influenciaRojo)
        {
            mapaRojo[tile.Key] = tile.Value - influenciaAzul[tile.Key];
            mapaAzul[tile.Key] = influenciaAzul[tile.Key] - tile.Value;

        }

    }
    public Dictionary<Tile, float> getInfluenceMap(InfluenceMap.Faccion faccion)
    {

        if (faccion == InfluenceMap.Faccion.Rojo)
        {
            return mapaRojo;
        }
        else
        {
            return mapaAzul;
        }

    }

    public float getInfluenceTile(Vector3 tilePosition, InfluenceMap.Faccion faccion ){
        Tile tile = gird.getTileByVector(tilePosition);
        if (tile == null){
            return 0;
        }

        switch (faccion)
        {   
            case InfluenceMap.Faccion.Rojo:
                return mapaRojo[tile];

            default:
                return mapaAzul[tile];
        }
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
