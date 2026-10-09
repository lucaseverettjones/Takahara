using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;
    public PBController pBController;
    public bool isPlaying;
    public bool enableCheats;
    public LayerMask enemyLayers;
    public LayerMask enemyRangedLayers;
	public float runSpeed = 40f;
    [SerializeField] public float manaCount;
    [SerializeField] public float manaPrice;
    public float manaGenTime;
    // public GameObject lightningBolt;
    public List<GameObject> lightningBolt = new List<GameObject>();
    public List<EnemyControllerAnim> enemyList = new List<EnemyControllerAnim>();
    bool lightningAvailable;
    public GameObject shockwaveStrike;
    bool shockWaveAvailable;
    public Transform shockWavePoint;
    public Transform newFire;
    float shockWaveTime;
    public int shockWaveDamage;
    public int lCheck;
	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
    public TMP_Text manaText;
    bool lightningAnimPlaying;
    bool shockWaveAnimPlaying;
    bool canAttack;
    int healthCheck;
	int x;
    public float shockWaveRange;
    public GameObject fireStart;
    public GameObject fireBallUI;
    [SerializeField] private Transform firePoint;
    public Animator anim;
    public int playerHealth;
    public EnemyControllerAnim eController;
    public Ranged_Enemy rController;
    [SerializeField] GameObject tutHealthLossBox;
    // public EnemyController eController1;
    // public EnemyController eController2;
    // public EnemyController eController3;
    // public EnemyController eController4;
    Rigidbody2D rb2d;
    public AudioSource soundObject;
    public AudioClip fireballStartSound;
    public AudioClip fireballImpactSound;
    public AudioClip sWSound;
    public AudioClip deathSFX;
    public AudioClip hitSFX;
    public AudioClip hitSFX2;
    public AudioClip hitSFX3;
    public AudioClip[] hitSFXs = new AudioClip[3];
    Rigidbody2D PlayerRB;
    // private GameObject target = null;
    // private Vector3 offset;
    // public GameObject fireball;
	// Update is called once per frame
    static Vector2 checkpoint;
    static bool isCheckpoint;
    const string hurt = "Playerhit";

    //  void OnTriggerStay2D(Collider2D col){
    //     target = col.gameObject;
    //     offset = target.transform.position - transform.position;
    // }
    // void OnTriggerExit2D(Collider2D col){
    //     target = null;
    // }
    // void LateUpdate(){
    //     if (target != null) {
    //         target.transform.position = transform.position+offset;
    //     }
    // }
    void Awake()
    {
        // target = null;
        PlayerRB = GetComponent<Rigidbody2D>();
        manaText.text = manaCount.ToString();
        StartCoroutine("ManaGen");
        lightningAvailable = false;
        // canAttack = true;
        // Pb.BarValue = 100;
        isPlaying = true;
        manaCount = 100f;
        rb2d = GetComponent<Rigidbody2D>();
        hitSFXs[0] = hitSFX;
        hitSFXs[1] = hitSFX2;
        hitSFXs[2] = hitSFX3;
        if(isCheckpoint)
        {
            transform.position = new Vector3(checkpoint.x, checkpoint.y, transform.position.z);
        }
    }
	void Update () {
        if(isPlaying)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            
                if (Input.GetButtonDown("Jump"))
		    {
			    jump = true;
                anim.SetBool("IsJumping", true);
		    }

		    if (Input.GetButtonDown("Crouch"))
		    {
			    crouch = true;
		    } else if (Input.GetButtonUp("Crouch"))
		    {
			    crouch = false;
		    }

        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            Application.Quit(); 
        }
        if(enableCheats == true)
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                isCheckpoint = false;
                LoadScene2(0);
            }
            
            if(Input.GetKeyDown(KeyCode.P))
            {
                isCheckpoint = false;
                LoadScene2(0);
            }
            if(Input.GetKeyDown(KeyCode.O))
            {
                isCheckpoint = false;
                LoadScene2(1);
            }
            if(Input.GetKeyDown(KeyCode.I))
            {
                isCheckpoint = false;
                LoadScene2(2);
            }
            if(Input.GetKeyDown(KeyCode.U))
            {
                isCheckpoint = false;
                LoadScene2(3);
            }
            if(Input.GetKeyDown(KeyCode.Y))
            {
                isCheckpoint = false;
                LoadScene2(4);
            }
            if(Input.GetKeyDown(KeyCode.T))
            {
                isCheckpoint = false;
                LoadScene2(5);
            }
            if(Input.GetKeyDown(KeyCode.K))
            {
                isCheckpoint = false;
                LoadScene2(6);
            }
        }
        
        pBController.Pb.BarValue = playerHealth;

		anim.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
        if(Input.GetMouseButtonDown(1) && manaCount <= 0f)
        {
            Debug.Log("You are out of Mana");
            
        }
        if(Input.GetKeyDown(KeyCode.F) && shockWaveAvailable == true && shockWaveAnimPlaying == false && controller.m_Grounded == true && isPlaying)
        {
            soundObject.PlayOneShot(sWSound);
            StartCoroutine("ShockWaveStrike");
        }
        if(Input.GetMouseButtonDown(1) && manaCount >= 25f && isPlaying)
        {
            FireBallStart();
            
        }
        if(manaCount < 50)
        {
            lightningAvailable = false;
            shockWaveAvailable = false;
        }
        if(manaCount >= 50)
        {
            lightningAvailable = true;
            shockWaveAvailable = true;
        }
        if(manaCount >= 100 && Input.GetKeyDown(KeyCode.C))
        {
            manaCount -= 100;
            playerHealth = 100;
            pBController.Pb.BarValue = 100;
            Debug.Log(playerHealth);
            
        }
            
        if(Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(25);
            pBController.Pb.BarValue -= 25;
        }

        if(Input.GetKeyDown(KeyCode.E) && lightningAvailable == true && lightningAnimPlaying == false)
        {
            StartCoroutine("LightningStrike");
        }

        

        if(playerHealth <= 0)
        {
           
            // anim.SetBool("IsPlayerDead", true);
            // soundObject.PlayOneShot(deathSFX);
            StartCoroutine("DeathCycle");

            // LoadScene1(1);
        }

		
        // transform.Rotate (0,25,0*Time.deltaTime);

        
	}

    IEnumerator ManaGen()
    {
        yield return new WaitForSeconds(manaGenTime);
        manaCount += 1;
        manaText.text = manaCount.ToString();
        StartCoroutine("ManaGen");
    }

    /*IEnumerator LightningStrike()
    {
        LightningSpell();
        manaCount = manaCount - 50f;
        lightningAnimPlaying = true;
        eController.enemyHealth = eController.enemyHealth - 30;
        // enemyList[x].enemyHealth -= 30;
        yield return new WaitForSeconds(2);
        lightningBolt[x].SetActive(false);
        lightningAnimPlaying = false;

    }*/
    IEnumerator ShockWaveStrike()
    {
        ShockWaveSpell();
        manaCount = manaCount - 50f;
        shockWaveAnimPlaying = true;
        // eController.enemyHealth = eController.enemyHealth - 50;
        yield return new WaitForSeconds(.5f);
        shockWaveAnimPlaying = false;

    }
   

    public void TakeDamage(int damage)
    {
        if(isPlaying && !anim.GetCurrentAnimatorStateInfo(0).IsName(hurt))
        {
            anim.SetTrigger("PlayerHit");
            playerHealth = playerHealth - damage;
            if(playerHealth > 0)
            {
                soundObject.PlayOneShot(hitSFXs[Random.Range(1,3)]);
            }
            else
            {
                soundObject.PlayOneShot(deathSFX);
            }
        }
        
    }

    void LightningSpell()
    {
        Debug.Log("X = " + x);
        lightningBolt[x].SetActive(true);
    }

    void ShockWaveSpell()
    {
        Instantiate(shockwaveStrike, shockWavePoint.position, shockWavePoint.rotation);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(shockWavePoint.position, shockWaveRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
           enemy.GetComponent<EnemyControllerAnim>().TakeDamage(50);
            
        }
        Collider2D[] hitRangedEnemies = Physics2D.OverlapCircleAll(shockWavePoint.position, shockWaveRange, enemyRangedLayers);

        foreach(Collider2D ranged in hitRangedEnemies)
        {
            
            StartCoroutine(ranged.GetComponent<Ranged_Enemy>().TakeDamage(50));
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if(shockWavePoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(shockWavePoint.position, shockWaveRange);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // if(col.gameObject.tag == "Enemy")
        // {
        //     // eController.enemyHealth -= 10;
        // }
        // if(col.gameObject.tag == "Moving")
        // {
        //     col.SetParent(transform);
        // }
        if(col.gameObject.tag == "L1End")
        {
            isCheckpoint = false;
            SceneManager.LoadScene("Cutscene 3");
        }
        if(col.gameObject.tag == "L2End")
        {
            isCheckpoint = false;
            SceneManager.LoadScene("Cutscene 4");
        }
        if(col.gameObject.tag == "L3End")
        {
            isCheckpoint = false;
            SceneManager.LoadScene("Cutscene 5");
        }
        if(col.gameObject.tag == "HelpEnd")
        {
            isCheckpoint = false;
            SceneManager.LoadScene("Cutscene 1");
        }
        if(col.gameObject.tag == "Potion")
        {
            playerHealth += 50;
            healthCheck = playerHealth - 100;
            if(playerHealth > 100)
            {
                playerHealth = playerHealth - healthCheck;
            }
            pBController.Pb.BarValue += 50;
            Debug.Log(playerHealth);
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "Moving")
        {
            col.gameObject.GetComponent<Animator>().enabled = true;
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // if(col.gameObject.tag == "Ranged Enemy" && Input.GetKeyDown(KeyCode.F) && shockWaveAvailable == true && shockWaveAnimPlaying == false && controller.m_Grounded == true)
        // {
        //     StartCoroutine("ShockWaveStrike");
        //     Destroy(col.gameObject);
        // }
        // if(col.gameObject.tag == "Ranged Enemy")
        // {
        //     if(Input.GetKeyDown(KeyCode.F) && shockWaveAvailable == true && shockWaveAnimPlaying == false && controller.m_Grounded == true)
        //     {
        //         StartCoroutine("ShockWaveStrike");
        //         rController.StartCoroutine(rController.TakeDamage(shockWaveDamage));
                
        //     }
        //     Debug.Log("Range E detected.");
        // }
        if(col.gameObject.tag == "Potion")
        {
            playerHealth += 50;
            healthCheck = playerHealth - 100;
            if(playerHealth > 100)
            {
                playerHealth = playerHealth - healthCheck;
            }
            pBController.Pb.BarValue += 50;
            Debug.Log(playerHealth);
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "TutHealthLoss")
        {
            playerHealth -= 50;
            pBController.Pb.BarValue -= 50;
            anim.SetTrigger("PlayerHit");
            tutHealthLossBox.SetActive(false);
        }
        if(col.gameObject.tag == "L2Death")
        {
            SceneManager.LoadScene("Level 2");
        }
        if(col.gameObject.tag == "L3Death")
        {
            TakeDamage(playerHealth);
        }
        if(col.gameObject.tag == "BoltCheck1")
        {
            x = 0;
            
            lightningAvailable = true;
        }
        if(col.gameObject.tag == "BoltCheck2")
        {
            x = 1;
            lightningAvailable = true;
        }
        if(col.gameObject.tag == "BoltCheck3")
        {
            x = 2;
            lightningAvailable = true;
        }
        if(col.gameObject.tag == "BoltCheck4")
        {
            x = 3;
            lightningAvailable = true;
        }
        if(col.gameObject.tag == "L1")
        {
            lCheck = 1;
            Debug.Log("L1");
        }
        if(col.gameObject.tag == "L2")
        {
            lCheck = 2;
            Debug.Log("L2");
        }
        if(col.gameObject.tag == "L3")
        {
            lCheck = 3;
            Debug.Log("L3");
        }
        if(col.gameObject.tag == "Toxic")
        {
            
            pBController.Pb.BarValue -= 100;
            TakeDamage(100);
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            StartCoroutine("DeathCycle");
        }
        if(col.gameObject.tag == "Checkpoint")
        {
            isCheckpoint = true;
            checkpoint = new Vector2(transform.position.x, transform.position.y);
            col.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if(col.gameObject.tag == "EnemySword")
        {
            TakeDamage(10);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "BoltCheck1")
        {
            lightningAvailable = false;
        }
         if(col.gameObject.tag == "BoltCheck2")
        {
            
            lightningAvailable = false;
        }
        if(col.gameObject.tag == "BoltCheck3")
        {
            
            lightningAvailable = false;
        }
        if(col.gameObject.tag == "BoltCheck4")
        {
           
            lightningAvailable = false;
        }        
    }
    
    public void LoadScene(int level)
    { 
        level = 2;
        Application.LoadLevel(level);
    }
    public void LoadScene1(int level)
    { 
        level = 1;
        Application.LoadLevel(level);
    }
    public void LoadScene2(int level)
    { 
        
        Application.LoadLevel(level);
    }

    public void OnLanding()
    {
        anim.SetBool("IsJumping", false);
    }

    void FireBallStart()
    {
        Instantiate(fireStart, newFire.position, newFire.rotation);
        manaCount = manaCount - manaPrice;
        soundObject.PlayOneShot(fireballStartSound);
        
    }
    public void FireImpact()
    {
        soundObject.PlayOneShot(fireballImpactSound);
    }
	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
    // void PlayerDie()
    // {
        
    // }
    IEnumerator DeathCycle()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        anim.SetBool("IsPlayerDead", true);
        PlayerRB.velocity = Vector3.zero;
        isPlaying = false;
        
        Debug.Log(lCheck);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}

