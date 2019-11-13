using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartButton);
        exitButton.onClick.AddListener(ExitButton);
    }

    // Update is called once per frame
    void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    void ExitButton()
    {
        Application.Quit();
    }
}
