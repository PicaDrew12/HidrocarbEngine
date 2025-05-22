using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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


    public string Format(string input)
    {
        // Convert A2 → A<sub>2</sub>
        input = Regex.Replace(input, @"(?<letter>[A-Za-z])(?<number>\d+)", "${letter}<sub>${number}</sub>");

        // Convert **bold** → <b>bold</b>
        input = Regex.Replace(input, @"\*(.*?)\*", "<b>$1</b>");

        // Convert #italic# → <i>italic</i>
        input = Regex.Replace(input, @"#(.*?)#", "<i>$1</i>");

        return input;
    }


    public void Print(string text, int textSize = 37, bool delay = true)
    {
       string finalText = Format(text);
        reactieText.fontSize = textSize;
        reactieText.text = finalText;
        if (delay)
        {
            StartCoroutine(WritingClearDelay());
        }

        
    }

    IEnumerator WritingClearDelay()
    {
        yield return new WaitForSeconds(4);
        Clear();
       
    }

    public void PrintCatalizator(string text)
    {
        catalizatorText.text = text;    
    }

    public void TypeText(string type, int carbonCount,string reactie, string catalizator)
    {
        reactieText.text = "";
        catalizatorText.text = "";
    }

    public void Clear()
    {
        reactieText.text = "";
        catalizatorText.text = "";
    }
    /*
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
    */


}
