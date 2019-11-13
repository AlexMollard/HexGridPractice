using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingIcon : MonoBehaviour
{
    public float SpinSpeed = 1.0f;
    public bool IsIcon = false;
    public bool IsText = false;
    public float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        timer = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsIcon)
            transform.Rotate(new Vector3(0,0,SpinSpeed));
        else
        {
            timer -= Time.deltaTime;
            if (timer > 3)
                GetComponent<TextMeshProUGUI>().text = "Loading";
            else if (timer > 2)
                GetComponent<TextMeshProUGUI>().text = "Loading.";
            else if (timer > 1)
                GetComponent<TextMeshProUGUI>().text = "Loading..";
            else if (timer > 0)
                GetComponent<TextMeshProUGUI>().text = "Loading...";
            else
                timer = 4.0f;
        }
    }
}
