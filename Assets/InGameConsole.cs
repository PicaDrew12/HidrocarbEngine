using UnityEngine;
using UnityEngine.UI;

public class InGameConsole : MonoBehaviour
{
    public Text consoleText; // Reference to the Text component where logs will be displayed
    public GameObject consoleParent; // Reference to the parent GameObject of the consoleText

    private string logOutput = "";

    void Start()
    {
        // Initialize the console to be invisible
        if (consoleParent != null)
        {
            consoleParent.SetActive(false);
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string logEntry = logString;

        switch (type)
        {
            case LogType.Error:
                logEntry = "<color=red>" + logString + "</color>";
                break;
            case LogType.Assert:
                logEntry = "<color=yellow>" + logString + "</color>";
                break;
            case LogType.Warning:
                logEntry = "<color=orange>" + logString + "</color>";
                break;
            case LogType.Log:
                logEntry = logString;
                break;
            case LogType.Exception:
                logEntry = "<color=red>" + logString + "\n" + stackTrace + "</color>";
                break;
        }

        logOutput += logEntry + "\n";
       // consoleText.text = logOutput;
    }

    void Update()
    {
        // Check if CTRL + SHIFT + O is pressed
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    ToggleConsoleVisibility();
                }
            }
        }
    }

    void ToggleConsoleVisibility()
    {
        if (consoleParent != null)
        {
            consoleParent.SetActive(!consoleParent.activeSelf);
        }
    }

    // Optional: Method to clear the console
    public void ClearConsole()
    {
        logOutput = "";
        consoleText.text = logOutput;
    }
}
