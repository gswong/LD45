using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    private float startTime;
    private float delayTime;
    private Text t;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        delayTime = startTime + 3f;
        t = GetComponent<Text>();
        t.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > delayTime) {
            t.text = "Press any key to play again.";
            if (Input.anyKey) {
                SceneManager.LoadScene("GameplayScene");
            }
        }
        t.color = Color.Lerp(Color.black, Color.white, Time.time - delayTime);
    }
}
