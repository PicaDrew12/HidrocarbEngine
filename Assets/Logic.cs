using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Logic : MonoBehaviour
{
    public GameObject carbonPrefab;
    public GameObject hydrogenPrefab;
    public GameObject connectionPrefab;
    public Vector3 initialPosition;
    public Transform smallConection;
    int lift;
    float rotationOffset;
    float positionYOffset;
    float scaleYOffset;
    Vector3 connectionPosition;
    public logicManager logicScript;
    public List<GameObject> carbonList = new List<GameObject>();
    GameObject carbon;
    GameObject conection;

    string tagObj;
    alcanScript alcanScript;
    Transform hydrogenTransform;
    public GameObject Ch4;

    private void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("logicManager").GetComponent<logicManager>();
        alcanScript = GameObject.FindGameObjectWithTag("Molecule").GetComponent<alcanScript>();
    }



    public void GenerateMolecule(int carbonCount)
    {
        int riseIndex = 1;
        initialPosition.x = 0;


        int hidrogenCount = carbonCount * 2 + 2;
        if (carbonCount == 1)
            
        {
            logicScript.HidrogenList.Clear();
            Instantiate(connectionPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
            GameObject c4 =  Instantiate(Ch4, new Vector3(0,0,0),Quaternion.identity);

            carbonList.Add(c4);
            
            foreach (Transform child in c4.transform)
            {
                // Perform operations on each child
                Debug.Log("Child name: " + child.name);
                
                if(child.tag == "smallConection")
                {
                    logicScript.HidrogenConectionsList.Add(child.gameObject);
                }
            }

            c4.transform.parent = GameObject.FindGameObjectWithTag("Molecule").transform;

        }
        else
        {


            for (int i = 1; i <= carbonCount; i++)
            {
                if (riseIndex % 2 == 0)
                {
                    lift = 0;
                    rotationOffset = 36;
                    positionYOffset = 0.5f;
                    scaleYOffset = 0.3f;
                    tagObj = "down";
                }
                else
                {
                    lift = 1;
                    rotationOffset = -36;
                    positionYOffset = -0.5f;
                    scaleYOffset = 0f;
                    tagObj = "up";
                }

                if (i == 1 || i == carbonCount)
                {
                    carbon = Instantiate(carbonPrefab, new Vector3(initialPosition.x, initialPosition.y + lift, initialPosition.z), Quaternion.Euler(0, 0, 0));
                    carbon.tag = tagObj;
                    connectionPosition = new Vector3(initialPosition.x + 0.5f, initialPosition.y + lift + positionYOffset, initialPosition.z);
                }
                else
                {
                    carbon = Instantiate(carbonPrefab, new Vector3(initialPosition.x, initialPosition.y + lift, initialPosition.z), Quaternion.identity);
                    carbon.tag = tagObj;
                    connectionPosition = new Vector3(initialPosition.x + 0.5f, initialPosition.y + lift + positionYOffset, initialPosition.z);

                }
                carbonList.Add(carbon);
                if (i != carbonCount)
                {
                    conection = Instantiate(connectionPrefab, connectionPosition, Quaternion.Euler(0, 0, 90 + rotationOffset));
                }
                conection.transform.parent = GameObject.FindGameObjectWithTag("Molecule").transform;
                initialPosition.x = initialPosition.x + 1.3f;
                riseIndex = riseIndex + 1;

            }

            initialPosition = new Vector3(0, 0, 0);
            riseIndex = 1;
            Invoke("wow", 0.3f);

        }
    }
    //FLiping First Hydrogen and its conection :)
    public void wow()
    {
        Debug.Log("OKAY");

        
        hydrogenTransform.position += new Vector3(-2f, 0f, 0f);

        Transform hydrogenConectionTransform = logicScript.HidrogenConectionsList[1].transform;
        hydrogenConectionTransform.position += new Vector3(-1f, 0f, 0f);
        alcanScript.startRotation();


    }
    void Update()
    {
        if (logicScript != null && logicScript.HidrogenList.Count > 1 && logicScript.HidrogenList[1] != null)
        {
            hydrogenTransform = logicScript.HidrogenList[1].transform;
        }
        
    }


}
