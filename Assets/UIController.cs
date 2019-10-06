using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject enemiesValue = GameObject.Find("EnemiesValue");
        enemiesValue.GetComponent<Text>().text = enemyList.Length.ToString();
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GameObject livesValue = GameObject.Find("LivesValue");
        livesValue.GetComponent<Text>().text = player.Lives.ToString();
        if (enemiesValue.GetComponent<Text>().text == "0") {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
