using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH4Script : MonoBehaviour
{
    public logicManager logicScript;

    // Start is called before the first frame update
    void Start()
    {
        

        logicScript = GameObject.FindGameObjectWithTag("logicManager").GetComponent<logicManager>();
        logicScript.HidrogenList.Clear();
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            logicScript.HidrogenList.Add(GameObject.FindGameObjectsWithTag("Player")[i]);
        }
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
