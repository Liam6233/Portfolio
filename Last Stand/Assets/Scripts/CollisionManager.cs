using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    GameObject ship;

    [SerializeField]
    GameObject playerBulletPrefab;

    SpriteRenderer shipColor;

    [SerializeField]
    List<GameObject> enemyList;

    public List<GameObject> bulletList;

    // Start is called before the first frame update
    void Start()
    {
        shipColor = ship.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = ship;
        GameObject enemy;
        bool playerCollision;
        bool bulletCollision;
        bool playerImmune = player.GetComponent<Player>().isImmune;
        //shipColor.color = Color.white;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null)
            {
                enemy = enemyList[i];
            }
            else
            {
                enemyList.RemoveAt(i);
                i--;
                continue;
            }
            
            playerCollision = AABBCollision(player, enemy);
            
            if (playerCollision && !playerImmune)
            {
                //shipColor.color = Color.red;
                ship.GetComponent<Player>().LoseHealth();
                ship.GetComponent<Player>().isImmune = true ;
                enemyList[i].GetComponent<SpriteRenderer>().color = Color.red;
                if (enemyList[i].GetComponent<Enemy>())
                {
                    enemyList[i].GetComponent<Enemy>().KillEnemy();
                }

                
                i = enemyList.Count;
            }
            else 
            {
                enemyList[i].GetComponent<SpriteRenderer>().color = Color.white;
            }

            for(int j = 0; j < bulletList.Count; j++)
            {
                if(bulletList[j] != null)
                {
                    bulletCollision = PointToShape(bulletList[j], enemy);
                    if (bulletCollision && enemyList[i].GetComponent<Enemy>())
                    {
                        enemyList[i].GetComponent<Enemy>().TakeDamage();
                        bulletList[j].GetComponent<Bullet>().AddToTimer();
                    }
                }
                
            }
        }   
        clearBulletList();
    }

    public bool AABBCollision(GameObject player, GameObject obstical)
    {
        bool areColliding = false;
        Bounds playerBox = player.GetComponent<SpriteRenderer>().bounds;
        Bounds obsticalBox = obstical.GetComponent<SpriteRenderer>().bounds;

        if (obsticalBox.min.x < playerBox.max.x &&
            obsticalBox.max.x > playerBox.min.x &&
            obsticalBox.max.y > playerBox.min.y &&
            obsticalBox.min.y < playerBox.max.y)
        {
            areColliding = true;
        }
        return areColliding;
    }


    public bool PointToShape(GameObject bullet, GameObject enemy)
    {
        bool areColliding = false;
        bool bulletHit = bullet.GetComponent<Bullet>().hitEnemy;
        Bounds bulletBox = bullet.GetComponent<SpriteRenderer>().bounds;
        Bounds obsticalBox = enemy.GetComponent<SpriteRenderer>().bounds;

        if (!bulletHit && obsticalBox.max.y > bulletBox.center.y &&
            obsticalBox.min.y < bulletBox.center.y)
        {
            areColliding = true;
            bullet.GetComponent<Bullet>().hitEnemy = true;
            ship.GetComponent<Player>().score += 5;
            ship.GetComponent<Player>().healthScore += 5;
        }
        return areColliding;
    }



    public void AddEnemyToList(GameObject obj)
    {
        enemyList.Add(obj);
    }

    public void AddBulletToList(GameObject obj)
    {
        bulletList.Add(obj);
    }

    private void clearBulletList()
    {
        for(int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i] == null)
            {
                bulletList.RemoveAt(i);
            }
        }
    }
}
