using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controlador : MonoBehaviour
{
    public GameObject R;
    public GameObject A;
    public GameObject ganador;
    public bool gameEnded = false;
    public List<GameObject> teamR = new List<GameObject>();
    public List<GameObject> teamA = new List<GameObject>();
    public GameObject baseRoja;
    public GameObject baseAzul;
    public List<GameObject> basesInicioTeamR = new List<GameObject>();
    public List<GameObject> basesInicioTeamA = new List<GameObject>();
    public List<GameObject> basesTeamR = new List<GameObject>();
    public List<GameObject> basesTeamA = new List<GameObject>();
    public GameObject botonrestart;
    public GameObject textWA;
    public GameObject textWR;
    public GameObject HRoja;
    public GameObject HAzul;
    public List<GameObject> zonasH = new List<GameObject>();
    public string ModoAzul;
    public string ModoRojo;
    private bool manualRojo = false;
    private bool manualAzul = false;
    private bool guerraTotal = false;
    public GameObject modoDefensivoAzul;
    public GameObject modoOfensivoAzul;
    public GameObject modoDefensivoRojo;
    public GameObject modoOfensivoRojo;
    public GameObject guerraTotalBoton;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < R.transform.childCount; i++)
        {
            teamR.Add(R.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < A.transform.childCount; i++)
        {
            teamA.Add(A.transform.GetChild(i).gameObject);
        }
        ModoAzul = "Equilibrado";
        ModoRojo = "Equilibrado";
    }

    // Update is called once per frame
    void Update()
    {
        checkBasesBando();
        checkModo("R");
        checkModo("A");
        if (!gameEnded){

            hasAWon();
            hasRWon();

        }
        //Debug.Log("Dominio Azul " + getDominio("A") );
        //Debug.Log("Dominio Rojo " + getDominio("R") );

        if(gameEnded && ganador == A){
            //Debug.Log("Victoria Azul");
            botonrestart.SetActive(true);
            textWA.SetActive(true);
            //mandar mensaje a canvas para que active elementos de pantalla final
        } else if(gameEnded){
            //Debug.Log("Victoria Roja");
            botonrestart.SetActive(true);
            textWR.SetActive(true);
            //mandar mensaje a canvas que active
        }
        
    }

    public void checkBasesBando(){ // realiza cambios de bando
        List<GameObject> aux1 = new List<GameObject>();
        List<GameObject> aux2 = new List<GameObject>();
        foreach(GameObject r in basesTeamR){
            KeypointBase rbase = r.GetComponent<KeypointBase>();
            if(rbase.getBando() == "A"){
                basesTeamA.Add(r);
                aux1.Add(r);
            }
        }
        foreach(GameObject a in basesTeamA){
            KeypointBase abase = a.GetComponent<KeypointBase>();
            if(abase.getBando() == "R"){
                basesTeamR.Add(a);
                aux2.Add(a);
            }
        }
        foreach(GameObject r in aux1){
            basesTeamR.Remove(r);
        }
        foreach(GameObject a in aux2){
            basesTeamA.Remove(a);
        }

    }

    public float getDominio(string Bando){
        int count;
        List<GameObject> bases;
        float uno = 1f;
        GameObject basep;
        if(Bando == "A"){
            bases = basesTeamA;
            basep = baseAzul;
            KeypointBase baseprin = basep.GetComponent<KeypointBase>();
            if(baseprin.getBando() == "R"){
                uno = 0f;
            }
        } else {
            bases = basesTeamR;
            basep = baseRoja;
            KeypointBase baseprin = basep.GetComponent<KeypointBase>();
            if(baseprin.getBando() == "A"){
                uno = 0f;
            }
        }
        
        count = bases.Count;
        return (((float) count + uno) / 6f) *100f;   
    }

    void hasAWon(){
       KeypointBase rbase = baseRoja.GetComponent<KeypointBase>();
       if(rbase.getBando() == "A"){
            ganador = A;
            gameEnded = true;
       }
    }
    void hasRWon(){
       KeypointBase abase = baseAzul.GetComponent<KeypointBase>();
       if(abase.getBando() == "R"){
            ganador = R;
            gameEnded = true;
       }
    }

    public void restartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void setModo(string bando, string modo) 
    {
        if (!manualAzul)
            if (bando == "A") ModoAzul = modo;
        if (!manualRojo)
            if (bando == "R") ModoRojo = modo;
    }

    public string getModo(string bando)
    {
        string modo = "";
        if (bando == "A") modo = ModoAzul;
        if (bando == "R") modo = ModoRojo;
        return modo;
    }


    private void checkModo(string bando) 
    {
        float dominio = getDominio(bando);
        if (dominio > 60)
        {
            setModo(bando, "Ofensivo");
            //Debug.Log("bando: " + bando + " Modo: " + getModo(bando));
        }
        if (dominio <= 60 && dominio >= 40)
        {    
            setModo(bando, "Equilibrado");
            //Debug.Log("bando: " + bando + " Modo: " + getModo(bando));
        }
        if (dominio < 40)
        {
            setModo(bando, "Defensivo");
            //Debug.Log("bando: " + bando + " Modo: " + getModo(bando));
        }
            
    }

    public void activarModoOfensivoRojo() 
    {
        
        if (manualRojo)
        {
            manualRojo = false;
            modoDefensivoRojo.SetActive(true);
        }
        else 
        {
            manualRojo = true;
            modoDefensivoRojo.SetActive(false);
            ModoRojo = "Ofensivo";
        }
        
    }

    public void activarModoDefensivoRojo() 
    {
        
        if (manualRojo)
        {
            manualRojo = false;
            modoOfensivoRojo.SetActive(true);
        }
        else
        {
            manualRojo = true;
            modoOfensivoRojo.SetActive(false);
            ModoRojo = "Defensivo";
        }
        
    }

    public void activarModoOfensivoAzul()
    {
        if (manualAzul)
        {
            manualAzul = false;
            modoDefensivoAzul.SetActive(true);
        }
        else
        {
            manualAzul = true;
            modoDefensivoAzul.SetActive(false);
            ModoAzul = "Ofensivo";
        }
    }

    public void activarModoDefensivoAzul()
    {
        if (manualAzul)
        {
            manualAzul = false;
            modoOfensivoAzul.SetActive(true);
        }
        else
        {
            manualAzul = true;
            modoOfensivoAzul.SetActive(false);
            ModoAzul = "Defensivo";
        }
    }

    
    public void activarGuerraTotal() 
    {
        if (guerraTotal)
        {
            guerraTotal = false;
            modoOfensivoAzul.SetActive(true);
            modoDefensivoAzul.SetActive(true);
            modoOfensivoRojo.SetActive(true);
            modoDefensivoRojo.SetActive(true);
            manualAzul = false;
            manualRojo = false;
        }
        else 
        {
            guerraTotal = true;
            modoOfensivoAzul.SetActive(false);
            modoDefensivoAzul.SetActive(false);
            modoOfensivoRojo.SetActive(false);
            modoDefensivoRojo.SetActive(false);
            manualAzul = true;
            manualRojo = true;
            ModoAzul = "GuerraTotal";
            ModoRojo = "GuerraTotal";
        }
    }
    
}
