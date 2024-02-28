using UnityEngine;
using System.Collections;

//Estructura que se encuentra tanto en el AgentNPC como Steering behavior
[System.Serializable]
public class Steering
{
    //Componente escalar que representa la aceleración agular a aplicar(Puede ser rotación)
    public float angular;

    //Componente vectorial que representa la aceleración lineal
    public Vector3 linear;

    public Steering()
    {
        angular = 0.0f;
        linear = Vector3.zero;
    }
}
