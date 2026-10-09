using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;
    public Animator anim;
    BoxCollider2D box;
    Rigidbody2D rb;
    [SerializeField] float castDistance;
    float attackDelay;
    public EnemyControllerAnim eController;
    bool attacking = false;
    [SerializeField] Vector2 boxSize;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        // enemyController = GameObject.Fin
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine("Attack");
        }
              
       
    }
    IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        rb.velocity = Vector3.zero;
        attacking = true;
        // timeBtwAttack = startTimeBtwAttack;  
        Debug.Log("LeftClick");
        
        yield return new WaitForSeconds(attackDelay/2);
        if(EnemyInSight())
        {
            eController.TakeDamage(damage);
            
            
        }
        yield return new WaitForSeconds(attackDelay/2);
        attacking = false;
    }
    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     if(attackPoint == null)
    //     {
    //         return;
    //     }
    //     Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    // }
    bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, box.bounds.size, 0, Vector2.right * transform.localScale.x, castDistance, whatIsEnemies);
        return hit;
    }
    // void OnTriggerEnter2D(BoxCollider2D col)
    // {
    //     col.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
    // }
}
