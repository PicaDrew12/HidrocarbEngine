using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 10f;      // Default speed of the orbit around the object
    public float zoomSpeed = 5f;           // Zoom speed
    public float minZoom = 2f;             // Minimum zoom distance
    public float maxZoom = 10f;            // Maximum zoom distance
    public float smoothness = 5f;           // Smoothing factor for movement
    public Transform targetObject;          // The object to orbit around

    // UI Elements
    public Toggle rotationToggle;            // Toggle to enable/disable rotation
    public Slider rotationSlider;            // Slider to control rotation speed
    public bool speedBasedOnSlider = false; // Use slider for speed control

    private Vector3 currentRotation;         // Current rotation of the camera
    private float currentZoom;               // Current zoom level
    private Vector3 panOffset;               // Offset for panning
    private Vector3 orbitCenter;             // Current center of orbiting
    private Vector3 velocity = Vector3.zero; // Velocity for smooth damping
    private float previousMouseX;            // Previous mouse X position for panning
    private float previousMouseY;            // Previous mouse Y position for panning

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object not set for CameraMovement script.");
            enabled = false;
            return;
        }

        // Initialize current rotation and zoom based on starting position
        currentRotation = transform.eulerAngles;
        currentZoom = Vector3.Distance(transform.position, targetObject.position);
        orbitCenter = targetObject.position; // Initialize orbit center at target's position

        // Set up UI listeners
        rotationToggle.onValueChanged.AddListener(delegate { ToggleRotation(rotationToggle.isOn); });
        rotationSlider.onValueChanged.AddListener(delegate { UpdateRotationSpeed(rotationSlider.value); });

        // Ensure rotation speed is set initially
        rotationSpeed = rotationSlider.value;
    }

    private void ToggleRotation(bool isOn)
    {
        if (isOn)
        {
            // Reset to original rotation when toggled back on
            currentRotation = Vector3.zero;
        }
    }

    private void UpdateRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }

    private void Update()
    {
        // Handle zoom based on scroll input
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scrollWheel * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // Update the orbit center based on panning
        HandlePanning();

        // If the rotation toggle is enabled, orbit around the orbit center
        if (rotationToggle.isOn)
        {
            currentRotation.y += rotationSpeed * Time.deltaTime; // Constant horizontal rotation
        }
        else
        {
            // Handle arrow key input for orbiting when toggle is off
            HandleArrowKeyOrbit();
        }

        // Calculate the orbit position
        Quaternion rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        Vector3 orbitPosition = orbitCenter + rotation * Vector3.back * currentZoom;

        // Smoothly update camera position
        transform.position = Vector3.SmoothDamp(transform.position, orbitPosition, ref velocity, smoothness * Time.deltaTime);

        // Smoothly look at the target
        transform.LookAt(orbitCenter);
    }

    private void HandleArrowKeyOrbit()
    {
        // Handle left/right arrow keys for orbiting
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentRotation.y -= rotationSpeed * Time.deltaTime; // Rotate left
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentRotation.y += rotationSpeed * Time.deltaTime; // Rotate right
        }

        // Handle up/down arrow keys for changing orbit axis
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentRotation.x -= rotationSpeed * Time.deltaTime; // Rotate up
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentRotation.x += rotationSpeed * Time.deltaTime; // Rotate down
        }
    }

    private void HandlePanning()
    {
        // Panning logic with right mouse button
        if (Input.GetMouseButton(1)) // Right mouse button is pressed
        {
            // Calculate mouse movement
            float deltaX = Input.mousePosition.x - previousMouseX;
            float deltaY = Input.mousePosition.y - previousMouseY;

            // Update the orbit center based on mouse movement
            orbitCenter += new Vector3(-deltaX * 0.01f, -deltaY * 0.01f, 0); // Adjust sensitivity as needed

            // Store current mouse position for next frame
            previousMouseX = Input.mousePosition.x;
            previousMouseY = Input.mousePosition.y;
        }
        else
        {
            // Update previous mouse positions when not panning
            previousMouseX = Input.mousePosition.x;
            previousMouseY = Input.mousePosition.y;
        }
    }
}
