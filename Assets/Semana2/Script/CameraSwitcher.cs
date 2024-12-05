using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera; // Asigna aquí la cámara principal en el Inspector
    public Camera layerCamera; // Asigna aquí la cámara secundaria en el Inspector

    void Start()
    {
        // Asegúrate de que solo la cámara principal esté activa al iniciar
        mainCamera.enabled = true;
        layerCamera.enabled = false;
    }

    void Update()
    {
        // Alternar cámaras con una tecla (ejemplo: tecla "C")
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Cambia el estado de las cámaras
            mainCamera.enabled = !mainCamera.enabled;
            layerCamera.enabled = !layerCamera.enabled;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Busca todos los objetos que tengan el script PathFinding
            PathFinding[] pathFinders = FindObjectsOfType<PathFinding>();

            // Aplica la función changeToTatico() en cada uno
            foreach (PathFinding pathFinder in pathFinders)
            {
                pathFinder.changeToTatico();
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            // Busca todos los objetos que tengan el script PathFinding
            PathFinding[] pathFinders = FindObjectsOfType<PathFinding>();

            // Aplica la función changeToTatico() en cada uno
            foreach (PathFinding pathFinder in pathFinders)
            {
                pathFinder.changeToLRTA();
            }
        }  

        if (Input.GetKeyDown(KeyCode.P)){
            
             foreach(GameObject npc in UnitsSelection.npcsSelected){
                if (npc.GetComponent<PathFinding>().pathFindingTactico){
                    npc.GetComponent<PathFinding>().changeToLRTA();
                } else {
                    npc.GetComponent<PathFinding>().changeToTatico();
                }
                
             }
        }      
    }
}
