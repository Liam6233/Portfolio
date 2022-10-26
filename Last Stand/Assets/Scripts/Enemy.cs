using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
   
    [SerializeField]
    int health;

    [SerializeField]
    public int type;

    float tookDamage = 1;

    public Vector2 position;
    Vector2 velocity = Vector2.zero;

    // enemies will have a set path that they take, it is randomly chosen 
    // out of a few set paths
    // they include 1 Sinwave,2 straight line (may be at angle) 
    // maybe 1 more
    public int movementType;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    float randVelAddition;
    void Start()
    {
        position = transform.position;
        SetMovement();

        randVelAddition = Random.Range(-0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // based on movement type, posisition is updated using different math functions
        if (movementType == 1)
        {
            velocity.x = (-2.5f + randVelAddition) * Time.deltaTime;
        }
        else if (movementType == 2)
        {
            velocity.x = -1.5f * Time.deltaTime;
            velocity.y = -1.5f * Mathf.Sin(Time.time) * Time.deltaTime;
        }
        else if (movementType == 3)
        {
            velocity.x = -1.5f * Time.deltaTime;
            velocity.y = -1.5f * Time.deltaTime;
        }
        else if (movementType == 4)
        {
            velocity.x = -1.5f * Time.deltaTime;
            velocity.y = 1.5f * Time.deltaTime;
        }
        position += velocity;
        transform.position = position;

        if (tookDamage < 0.1f)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        tookDamage += Time.deltaTime;
}

private void SetMovement()
    {
        movementType = Random.Range(1, 5);
        
    }

    public void TakeDamage()
    {
        health--;
        tookDamage = 0;
    }

    public void KillEnemy()
    {
        health = 0;
        tookDamage = 0;
    }
}
