using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponenteIA : MonoBehaviour
{
    // Start is called before the first frame update
    Controlador controladorJuego;
    ActionManager actionManager;
    StateMachineIA stateMachine;

    void Start()
    {
        actionManager = GetComponent<ActionManager>();
        stateMachine = GetComponent<StateMachineIA>();
        controladorJuego = GameObject.Find("ControladorJuego").GetComponent<Controlador>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //########### STATE ATTACK #############################################

    public bool conditionAttack() 
    {
        if (GetComponent<AgentNPC>().getBando() == "R")
        {
            return enemigosCerca(controladorJuego.teamA);
        }
        if (GetComponent<AgentNPC>().getBando() == "A")
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
            if (((enemigoNPC.Position - GetComponent<AgentNPC>().Position).magnitude) < 10 &&
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
        if (GetComponent<AgentNPC>().getBando() == "R")
        {
            objetivo = getObjetivo(controladorJuego.teamA);
        }
        if (GetComponent<AgentNPC>().getBando() == "A")
        {
            objetivo = getObjetivo(controladorJuego.teamR);
        }
        GetComponent<Atacar>().setTarget(objetivo.GetComponent<AgentNPC>());
        GetComponent<Movimiento>().setTarget(objetivo);
    }

    //########### STATE CAPTURE #############################################

    public bool conditionCapture() 
    {
        string bando = GetComponent<AgentNPC>().getBando();

        if (bando == "R") { return !comprobarAtaqueBasePrincipal(controladorJuego.baseAzul);}
        else { return !comprobarAtaqueBasePrincipal(controladorJuego.baseRoja);}
    }

    public void fijarObjetivoBase()
    {

        GameObject objetivo = null;
        string bando = GetComponent<AgentNPC>().getBando();
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
        if (GetComponent<AgentNPC>().getBando() == "R")
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
        string bando = GetComponent<AgentNPC>().getBando();

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

    //######### GENERAL ############3

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
            if (distancia > (objetivo.transform.position - GetComponent<Agent>().Position).magnitude)
            {
                if (enemigoMuerto(objetivo.GetComponent<AgentNPC>())) continue;
                distancia = (objetivo.transform.position - GetComponent<Agent>().Position).magnitude;
                objetivoActual = objetivo;
            }
        }
        return objetivoActual;
    }

    public bool enemigoMuerto(AgentNPC enemigo) 
    {
        return enemigo != null && enemigo.getVida() == 0; 
    }
}
