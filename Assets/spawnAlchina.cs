using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnAlchina : MonoBehaviour
{
    public GameObject carbonPrefab;
    public GameObject hydrogenPrefab;
    public GameObject connectionPrefab;
    public Vector3 initialPosition;

    // Variables to control the position and behavior of carbon and hydrogen
    private int lift;
    private float rotationOffset;
    private float positionYOffset;
    private float scaleYOffset;
    private Vector3 connectionPosition;

    // Logic scripts
    public logicManager logicScript;
    private alcanScript alcanScript;

    // List to store carbon GameObjects
    public List<GameObject> carbonList = new List<GameObject>();

    // Current molecule components
    private GameObject currentCarbon;
    private GameObject currentConnection;
    private string tagObj;
    private Transform hydrogenTransform;

    public GameObject piConnection;
    public int indexForKeepingCountOfCarbons;

    // Called once on script initialization
    private void Start()
    {
        indexForKeepingCountOfCarbons = 0;
        logicScript = GameObject.FindGameObjectWithTag("logicManager").GetComponent<logicManager>();
        alcanScript = GameObject.FindGameObjectWithTag("Molecule").GetComponent<alcanScript>();
    }

    // Generates the molecule with the specified number of carbon atoms
    public void GenerateMolecule(int carbonCount)
    {
        indexForKeepingCountOfCarbons = 0;
        ResetInitialPosition();
        int riseIndex = 1;
        int hydrogenCount = CalculateHydrogenCount(carbonCount);

        for (int i = 1; i <= carbonCount; i++)
        {
            SetCarbonAttributes(riseIndex);

            if (IsFirstOrLastCarbon(i, carbonCount))
            {
               
                InstantiateEndCarbon(i);
            }
            else
            {
                InstantiateMiddleCarbon();
                
            }

            AddCarbonToList();

            if (IsNotLastCarbon(i, carbonCount))
            {
                InstantiateConnection();
            }

            MoveToNextPosition();
            riseIndex++;
        }
    }

    // Resets the initial position for carbon placement
    private void ResetInitialPosition()
    {
        initialPosition.x = 0;
    }

    // Calculates the total hydrogen count based on the number of carbon atoms
    private int CalculateHydrogenCount(int carbonCount)
    {
        return carbonCount * 2 + 2;
    }

    // Sets attributes for carbon positioning and tagging
    private void SetCarbonAttributes(int riseIndex)
    {
        if (riseIndex % 2 == 0)
        {
            ConfigureCarbonAttributes(0, 36, 0.5f, 0.3f, "down");
        }
        else
        {
            ConfigureCarbonAttributes(1, -36, -0.5f, 0f, "up");
        }
    }

    // Configures carbon attributes for position and rotation
    private void ConfigureCarbonAttributes(int liftValue, float rotation, float positionY, float scaleY, string tag)
    {
        lift = liftValue;
        rotationOffset = rotation;
        positionYOffset = positionY;
        scaleYOffset = scaleY;
        tagObj = tag;
    }

    // Checks if the current carbon is the first or last in the sequence
    private bool IsFirstOrLastCarbon(int currentIndex, int totalCarbons)
    {
        return currentIndex == 1 || currentIndex == totalCarbons;
    }

    // Instantiates the first or last carbon atom and its connections
    private void InstantiateEndCarbon(int index)
    {
        InstantiateCarbon(initialPosition, Quaternion.identity);

        if (index == 1)
        {
            CreatePiConnection();
            LogDebugInfo();
        }
    }

    // Creates a π-bond connection for the first carbon
    private void CreatePiConnection()
    {
        Vector3 piConnectionOffset = new Vector3(0, 0.2f, 0);
        InstantiatePiConnection(piConnectionOffset);
        piConnectionOffset.y = 0.1f;
        InstantiatePiConnection(piConnectionOffset);
    }

    // Instantiates a π-bond connection and sets its parent
    private void InstantiatePiConnection(Vector3 offset)
    {
        piConnection = Instantiate(connectionPrefab, connectionPosition + offset, Quaternion.Euler(0, 0, 90 + rotationOffset));
        piConnection.transform.parent = GetMoleculeTransform();
        logicScript.CarbonConection.Add(piConnection);
    }

    // Logs debug information
    private void LogDebugInfo()
    {
        Debug.Log("MAMA EI DE CHIMIE");
        Debug.Log("DA TOT E FAINA");
    }

    // Instantiates a middle carbon atom
    private void InstantiateMiddleCarbon()
    {
        InstantiateCarbon(initialPosition, Quaternion.identity);
    }

    // Instantiates a carbon prefab at a given position and rotation
    private void InstantiateCarbon(Vector3 position, Quaternion rotation)
    {
        currentCarbon = Instantiate(carbonPrefab, new Vector3(position.x, position.y + lift, position.z), rotation);
        currentCarbon.tag = tagObj;
        currentCarbon.GetComponent<SpawnAlchinaCarbons>().carbonIndex = indexForKeepingCountOfCarbons;
        connectionPosition = new Vector3(position.x + 0.5f, position.y + lift + positionYOffset, position.z);
        indexForKeepingCountOfCarbons++;
    }

    // Adds the current carbon atom to the carbon list
    private void AddCarbonToList()
    {
        carbonList.Add(currentCarbon);
    }

    // Checks if the current carbon is not the last in the sequence
    private bool IsNotLastCarbon(int currentIndex, int totalCarbons)
    {
        return currentIndex != totalCarbons;
    }

    // Instantiates a connection between carbon atoms
    private void InstantiateConnection()
    {
        currentConnection = Instantiate(connectionPrefab, connectionPosition, Quaternion.Euler(0, 0, 90 + rotationOffset));
        currentConnection.transform.parent = GetMoleculeTransform();
        logicScript.CarbonConection.Add(currentConnection);
    }

    // Retrieves the Molecule GameObject's transform
    private Transform GetMoleculeTransform()
    {
        return GameObject.FindGameObjectWithTag("Molecule").transform;
    }

    // Moves the position for the next carbon atom
    private void MoveToNextPosition()
    {
        initialPosition.x += 1.3f;
    }

    // Adjusts the position of the first hydrogen and its connection
    public void wow()
    {
        Debug.Log("OKAY");

        hydrogenTransform.position += new Vector3(-2f, 0f, 0f);

        Transform hydrogenConectionTransform = logicScript.HidrogenConectionsList[1].transform;
        hydrogenConectionTransform.position += new Vector3(-1f, 0f, 0f);
    }

    // Updates the hydrogen transform if logicScript is valid
    private void Update()
    {
        if (IsLogicScriptValid())
        {
            UpdateHydrogenTransform();
        }
    }

    // Checks if logicScript and its hydrogen list are valid
    private bool IsLogicScriptValid()
    {
        return logicScript != null && logicScript.HidrogenList.Count > 1 && logicScript.HidrogenList[1] != null;
    }

    // Updates the hydrogen transform based on the logic script
    private void UpdateHydrogenTransform()
    {
        hydrogenTransform = logicScript.HidrogenList[1].transform;
    }
}
