using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // The object to rotate around
    public float orbitSpeed = 1f; // Speed of rotation
    public float orbitRadius = 5f; // Radius of the orbit

    private Vector3 orbitOffset; // Offset from target to camera

    void Start()
    {
        // Calculate initial offset from target to camera
        orbitOffset = transform.position - target.position;
    }

    void Update()
    {
        // Calculate the desired position based on orbiting
        Vector3 desiredPosition = target.position + orbitOffset;
        // Calculate the new position by rotating around the target
        transform.position = Quaternion.Euler(0, orbitSpeed * Time.time, 0) * (desiredPosition + (transform.right * orbitRadius));

        // Make the camera look at the target
        transform.LookAt(target.position);
    }
}
