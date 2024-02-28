using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//t = t + dt.  El tiempo es la acumulación del delta time cuando esta haceindo el salto
//pt = p0 + v * sm * t + (g*t^2)/2

public class Jumping : VelocityMatching
{

    const float gravedad = -9.81f;

    //Punto de salto (Normalmente viene dado como un waypoint de la búsqueda de caminos)
    public JumpPoint jumpPoint;

    // Mantiene si el salto es alcanzable
    bool canAchieve = false;

    //Máxima velocidad de salto 
    public float maxYVelocity;

    //Velocidad máxima del personaje
    public float maxSpeed;

    public float totalTime = 0;
    float accTime = 0;
    bool saltando = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Devuelve el steering para el salto
    public override Steering GetSteering(Agent agent) 
    {
        Steering steer = new Steering();

        if (saltando) 
        {
            
            //float posy = agent.Position.y;
            if (accTime >= totalTime)
            {
                //agent.Position = new Vector3(agent.Position.x, 0, agent.Position.z);
                saltando = false;
            }
            else { agent.Position = new Vector3(agent.Position.x, (0 + maxYVelocity * accTime + gravedad * (accTime * accTime) / 2), agent.Position.z); }
            accTime += Time.deltaTime;
            return steer;
        }

        //Comprobamos si tenemos una trayectoria si no creamos una.
        if (target == null) 
        {
            //Calcula el punto de destino
            //Si puede alcanzarse canAchieve = True
            //target = calculateTarget();
            return steer;
        }

        maxSpeed = agent.MaxSpeed;
        calculateTarget();

        //Comprutrbs si el salto es alcanzable
        if (!canAchieve) 
        {
            //Si no devolvemos un steering con valores 0
            return steer;
        }

        //Comprobamos si hemos alcanzado el punto de salto
        if ((target.Position - agent.Position).magnitude < 0.5 && (target.Velocity - agent.Velocity).magnitude < 0.5) 
        {
            //Si hemos alcanzado el punto de salto realizamos el salto
            saltando = true;
            
            //float posy = agent.Position.y;
            agent.Position = new Vector3(agent.Position.x, (0 + maxYVelocity * accTime + gravedad * (accTime * accTime) / 2), agent.Position.z);
            accTime += Time.deltaTime;
            steer.linear = Vector3.zero;
            return steer;
        }

        return base.GetSteering(agent);
    }

    //Lleva a cabo el cálculo de la trayectoria
    public void calculateTarget() 
    {
        //Crear un nuevo agent
        // target = new Agent();
        // target.position = jumpPoint.jumpLocation;
  
        //Calcula el primer tiempo de salto
        float sqrtTerm = Mathf.Sqrt(2 * gravedad * jumpPoint.DeltaPosition.y + maxYVelocity * maxYVelocity);
        float time = (-maxYVelocity - sqrtTerm) / gravedad;

        //Comprobamos si es la solución correcta de la ecuación de segundo grado
        if (!checkJumpTime(time)) 
        {
            //Si no es la solución correcta probamos con la otra
            time = (-maxYVelocity + sqrtTerm) / gravedad;
            checkJumpTime(time);
        }

        //return new Agent();
    }

    public bool checkJumpTime(float time) 
    {
        //Calculamos la velocidad en el plano x z
        float vx = jumpPoint.DeltaPosition.x / time;
        float vz = jumpPoint.DeltaPosition.z / time;
        float speedSq = vx * vx + vz * vz;

        //Comprobamos que no supere a la velocidad máxima 
        if (time > 0 && speedSq < maxSpeed * maxSpeed)
        {
            target.Velocity = new Vector3(vx, 0, vz);
           
            canAchieve = true;
            totalTime = time;
            return true;
        }

        return false;
    }

}
