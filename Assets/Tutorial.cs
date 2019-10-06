using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject tutorial1;
    GameObject tutorial2;
    GameObject tutorial3;
    void Start()
    {
        tutorial1 = GameObject.Find("Tutorial1");
        tutorial1.SetActive(true);
        tutorial2 = GameObject.Find("Tutorial2");
        tutorial2.SetActive(false);
        tutorial3 = GameObject.Find("Tutorial3");
        tutorial3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("TutorialPlayer").GetComponent<TutorialPlayer>().ps == TutorialPlayer.PlayerState.CatchReadyCaughtProjectile) {
            tutorial2.SetActive(true);
        }
        if (GameObject.Find("TutorialEnemy") == null) {
            tutorial3.SetActive(true);
        }
        
        if(tutorial3.active) {
            if (Input.anyKey) {
                SceneManager.LoadScene("GameplayScene");
            }
        }
    }
}
