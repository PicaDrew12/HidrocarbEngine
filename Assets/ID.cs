using UnityEngine;
using System;

public class ID : MonoBehaviour
{
    public string id;

    private void Awake()
    {
        if (string.IsNullOrEmpty(id))
            id = Guid.NewGuid().ToString();
    }
}
