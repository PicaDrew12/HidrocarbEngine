using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenWriterScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI reactieText;
    public  TextMeshProUGUI catalizatorText;
    //Variables
    public int hidrogenareCount;
    void Start()
    {
      InitVariables();
    }

    public void InitVariables()
    {
        hidrogenareCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
          
        }
        
    }

    public void TypeText(string type, int carbonCount,string reactie, string catalizator)
    {
        reactieText.text = "";
        catalizatorText.text = "";
    }

    public string HalogenareAlcan(int carbonCount)
    {
        string additive = "Cl2";
        string final = "";
        if(hidrogenareCount == 0)
        {
            final = $"CH4 + ";
        }
        return final;
    }


}
