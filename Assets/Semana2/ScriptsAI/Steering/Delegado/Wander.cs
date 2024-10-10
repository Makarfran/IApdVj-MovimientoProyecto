using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Face // debe heredar de Face
{
    [SerializeField]
    protected float wanderOffSet; // separación desde el npc. Centro del circulo
    [SerializeField]
    protected float wanderRadius; // radio del círculo
    [SerializeField]
    protected float wanderRate; // max radians o degree en el que calcular los puntos aleatorios.
    protected float wanderOrientation; // current orientation del punto aleatorio calculado.
    [SerializeField]
    protected float maxAcceleration;
    protected GameObject delegatedAgent;
    public bool ModoDep;

    // Material para dibujar las líneas y la circunferencia con GL
    private Material lineMaterial;

    void Start()
    {
        delegatedAgent = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        if (!ModoDep)
        {
            delegatedAgent.GetComponent<MeshRenderer>().enabled = false;
        }

        delegatedAgent.AddComponent<Agent>();
        this.nameSteering = "Wander";
        this.Weight = 1f;

        CreateLineMaterial(); // Crear material para las líneas GL
    }

    void Update()
    {
        // Obtener el agente principal
        Agent agent = GetComponent<Agent>();

        // Actualizar el objetivo y la orientación en cada frame
        GetSteering(agent);
    }

    public override Steering GetSteering(Agent agent)
    {
        // % de wanderRate
        wanderOrientation = RandomBinomial() * wanderRate;
        float targetOrientation = wanderOrientation + agent.Orientation;

        // center of the wander circle
        Vector3 target = agent.Position + wanderOffSet * agent.OrientationToVector();

        // target location
        target += (wanderRadius * OrientationToVector(targetOrientation));

        // delegación al Steering de Face
        this.Rtarget = delegatedAgent.GetComponent<Agent>();
        this.Rtarget.Position = target;
        this.Rtarget.Orientation = targetOrientation;

        Steering steering = base.GetSteering(agent);
        steering.linear = maxAcceleration * agent.OrientationToVector();
        return steering;
    }

    private float RandomBinomial()
    {
        return Random.value - Random.value;
    }

    private Vector3 OrientationToVector(float orientation)
    {
        return new Vector3(Mathf.Cos(Bodi.MapToRangePi(orientation)), 0, Mathf.Sin(Bodi.MapToRangePi(orientation)));
    }

    // Método para dibujar la circunferencia y las líneas en tiempo de ejecución usando GL
    void OnRenderObject()
    {
        if (ModoDep)
        {
            Agent agent = GetComponent<Agent>();

            // Dibujar la circunferencia azul alrededor del personaje
            DrawCircle(agent.Position + wanderOffSet * agent.OrientationToVector(), wanderRadius, Color.blue);

            // Dibujar la línea roja que muestra la dirección del personaje
            Vector3 viewDirection = agent.OrientationToVector() * 2.0f; // El 2.0f define la longitud del rayo
            DrawLine(agent.Position, agent.Position + viewDirection, Color.red);

            // Dibujar la línea verde hacia el objetivo
            if (Rtarget != null)
            {
                DrawLine(agent.Position, Rtarget.Position, Color.green);
            }
        }
    }

    // Crear material para usar con GL
    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity built-in shader para líneas simples
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Método para dibujar una línea en tiempo de ejecución usando GL
    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        lineMaterial.SetPass(0); // Activar el material para dibujar con GL

        GL.Begin(GL.LINES);
        GL.Color(color);
        GL.Vertex(start);
        GL.Vertex(end);
        GL.End();
    }

    // Método para dibujar una circunferencia en tiempo de ejecución usando GL
    void DrawCircle(Vector3 center, float radius, Color color)
    {
        int segments = 50; // Número de segmentos para aproximar el círculo
        float angleIncrement = 2 * Mathf.PI / segments;

        lineMaterial.SetPass(0); // Activar el material para dibujar con GL

        GL.Begin(GL.LINES);
        GL.Color(color);

        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleIncrement;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            GL.Vertex(prevPoint);
            GL.Vertex(nextPoint);

            prevPoint = nextPoint;
        }

        GL.End();
    }

    // *** Añadimos la ayuda visual para depuración en el editor ***
    void OnDrawGizmos()
    {
        if (ModoDep)
        {
            Agent agent = GetComponent<Agent>();

            // Rayo rojo: Dirección de vista del personaje
            Gizmos.color = Color.red;
            Vector3 direction = agent.OrientationToVector();
            Gizmos.DrawRay(agent.Position, direction * 2.0f); // El 2.0f define la longitud del rayo

            // Rayo verde: Desde el personaje al objetivo
            if (Rtarget != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(agent.Position, Rtarget.Position);
            }

            // Circunferencia azul: Definir el área de wander
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(agent.Position + wanderOffSet * agent.OrientationToVector(), wanderRadius);
        }
    }
}
