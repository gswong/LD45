using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CameraController : MonoBehaviour
{
    GameObject player;
    private Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scene = SceneManager.GetActiveScene();
        if (scene.name == "BossScene") {
            GameObject.Find("Goal").GetComponent<Text>().text = "Defeat the boss!";
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
