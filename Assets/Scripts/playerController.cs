using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public gameController gameController;
    public cameraControlScript camController;

    public float Xvelocity = 0f;
    public float Yvelocity = 0f;

    public int maxHealth = 3;
    public int health = 3;

    public string PlayerType = "Basic";
    public string PlayerAbility;

    public GameObject bullet;
    int bulletTimer = 0;
    public int bulletRate = 10;
    public int bulletCount = 1;
    public int bulletDamage = 1;

    [SerializeField] int invFrames = 0;
    GameObject textManager;

    private void Start()
    {
        textManager = GameObject.Find("Canvas");
    }

    void FixedUpdate()
    {
        //Limiting Position
        if (transform.position.y > 1.45 && Yvelocity > 0)
        {
            Yvelocity = 0;
        }
        if (transform.position.y < -1.45 && Yvelocity < 0)
        {
            Yvelocity = 0;
        }
        if (transform.position.x > 1.37 && Xvelocity > 0)
        {
            Xvelocity = 0;
        }
        if (transform.position.x < -1.37 && Xvelocity < 0)
        {
            Xvelocity = 0;
        }

        //Move Object
        transform.position += new Vector3(Xvelocity, Yvelocity, 0);

        //Input Management!!!! :D
        if (Input.GetKey("up"))
        {
            Yvelocity = 0.015f;
        }
        else if (Input.GetKey("down"))
        {
            Yvelocity = -0.015f;
        }
        else
        {
            Yvelocity = 0;
        }

        if (Input.GetKey("right"))
        {
            Xvelocity = 0.015f;
        }
        else if (Input.GetKey("left"))
        {
            Xvelocity = -0.015f;
        }
        else
        {
            Xvelocity = 0;
        }

        if (Input.GetKey("z") && bulletTimer > bulletRate)
        {
            ShootBullet();
        }
        bulletTimer++;

        //Reduces invulnerability frames
        invFrames -= 1;
    }

    private void ShootBullet()
    {
        for (int bulletsFired = 0; bulletsFired < bulletCount; bulletsFired++) //Changes the shooting offset if shooting multiple bullets
        {
            Vector3 bulletPos;
            if (bulletCount == 1)
            {
                bulletPos = transform.position;
            }
            else
            {
                bulletPos = transform.position + new Vector3((-(float)bulletCount / 40) + ((float)bulletCount / 20) / (bulletCount - 1) * bulletsFired, 0, 0);
            }
            GameObject latestBullet = Instantiate(bullet, bulletPos, transform.rotation);

            latestBullet.GetComponent<playerBulletScript>().bulletType = PlayerType;
            latestBullet.GetComponent<playerBulletScript>().bulletDamage = bulletDamage;
        }

        bulletTimer = 0;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && invFrames < 1) //Collission with enemy
        {
            health -= 1;
            invFrames = 30;
            textManager.GetComponent<textManager>().CounterRefresh();
            camController.shake = 15;
        }else if (collision.gameObject.CompareTag("Bullet") && invFrames < 1) //Collission with bullet (Destroys the bullet)
        {
            health -= 1;
            invFrames = 30;
            textManager.GetComponent<textManager>().CounterRefresh();
            Destroy(collision.gameObject);
            camController.shake = 15;
        }
    }
}
