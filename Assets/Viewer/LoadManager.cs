using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif
using SFB;

public class LoadManager : MonoBehaviour
{
    public List<GameObject> prefabs;
    public GameObject conection;
    public GameObject parentObject; // Add this line to reference the parent object

    [System.Serializable]
    public class SerializableTransform
    {
        public string name;
        public Vector3 localPosition;
        public Vector3 localScale;
        public Quaternion localRotation;
        public Vector3 globalPosition;
        public Vector3 globalScale;
        public Quaternion globalRotation;
        public List<SerializableTransform> children = new List<SerializableTransform>();
    }

    public Transform CreateTransformFromSerializable(SerializableTransform serializableTransform, List<GameObject> prefabs, GameObject conection, Transform parent = null)
    {
        string prefabName = serializableTransform.name.Replace("(Clone)", "").Trim();
        GameObject prefab = prefabs.Find(p => p.name == prefabName);

        if (prefab == null)
        {
            Debug.LogWarning($"Prefab with name {prefabName} not found!");
            Debug.LogError(prefabName);

            return null;
        }

        GameObject go;
        if (prefabName == "conection")
        {
            go = Instantiate(conection);
        }
        else
        {
            go = Instantiate(prefab);
        }

        RemoveUserScripts(go);
        Transform transform = go.transform;

        // Use the provided parent or default to the root parentObject
        if (parent != null)
        {
            transform.parent = parent;
        }
        else
        {
            transform.parent = parentObject.transform;
        }

        transform.localPosition = serializableTransform.localPosition;
        transform.localScale = serializableTransform.localScale;
        transform.localRotation = serializableTransform.localRotation;

        foreach (var child in serializableTransform.children)
        {
            CreateTransformFromSerializable(child, prefabs, conection, transform);
        }

        return transform;
    }

    private static void RemoveUserScripts(GameObject go)
    {
        if (go == null) return;

        var components = go.GetComponents<Component>();
        foreach (var component in components)
        {
            if (component == null) continue;

            Type type = component.GetType();
            if (type.Namespace == null || (!type.Namespace.StartsWith("UnityEngine") && !type.Namespace.StartsWith("UnityEditor")))
            {
                DestroyImmediate(component);
            }
        }

        foreach (Transform child in go.transform)
        {
            if (child != null)
            {
                RemoveUserScripts(child.gameObject);
            }
        }
    }

    [ContextMenu("Load Alcan")]
    public void LoadGameObjectFromFile()
    {
        GameObject.FindGameObjectWithTag("Molecule").GetComponent<ViewerAlcanScript>().DeleteAllChildren();
        string path = GetLoadFilePath();
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            var serializableTransform = JsonConvert.DeserializeObject<SerializableTransform>(json);
            CreateTransformFromSerializable(serializableTransform, prefabs, conection, parentObject.transform); // Ensure the parent object is set here
            Debug.Log("Loaded from " + path);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Load the specified scene
            SceneManager.LoadScene("start");
        }
    }

    private string GetLoadFilePath()
    {
#if UNITY_EDITOR
        // For editor-only file dialog
        return EditorUtility.OpenFilePanel("Load GameObject", "", "molecule");
#else
        // For builds, use SFB to show file dialog
        var extensions = new[] {
            new ExtensionFilter("Molecule Files", "molecule"),
            new ExtensionFilter("All Files", "*" ),
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Load GameObject", "", extensions, false);
        return paths.Length > 0 ? paths[0] : null;
#endif
    }
}
