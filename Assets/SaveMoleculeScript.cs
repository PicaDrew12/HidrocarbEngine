using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using SFB;

public class SaveMoleculeScript : MonoBehaviour
{
    public GameObject alcan;
    public TextMeshProUGUI title;

    [ContextMenu("Save Alcan")]
    public void SaveAlcan()
    {
        if (alcan != null && title != null)
        {
            string filePath = GetSaveFilePath();
            if (!string.IsNullOrEmpty(filePath))
            {
                SaveGameObjectToFile(alcan, title.text, filePath);
            }
        }
        else
        {
            Debug.LogWarning("Alcan GameObject or title is not assigned!");
        }
    }

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

    public static SerializableTransform CreateSerializableTransform(Transform transform)
    {
        var serializableTransform = new SerializableTransform
        {
            name = transform.name,
            localPosition = transform.localPosition,
            localScale = transform.localScale,
            localRotation = transform.localRotation,
            globalPosition = transform.position,
            globalScale = transform.lossyScale,
            globalRotation = transform.rotation
        };

        foreach (Transform child in transform)
        {
            serializableTransform.children.Add(CreateSerializableTransform(child));
        }

        return serializableTransform;
    }

    public static void SaveGameObjectToFile(GameObject go, string title, string filePath)
    {
        var serializableTransform = CreateSerializableTransform(go.transform);

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        string json = JsonConvert.SerializeObject(serializableTransform, Newtonsoft.Json.Formatting.Indented, settings);

        File.WriteAllText(filePath, json);
        Debug.Log("Saved to " + filePath);
    }

    private string GetSaveFilePath()
    {
#if UNITY_EDITOR
        // For editor-only file dialog
        return EditorUtility.SaveFilePanel("Save GameObject", "", title.text + ".molecule", "molecule");
#else
        // For builds, use SFB to show file dialog
        var extensionList = new[] {
            new ExtensionFilter("Molecule Files", "molecule"),
            new ExtensionFilter("All Files", "*" ),
        };
        string path = StandaloneFileBrowser.SaveFilePanel("Save GameObject", "", title.text, extensionList);
        return path;
#endif
    }
}
