using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class alcanScript : MonoBehaviour
{
    public bool rotateEnabled = false;  // Set this variable to true to enable rotation
    public float rotationSpeed = 5f;   // Adjust the speed of rotation as needed
    Logic spawnCarbonScript;
    public Toggle rotationToggle;
    public Slider rotationSlider;
    public bool speedBasedOnSlider;
    public bool rotateEnabledAlDoileaPtCaPrimulNuMere = false;

    private void Start()
    {
        spawnCarbonScript = GameObject.FindGameObjectWithTag("carbonSpawner").GetComponent<Logic>();
        rotateEnabled = true;
        rotateEnabledAlDoileaPtCaPrimulNuMere = true;
    }

    public void startRotation()
    {
        rotateEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (speedBasedOnSlider)
        {
            rotationSpeed = rotationSlider.value;
        }

        if (rotationToggle.isOn)
        {
            if (rotateEnabledAlDoileaPtCaPrimulNuMere)
            {
                rotateEnabled = true;
            }
        }
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
        if (!rotationToggle.isOn)
        {
            RotateWithArrowKeys();
        }
    }

    private void RotateWithArrowKeys()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
    }


    [ContextMenu("Reset ROtation")]
    public void ResetRotation()
    {
        MoveAndRotateToObject(gameObject, gameObject.transform.position, Quaternion.Euler(0, 0, 0), 1);
    }

    public void MoveAndRotateToObject(GameObject obj, Vector3 targetPosition, Quaternion targetRotation, float speed)
    {
        StartCoroutine(MoveAndRotateCoroutine(obj, targetPosition, targetRotation, speed));
    }

    private IEnumerator MoveAndRotateCoroutine(GameObject obj, Vector3 targetPosition, Quaternion targetRotation, float speed)
    {
        float startTime = Time.time;
        Vector3 startPosition = obj.transform.position;
        Quaternion startRotation = obj.transform.rotation;

        while (Time.time - startTime < speed)
        {
            float t = (Time.time - startTime) / speed;
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            obj.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        // Ensure final position and rotation are exactly as desired
        obj.transform.position = targetPosition;
        obj.transform.rotation = targetRotation;
    }
}
