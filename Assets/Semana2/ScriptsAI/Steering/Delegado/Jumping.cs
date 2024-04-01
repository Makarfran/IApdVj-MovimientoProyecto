using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//t = t + dt.  El tiempo es la acumulaci�n del delta time cuando esta haceindo el salto
//pt = p0 + v * sm * t + (g*t^2)/2

public class Jumping : VelocityMatching
{

    const float gravedad = -9.81f;

    //Punto de salto (Normalmente viene dado como un waypoint de la b�squeda de caminos)
    public JumpPoint jumpPoint;

    // Mantiene si el salto es alcanzable
    bool canAchieve = false;

    //M�xima velocidad de salto 
    public float maxYVelocity;

    //Velocidad m�xima del personaje
    public float maxSpeed;

    public float totalTime = 0;
    float accTime = 0;
    public bool saltando = false;

    // Start is called before the first frame update
    void Start()
    {
        this.Weight = 0.2f;
        this.NameSteering = "Jumping";
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
                Debug.Log("cacatua");
                //agent.Position = new Vector3(agent.Position.x, 0, agent.Position.z);
                saltando = false;
                accTime = 0;
                if(this.GetComponent<PathFollowing>() != null){
                    Agent jp = this.GetComponent<PathBasico>().getSigJP(this.target);
                    
                    if(jp != null){
                        this.Target = jp;
                        this.jumpPoint = jp.GetComponent<JumpPoint>();

                    } else {
                        this.Target = null;
                        this.jumpPoint = null;
                    }
                }
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
        Debug.Log(target.Velocity.magnitude);
        Debug.Log(agent.Velocity.magnitude);
        if ((jumpPoint.JumpLocation - agent.Position).magnitude < 1f && (target.Velocity - agent.Velocity).magnitude < 10f) 
        {
            Debug.Log("culo");
            //Si hemos alcanzado el punto de salto realizamos el salto
            saltando = true;
            
            //float posy = agent.Position.y;
            agent.Position = new Vector3(agent.Position.x , (0 + maxYVelocity * accTime + gravedad * (accTime * accTime) / 2), agent.Position.z);
            accTime += Time.deltaTime;
            steer.linear = Vector3.zero;
            return steer;
        }

        return base.GetSteering(agent);
    }

    //Lleva a cabo el c�lculo de la trayectoria
    public void calculateTarget() 
    {
        //Crear un nuevo agent
        // target = new Agent();
        // target.position = jumpPoint.jumpLocation;
  
        //Calcula el primer tiempo de salto
        float sqrtTerm = Mathf.Sqrt(2 * gravedad * jumpPoint.DeltaPosition.y + maxYVelocity * maxYVelocity);
        float time = (-maxYVelocity - sqrtTerm) / gravedad;
        


        //Comprobamos si es la soluci�n correcta de la ecuaci�n de segundo grado
        if (!checkJumpTime(time)) 
        {
            //Si no es la soluci�n correcta probamos con la otra
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
        //Debug.Log(speedSq);

        //Comprobamos que no supere a la velocidad m�xima 
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
