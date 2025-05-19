using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAlchenaHidrogens : MonoBehaviour
{
    private int collisionCount = 0;
    public GameObject hydrogen;
    public GameObject conection;
    float conectionOffset;
    public logicManager logicScipt;
    GameObject spawnedHydrogen;
    GameObject hydrogenConection;
    int rotationZ;
    int rotationX;
    Quaternion rotation;
    float lift;
    float conectionRotationX;
    float conectionPositionY;
    int flip;
    public Logic spawnCarbonScript;
    public string carbonType;
    public spawnAlchena spawnAlchenaScript;





    void Start()
    {
        spawnCarbonScript = GameObject.FindGameObjectWithTag("carbonSpawner").GetComponent<Logic>();
        collisionCount = 0;
        logicScipt = GameObject.FindGameObjectWithTag("logicManager").GetComponent<logicManager>();
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Molecule").transform;
        spawnAlchenaScript = GameObject.FindGameObjectWithTag("spawnAlchena").GetComponent<spawnAlchena>();
        Invoke("CheckCollision", 1f);
    }

    // Function to be called after the delay
    void CheckCollision()
    {


        SpawnOnAllSides(collisionCount);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Conection"))
        {
            // Get the Y angle of the object
            float yAngle = other.transform.eulerAngles.y;



            // Increment collision count
            collisionCount++;

            // Your additional code here
        }
    }


    void SpawnOnAllSides(int colisionCount)
    {

        Transform sphereTransform = transform;
        int iterationNumber = 4 - colisionCount;



        for (int i = 0; i < iterationNumber; i++)
        {


            if (iterationNumber == 2)//ARE 2 MAINI
            {
                if (i % 2 == 0)
                {
                    flip = 1;
                }
                else
                {
                    flip = -1;
                }
                if (gameObject.tag == "up")
                {
                    lift = 0.5f;
                    conectionRotationX = -33;
                    conectionPositionY = -0.3f;

                }
                else
                {
                    lift = -0.5f;
                    conectionRotationX = 33;
                    conectionPositionY = 0.3f;

                }

                Quaternion rotation = Quaternion.Euler(0, i * 180, 0);

                Vector3 position = sphereTransform.position + rotation * Vector3.forward + new Vector3(0, lift, 0);



                spawnedHydrogen = Instantiate(hydrogen, position, rotation);



                hydrogenConection = Instantiate(conection, spawnedHydrogen.transform.position + rotation * new Vector3(0, conectionPositionY, -0.5f), Quaternion.Euler(flip * (90 + conectionRotationX), 0, 0));
                spawnedHydrogen.transform.parent = sphereTransform;
                hydrogenConection.transform.parent = sphereTransform;
            }
            else //ARE 3 MAINI
            {

                if (i == 1)
                {
                    rotationZ = 90;
                    rotationX = 0;
                    if (gameObject == spawnAlchenaScript.carbonList[0])
                    {
                        flip = 1;

                    }
                    else
                    {
                        flip = -1;
                    }


                    if (gameObject.tag == "up")
                    {
                        lift = -0.5f;
                        conectionRotationX = 33;
                        conectionPositionY = 0.3f;


                    }
                    else
                    {
                        lift = 0.5f;
                        conectionRotationX = -33;
                        conectionPositionY = -0.3f;

                    }


                    rotation = Quaternion.Euler(0, 90 * i, 0);
                    Vector3 position = sphereTransform.position + rotation * Vector3.forward + new Vector3(0, lift, 0);


                    spawnedHydrogen = Instantiate(hydrogen, position, rotation);

                    hydrogenConection = Instantiate(conection, spawnedHydrogen.transform.position + rotation * new Vector3(0, conectionPositionY, -0.5f), Quaternion.Euler(0, 0, 90 + (rotationX + conectionRotationX) * flip));



                    spawnedHydrogen.transform.parent = sphereTransform;
                    hydrogenConection.transform.parent = sphereTransform;


                }
                else
                {
                    rotationZ = 0;
                    rotationX = 90;
                    if (i == 0)
                    {
                        flip = 1;

                    }

                    if (i == 1)
                    {
                        flip = 1;

                    }
                    if (i == 2)
                    {
                        flip = -1;

                    }
                    if (gameObject.tag == "up")
                    {
                        lift = 0.5f;
                        conectionRotationX = -33;
                        conectionPositionY = -0.3f;


                    }
                    else
                    {
                        lift = -0.5f;
                        conectionRotationX = 33;
                        conectionPositionY = 0.3f;

                    }


                    rotation = Quaternion.Euler(0, 90 * i, 0);
                    Vector3 position = sphereTransform.position + rotation * Vector3.forward + new Vector3(0, lift, 0);


                    spawnedHydrogen = Instantiate(hydrogen, position, rotation);

                    hydrogenConection = Instantiate(conection, spawnedHydrogen.transform.position + rotation * new Vector3(0, conectionPositionY, -0.5f), Quaternion.Euler((rotationX + conectionRotationX) * flip, 0, rotationZ));



                    spawnedHydrogen.transform.parent = sphereTransform;
                    hydrogenConection.transform.parent = sphereTransform;

                }
                //////////



            }

            if (logicScipt != null)
            {
                logicScipt.Listing(spawnedHydrogen, hydrogenConection);
            }
            else
            {
                Debug.Log("WTF");
            }




        }

    }

    // fra deci asta  detrectedaza cand apasi pe carbon
    void Update()
    {
        // Check for right mouse button click
        if (Input.GetMouseButtonDown(1)) // 1 corresponds to the right mouse button
        {
            // Cast a ray from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits the current game object
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                if (gameObject == spawnCarbonScript.carbonList[0])
                {
                    collisionCount = 1;
                }
                // Right-click detected on the current game object

                Debug.Log("Right-clicked on: " + gameObject.name);
                Info();

                // Add your custom logic here
            }
        }
    }

    public void Info()
    {
        if (collisionCount == 1)
        {
            carbonType = "primar";
        }
        else if (collisionCount == 2)
        {
            carbonType = "secundar";
        }
        Debug.Log(carbonType);
    }
}
