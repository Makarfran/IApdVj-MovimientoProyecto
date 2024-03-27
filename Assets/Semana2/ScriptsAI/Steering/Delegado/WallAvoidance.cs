using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidance : Seek
{

    // Declara las variables que necesites para este SteeringBehaviour
    RaycastHit hit;

    //Distancaia mínima a un obstáculo (como de lejos nos movemos para evitar la colisión)
    //Distancia de separación
    float avoidDistance;

    //Distancia del rayo
    //En función del tiempo (fps) 
    float lookahead;

    //número de bigotes
    public int nrays;

    // Start is called before the first frame update
    void Start()
    {
        this.nameSteering = "WallAvoidance";
        lookahead = 0.01f * (1.0f / Time.deltaTime);
    }


    public override Steering GetSteering(Agent agent)
    {

        Steering steer = new Steering();

        //Distancia de separación dos veces el ancho del personaje
        avoidDistance = 2 * agent.InteriorRadius;

        //Calcula el origen, dirección y tamaño del vector rayo de colisión (bigote)
        Vector3 origen = agent.transform.position;
        Vector3 direction = agent.Velocity.normalized;
        Vector3 future = direction * agent.MaxAcceleration;

        //Calculamos el ángulo de la velocidad actual
        float anguloNPC = Mathf.Atan2(direction.x, direction.z);
        //Calculamos el ángulo del primer bigote
        float angulo = anguloNPC - Mathf.PI / 2 + Mathf.PI / (nrays + 1);

        //Dibujamos los bigotes
        draw2(origen, direction, anguloNPC, angulo);

        //Para cada bigote comprobamos si hay colisión
        for (int i = 1; i <= nrays; i++)
        {
            float distancia = future.magnitude * lookahead;
            Vector3 otraDirection = new Vector3(Mathf.Sin(angulo), 0f, Mathf.Cos(angulo));

            if (nrays % 2 != 0 && i != ((nrays + 1) / 2))
            // if ( angulo != anguloNPC ) 
            {
                distancia = distancia / 3;
            }

            bool collision = Physics.Raycast(origen, otraDirection, out hit, distancia);
            // Si hay colisión delegamos a seek
            if (collision)
            {
                //Para ver el bigote que esta colisionando y su normal
                draw(new Ray(origen, otraDirection), hit);

                //Cambiamos el target y delegamos en seek
                
                Vector3 newTarget = hit.point + (hit.normal * avoidDistance);
                Vector3 desired_velocity = newTarget - agent.transform.position;
                desired_velocity = desired_velocity.normalized * agent.MaxAcceleration;
                steer.linear = desired_velocity - agent.Velocity;
                return steer;
            }
            //calculamos el ángulo del siguiente bigote
            angulo = angulo + Mathf.PI / (nrays + 1);
        }

        return base.GetSteering(agent);
    }

    //Para debug
    void draw(Ray ray, RaycastHit hit)
    {
        string str = hit.transform.gameObject.name;
        if (str.Equals("Cube"))
        {
            ray.origin += Vector3.up;
            hit.point += Vector3.up;

            Debug.DrawLine(ray.origin, hit.point, Color.red);
            Debug.DrawLine(hit.point, hit.point + (hit.normal * avoidDistance), Color.blue);
        }


    }

    void draw2(Vector3 origin, Vector3 direccion, float angNPC, float ang)
    {

        origin += Vector3.up * 2;

        for (int i = 1; i <= nrays; i++)
        {
            direccion.x = Mathf.Sin(ang);
            direccion.z = Mathf.Cos(ang);
            Debug.DrawLine(origin, origin + direccion, Color.white);
            ang = ang + Mathf.PI / (nrays + 1);
        }

    }
}
