using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadGame : MonoBehaviour
{
    float timer = 0.0f;
    public TextMeshProUGUI loadingText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0 && timer < 1)
        {
            StartCoroutine(LoadYourAsyncScene());
            timer = 400f;
        }
        timer += Time.deltaTime;
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
