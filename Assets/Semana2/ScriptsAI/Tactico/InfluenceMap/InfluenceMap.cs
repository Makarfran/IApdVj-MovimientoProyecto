using UnityEngine;
using System.Collections.Generic;

public class InfluenceMap : MonoBehaviour
{
    public Grid grid;  // El Grid predefinido que se asignará desde la UI de Unity
    public int radio = 5;  // El radio de influencia del personaje
    public float influenciaBase = 1.0f;  // La influencia base I0
    public enum Faccion { Rojo, Azul };  // Facción del personaje
    public Faccion faccion;

    private Vector3 posicionAnterior;  // Posición anterior del personaje
    private bool gridInicializado = false;  // Variable para controlar si el grid está listo

    // Diccionario para guardar tiles afectados y su influencia
    private Dictionary<Vector3, float> tilesInfluenciados = new Dictionary<Vector3, float>();

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
        if (posicionAnterior != transform.position)
        {
            // Primero eliminar las influencias anteriores
            EliminarInfluencias();

            // Luego aplicar las nuevas influencias en el área alrededor de la nueva posición
            ActualizarInfluencias();

            // Actualizar la posición anterior
            posicionAnterior = transform.position;
        }
    }

    // Método para verificar si el Grid ya está inicializado
    private void VerificarGridInicializado()
    {
        if (grid != null && grid.estaInicializado)
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
    private float CalcularInfluencia(Vector3 agentPosition, Vector3 tilePosition)
    {
        float distance = Vector3.Distance(new Vector3(agentPosition.x, 0, agentPosition.z), new Vector3(tilePosition.x, 0, tilePosition.z));
        float influence = distance == 0 ? influenciaBase : (influenciaBase / distance) - 1;
        return Mathf.Max(influence, 0);  // Asegura que la influencia no sea negativa
    }

    // Método auxiliar para actualizar la influencia en un tile específico
    private void ActualizarInfluenciasAux(Vector3 tilePosition, float influence)
    {
        // Actualizar el diccionario y añadir la influencia en el InfluenceManager
        if (tilesInfluenciados.ContainsKey(tilePosition)){
            InfluenceManager.Instance.EliminarInfluencia(tilePosition,influence,faccion);
        }
        tilesInfluenciados[tilePosition] = influence;
        InfluenceManager.Instance.AgregarInfluencia(tilePosition, influence, faccion);
    }

    // Método para actualizar y aplicar la influencia en el área dentro del radio alrededor de la posición actual
    private void ActualizarInfluencias()
    {
        if (!gridInicializado) return;

        Vector3 agentPosition = transform.position;

        for (int x = -radio; x <= radio; x++)
        {
            for (int z = -radio; z <= radio; z++)
            {
                Vector3 tilePosition = new Vector3(agentPosition.x + x, agentPosition.y, agentPosition.z + z);
                Tile tile = grid.getTileByVector(tilePosition);

                if (tile != null)
                {   
                    tilePosition = tile.getPosition();  // Posición del tile en el grid
                    float influence = CalcularInfluencia(agentPosition, tilePosition);
                    if (influence > 0)
                    {
                        // Utilizar el método auxiliar para actualizar la influencia en el tile
                        ActualizarInfluenciasAux(tilePosition, influence);
                    }
                }
            }
        }
    }
}
