using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyControllerDeprecated : MonoBehaviour
{
    //TO BE ABSOLUTELY CLEAR, THIS IS A DEPRECATED SCRIPT!
    //DIRECT ALL IMPROVEMENT EFFORTS TO "ENEMYCONTROLLERANIM" INSTEAD!
    [Header("Attributes")]
    [SerializeField] int maxHealth = 100;
    public int enemyHealth;
    [SerializeField] float speed = 200f;
    public PBController pBController;

    [Header("Pathfinding")]
    [SerializeField] Transform target;
    [SerializeField] float nextWaypointDistance = 3f;
    [SerializeField] int pathUpdate = 1;
    Path path;
    [SerializeField] float activateDistance;
    int currentWayPoint;
    bool reachedEnd = false;
    Seeker seeker;
    Rigidbody2D rb;
    [SerializeField] float jumpNodeHeightRequirement = 0.5f;
    [SerializeField] float jumpModifier = 0.3f;
    [SerializeField] float jumpCheckOffset = 0.1f;
    bool isGrounded = false;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;

    [Header("CustomBehavior")]
    [SerializeField] bool followEnabled = true;
    [SerializeField] bool jumpEnabled = true;
    [SerializeField] bool directionLookEnabled = true;
    Animator anim;
    const string idle = "Enemy_Idle";
    const string run = "Enemy_Run";
    const string attack = "Enemy_Attack";
    const string hurt = "Enemy_Hurt";
    const string die = "Enemy_Die";
    BoxCollider2D box;
    float attackDelay = 1;
    PlayerController playerController;
    [SerializeField] float castDistance;
    bool stunned = false;
    [SerializeField] Vector2 boxSize;
    float height;
    public AudioSource soundObject;
    public AudioClip enemySwordSound;
    bool attacking = false;
    bool dying = false;

    [SerializeField] GameObject fire;

    void Start()
    {
        enemyHealth = maxHealth;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        target = GameObject.Find("Player").GetComponent<Transform>();

        StartCoroutine("CreatePath");
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void Update()
    {
        stunned = anim.GetCurrentAnimatorStateInfo(0).IsName(hurt) || anim.GetCurrentAnimatorStateInfo(0).IsName(die);
        if(!stunned)
        {
            if(enemyHealth <= 0)
            {
                StartCoroutine("Die");
            }
            else if (playerController.playerHealth > 0)
            {
                if(PlayerInSight())
                {
                    if (!attacking)
                    {
                        StartCoroutine("Attack");
                    }
                }
                else if(TargetInDistance() && followEnabled)
                {
                    PathFollow();
                }
                else
                {
                    anim.Play(idle);
                }
            }
            else
            {
                StartCoroutine("PlayerDead");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("DamageTaken");
        enemyHealth -= damage;
        if(enemyHealth < 1 && !dying)
        {
            StartCoroutine("Die");
        } else
        {
            anim.Play(hurt);
        }
    }

    IEnumerator Die()
    {
        dying = true;
        Destroy(box);
        Destroy(rb);
        anim.Play(die);
        yield return new WaitForSeconds(2);
        Debug.Log("Enemy Died!");
        Destroy(gameObject);
    }

    void PathFollow()
    {
        //Check if there is a path
        if(path == null || stunned)
        {
            return;
        } else
        //Reached end if path
        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            return;
        } else
        {
            reachedEnd = false;
        }
        anim.Play(run);
        //Check for ground
        isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, groundLayer);
        //Direction calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        //Jump if needed
        rb.AddForce(force);
        if(jumpEnabled && isGrounded && direction.y > jumpNodeHeightRequirement)
        {
            rb.AddForce(Vector2.up * speed * jumpModifier);
        }
        //Move
        //Go to next waypoint if needed
        if(distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
        //Turn around
        if(directionLookEnabled)
        {
            TurnAround();
        }
    }

    void TurnAround()
    {
        if(rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else
        {
            transform.localScale = Vector3.one;
        }
    }

    IEnumerator CreatePath()
    {
        if(seeker.IsDone() && followEnabled && TargetInDistance())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        yield return new WaitForSeconds(pathUpdate);
        StartCoroutine("CreatePath");
    }

    bool TargetInDistance()
    {
        return Vector2.Distance(rb.position, target.transform.position) < activateDistance;
    }

    IEnumerator Attack()
    {
        if(playerController.playerHealth > 0)
        {
            rb.velocity = Vector3.zero;
            attacking = true;
            anim.Play(attack, 0, 0f);
            // soundObject.PlayOneShot(enemySwordSound);
            yield return new WaitForSeconds(attackDelay / 2);
            if (PlayerInSight() && !stunned)
            {
                playerController.TakeDamage(25);
                pBController.Pb.BarValue -= 25;
            }
            yield return new WaitForSeconds(attackDelay / 2);
            attacking = false;
        } else
        {
            yield return new WaitForSeconds(0.0f);
        }
    }

    bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, box.bounds.size, 0, Vector2.right * transform.localScale.x, castDistance, playerLayer);
        return hit;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "FireBall")
        {
            // enemyHealth = 0;
            TakeDamage(50);
        }
    }

    IEnumerator DamageWait()
    {
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(0.5f);
        anim.Play(idle);
    }
}