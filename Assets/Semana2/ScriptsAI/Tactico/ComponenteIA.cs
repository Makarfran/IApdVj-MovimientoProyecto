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
        GameObject target = null;
        //Añadir condicion hayObjetivoCaptura
       
        if (bando == "R") { return !comprobarAtaqueBasePrincipal(controladorJuego.baseAzul) && fijarObjetivoBase(out target);}
        else { return !comprobarAtaqueBasePrincipal(controladorJuego.baseRoja) && fijarObjetivoBase(out target); }
    }

    public bool fijarObjetivoBase(out GameObject objetivo)
    {
        string bando = npc.getBando();
        List<GameObject> auxBases = new List<GameObject>();

        if (getModo() == "Equilibrado" && npc.getTipo() == "Scout") 
        {
            
        }

        if (bando == "R")
        {
            if (getModo() == "Defensivo")
            {
                foreach (GameObject b in controladorJuego.basesInicioTeamR)
                {
                    if (b.GetComponent<KeypointBase>().getBando() != bando) auxBases.Add(b);
                }
            }
            else if (getModo() == "Ofensivo" || getModo() == "GuerraTotal")
            {
                foreach (GameObject b in controladorJuego.basesInicioTeamA)
                {
                    if (b.GetComponent<KeypointBase>().getBando() != bando) auxBases.Add(b);
                }
                if (condicionAtaqueBasePrincipal(controladorJuego.basesInicioTeamA, bando)) { auxBases.Add(controladorJuego.baseAzul); }
            }
            else 
            {
                if (npc.getTipo() == "Scout")
                {
                    auxBases.AddRange(apoyarConquista(controladorJuego.basesTeamA));
                }
                else 
                {
                    auxBases.AddRange(controladorJuego.basesTeamA);
                    if (condicionAtaqueBasePrincipal(controladorJuego.basesInicioTeamA, bando)) { auxBases.Add(controladorJuego.baseAzul); }
                }
            }
           
        }
        else
        {
            if (getModo() == "Defensivo")
            {
                foreach (GameObject b in controladorJuego.basesInicioTeamA)
                {
                    if (b.GetComponent<KeypointBase>().getBando() != bando) auxBases.Add(b);
                }
            }
            else if (getModo() == "Ofensivo" || getModo() == "GuerraTotal")
            {
                foreach (GameObject b in controladorJuego.basesInicioTeamR)
                {
                    if (b.GetComponent<KeypointBase>().getBando() != bando) auxBases.Add(b);
                }
                if (condicionAtaqueBasePrincipal(controladorJuego.basesInicioTeamR, bando)) { auxBases.Add(controladorJuego.baseRoja); }
            }
            else
            {
                if (npc.getTipo() == "Scout")
                {
                    auxBases.AddRange(apoyarConquista(controladorJuego.basesTeamR));
                }
                else 
                {
                    auxBases.AddRange(controladorJuego.basesTeamR);
                    if (condicionAtaqueBasePrincipal(controladorJuego.basesInicioTeamR, bando)) { auxBases.Add(controladorJuego.baseRoja); }
                }
            }
        }
        objetivo = getObjetivo(auxBases);
        //GetComponent<Atacar>().setTarget(objetivo.GetComponent<AgentNPC>());
        //GetComponent<Movimiento>().setTarget(objetivo);
        if (objetivo != null) return true;
        else return false;
    }

    private List<GameObject> apoyarConquista(List<GameObject> basesEnemigas) 
    {
        List<GameObject> basesApoyables = new List<GameObject>();
        foreach (GameObject b in basesEnemigas) 
        {
            if (b.GetComponent<KeypointBase>().hayAliados(npc.getBando())) basesApoyables.Add(b);
        }

        return basesApoyables;
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
        List<GameObject> basesAComprobar = new List<GameObject>();
        GameObject basePrincipal;
        string modo = GetComponent<ComponenteIA>().getModo();
        if (npc.getBando() == "R")
        {
            basePrincipal = controladorJuego.baseRoja;
            switch (modo)
            {
                case "Ofensivo":
                    basesAComprobar.AddRange(controladorJuego.basesInicioTeamA);
                    break; 

                case "Defensivo":
                    basesAComprobar.AddRange(controladorJuego.basesInicioTeamR);
                    break;

                case "Equilibrado":
                    basesAComprobar.AddRange(controladorJuego.basesTeamR);
                    break;
            }
        }
        else 
        {
            basePrincipal = controladorJuego.baseAzul;
            switch (modo)
            {
                case "Ofensivo":
                    basesAComprobar.AddRange(controladorJuego.basesInicioTeamR);
                    break;

                case "Defensivo":
                    basesAComprobar.AddRange(controladorJuego.basesInicioTeamA);
                    break;

                default:
                    basesAComprobar.AddRange(controladorJuego.basesTeamA);
                    break;
            }
        }

        return comprobarAtaqueBasePrincipal(basePrincipal) || comprobarBaseAtacada(basesAComprobar);
        /*
        if (npc.getBando() == "R")
        {
            //waypoints.Add(controladorJuego.baseRoja);
            //waypoints.AddRange(controladorJuego.basesTeamR);
            string modo = GetComponent<ComponenteIA>().getModo();
            switch (modo)
            {
                case "Ofensivo":
                    {
                        List<GameObject> bases = new List<GameObject>();
                        foreach (GameObject b in controladorJuego.basesInicioTeamA)
                        {
                            if (b.GetComponent<KeypointBase>().getBando() == npc.getBando()) bases.Add(b);
                        }

                        return comprobarAtaqueBasePrincipal(controladorJuego.baseRoja) || comprobarBaseAtacada(bases);
                    }

                case "Defensivo":
                    {
                        List<GameObject> bases = new List<GameObject>();
                        foreach (GameObject b in controladorJuego.basesInicioTeamR) 
                        {
                            if (b.GetComponent<KeypointBase>().getBando() == npc.getBando()) bases.Add(b);
                        }

                        return comprobarAtaqueBasePrincipal(controladorJuego.baseRoja) || comprobarBaseAtacada(bases);
                    }
                    

                case "GuerraTotal":
                    return comprobarAtaqueBasePrincipal(controladorJuego.baseRoja);

                default:
                    return comprobarAtaqueBasePrincipal(controladorJuego.baseRoja) || comprobarBaseAtacada(controladorJuego.basesTeamR);

            }
            
        }
        else 
        {
            //waypoints.Add(controladorJuego.baseAzul);
            //waypoints.AddRange(controladorJuego.basesTeamA);
            return comprobarAtaqueBasePrincipal(controladorJuego.baseAzul) || comprobarBaseAtacada(controladorJuego.basesTeamA);
        }
        //return baseAtacada(waypoints);
        */
    }

    public bool fijarObjetivoDefensa(out GameObject objetivo) 
    {
        //GameObject objetivo = null;
        string bando = npc.getBando();
        List<GameObject> basesAtacadas = new List<GameObject>();

        if (bando == "R")
        {
            /*
            if (comprobarAtaqueBasePrincipal(controladorJuego.baseRoja)) objetivo = controladorJuego.baseRoja;
            else 
            {
                foreach (GameObject baseAtacada in controladorJuego.basesTeamR)
                {
                    KeypointBase baseA = baseAtacada.GetComponent<KeypointBase>();
                    if (baseA.getLifeP() < baseA.getLifePMax()) basesAtacadas.Add(baseAtacada);
                }
                objetivo = getObjetivo(basesAtacadas);
            }
            */
            if (comprobarAtaqueBasePrincipal(controladorJuego.baseRoja)) objetivo = controladorJuego.baseRoja;
            else 
            {
                switch (getModo())
                {
                    case "Ofensivo":
                    case "GuerraTotal":
                        foreach (GameObject baseAtacada in controladorJuego.basesInicioTeamA)
                        {
                            KeypointBase baseA = baseAtacada.GetComponent<KeypointBase>();
                            if (baseA.getBando() == npc.getBando() && baseA.getLifeP() < baseA.getLifePMax()) basesAtacadas.Add(baseAtacada);
                        }
                        break;

                    case "Defensivo":
                        foreach (GameObject baseAtacada in controladorJuego.basesInicioTeamR)
                        {
                            KeypointBase baseA = baseAtacada.GetComponent<KeypointBase>();
                            if (baseA.getBando() == npc.getBando() && baseA.getLifeP() < baseA.getLifePMax()) basesAtacadas.Add(baseAtacada);
                        }
                        break;

                    case "Equilibrado":
                        foreach (GameObject baseAtacada in controladorJuego.basesTeamR)
                        {
                            KeypointBase baseA = baseAtacada.GetComponent<KeypointBase>();
                            if (baseA.getLifeP() < baseA.getLifePMax()) basesAtacadas.Add(baseAtacada);
                        }
                        break;
                }
                objetivo = getObjetivo(basesAtacadas);
            }
        }
        else
        {
            /*
            if (comprobarAtaqueBasePrincipal(controladorJuego.baseAzul)) objetivo = controladorJuego.baseAzul;
            else
            {
                foreach (GameObject baseAtacada in controladorJuego.basesTeamA)
                {
                    KeypointBase baseA = baseAtacada.GetComponent<KeypointBase>();
                    if (baseA.getLifeP() < baseA.getLifePMax()) basesAtacadas.Add(baseAtacada);
                }
                objetivo = getObjetivo(basesAtacadas);
            }
            */
            if (comprobarAtaqueBasePrincipal(controladorJuego.baseAzul)) objetivo = controladorJuego.baseAzul;
            else 
            {
                switch (getModo())
                {
                    case "Ofensivo":
                    case "GuerraTotal":
                        foreach (GameObject baseAtacada in controladorJuego.basesInicioTeamR)
                        {
                            KeypointBase baseA = baseAtacada.GetComponent<KeypointBase>();
                            if (baseA.getBando() == npc.getBando() && baseA.getLifeP() < baseA.getLifePMax()) basesAtacadas.Add(baseAtacada);
                        }
                        break;

                    case "Defensivo":
                        foreach (GameObject baseAtacada in controladorJuego.basesInicioTeamA)
                        {
                            KeypointBase baseA = baseAtacada.GetComponent<KeypointBase>();
                            if (baseA.getBando() == npc.getBando() && baseA.getLifeP() < baseA.getLifePMax()) basesAtacadas.Add(baseAtacada);
                        }
                        break;

                    default:
                        foreach (GameObject baseAtacada in controladorJuego.basesTeamA)
                        {
                            KeypointBase baseA = baseAtacada.GetComponent<KeypointBase>();
                            if (baseA.getLifeP() < baseA.getLifePMax()) basesAtacadas.Add(baseAtacada);
                        }
                        break;
                }
                objetivo = getObjetivo(basesAtacadas);
            }
        }

        if (objetivo != null) return true;
        else return false;
        

    }

    //################### STATEFLEE #################


    public bool conditionFlee()
    {
        if (npc.vidaBaja() && !estoyEnCuraPrincipal(npc.getBando())) 
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
        if (getModo() == "Defensivo") 
        {
            if (bando == "R")
                objetivo = controladorJuego.HRoja;
            if (bando == "A")
                objetivo = controladorJuego.HAzul;
        }
        else
        {
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

    public bool comprobarModoParaPatrulla() 
    {
        return (controladorJuego.getModo(npc.getBando()) == "Equilibrado" || controladorJuego.getModo(npc.getBando()) == "Defensivo");
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


    //############### STATESCOUT ###############

    public void fijarObjetivoScout() 
    {
        InfluenceManager infuenceManager = GameObject.Find("MapaInfluencia").transform.GetChild(1).gameObject.GetComponent<InfluenceManager>();
        for (int i = 0; i < 1000; i++)
        {
            Tile tile = GetComponent<InfluenceMap>().tileAleatoria();
            if (tile.pasable && infuenceManager.hayInfluencia(tile, npc.getBando()) && 
                infuenceManager.getInfluenceTile(tile, npc.getBando()) == 0) 
            {
                GetComponent<Exploracion>().setTarget(tile);
            }
        }
    }

    //######### GENERAL ############

    private bool comprobarBaseAtacada(List<GameObject> waypoints) 
    {
        foreach (GameObject wayPoint in waypoints) 
        {
            KeypointBase keypoint = wayPoint.GetComponent<KeypointBase>();
            if (keypoint.getBando() == npc.getBando() && keypoint.getLifeP() < keypoint.getLifePMax()) return true;
        }
        return false;

    }

    public bool comprobarAtaqueBasePrincipal(GameObject basePrincipal)
    {
        KeypointBase keypointBP = basePrincipal.GetComponent<KeypointBase>();
        if (keypointBP.getLifeP() < keypointBP.getLifePMax())
        {
            return true;
        }
        else { return false; }
    }
    public bool comprobarAtaqueBasePrincipal(string bando)
    {
        GameObject basePrincipal;
        if (bando == "R") basePrincipal = controladorJuego.baseRoja;
        else basePrincipal = controladorJuego.baseAzul;

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

    public bool objetivoBasePrincipal() 
    {
        if (npc.getBando() == "R")
            return GetComponent<Movimiento>().getTarget() == controladorJuego.baseAzul && 
            (GetComponent<Movimiento>().getTarget().transform.position - npc.Position).magnitude < 4;
        else
            return GetComponent<Movimiento>().getTarget() == controladorJuego.baseRoja &&
                   (GetComponent<Movimiento>().getTarget().transform.position - npc.Position).magnitude < 4;
    }

    public bool estoyEnCuraPrincipal(string bando) 
    {
        if (bando == "R")
        {
            return (controladorJuego.HRoja.transform.position - npc.Position).magnitude < 3;
        }
        else 
        {
            return (controladorJuego.HAzul.transform.position - npc.Position).magnitude < 3;
        }
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

    public string getModo() 
    {
        return controladorJuego.getModo(npc.getBando());
    }

    public void desactivarIA()
    {
        npc.GetComponent<StateMachineIA>().enabled = false;
        npc.GetComponent<ActionManager>().enabled = false;
        npc.GetComponent<PathFinding>().clearCamino();
    }

    public void activarIA()
    {
        npc.GetComponent<StateMachineIA>().enabled = true;
        npc.GetComponent<ActionManager>().enabled = true;
       
    }



}
