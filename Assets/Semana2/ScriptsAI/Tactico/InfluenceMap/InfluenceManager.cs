using UnityEngine;
using System.Collections.Generic;

public class InfluenceManager : MonoBehaviour
{
    public static InfluenceManager Instance { get; private set; }

    // Prefab para representar la influencia en cada tile
    public GameObject tileVisualPrefab;

    // Diccionarios para almacenar la influencia en cada tile
    private Dictionary<Vector3, float> influenciaRojo = new Dictionary<Vector3, float>();
    private Dictionary<Vector3, float> influenciaAzul = new Dictionary<Vector3, float>();

    // Diccionario que almacenará el GameObject visual asociado a cada tile
    private Dictionary<Vector3, GameObject> visualTiles = new Dictionary<Vector3, GameObject>();

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

    // Método para agregar influencia a un tile
    public void AgregarInfluencia(Vector3 position, float influence, InfluenceMap.Faccion faccion)
    {
        if (faccion == InfluenceMap.Faccion.Rojo)
        {
            if (influenciaRojo.ContainsKey(position))
                influenciaRojo[position] += influence;
            else
                influenciaRojo[position] = influence;
        }
        else if (faccion == InfluenceMap.Faccion.Azul)
        {
            if (influenciaAzul.ContainsKey(position))
                influenciaAzul[position] += influence;
            else
                influenciaAzul[position] = influence;
        }

        // Actualizar visualización
        ActualizarVisualTile(position);
    }

    // Método para eliminar influencia de un tile
    public void EliminarInfluencia(Vector3 position, float influence, InfluenceMap.Faccion faccion)
    {
        Debug.Log("eliminando tile " + position);
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
    private void ActualizarVisualTile(Vector3 position)
    {
        // Ajustar la posición visual del tile para el eje Z como altura y Y constante
        Vector3 visualPosition = new Vector3(position.x, position.y + 1.5f, position.z  );

        // Obtener o crear el objeto visual asociado al tile
        if (!visualTiles.ContainsKey(position))
        {
            GameObject tileVisual = Instantiate(tileVisualPrefab, visualPosition, Quaternion.Euler(90f, 0f, 0f));
            visualTiles[position] = tileVisual;

            // Añadimos esto para asegurarnos de que se están creando correctamente
           // Debug.Log($"Tile visual creado en la posición {visualPosition}");
        }

        GameObject visual = visualTiles[position];
        SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();

        // Obtener las influencias de las dos facciones
        float influenciaRojoVal = influenciaRojo.ContainsKey(position) ? influenciaRojo[position] : 0f;
        float influenciaAzulVal = influenciaAzul.ContainsKey(position) ? influenciaAzul[position] : 0f;

        // Normalizar las influencias para que sumen 1
        float totalInfluencia = influenciaRojoVal + influenciaAzulVal;
        float rojo = totalInfluencia > 0 ? influenciaRojoVal / totalInfluencia : 0f;
        float azul = totalInfluencia > 0 ? influenciaAzulVal / totalInfluencia : 0f;

        // Asignar un color basado en la influencia
        Color color = new Color(rojo, 0f, azul, 0.5f);  // El verde es 0 porque no se usa
        renderer.color = color;

        // Añadimos un Debug para asegurarnos de que el color se está asignando correctamente
        //Debug.Log($"Tile en {position} con influencia roja: {rojo}, azul: {azul}, color asignado: {color}");
    }
}
