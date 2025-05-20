using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnAlchena : MonoBehaviour
{
    public GameObject carbonPrefab;
    public GameObject hydrogenPrefab;
    public GameObject connectionPrefab;
    public Vector3 initialPosition;
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
    public Transform hydrogenTransform;
    public GameObject piConnection;

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

            if (i == 1 )
            {
                carbon = Instantiate(carbonPrefab, new Vector3(initialPosition.x, initialPosition.y + lift, initialPosition.z), Quaternion.Euler(0, 0, 0));
                carbon.tag = tagObj;

                connectionPosition = new Vector3(initialPosition.x + 0.5f, initialPosition.y + lift + positionYOffset, initialPosition.z);
                if (i == 1)
                {
                    piConnection = Instantiate(connectionPrefab, connectionPosition + new Vector3(0, 0.2f, 0), Quaternion.Euler(0, 0, 90 + rotationOffset));
                    piConnection.transform.parent = GameObject.FindGameObjectWithTag("Molecule").transform;
                    logicScript.CarbonConection.Add(piConnection);
                    Debug.Log("MAMA EI DE CHIMIE");
                }
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
                conection.transform.parent = GameObject.FindGameObjectWithTag("Molecule").transform;
                logicScript.CarbonConection.Add(conection);
            }
            initialPosition.x = initialPosition.x + 1.3f;
            riseIndex = riseIndex + 1;
        }

        //Invoke("wow", 5);
    }

    public void wow()
    {
        if (hydrogenTransform == null)
        {
            Debug.LogError("hydrogenTransform is not assigned! Cancelling operation.");
            return;
        }

        Debug.Log("OKAY");
        hydrogenTransform.position += new Vector3(-2f, 0f, 0f);

        if (logicScript.HidrogenConectionsList.Count > 1)
        {
            Transform hydrogenConectionTransform = logicScript.HidrogenConectionsList[1].transform;
            hydrogenConectionTransform.position += new Vector3(-1f, 0f, 0f);
        }

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
