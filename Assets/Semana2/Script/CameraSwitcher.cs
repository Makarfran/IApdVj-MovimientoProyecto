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
    }
}
