using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Transform firePoint;
    public GameObject fireBall;
    public PlayerController controller;
    public EnemyControllerAnim eController;
    public LayerMask enemyLayers;
    public int attackDamage = 50;
    bool fireballReady;
    // public Transform attackPoint;
    // public float attackRange = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        fireballReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && controller.manaCount <= 0f)
        {
            Debug.Log("You are out of Mana");
        }
        
        if(Input.GetMouseButtonDown(1) && fireballReady && controller.manaCount >= 25f && controller.isPlaying)
        {
            Shoot();
        }
        // if(Input.GetMouseButtonDown(1))
        // {
        //     Shoot();
        // }
    }

    void Shoot()
    {
        if(fireballReady)
        {
            Instantiate(fireBall, firePoint.position, firePoint.rotation);
        }
        StartCoroutine(fbWait());
    }
    IEnumerator fbWait()
    {
        fireballReady = false;
        yield return new WaitForSeconds(.4f);
        fireballReady = true;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
        if(col.gameObject.tag == "Arrow")
        {
            Physics2D.IgnoreCollision( col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        // if(col.gameObject.name == "Enemy")
        // {
        //     eController.TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
        // if(col.gameObject.name == "Enemy (1)")
        // {
        //     eController.TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
        // if(col.gameObject.name == "Enemy (2)")
        // {
        //     eController.TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
        // if(col.gameObject.name == "Enemy (3)")
        // {
        //     eController.TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
        // if(col.gameObject.tag == "Enemy")
        // {
            
        //     eController.TakeDamage(attackDamage);
        //     Debug.Log(eController.enemyHealth);
        // }
    }
}
