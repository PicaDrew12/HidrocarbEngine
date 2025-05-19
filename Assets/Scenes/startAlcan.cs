using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startAlcan : MonoBehaviour
{
    public bool rotateEnabled = false;  // Set this variable to true to enable rotation
    public float rotationSpeed = 5f;   // Adjust the speed of rotation as needed
    Logic spawnCarbonScript;
    
    
    public bool speedBasedOnSlider;
    private void Start()
    {
        
        rotateEnabled = true;


    }
    public void startRotation()
    {

        rotateEnabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        

       
        
            if (rotateEnabled)
            {

                // Rotate the GameObject on the Y-axis
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        

        if (Input.GetMouseButtonDown(1)) // 1 corresponds to the right mouse button
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits a Collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is the one you're interested in
                if (hit.collider.gameObject == gameObject)
                {
                    // Perform actions when right-clicking on the object
                    Debug.Log("Right-clicked on " + gameObject.name);
                }
            }
        }

        // Check if rotation is enabled

    }
}
