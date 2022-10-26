using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TextMesh healthPrefab;

    [SerializeField]
    TextMesh scorePrefab;

    [SerializeField]
    GameObject player;

    [SerializeField]
    Camera cam;
    static float camHeight;
    float camWidth;


    GameObject playerHealth;
    GameObject playerScore;
    GameObject gameTime;

    float timer = 0;
    void Start()
    {
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        float camLeftEdge = cam.transform.position.x - camWidth / 2;
        Vector3 healthPosition = new Vector3(camLeftEdge + 0.5f, (cam.transform.position.y + camHeight / 2), 0);
        Vector3 scorePosition = new Vector3(camLeftEdge + 0.5f, (cam.transform.position.y - camHeight / 2) + 1, 0);
        Vector3 timePosition = new Vector3(camLeftEdge + 0.5f, (cam.transform.position.y + camHeight / 2) - 0.8f, 0);
        healthPrefab.text = "Health: " + player.GetComponent<Player>().health;
        playerHealth = Instantiate(healthPrefab.gameObject, healthPosition, Quaternion.identity, transform);

        scorePrefab.text = "Score: " + player.GetComponent<Player>().score;
        playerScore = Instantiate(scorePrefab.gameObject, scorePosition, Quaternion.identity, transform);

        gameTime = Instantiate(healthPrefab.gameObject, timePosition, Quaternion.identity, transform);

        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        playerHealth.GetComponent<TextMesh>().text = "Health: " + player.GetComponent<Player>().health;
        playerScore.GetComponent<TextMesh>().text = "Score: " + player.GetComponent<Player>().score;
        gameTime.GetComponent<TextMesh>().text ="Time Survived: " + ((int)timer).ToString();
    }
}
