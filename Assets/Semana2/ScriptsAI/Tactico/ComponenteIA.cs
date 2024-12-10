using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponenteIA : MonoBehaviour
{
    // Start is called before the first frame update
    Controlador controladorJuego;
    ActionManager actionManager;
    StateMachineIA stateMachine;
    AgentNPC npc;

    void Start()
    {
        actionManager = GetComponent<ActionManager>();
        stateMachine = GetComponent<StateMachineIA>();
        controladorJuego = GameObject.Find("ControladorJuego").GetComponent<Controlador>();
        npc = GetComponent<AgentNPC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //########### STATE ATTACK #############################################

    public bool conditionAttack() 
    {
        if (npc.getBando() == "R")
        {
            return enemigosCerca(controladorJuego.teamA);
        }
        if (npc.getBando() == "A")
        {
            return enemigosCerca(controladorJuego.teamR);
        }

        return false;
    }

    private bool enemigosCerca(List<GameObject> enemigos) 
    {
        foreach (GameObject enemigo in enemigos)
        {
            AgentNPC enemigoNPC = enemigo.GetComponent<AgentNPC>();
            //Falta condicion de visibilidad
            if ((enemigoNPC.Position - npc.Position).magnitude < 10 &&
                  !enemigoMuerto(enemigoNPC))
            {
                Debug.Log("condicion activa");
                return true;
            }
        }
        return false;
    }

    public void fijarObjetivo()
    {

        GameObject objetivo = null;
        
        //Si no funciona probar getBando
        if (npc.getBando() == "R")
        {
            objetivo = getObjetivo(controladorJuego.teamA);
        }
        if (npc.getBando() == "A")
        {
            objetivo = getObjetivo(controladorJuego.teamR);
        }
        GetComponent<Atacar>().setTarget(objetivo.GetComponent<AgentNPC>());
        GetComponent<Movimiento>().setTarget(objetivo);
    }

    //########### STATE CAPTURE #############################################

    public bool conditionCapture() 
    {
        string bando = npc.getBando();

        if (bando == "R") { return !comprobarAtaqueBasePrincipal(controladorJuego.baseAzul);}
        else { return !comprobarAtaqueBasePrincipal(controladorJuego.baseRoja);}
    }

    public void fijarObjetivoBase()
    {

        GameObject objetivo = null;
        string bando = npc.getBando();
        List<GameObject> auxBases;
        //Si no funciona probar getBando
        if (bando == "R")
        {
            auxBases = new List<GameObject>(controladorJuego.basesTeamA);
            //auxBases.Remove(controladorJuego.baseAzul);
            if (condicionAtaqueBasePrincipal(controladorJuego.basesInicioTeamA, bando)) { auxBases.Add(controladorJuego.baseAzul); }
        }
        else
        {
            auxBases = new List<GameObject>(controladorJuego.basesTeamR);
            //auxBases.Remove(controladorJuego.baseAzul);
            if (condicionAtaqueBasePrincipal(controladorJuego.basesInicioTeamR, bando)) { auxBases.Add(controladorJuego.baseRoja); }
        }
        objetivo = getObjetivo(auxBases);
        //GetComponent<Atacar>().setTarget(objetivo.GetComponent<AgentNPC>());
        GetComponent<Movimiento>().setTarget(objetivo);
    }

    private bool condicionAtaqueBasePrincipal(List<GameObject> bases, string bando) 
    {
        foreach (GameObject b in bases)
        {
            if (b.GetComponent<KeypointBase>().getBando() == bando)
            {
                //bases.Add(controladorJuego.baseAzul);
                return true;
            }
        }
        return false;
    }

    //################### STATEDEFEND #################

    public bool condicionDefend() 
    {
        //List<GameObject> waypoints = new List<GameObject>();
        if (npc.getBando() == "R")
        {
            //waypoints.Add(controladorJuego.baseRoja);
            //waypoints.AddRange(controladorJuego.basesTeamR);
            return comprobarAtaqueBasePrincipal(controladorJuego.baseRoja) || comprobarBaseAtacada(controladorJuego.basesTeamR);
        }
        else 
        {
            //waypoints.Add(controladorJuego.baseAzul);
            //waypoints.AddRange(controladorJuego.basesTeamA);
            return comprobarAtaqueBasePrincipal(controladorJuego.baseAzul) || comprobarBaseAtacada(controladorJuego.basesTeamA);
        }
        //return baseAtacada(waypoints);
    }

    public void fijarObjetivoDefensa() 
    {
        GameObject objetivo = null;
        string bando = npc.getBando();

        if (bando == "R")
        {
            if (comprobarAtaqueBasePrincipal(controladorJuego.baseRoja)) objetivo = controladorJuego.baseRoja;
            else objetivo = getObjetivo(controladorJuego.basesTeamR);
        }
        else
        {
            if (comprobarAtaqueBasePrincipal(controladorJuego.baseAzul)) objetivo = controladorJuego.baseAzul;
            else objetivo = getObjetivo(controladorJuego.basesTeamA);
        }
        GetComponent<Movimiento>().setTarget(objetivo);

    }

    //################### STATEFLEE #################


    public bool conditionFlee()
    {
        if (npc.vidaBaja()) 
        {
            return true;
        }
        return false;
    }

    /* cambiada por polimorfismo
    public bool vidaBaja() 
    {

        if (npc.getVida() / npc.getMaxVida() <= 0.3f) 
        {
            return true;
        }
        return false;
    }
    */

    public bool fullVida() 
    {
        if (npc.getVida() == npc.getMaxVida()) return true;
        return false;
    }

    public void fijarObjetivoHeal() 
    {
        GameObject objetivo = null;
        string bando = npc.getBando();
        List<GameObject> auxZH = new List<GameObject>();
        foreach (GameObject HPoint in controladorJuego.zonasH) 
        {
            if (HPoint.GetComponent<KeypointCura>().getBando() == bando || HPoint.GetComponent<KeypointCura>().getBando() == "None") auxZH.Add(HPoint);
        }

        objetivo = getObjetivo(auxZH);

        if (objetivo == null || (objetivo.transform.position - npc.Position).magnitude < 3) 
        {
            if (bando == "R")
                objetivo = controladorJuego.HRoja;
            if (bando == "A")
                objetivo = controladorJuego.HAzul;
        }

        GetComponent<Movimiento>().setTarget(objetivo);
    }

    //######### STATEPATROL ###########

    public bool condicionPatrol() 
    {
        
        if (npc.GetComponent<ActivarPatrulla>().camino != null) 
        {
            //Debug.Log("StatePatrol");
            return true;
        }
        
        return false;
    }


    //################### STATEBERSERKER #################

    public void fijarObjetivoBerserker() 
    {
        GameObject objetivo = null;
        List<GameObject> objetivos = new List<GameObject>();
        
        //Si no funciona probar getBando
        if (npc.getBando() == "R")
        {
            objetivos.AddRange(controladorJuego.teamA);
            objetivos.AddRange(controladorJuego.teamR);
            objetivos.Remove(gameObject);
            objetivo = getObjetivo(objetivos);
        }
        if (npc.getBando() == "A")
        {
            objetivos.AddRange(controladorJuego.teamR);
            objetivos.AddRange(controladorJuego.teamA);
            objetivos.Remove(gameObject);
            objetivo = getObjetivo(objetivos);
        }
        GetComponent<Atacar>().setTarget(objetivo.GetComponent<AgentNPC>());
        GetComponent<Movimiento>().setTarget(objetivo);
    }


    //######### GENERAL ############

    private bool comprobarBaseAtacada(List<GameObject> waypoints) 
    {
        foreach (GameObject wayPoint in waypoints) 
        {
            KeypointBase keypoint = wayPoint.GetComponent<KeypointBase>();
            if (keypoint.getLifeP() < keypoint.getLifePMax()) return true;
        }
        return false;

    }

    private bool comprobarAtaqueBasePrincipal(GameObject basePrincipal)
    {
        KeypointBase keypointBP = basePrincipal.GetComponent<KeypointBase>();
        if (keypointBP.getLifeP() < keypointBP.getLifePMax())
        {
            return true;
        }
        else { return false; }
    }

    private GameObject getObjetivo(List<GameObject> objetivos)
    {
        float distancia = float.MaxValue;
        GameObject objetivoActual = null;
        foreach (GameObject objetivo in objetivos)
        {
            if (distancia > (objetivo.transform.position - npc.Position).magnitude)
            {
                if (enemigoMuerto(objetivo.GetComponent<AgentNPC>())) continue;
                distancia = (objetivo.transform.position - npc.Position).magnitude;
                objetivoActual = objetivo;
            }
        }
        return objetivoActual;
    }

    public bool enemigoMuerto(AgentNPC enemigo) 
    {
        return enemigo != null && enemigo.getVida() == 0; 
    }

    public void pararMovimiento() 
    {
        GetComponent<PathFinding>().clearCamino();
    }

    public bool distanciaHeal() 
    
    {
        if (GetComponent<Movimiento>().getTarget() != null) 
        {

            if (GetComponent<Movimiento>().getTarget().GetComponent<KeypointCura>() &&
               (GetComponent<Movimiento>().getTarget().transform.position - npc.Position).magnitude < 2.5f)
            {
                //Debug.Log("De heal a idle");
                return true;
            } 
        }
        return false;
    }

    public bool enemigoAgresivo() 
    {
        GameObject objetivo = null;
        if (npc.getBando() == "R")
        {
            objetivo = getObjetivo(controladorJuego.teamA);
        }
        if (npc.getBando() == "A")
        {
            objetivo = getObjetivo(controladorJuego.teamR);
        }

        if ((objetivo != null && (objetivo.transform.position - npc.Position).magnitude < 8) &&
            (objetivo.GetComponent<Atacar>().getTarget() != null && objetivo.GetComponent<Atacar>().getTarget().gameObject == gameObject))
        {
            return true;
        }
        else return false;
        /*
        if ((objetivo.GetComponent<Atacar>().getTarget() != null &&
             (objetivo.transform.position - npc.Position).magnitude < 8) &&
             objetivo.GetComponent<Atacar>().getTarget().gameObject == gameObject)
        */
    }

    public bool enemigoHuido(AgentNPC enemigo) 
    {
        return (enemigo.Position - GetComponent<Agent>().Position).magnitude >= 12;
    }

    public bool elite() 
    {
        return npc.getTipo() == "Elite";
    }
    public bool scout()
    {
        return npc.getTipo() == "Scout";
    }
    public bool infanteria()
    {
        return npc.getTipo() == "Infanteria";
    }
}
