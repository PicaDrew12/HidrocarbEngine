using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClorScript : MonoBehaviour
{
    public Transform targetTransform;
    
    public logicManager logicManagerScript;
    public GameObject target;
    public MainLogic mainLogic;
    public ConectionScript conectionScript;
    public GameObject hidrogen;
    public GameObject wtf;
    alcanScript alcanScript;
    public float speed;



    void Start()
    {
        
        
        logicManagerScript = GameObject.FindGameObjectWithTag("logicManager").GetComponent<logicManager>();
        mainLogic = GameObject.FindGameObjectWithTag("MainLogic").GetComponent<MainLogic>();
        if(mainLogic.tipHidrocarbura == "Alchena")
        {
            Debug.Log("E alchena la halogenare");
            targetTransform = logicManagerScript.HidrogenList[4].transform;
        }
        else
        {
            targetTransform = logicManagerScript.HidrogenList[mainLogic.index].transform;
        }
        
        conectionScript = GameObject.FindGameObjectWithTag("Conection").GetComponent<ConectionScript>();
        wtf = GameObject.FindGameObjectWithTag("Conection");
        alcanScript = GameObject.FindGameObjectWithTag("Molecule").GetComponent<alcanScript>();
        speed = mainLogic.HalogenareSpeed;

    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        mainLogic.isAnimating = true;
        mainLogic.isClorInMovement = true;
        if (targetTransform != null)
        {
            // Calculate the direction from the current position to the target position
            Vector3 direction = targetTransform.position - transform.position;

            // Normalize the direction vector to have a magnitude of 1
            direction.Normalize();

            // Move the object towards the target with a constant speed
            transform.Translate(direction * speed * Time.deltaTime);

            // Check if the object has reached the target position
            if (Vector3.Distance(transform.position, targetTransform.position) < 0.1f)
            {
                // Set the object's position to the target position
                transform.position = targetTransform.position;

                // Optional: Do something when the object reaches the target
                Debug.Log("Object reached the target!");
                mainLogic.isClorInMovement = false;

                enabled = false;
                mainLogic.conectionObject.transform.parent = null;
                Vector3 hidrogenVector = mainLogic.lastHidrogen.transform.position;
                Destroy(mainLogic.lastHidrogen);
                GameObject spawnedHidrogen = Instantiate(hidrogen,hidrogenVector, Quaternion.identity);
                spawnedHidrogen.transform.parent = mainLogic.conectionObject.transform;
                
                conectionScript.FlyTo();
                mainLogic.index ++;
                
                mainLogic.ClorObject.transform.parent = GameObject.FindGameObjectWithTag("Molecule").transform;
                alcanScript.rotateEnabled = true;
                mainLogic.isAnimating = false;




            }
        }
        else
        {
            Debug.LogError("Target transform is not assigned!");
        }
    }
    public void doShit()
    {
        
        Debug.Log("DID SHIT");
    }
}
