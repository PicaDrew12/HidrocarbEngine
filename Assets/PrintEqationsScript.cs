using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrintEqationsScript : MonoBehaviour
{
    // Start is called before the first frame update
    MainLogic mainLogic;


    void Start()
    {
         mainLogic = GameObject.FindAnyObjectByType<MainLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string FormatForAddition(string molecule, string adder, string result)
    {
        return $"{molecule} + {adder} => {result}";
    }

    public void FormareAldehidaFormicaAlcani()
    {
        mainLogic.screenWriterScript.Print("CH4 + O2 => CH2O + H2O");
    }

    public void HalogenareAlcan(int carbonCount, string formula)
    {
        mainLogic.screenWriterScript.Print(FormatForAddition(formula,"Cl2",""));
    }
}
