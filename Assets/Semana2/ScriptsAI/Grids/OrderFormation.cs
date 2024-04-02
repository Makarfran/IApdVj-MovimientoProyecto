using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderFormation : MonoBehaviour
{
    FormationManager formationManager;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<FormationManager>() == null) { gameObject.AddComponent<FormationManager>(); }
        formationManager = GetComponent<FormationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("f") || Input.GetKey("x") || Input.GetKey("e")) 
        {
            if (Input.GetKey("f")) {formationManager.pattern = new Frontal();}
            else if (Input.GetKey("x")) { formationManager.pattern = new Formation360(); }
            else if(Input.GetKey("e")) { formationManager.pattern = new FormationEagle();  }

            if (formationManager.slotAssignments.Count > 0) 
            {
                    formationManager.RemoveAllCharacters();
            }

            List<GameObject> seleccionados = UnitsSelection.npcsSelected;
            formationManager.AddCharacters(seleccionados);
            formationManager.UpdateSlots();
        }
    }
}
