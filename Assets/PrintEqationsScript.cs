using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        mainLogic.screenWriterScript.Print(FormatForAddition(formula,"Cl2",Halogenate(formula)));
    }

    public static string Halogenate(string formula)
    {
        string[] groups = formula.Split('-');
        for (int i = 0; i < groups.Length; i++)
        {
            string group = groups[i];

            // Find number of H atoms in group
            Match match = Regex.Match(group, @"C(H\d*)(Cl\d*)?");
            if (match.Success)
            {
                string hPart = match.Groups[1].Value; // e.g., H3
                int hCount = hPart == "H" ? 1 : int.Parse(hPart.Substring(1));

                if (hCount > 0)
                {
                    // Replace one H with Cl
                    hCount--;

                    string newGroup = "C";
                    if (hCount > 0)
                        newGroup += "H" + hCount;
                    if (!string.IsNullOrEmpty(match.Groups[2].Value))
                    {
                        // already has some Cl
                        string clPart = match.Groups[2].Value;
                        int clCount = clPart == "Cl" ? 1 : int.Parse(clPart.Substring(2));
                        clCount++;
                        newGroup += "Cl" + clCount;
                    }
                    else
                    {
                        newGroup += "Cl";
                    }

                    // Replace old group
                    groups[i] = newGroup;
                    return string.Join("-", groups) + " + HCl";
                }
            }
        }

        return "No more hydrogens to replace.";
    }
}
