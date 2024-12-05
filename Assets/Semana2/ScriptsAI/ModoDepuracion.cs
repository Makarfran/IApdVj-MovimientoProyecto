using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModoDepuracion : MonoBehaviour
{

    private bool OnSwitch = false;
    private List<GameObject> toDestroy;
    public Material matTrans;
    // Start is called before the first frame update
    void Start()
    {
        toDestroy = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            if(!OnSwitch){
                OnSwitch = true;
                Grid[] grids = GameObject.FindObjectsOfType<Grid>();
                patrulladepurator[] patrulla = GameObject.FindObjectsOfType<patrulladepurator>();
                if(patrulla.Length != 0){
                    foreach(patrulladepurator p in patrulla){
                        p.activateDepuration();
                    }
                }
                if(grids.Length != 0){
                    foreach(Grid g in grids){
                        g.ActivateGrid();
                    }
                }
                GameObject[] obstaculos = GameObject.FindGameObjectsWithTag("Obstaculos");
                if(obstaculos.Length != 0){
                    foreach(GameObject obs in obstaculos){
                        if( obs.GetComponent<BoxCollider>() != null){
                            BoxCollider caps = obs.GetComponent<BoxCollider>();
                            GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            toDestroy.Add(a);
                            a.transform.parent = obs.transform;
                            a.GetComponent<BoxCollider>().enabled = false;
                            a.GetComponent<MeshRenderer>().material = matTrans;
                            a.transform.position = new Vector3(obs.transform.position.x + caps.center.x, obs.transform.position.y + caps.center.y, obs.transform.position.z + caps.center.z);
                            a.transform.rotation = obs.transform.rotation;
                            a.transform.localScale = new Vector3(caps.size.x , caps.size.y , caps.size.z );
                            


                        }
                    }
                }
                GameObject[] paredes = GameObject.FindGameObjectsWithTag("Pared");
                
                if(paredes.Length != 0){
                    foreach(GameObject par in paredes){
                        if( par.GetComponent<BoxCollider>() != null){
                            BoxCollider caps = par.GetComponent<BoxCollider>();
                            GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            a.transform.parent = par.transform;
                            a.GetComponent<BoxCollider>().enabled = false;
                            a.GetComponent<MeshRenderer>().material = matTrans;
                            a.transform.position = new Vector3(par.transform.position.x + caps.center.x,  par.transform.position.y + caps.center.y, par.transform.position.z +  caps.center.z);
                            a.transform.localScale = new Vector3(caps.size.x , caps.size.y , caps.size.z );
                            toDestroy.Add(a);


                        }
                    }
                }
                GameObject[] npcs = GameObject.FindGameObjectsWithTag("Npc");
                if(npcs.Length != 0){
                    foreach(GameObject npc in npcs){
                        if(npc.GetComponent<Wander>() != null){
                            if(npc.GetComponent<Wander>().target != null){
                                npc.GetComponent<Wander>().ModoDep = true;
                            }
                        }
                        if( npc.GetComponent<CapsuleCollider>() != null){
                            CapsuleCollider caps = npc.GetComponent<CapsuleCollider>();
                            GameObject a = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            a.transform.parent = npc.transform;
                            a.transform.position = npc.transform.position;
                            a.GetComponent<SphereCollider>().enabled = false;
                            a.GetComponent<MeshRenderer>().material = matTrans;
                            a.transform.position = new Vector3(npc.transform.position.x + caps.center.x, npc.transform.position.y + caps.center.y , npc.transform.position.z + caps.center.z );
                            a.transform.localScale = new Vector3(npc.transform.localScale.x + caps.radius, npc.transform.localScale.y + caps.height ,npc.transform.localScale.z + caps.radius );
                            toDestroy.Add(a);


                        } else if(npc.GetComponent<BoxCollider>() != null){
                            BoxCollider box = npc.GetComponent<BoxCollider>();
                            GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            a.transform.parent = npc.transform;
                            a.GetComponent<BoxCollider>().enabled = false;
                            a.GetComponent<MeshRenderer>().material = matTrans;
                            a.transform.position = new Vector3(npc.transform.position.x + box.center.x , npc.transform.position.y + box.center.y , npc.transform.position.z + box.center.z );
                            a.transform.localScale = new Vector3(npc.transform.localScale.x + box.size.x, npc.transform.localScale.y + box.size.y , npc.transform.localScale.z + box.size.z );
                            toDestroy.Add(a);
                        }
                        if(npc.GetComponent<AgentNPC>() != null){
                            npc.GetComponent<AgentNPC>().ActivarDep();
                        } else if(npc.GetComponent<AgentNPCElite>() != null){
                            npc.GetComponent<AgentNPCElite>().ActivarDep();
                        }else if(npc.GetComponent<AgentNPCScout>() != null){
                            npc.GetComponent<AgentNPCScout>().ActivarDep();
                        } else if(npc.GetComponent<AgentNPCInfanteria>() != null){
                            npc.GetComponent<AgentNPCInfanteria>().ActivarDep();
                        }
                    }
                }
                
            } else {
                OnSwitch = false;
                Grid[] grids = GameObject.FindObjectsOfType<Grid>();
                if(grids.Length != 0){
                    foreach(Grid g in grids){
                        g.DeactivateGrid();
                    } 
                }
                patrulladepurator[] patrulla = GameObject.FindObjectsOfType<patrulladepurator>();
                if(patrulla.Length != 0){
                    foreach(patrulladepurator p in patrulla){
                        p.deactivateDepuration();
                    }
                }
                GameObject[] npcs = GameObject.FindGameObjectsWithTag("Npc");
                if(npcs.Length != 0){
                    foreach(GameObject npc in npcs){
                        if(npc.GetComponent<Wander>() != null){
                            
                            npc.GetComponent<Wander>().ModoDep = false;
                            
                        }
                        if(npc.GetComponent<AgentNPC>() != null){
                            npc.GetComponent<AgentNPC>().DeactivarDep();
                        } else if(npc.GetComponent<AgentNPCElite>() != null){
                            npc.GetComponent<AgentNPCElite>().DeactivarDep();
                        }else if(npc.GetComponent<AgentNPCScout>() != null){
                            npc.GetComponent<AgentNPCScout>().DeactivarDep();
                        } else if(npc.GetComponent<AgentNPCInfanteria>() != null){
                            npc.GetComponent<AgentNPCInfanteria>().DeactivarDep();
                        }
                    }
                }
                if(toDestroy[0] != null){
                    foreach(GameObject a in toDestroy){
                        Destroy(a);
                    }
                    toDestroy.Clear();
                }
            }
        }
    }
}
