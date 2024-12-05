using UnityEngine;
using System.Collections.Generic;

public class InfluenceMap : MonoBehaviour
{
    public InfluenceGrid grid;  // El Grid predefinido que se asignará desde la UI de Unity
    [SerializeField] public int radio = 5;  // El radio de influencia del personaje
    [SerializeField] public float influenciaBase = 10.0f;  // La influencia base I0
    public enum Faccion { Rojo, Azul };  // Facción del personaje
    [SerializeField] public Faccion faccion;

    private Vector3 posicionAnterior;  // Posición anterior del personaje
    private bool gridInicializado = false;  // Variable para controlar si el grid está listo

    // Diccionario para guardar tiles afectados y su influencia
    private Dictionary<Tile, float> tilesInfluenciados = new Dictionary<Tile, float>();

    void Start()
    {
        // Inicializar la posición anterior al inicio del personaje
        posicionAnterior = transform.position;
    }

    void Update()
    {
        // Verificamos si el Grid está inicializado antes de calcular influencias
        if (!gridInicializado)
        {
            VerificarGridInicializado();
            return;  // Si no está inicializado, no calculamos nada
        }

        // Verificar si el personaje ha cambiado de posición y recalcular influencias
       // if (posicionAnterior != transform.position)
        //{
            // Primero eliminar las influencias anteriores
            //EliminarInfluencias();

            // Luego aplicar las nuevas influencias en el área alrededor de la nueva posición
            ActualizarInfluencias();

            // Actualizar la posición anterior
           // posicionAnterior = transform.position;
        //}
    }

    // Método para verificar si el Grid ya está inicializado
    private void VerificarGridInicializado()
    {
        if (grid != null && grid.estaInicializado && InfluenceManager.Instance.gridInicializado)
        {
            gridInicializado = true;  // El grid ya está listo
            ActualizarInfluencias();  // Podemos aplicar las influencias iniciales
        }
    }

    // Método para eliminar la influencia en todos los tiles previamente influenciados
    private void EliminarInfluencias()
    {
        foreach (var tile in tilesInfluenciados)
        {
            InfluenceManager.Instance.EliminarInfluencia(tile.Key, tile.Value, faccion);
        }
        tilesInfluenciados.Clear();  // Limpiar el diccionario ya que se han eliminado todas las influencias
    }

    // Método auxiliar para calcular la influencia basada en la posición del agente y del tile
    private float CalcularInfluencia(Vector3 agentPosition, Tile tile)
    {
        Vector3 tilePosition = tile.getPosition();  // Posición del tile en el grid
        float distance = Vector3.Distance(new Vector3(agentPosition.x, 0, agentPosition.z), new Vector3(tilePosition.x, 0, tilePosition.z));
        float influence = distance == 0 ? influenciaBase : (influenciaBase / distance) - 1;
        return Mathf.Max(influence, 0);  // Asegura que la influencia no sea negativa
    }

    // Método auxiliar para actualizar la influencia en un tile específico
    private void ActualizarInfluenciasAux(Tile tile, float influence)
    {
        // Actualizar el diccionario y añadir la influencia en el InfluenceManager
        if (tilesInfluenciados.ContainsKey(tile))
        {
            //InfluenceManager.Instance.EliminarInfluencia(tile, influence, faccion);
        }
        tilesInfluenciados[tile] = influence;
        InfluenceManager.Instance.AgregarInfluencia(tile, influence, faccion);
    }

    // Método para actualizar y aplicar la influencia en el área dentro del radio alrededor de la posición actual
    // Método para actualizar y aplicar la influencia en el área dentro del radio alrededor de la posición actual
    private void ActualizarInfluencias()
    {   

        if (!InfluenceManager.Instance.gridInicializado){
            return;
        }
        // Obtenemos la posición actual del personaje
        Vector3 posicionPersonaje = transform.position;

        // Calculamos las coordenadas del tile central en base a la posición actual del personaje en el grid
        Tile tileCentral = grid.getTileByVector(posicionPersonaje);

        // Iteramos sobre los tiles en el área dentro del radio en una cuadrícula
        int radioTiles = Mathf.CeilToInt(radio); // Convertimos el radio a entero para usar en coordenadas de grid
        for (int x = -radioTiles; x <= radioTiles; x++)
        {
            for (int z = -radioTiles; z <= radioTiles; z++)
            {
                // Calculamos la posición del tile actual en relación al tile central
                Tile tileActual = grid.getTile(tileCentral.fila + x, tileCentral.columna + z);

                // Si el tile es nulo o no está dentro del radio, lo saltamos
                if (tileActual == null) continue;

                // Calculamos la distancia del tile actual al personaje
                float distancia = Vector3.Distance(new Vector3(tileActual.getPosition().x, 0, tileActual.getPosition().z),
                                                   new Vector3(posicionPersonaje.x, 0, posicionPersonaje.z));

                // Si la distancia está dentro del radio, calculamos la influencia y la aplicamos
                if (distancia <= radio)
                {
                    float influencia = CalcularInfluencia(posicionPersonaje, tileActual);
                    ActualizarInfluenciasAux(tileActual, influencia);
                }
            }
        }
    }

    public void setFaccion(Faccion faccion){
        this.faccion = faccion;
    }
}
