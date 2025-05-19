using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcherStart : MonoBehaviour
{

    public Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Switch()
    {
        SceneManager.LoadScene(1);
    }

    public void SwitchScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void OnHover()
    {
        
    }
}
