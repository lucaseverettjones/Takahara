using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyControllerAnim : MonoBehaviour
{
    //Pathfinding
    [Header("Pathfinding")]
    [SerializeField] Transform target;
    [SerializeField] float vision = 50f;
    [SerializeField] float range = 1f;
    [SerializeField] float pathUpdateSeconds = 0.5f;
    Path path;
    int currentWaypoint;
    Seeker seeker;
    //Physics
    [Header("Physics")]
    [SerializeField] float speed = 1f;
    [SerializeField] float nextWaypointDistance = 3f;
    [SerializeField] float jumpNodeHeighRequirement = 0.5f;
    [SerializeField] float jumpModifier = 1;
    [SerializeField] float jumpCheckOffset = 0.1f;
    bool isGrounded;
    Rigidbody2D rb;
    //Custom Behavior
    [Header("Custom Behavior")]
    [SerializeField] bool follow = true;
    [SerializeField] bool jump = true;
    [SerializeField] bool look = true;
    //Animation
    Animator anim;
    AnimatorStateInfo info;
    const string die = "Enemy_Die";
    const string hurt = "Enemy_Hurt";
    const string attack = "Enemy_Attack";
    const string run = "Enemy_Run";
    const string idle = "Enemy_Idle";
    PlayerController playerController;
    //Stats
    int health = 100;
    [SerializeField] AudioSource hurtSound;
    [SerializeField] AudioSource dieSound;
    [SerializeField] AudioSource attackSound;
    [SerializeField] AudioSource walkSound;
    LayerMask ground;
    BoxCollider2D box;
    RaycastHit2D cast;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").GetComponent<Transform>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine("UpdatePath");
        ground = LayerMask.GetMask("Ground");
        box = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        cast = Physics2D.BoxCast(transform.position, box.bounds.extents, 0f, Vector2.left * transform.localScale.x, box.bounds.extents.x, ground);
        if(health <= 0)
        {
            if(info.IsName(hurt) && info.normalizedTime >= 1)
            {
                anim.Play(die);
                dieSound.Play();
            } else if(info.IsName(die) && info.normalizedTime >= 1)
            {
                GameObject.Find("KillCounter").GetComponent<KillCounter>().EnemyKilled();
                Destroy(gameObject);
            }
        } else
        {
            if(EnemyInSight() && playerController.playerHealth > 0)
            {
                if(EnemyInRange() && CanAct())
                {
                    anim.Play(attack, 0, 0f);
                    attackSound.Play();
                    walkSound.enabled = false;
                } else if(follow)
                {
                    Move();
                    walkSound.enabled = true;
                } else if(CanAct())
                {
                    anim.Play(idle, 0, 0f);
                }
            } else if(CanAct())
            {
                anim.Play(idle, 0, 0f);
            }
        }
    }

    bool EnemyInSight()
    {
        return Vector2.Distance(transform.position, target.transform.position) < vision;
    }

    bool EnemyInRange()
    {
        return Vector2.Distance(transform.position, target.transform.position) < range;
    }

    bool CanAct()
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }

    public void TakeDamage(int damage)
    {
        if(!info.IsName(hurt) && health > 0)
        {
            health -= damage;
            anim.Play(hurt, 0, 0f);
            hurtSound.Play();
            walkSound.enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "FireBall")
        {
            TakeDamage(50);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Toxic"))
        {
            TakeDamage(100);
        }
    }

    void Move()
    {
        //Check if there is a need to move
        if(path == null || currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        //Check if grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector3.down, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, ground);
        //Calculate movement
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;
        //Jump OR move
        if(isGrounded && jump && direction.y > jumpNodeHeighRequirement && !EnemyInRange())
        {
            rb.AddForce(Vector2.up * speed * jumpModifier * direction.y * Time.deltaTime / Time.fixedDeltaTime, ForceMode2D.Impulse);
            if(CanAct())
            {
                anim.Play(run, 0, 0f);
            }
        } else if(direction.y < jumpNodeHeighRequirement)
        {
            rb.AddForce(force * Time.deltaTime / Time.fixedDeltaTime);
            if(CanAct())
            {
                anim.Play(run, 0, 0f);
            }
        } else if(CanAct())
        {
            anim.Play(idle, 0, 0f);
        }
        //Iterate waypoint
        float distance =  Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        //Look
        if(look)
        {
            if(rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            } else if(rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    IEnumerator UpdatePath()
    {
        seeker.StartPath(rb.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(pathUpdateSeconds);
        StartCoroutine("UpdatePath");
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }   
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
