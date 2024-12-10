using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentNPC : Agent
{
    // Este será el steering final que se aplique al personaje.
    public string bando;
    [SerializeField] protected Steering steer;
    // Todos los steering que tiene que calcular el agente.
    private List<SteeringBehaviour> listSteerings;
    private bool ModoDep;
    [SerializeField] protected int maxVida;
    [SerializeField] protected float vida;
    [SerializeField] protected int atq;
    [SerializeField] public float costeAtaque = 200f;
    protected float range;
    public float tam = 1;
    [SerializeField] protected Grid grid;

    protected string tipoUnidad = "";
    [SerializeField] Vector3 respawnPosition;
    protected int respawnTime;


    [SerializeField] public GameObject bocadilloCube;



    // Factores entre tipos de unidades
    public Dictionary<string, Dictionary<string, float>> UnitAttackFactor = new Dictionary<string, Dictionary<string, float>>()
    {
        { "Scout", new Dictionary<string, float> { { "Scout", 1f }, { "Infanteria", 1.5f }, { "Elite", 1f } } },
        { "Infanteria", new Dictionary<string, float> { { "Scout", 1.75f }, { "Infanteria", 1f }, { "Elite", 0.25f } } },
        { "Elite", new Dictionary<string, float> { { "Scout", 1.75f }, { "Infanteria", 1.5f }, { "Elite", 1f } } }
    };

    // Factores de ataque por terreno
    public Dictionary<string, Dictionary<string, float>> TerrainAttackFactor = new Dictionary<string, Dictionary<string, float>>()
    {
        { "Hierba", new Dictionary<string, float> { { "Scout", 1.25f }, { "Infanteria", 0.75f }, { "Elite", 1f } } },
        { "Desierto", new Dictionary<string, float> { { "Scout", 2f }, { "Infanteria", 1f }, { "Elite", 1f } } },
        { "Camino", new Dictionary<string, float> { { "Scout", 2f }, { "Infanteria", 1f }, { "Elite", 0.25f } } }
    };

    // Factores de defensa por terreno
    public Dictionary<string, Dictionary<string, float>> TerrainDefenseFactor = new Dictionary<string, Dictionary<string, float>>()
    {
        { "Hierba", new Dictionary<string, float> { { "Scout", 1.75f }, { "Infanteria", 1f }, { "Elite", 0.5f } } },
        { "Desierto", new Dictionary<string, float> { { "Scout", 0.75f }, { "Infanteria", 1.25f }, { "Elite", 1f } } },
        { "Camino", new Dictionary<string, float> { { "Scout", 0.75f }, { "Infanteria", 0.75f }, { "Elite", 1f } } }
    };

    public string getTipo() 
    {
        return tipoUnidad;
    }

    public string getBando()
    {
        return bando;
    }

    public float getTam()
    {
        return tam;
    }

    public float getVida()
    {
        return vida;
    }

    public float getMaxVida()
    {
        return maxVida;
    }

    public float getRange()
    {
        return range;
    }

    public void setGrid(Grid g)
    {
        grid = g;
    }

    protected void Awake()
    {
        this.steer = new Steering();


        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        // La llamaremos listSteerings
        // Puedes usar GetComponents<>()
        SteeringBehaviour[] steers = GetComponents<SteeringBehaviour>();
        listSteerings = new List<SteeringBehaviour>(steers);
    }

    // Use this for initialization
    protected virtual void Start()
    {
        this.Velocity = Vector3.zero;

        //this.generateGrid();
        if (bando == "R"){
            respawnPosition = new Vector3(-42.9f,0f,0.6f);
        } else {
            respawnPosition = new Vector3(52.8f,0f,-9.8f);
        }
        bocadilloCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bocadilloCube.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Reducir el tamaño del cubo
                // Hacer que el cubo sea hijo del NPC
        bocadilloCube.transform.SetParent(transform);

        // Ajustar la posición local del cubo (respecto al NPC)
        bocadilloCube.transform.localPosition = new Vector3(-0.8f, 1.5f, 0f);
        //bocadilloCube.GetComponent<Renderer>().material.color = Color.green;
        bocadilloCube.GetComponent<Renderer>().enabled = false;
        bocadilloCube.GetComponent<Collider>().enabled = false;
        //changeColorCuracion();
        //changeColorPatrullar();
        //changeColorConquista();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // En cada frame se actualiza el movimiento
        ApplySteering(Time.deltaTime);
        if (ModoDep)
        {
            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), new Vector3(this.transform.position.x + Acceleration.x, this.transform.position.y + Acceleration.y + 3f, this.transform.position.z + Acceleration.z), Color.blue);
        }
        //recuperarVida();
        // En cada frame podría ejecutar otras componentes IA
    }

    private void ApplySteering(float deltaTime)
    {

        Acceleration = this.steer.linear;
        // Actualizar las propiedades para Time.deltaTime según NewtonEuler
        // La actualización de las propiedades se puede hacer en LateUpdate()
        Velocity += Acceleration * deltaTime;
        Rotation = this.steer.angular;

        Orientation += Rotation * deltaTime;
        //transform.rotation = Quaternion.Euler(0,Orientation, 0);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, Orientation);


        Position += (new Vector3(Mathf.Min(Velocity.x, MaxSpeed), 0, Mathf.Min(Velocity.z, MaxSpeed))) * deltaTime;


        // Rotation
        // Position
        // Orientation
    }

    public virtual void LateUpdate()
    {
        Steering kinematicFinal = new Steering();

        // Reseteamos el steering final.
        this.steer = new Steering();

        // Recorremos cada steering
        //Habrá que modificarlo cuando se tenga el actuador
        /*
        foreach(SteeringBehaviour b in listSteerings){
            
            kinematicFinal = b.GetSteering(this);
            
        }
        */

        List<SteeringBehaviour> auxList = new List<SteeringBehaviour>();
        foreach (SteeringBehaviour b in listSteerings)
        {
            if (GetComponent<StateMachineManager>() != null)
            {
                if (GetComponent<StateMachineManager>().CurrentState == StateMachineManager.wanderState)
                {
                    if (b.NameSteering == "Wander" || b.NameSteering == "WallAvoidance") { auxList.Add(b); }
                }
                else
                {
                    if (b.NameSteering != "Wander") { auxList.Add(b); }
                }
            }
            else
            {
                if (b.enabled == true) auxList.Add(b);
            }
        }
        kinematicFinal = Arbitro.getKinematicFinal(auxList, this);

        //foreach (SteeringBehaviour behavior in listSteerings)
        //    Steering kinematic = behavior.GetSteering(this);
        //// La cinemática de este SteeringBehaviour se tiene que combinar
        //// con las cinemáticas de los demás SteeringBehaviour.
        //// Debes usar kinematic con el árbitro desesado para combinar todos
        //// los SteeringBehaviour.
        //// Llamaremos kinematicFinal a la aceleraciones finales de esas combinaciones.

        // A continuación debería entrar a funcionar el actuador para comprobar
        // si la propuesta de movimiento es factible:
        // kinematicFinal = Actuador(kinematicFinal, self)


        // El resultado final se guarda para ser aplicado en el siguiente frame.
        this.steer = kinematicFinal;
    }

    public void ActivarDep()
    {
        this.ModoDep = true;
        bocadilloCube.GetComponent<Renderer>().enabled = true;
    }

    public void DeactivarDep()
    {
        this.ModoDep = false;
        bocadilloCube.GetComponent<Renderer>().enabled = false;
    }


    public float getDamage(float FA, float FD)
    {
        float damage = 0;


        if (Random.Range(0f, 101f) == 100)
        {
            damage = costeAtaque * 50; // Ataque suertudo
        }
        else
        {
            damage = FA / FD * costeAtaque * (Random.Range(0f, 51f) / 100f + 0.5f);
            if (damage <= (costeAtaque / 10))
            {
                damage = Random.Range(0f, (costeAtaque / 10f)) + (costeAtaque / 10f);
            }
        }

        return damage;

    }
    public void attackEnemy(AgentNPC target)
    {
        string atackTerreno = grid.getTileByVector(this.transform.position).getTipo();
        string defenseTerreno = grid.getTileByVector(target.transform.position).getTipo();

        float FAD = UnitAttackFactor[tipoUnidad][target.tipoUnidad];
        float FTA = TerrainAttackFactor[atackTerreno][tipoUnidad];
        float FTD = TerrainDefenseFactor[defenseTerreno][target.tipoUnidad];


        float FA = atq * FAD * FTA;
        float FD = target.atq * FTD;
        float damage = getDamage(FA, FD);

        target.pierdeVida(damage);

    }

    public int getAtaque()
    {
        return atq;
    }

    public float pierdeVida(float a)
    {
        float perdida = vida - a;
        vida = Mathf.Max(perdida, 0);

        if (vida == 0){
            gameObject.SetActive(false);
            Invoke("respawn", respawnTime);
        }
        return vida;
    }

    public void recuperarVida()
    {
        Vector3 boxSize = GetComponent<Collider>().bounds.size;
                // Si este objeto está en contacto con un obstáculo, invoca setImpasable()
                // Debug.Log("Tile: "+fila +" "+columna+" choca");
        float gana = vida + 0.5f;
        vida = Mathf.Min(gana, maxVida);
        
    
    }

    /*
    * porcentaje de mejora o empeoramiento de la heuristica segun el tipo de NPC
    */

    public virtual float getGCosteWeightCamino(Tile tile)
    {
        switch (tile.getTipo())
        {
            default:
                return 1f;
        }
    }

    public float getGCosteWeight(Tile tile)
    {
        //Tile inlfuenceTile = this.GetComponent<InfluenceMap>().grid.getTile(tile.fila, tile.columna);

        //Dictionary<Tile, float> tilesInfluenciados = InfluenceManager.Instance.getInfluenceMap(this.GetComponent<InfluenceMap>().faccion);
        float influence = InfluenceManager.Instance.getInfluenceTile(tile.pos, this.GetComponent<InfluenceMap>().faccion);
        float coste = CalcularFactorModificado(getGCosteWeightCamino(tile), influence);
        //float coste = getGCosteWeightCamino(tile);
        //Debug.Log("camino peso: " + getGCosteWeightCamino(tile) + " current influence: " + tilesInfluenciados[inlfuenceTile] + "coste: " + coste);
        return coste;


    }


    public virtual (float, float, float, float) getFactorInfluencia()
    {
        return (0.1f, 0.3f, 1.20f, 3f);
    }


    public float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return outMin + (outMax - outMin) * ((value - inMin) / (inMax - inMin));
    }

    // Método para calcular el nuevo factor modificado por la influencia
    public virtual float CalcularFactorModificado(float factorActual, float influenciaNeta)
    {
        var factoresInfluencia = getFactorInfluencia();
        float minMejora = factoresInfluencia.Item1;
        float maxMejora = factoresInfluencia.Item2;

        float minEmpeoramiento = factoresInfluencia.Item3;
        float maxEmpeoramiento = factoresInfluencia.Item4;

        float maxInfluencia = InfluenceManager.Instance.maxInfluence;
        float minInfluencia = 0f;
        float newFactor = factorActual;

        if (minMejora == 0 && maxMejora == 0 && minEmpeoramiento == 0 && maxEmpeoramiento == 0)
        {
            return factorActual;
        }

        if (influenciaNeta < 0)
        {

            newFactor = factorActual * Map(-influenciaNeta, minInfluencia, maxInfluencia, minEmpeoramiento, maxEmpeoramiento);
        }
        else if (influenciaNeta > 0)
        {

            newFactor = factorActual * (1 - Map(influenciaNeta, minInfluencia, maxInfluencia, minMejora, maxMejora));
        }

        return newFactor;
    }


    public void respawn(){
        
        vida = maxVida;
        this.transform.position = respawnPosition;
        gameObject.SetActive(true);
    }


    public void changeColor (Color color){
        bocadilloCube.GetComponent<Renderer>().material.color= color;
    }
    public void changeColorAtaque(){
        changeColor(new Color(1,0,0,1));
    }

    public void changeColorFijarObjetivo(){
        changeColor(new Color(0,0,1,1));
    }

    public void changeColorHuir(){
        changeColor(new Color(0,1,0,1));
    }

    // morado
    public void changeColorSoltarObjetivo(){
        changeColor(new Color(1,0,1,1));
    }

    // amarillo
    public void changeColorMovimiento(){
        changeColor(new Color(1,1,0,1));
    }

    //naranja
    public void changeColorSinConcretar(){
        changeColor(new Color(1,0.5f,0,1));
    }

    public virtual bool vidaBaja() { return false; }

}
