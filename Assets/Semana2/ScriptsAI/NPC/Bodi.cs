using UnityEngine;
using System.Collections;

public class Bodi : MonoBehaviour
{

    [SerializeField] protected float _mass = 1;
    [SerializeField] protected float _maxSpeed = 1;
    [SerializeField] protected float _maxRotation = 1;
    [SerializeField] protected float _maxAcceleration = 1;
    [SerializeField] protected float _maxAngularAcc = 1;
    [SerializeField] protected float _maxForce = 1;

    protected Vector3 _acceleration; // aceleración lineal
    protected float _angularAcc;  // aceleración angular
    [SerializeField]protected Vector3 _velocity; // velocidad lineal
    protected float _rotation;  // velocidad angular
    protected float _speed;  // velocidad escalar
    [SerializeField] protected float _orientation;  // 'posición' angular
    // Se usará transform.position como 'posición' lineal

    /// Un ejemplo de cómo construir una propiedad en C#
    /// <summary>
    /// Mass for the NPC
    /// </summary>
    public float Mass
    {
        get { return _mass; }
        set { _mass = Mathf.Max(0, value); }
    }

    // CONSTRUYE LAS PROPIEDADES SIGUENTES. PUEDES CAMBIAR LOS NOMBRE A TU GUSTO
    // Lo importante es controlar el set
    public float MaxForce {
        get { return _maxForce;}
        set { _maxForce = Mathf.Max(0, value);}
    }
    public float MaxSpeed {
        get { return _maxSpeed;}
        set { _maxSpeed = Mathf.Max(0, value);}
    }
   
    public Vector3 Velocity
    {
        get { return _velocity;  } // Modifica
        set { _velocity = value;}
    }
    
    public float MaxRotation {
        get { return _maxRotation;}
        set { _maxRotation = Mathf.Max(0, value);}
    }
    
    public float Rotation {
        get { return _rotation;}
        set { _rotation = Mathf.Min(value, MaxRotation);}
    }
    public float MaxAcceleration {
        get { return _maxAcceleration;}
        set { _maxAcceleration = Mathf.Max(0, value);}
    }
    // public Vector3 Acceleration
    
    public Vector3 Acceleration
    {
        get { return _acceleration;  } // Modifica
        set { _acceleration = value;}
    }


    public float MaxAngularAcc {
        get { return _maxAngularAcc;}
        set { _maxAngularAcc = Mathf.Max(0, value);}
    }

    public float AngularAcc {
        get { return _angularAcc;}
        set { _angularAcc = Mathf.Max(0, value);}
    }
    // public Vector3 Position. Recuerda. Esta es la única propiedad que trabaja sobre transform.
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public float Orientation {
        get { return _orientation;}
        set { _orientation = Bodi.mapTo360(value);}
    }
    
    public float Speed {
        get { return _speed;}
        set { _speed =  Mathf.Min( Mathf.Max(0, value), _maxSpeed);}
    }  

    // TE PUEDEN INTERESAR LOS SIGUIENTES MÉTODOS.
    // Añade todos los que sean referentes a la parte física.

    // public float Heading()
    //      Retorna el ángulo heading en (-180, 180) en grado o radianes. Lo que consideres
    public float Heading(){ //creo que tiene sentido pero a lo mejor me estoy marcando un triple
        if(Orientation >= 0){
            float remainder = Orientation % 360;
            if(0 <= remainder && remainder <= 180 ) {return remainder;}
            else {return remainder-360;}
            
        } else {
            float remainder = Orientation % -360;
            if(0 >= remainder  && remainder >= -180  ) {return remainder;}
            else {return remainder+360;}
        }
    }

    public static float mapTo360(float rotation){ //creo que tiene sentido pero a lo mejor me estoy marcando un triple
        
        float remainder = rotation % 360;
        return remainder;
        
    }

    public static float MapToRangePi(float rotation){
        const float PI = (float)Mathf.PI;
        if(rotation > 0){
            float remainder = rotation % 360;
            if(0 <= remainder && remainder <= 180 ) {return remainder * PI/180.0f;}
            else {return (remainder-360) * PI/180.0f;}
            
        } else {
            float remainder = rotation % -360;
            if(0 >= remainder  && remainder >= -180  ) {return remainder * PI/180.0f;}
            else {return (remainder+360) * PI/180.0f;}
        }
    }

    //Abandonad toda esperanza los que entreis aqui
    public static float sitienesPiyloquieresengradosPeroalreves(float pitoo){
        float grados = (pitoo * 180) / (float)Mathf.PI;

        return grados;
    }
    // public static float MapToRange(float rotation, Range r)
    //      Retorna un ángulo de (-180, 180) a (0, 360) expresado en grado or radianes
    // public float MapToRange(Range r)
    //      Retorna la orientación de este bodi, un ángulo de (-180, 180), a (0, 360) expresado en grado or radianes
    // public float PositionToAngle()
    //      Retorna el ángulo de una posición usando el eje Z como el primer eje
    // public Vector3 OrientationToVector()
    //      Retorna un vector a partir de una orientación usando Z como primer eje
    public Vector3 OrientationToVector() {
        float sin = Mathf.Sin(MapToRangePi(Orientation));
        float cos = Mathf.Cos(MapToRangePi(Orientation));
        Vector3 vector = new Vector3(sin, 0, cos);
        return vector;
    }    
    // public Vector3 VectorHeading()  // Nombre alternativo
    //      Retorna un vector a partir de una orientación usando Z como primer eje
    // public float GetMiniminAngleTo(Vector3 rotation)
    //      Determina el menor ángulo en 2.5D para que desde la orientación actual mire en la dirección del vector dado como parámetro
    // public void ResetOrientation()
    //      Resetea la orientación del bodi
    // public float PredictNearestApproachTime(Bodi other, float timeInit, float timeEnd)
    //      Predice el tiempo hasta el acercamiento más cercano entre este y otro vehículo entre B y T (p.e. [0, Mathf.Infinity])
    // public float PredictNearestApproachDistance3(Bodi other, float timeInit, float timeEnd)

}
