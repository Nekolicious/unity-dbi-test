using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public static SceneMaster Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
