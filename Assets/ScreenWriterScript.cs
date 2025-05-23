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


    public void Print(string text, int baseTextSize = 37, bool delay = true)
    {
        Clear();

        string finalText = Format(text);

        if (reactieText == null)
        {
            Debug.LogWarning("ReactieText is not assigned.");
            return;
        }

        // Adjust font size based on text length
        int maxLength = 200; // Arbitrary value for when the font should be smallest
        float minFontScale = 0.5f; // Minimum scale of font size (e.g., 50% of base size)

        // Clamp text length factor between 0 and 1
        float lengthFactor = Mathf.Clamp01((float)finalText.Length / maxLength);
        float fontScale = Mathf.Lerp(1f, minFontScale, lengthFactor);

        int adjustedFontSize = Mathf.RoundToInt(baseTextSize * fontScale);
        reactieText.fontSize = adjustedFontSize;

        reactieText.text = finalText;

        if (delay)
        {
            StartCoroutine(WritingClearDelay(text));
        }
    }


    IEnumerator WritingClearDelay(string text)
    {
        // Base delay values
        float baseDelay = 3f;        // Minimum wait time
        float delayPerChar = 0.05f;  // Extra time per character

        // Calculate total delay
        float totalDelay = baseDelay + (text.Length * delayPerChar);

        yield return new WaitForSeconds(totalDelay);
        Clear();
    }

    public void PrintCatalizator(string text)
    {
        Clear();
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
