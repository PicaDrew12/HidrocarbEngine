using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectionScript : MonoBehaviour
{
    public MainLogic mainLogic;

     // Adjust the speed as needed
    private Vector3 targetPosition;
    private bool isFlying = false;

    void Start()
    {
        
        mainLogic = GameObject.FindGameObjectWithTag("MainLogic").GetComponent<MainLogic>();
        
    }

    void Update()
    {
        if (isFlying)
        {
            GameObject connectionObject = GameObject.FindGameObjectWithTag("clorConection");

            if (connectionObject != null)
            {
                // Move towards the target position
                connectionObject.transform.position = Vector3.MoveTowards(connectionObject.transform.position, targetPosition, mainLogic.HalogenareSpeed * Time.deltaTime);

                // Check if the object is close enough to the target position
                if (Vector3.Distance(connectionObject.transform.position, targetPosition) < 0.01f)
                {
                    isFlying = false;
                    Destroy(connectionObject );
                    Debug.Log("DELETED");
                    
                    // Perform any additional actions when the object reaches the target
                }
            }
            else
            {
                Debug.LogWarning("No object with tag 'clorConection' found.");
            }
        }
    }

    public void StartFlying()
    {
        Invoke("FlyTo", 1f); // Adjust the delay as needed
    }

    public void FlyTo()
    {
        targetPosition = new Vector3(-5, 0, -5);
        isFlying = true;
        Debug.Log("DIDntttttttttttt SHIT");
        mainLogic.HalogenareSpeed = mainLogic.HalogenareSpeed+2;
    }

}
