using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    private float horizontal;
    bool facingRight;
    public PlayerController pController;
    public EnemyControllerAnim eController;
    public int attackDamage;
    // [SerializeField] private Camera mainCamera;
    // public AudioSource soundObject;
    // public AudioClip fireballImpactSound;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        facingRight = true;
        Flip();
    }
    public void Flip()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 fireScale = transform.localScale;
        // Vector3 fireScale = firePointLocation.transform.localScale;
        if(Input.GetAxis("Horizontal") < 0)
        {
            // facingRight = false;
            Fire();

        }
        
        if(Input.GetAxis("Horizontal") > 0)
        {
            // facingRight = true;
            Fire();
        }
        
        // if(facingRight == false)
        // {
        //     rb.velocity = -transform.right * speed;
        // }
        // else
        // {
        //     rb.velocity = transform.right * speed;
        // }
        // if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        // {
        //     facingRight = false;
        //     rb.velocity = -transform.right * speed;
        // }
        // if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        // {
        //     facingRight = true;
        //     rb.velocity = transform.right * speed;
        // }
        
    }

    void Fire()
    {
        if(facingRight == false)
        {
            rb.velocity = -transform.right * speed;
        }
        else
        {
            rb.velocity = transform.right * speed;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        // Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // mouseWorldPosition.z = 0f;
        // transform.position = mouseWorldPosition;
        // if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        // {
        //     Debug.Log("Facing Left");
        //     facingRight = false;
        //     // rb.velocity = -transform.right * speed;
        // }
        // if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        // {
        //     Debug.Log("facingRight");
        //     facingRight = true;
            // rb.velocity = transform.right * speed;
        // }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        // pController.manaCount = pController.manaCount - pController.manaPrice;
        // if(col.gameObject.tag == "Player")
        // {
            
        // }
        // if(col.gameObject.tag == "Enemy")
        // {
        //     eController.TakeDamage(100);
        //     Debug.Log(eController.enemyHealth);
        //     pController.FireImpact();
        //     Destroy(gameObject);
        // }
        // Debug.Log(col.gameObject.name);
        if(col.gameObject.tag == "Enemy")
        {
            // eController.GetComponent<EnemyController>().TakeDamage(attackDamage);
            // Debug.Log("Enemy Lost Health");
            // eController.StartCoroutine("Die");
            // Destroy(col.gameObject);
        }
        if(col.gameObject.name == "Ranged Enemy")
        {
            // eController.GetComponent<EnemyController>().TakeDamage(attackDamage);
            // // Debug.Log(eController.enemyHealth);
            // // eController.StartCoroutine("Die");
            // Destroy(col.gameObject);
        }
        // }
        // if(col.gameObject.name == "Enemy (1)")
        // {
        //     eController.GetComponent<EnemyController>().TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
        // if(col.gameObject.name == "Enemy (2)")
        // {
        //     eController.GetComponent<EnemyController>().TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
        // if(col.gameObject.name == "Enemy (3)")
        // {
        //    eController.GetComponent<EnemyController>().TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
        // if(col.gameObject.tag == "Enemy")
        // {
            
        //     eController.GetComponent<EnemyController>().TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
        if(col.gameObject.tag == "Enemy")
        {
            
            // eController.TakeDamage(attackDamage);
            // Debug.Log("EH" + eController.enemyHealth);
            // eController.StartCoroutine("Die");
            // Destroy(col.gameObject);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        
        Destroy(gameObject);
        
    }
    
    // void OnTriggerEnter2D(Collider2D col)
    // {
        
        
    // }
}
