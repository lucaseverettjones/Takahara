using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public LayerMask enemyRangedLayers;
    public int attackDamage = 50;
    bool attackAnimPlaying;
    public float attackTime;
    public AudioSource soundObject;
    public AudioClip playerSwordSwing;
    public Ranged_Enemy rController;
    public PlayerController pController;
    // Start is called before the first frame update
    void Start()
    {
        attackAnimPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && attackAnimPlaying == false && pController.isPlaying)
        {
            Attack();
            // anim.SetBool("IsJumping", true);
        }
        if(Input.GetMouseButtonDown(0) && attackAnimPlaying == true)
        {
            Debug.Log("Please Wait");
        }
    }

    void Attack()
    {
        
        anim.SetTrigger("Attack");
        soundObject.PlayOneShot(playerSwordSwing);
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyControllerAnim>().TakeDamage(attackDamage);
            
        }
        Collider2D[] hitRangedEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyRangedLayers);

        foreach(Collider2D ranged in hitRangedEnemies)
        {
            
            StartCoroutine(ranged.GetComponent<Ranged_Enemy>().TakeDamage(attackDamage));
        }

        StartCoroutine("AttackStopper");
        anim.SetBool("IsJumping", false);
        StartCoroutine("AirWait");
    }
    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator AttackStopper()
    {
        attackAnimPlaying = true;
        yield return new WaitForSeconds(attackTime);
        attackAnimPlaying = false;
    }
   
    IEnumerator AirWait()
    {
        yield return new WaitForSeconds(.51f);
        if(pController.controller.m_Grounded == false)
        {
            anim.SetBool("IsJumping", true);
        }
        if(pController.controller.m_Grounded == true)
        {
            anim.SetBool("IsJumping", false);
        }
        
    }
    // void OnTriggerEnter2D(Collider2D col){

    //     if(Input.GetMouseButtonDown(0) && col.gameObject.tag == "Ranged Enemy" && attackAnimPlaying == false)
    //     {  
    //         anim.SetTrigger("Attack");
    //         soundObject.PlayOneShot(playerSwordSwing);
    //         col.gameObject.GetComponent<Ranged_Enemy>().TakeDamage(attackDamage);
    //         StartCoroutine("AttackStopper");
    //     }
    // }
}
