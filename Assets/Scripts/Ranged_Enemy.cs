using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_Enemy : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform player;
    [SerializeField] int vision;
    [SerializeField] float offset;
    [SerializeField] float vOffset;
    [SerializeField] float force;
    bool stunned = false;
    bool firing = false;
    [SerializeField] float attackDelay;
    Animator anim;
    [SerializeField] AudioSource dieSound;
    [SerializeField] AudioSource attackSound;
    public int health = 25;
    const string attack = "Ranged_Attack";
    const string attack2 = "Ranged_Attack_2";
    const string die = "Ranged_Die";
    const string hurt = "Ranged_Hurt";
    const string idle = "Ranged_Idle";
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!stunned)
        {
            if(Vector3.Distance(transform.position, player.position) < vision)
            {
                if(transform.position.x > player.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    // Debug.Log("Turning Left");
                } else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    // Debug.Log("Turning Right");
                }
                if(!firing && playerController.playerHealth > 0)
                {
                    StartCoroutine(Attack());
                }
            } else
            {
                anim.Play(idle);
            }
        }
        
    }

    int Angle()
    {
        if(transform.localScale.x > 0)
        {
            return 0;
        } else
        {
            return 180;
        }
    }

    IEnumerator Attack()
    {
        if(!stunned && health > 0)
        {
            firing = true;
            anim.Play(attack);
            yield return new WaitForSeconds(attackDelay/2);
            attackSound.Play();
            GameObject newArrow = Instantiate(arrow, transform.position + new Vector3(offset * transform.localScale.x, vOffset, 0), Quaternion.Euler(0, 0, Angle()));
            newArrow.GetComponent<Rigidbody2D>().AddForce(Vector3.right * force * transform.localScale.x, ForceMode2D.Impulse);
            newArrow.GetComponent<Rigidbody2D>().AddTorque(-transform.localScale.x / 10, ForceMode2D.Impulse);
            yield return new WaitForSeconds(attackDelay/2);
            firing = false;
        } else
        {
            yield break;
        }
    }

    public IEnumerator TakeDamage(int damage)
    {
        stunned = true;
        health -= damage;
        if(health < 1)
        {
            anim.Play(die, 0, 0f);
            dieSound.Play();
            Destroy(gameObject.GetComponent<Collider2D>());
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            yield return new WaitForSeconds(2);
            GameObject.Find("KillCounter").GetComponent<KillCounter>().RangedEnemyKilled();
            Destroy(gameObject);
        } else
        {
            anim.Play(hurt, 0, 0f);
            yield return new WaitForSeconds(1f);
        }
        stunned = false;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "FireBall")
        {
            StartCoroutine(TakeDamage(50));
            // enemyHealth = 0;
            // StartCoroutine(TakeDamage(10));
            // Destroy(col.gameObject);
            // StartCoroutine("Die");
        }
    }
}
