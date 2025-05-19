using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicManager : MonoBehaviour
{
    
    
    

    public List<GameObject> CarbonConection = new List<GameObject>();
    public List<GameObject> HidrogenList = new List<GameObject>();
    public int index;
    public List<GameObject> HidrogenConectionsList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }
    public void Counter()
    {
        index = index + 1;
    }
    
    public void Listing(GameObject Hydrogen, GameObject HydrogenConection)
    {
        HidrogenList.Add(Hydrogen);
        HidrogenConectionsList.Add(HydrogenConection);


    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
