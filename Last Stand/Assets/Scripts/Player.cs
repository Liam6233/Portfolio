using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public int health = 10;
    public int score = 0;

    public int healthScore = 0;


    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    CollisionManager colManager;

    float scoreTimer = 0;

    float shotTimer = 0;

    public bool isImmune = false;
    float iframes = 0;
 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        
        if(healthScore >= 80 && health < 10)
        {
            health++;
            healthScore = 0;
        }
        // early implementation of Iframes so player does not instanly die on contact with things
        if (isImmune)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            iframes -= Time.deltaTime;
        }
        else if(health > 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
       
        if(iframes <= 0)
        {
            isImmune = false;
        }

        scoreTimer += Time.deltaTime;
        if(scoreTimer >= 1)
        {
            score++;
            scoreTimer = 0;
        }
        shotTimer += Time.deltaTime;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        // fires a bullet when player presses correct button
        Vector2 bulletPosition = new Vector2(transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x/2, transform.position.y);
        if (context.performed && shotTimer > 0.05f)
        {
            colManager.AddBulletToList(Instantiate(bulletPrefab, bulletPosition, Quaternion.identity));
            shotTimer = 0;
        }
    }

    public void LoseHealth()
    {
        health--;
        iframes = 1;
    }
}
