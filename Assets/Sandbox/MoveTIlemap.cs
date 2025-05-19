using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTIlemap : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    // Variables for zooming
    public float zoomSpeed = 1.0f;
    public float minZoom = 1.0f;
    public float maxZoom = 10.0f;
    public Camera mainCamera;

    void Start()
    {
        
    }

    void Update()
    {
        // Zooming with the scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);

        

        if (Input.GetMouseButtonDown(2))
        {
            isDragging = true;
            offset = transform.position - GetMouseWorldPos();
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
