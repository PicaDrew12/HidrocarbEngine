using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryLogic : MonoBehaviour
{
    public GameObject carbonPrefab; // Assign Carbon prefab in the inspector
    public GameObject hydrogenPrefab; // Assign Hydrogen prefab in the inspector
    public GameObject bondPrefab; // Assign Bond prefab in the inspector

    private Values values;

    // This dictionary will store connections for each carbon atom.
    private Dictionary<GameObject, List<GameObject>> connectedAtoms = new Dictionary<GameObject, List<GameObject>>();

    private void Start()
    {
        values = new Values();

        // Create Acetylene (C2H2)
        CreateAcetylene();
    }

    private void CreateAcetylene()
    {
        // Create the first carbon atom at the origin
        GameObject carbon1 = CreateAtom(Vector3.zero, carbonPrefab);

        // Create the second carbon atom next to the first
        GameObject carbon2 = CreateAtom(carbon1.transform.position + new Vector3(values.sigmaConectionLengthC_C, 0, 0), carbonPrefab);

        // Add a triple bond between the two carbon atoms
        AddAtomToCarbon(carbon1, carbon2, "t");

        // Add hydrogen atoms to both carbon atoms
        AddAtomToCarbon(carbon1, hydrogenPrefab, "s"); // Hydrogen for first carbon
        AddAtomToCarbon(carbon2, hydrogenPrefab, "s"); // Hydrogen for second carbon
    }

    private void AddAtomToCarbon(GameObject carbon, GameObject atomPrefab, string bondType)
    {
        // Ensure the carbon atom is tracked in the dictionary
        if (!connectedAtoms.ContainsKey(carbon))
        {
            connectedAtoms[carbon] = new List<GameObject>(); // Initialize if not present
        }

        int currentBondCount = GetCurrentBondCount(carbon);
        int requiredBonds = GetRequiredBonds(bondType);

        // Check if the carbon atom has enough bonding capacity
        if (currentBondCount + requiredBonds > 4)
        {
            Debug.LogError("Error: Carbon atom cannot accommodate more bonds.");
            return;
        }

        // Calculate the position based on existing connected atoms
        Vector3 atomPosition = CalculateAtomPosition(carbon, atomPrefab);
        GameObject atom = Instantiate(atomPrefab, atomPosition, Quaternion.identity);

        // Create bonds based on bond type
        if (bondType == "s")
        {
            CreateBond(carbon, atom);
        }
        else if (bondType == "d")
        {
            CreateBond(carbon, atom);
            CreateBond(carbon, atom, 0.6f); // Create second bond for double bond, offset vertically
        }
        else if (bondType == "t")
        {
            CreateBond(carbon, atom);
            CreateBond(carbon, atom, 0.6f); // First pi bond, offset vertically
            CreateBond(carbon, atom, 1.2f); // Second pi bond, offset vertically
        }

        // Store the connection
        connectedAtoms[carbon].Add(atom); // Track the new atom connected to carbon
    }

    private int GetCurrentBondCount(GameObject carbon)
    {
        // Safely check for connections in the dictionary
        if (connectedAtoms.ContainsKey(carbon))
            return connectedAtoms[carbon].Count;

        return 0;
    }

    private int GetRequiredBonds(string bondType)
    {
        switch (bondType)
        {
            case "s": return 1; // Sigma bond
            case "d": return 2; // Double bond (1 sigma + 1 pi)
            case "t": return 3; // Triple bond (1 sigma + 2 pi)
            default: return 0; // Unknown bond type
        }
    }

    private Vector3 CalculateAtomPosition(GameObject carbon, GameObject atomPrefab)
    {
        Vector3 carbonPosition = carbon.transform.position;
        int connectedCount = connectedAtoms[carbon].Count; // Count of already connected atoms
        float radius = values.sigmaConectionLengthC_H; // Default bond length

        // Set bond length based on atom type
        if (atomPrefab == hydrogenPrefab)
            radius = values.sigmaConectionLengthC_H;

        // Calculate the position based on connected atoms
        if (connectedCount == 0)
        {
            return carbonPosition + new Vector3(0, 0, radius); // First atom directly above
        }
        else
        {
            // For each connected atom, calculate the angle and position
            float angleStep = 360f / 4; // Assume a maximum of 4 connections for carbon

            // Determine the position using polar coordinates for circular arrangement
            float angle = connectedCount * angleStep;
            float xOffset = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float zOffset = radius * Mathf.Sin(Mathf.Deg2Rad * angle);

            return carbonPosition + new Vector3(xOffset, 0, zOffset);
        }
    }

    private GameObject CreateAtom(Vector3 position, GameObject prefab)
    {
        GameObject atom = Instantiate(prefab, position, Quaternion.identity);
        return atom;
    }

    public GameObject CreateBond(GameObject obj1, GameObject obj2, float offset = 0f)
    {
        // Ensure the objects are not null
        if (obj1 == null || obj2 == null)
        {
            Debug.LogError("One or both objects are null.");
            return null;
        }

        // Calculate the midpoint between the two objects with an optional vertical offset
        Vector3 position1 = obj1.transform.position;
        Vector3 position2 = obj2.transform.position;

        // Calculate the direction from obj1 to obj2
        Vector3 direction = position2 - position1;
        Vector3 midpoint = (position1 + position2) / 2; // Start with the midpoint

        // Adjust midpoint vertically based on offset
        if (offset != 0)
        {
            midpoint.y += offset; // Adjust the Y-coordinate based on the provided offset
        }

        // Calculate the rotation to point the cylinder towards obj2
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Adjust the rotation by adding 90 degrees to the x-axis
        rotation *= Quaternion.Euler(90, 0, 0); // Rotate 90 degrees around the X-axis

        // Calculate the length of the cylinder (the distance between the two objects)
        float length = direction.magnitude;

        // Instantiate the bond prefab (cylinder)
        GameObject bond = Instantiate(bondPrefab, midpoint, rotation);

        // Set the scale of the bond to match the length
        bond.transform.localScale = new Vector3(bond.transform.localScale.x, length / 2, bond.transform.localScale.z);

        return bond;
    }
}
