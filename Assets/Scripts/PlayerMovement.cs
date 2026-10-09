using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isFacingRight = true;
    public bool jump = false;
    // public Transform fireballPosition;
    // public GameObject fireballPrefab;
    public GameObject fireStart;
    public int health;
    public Animator anim;
    // [SerializeField] bool IsGrounded;  
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform firePoint;
    // public CharacterController2D controller;

    

	
   
    void Start()
    {
        // anim.SetBool("IsJumping", false);
    }
    void FireBallStart()
    {
        Instantiate(fireStart, firePoint.position, firePoint.rotation);
    }
    void Update()
    {
        
        horizontal = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(horizontal));
        
        if(Input.GetMouseButtonDown(1))
        {
            FireBallStart();
        }
        // if(!IsGrounded())
        //     {
        //         Debug.Log("not grounded");           
        //     }
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            // jump = true;
           
        }
        if (IsGrounded())
        {
            // Debug.Log("Grounded");
            anim.SetBool("IsJumping", false);
            // anim.SetBool("IsJumping", true);
            // if(IsGrounded())
            // {
            //     anim.SetBool("IsJumping", false);           
            // }
            
            
        }
        if(!IsGrounded())
        {
            // Debug.Log("Not Grounded");
            anim.SetBool("IsJumping", true);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        // if(IsGrounded() == true)
        // {
        //     anim.SetBool("IsJumping", false);
        // }
        
        Vector3 characterScale = transform.localScale;
        // Vector3 fireScale = firePointLocation.transform.localScale;
        if(Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = -7;
            // fireScale.x = -10;
            // firePointLocation.transform.Rotate(0f, 180f, 0f);
            // Flip();
        }
        
        if(Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = 7;
            // fireScale.x = 10;
            // firePointLocation.transform.Rotate(0f, 180f, 0f);
            // Flip();
        }
        transform.localScale = characterScale;
        // firePointLocation.transform.localScale = fireScale;
        
    }

    // void Flip()
    // {
    //     // Vector3 characterScale = transform.localScale;
    //     if(Input.GetAxis("Horizontal") < 0)
    //     {
    //         // characterScale.x = -10;
    //         // transform.Rotate(0f, 180f, 0f);
    //         transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
    //         // firePointLocation.transform.Rotate(0f, 180f, 0f);
    //         // Flip();
    //     }
        
    //     if(Input.GetAxis("Horizontal") > 0)
    //     {

    //         // transform.Rotate(0f, -180f, 0f);
    //         transform.eulerAngles = new Vector3(0, 0, 0); // Normal
    //         // characterScale.x = 10;
    //         // firePointLocation.transform.Rotate(0f, 180f, 0f);
    //         // Flip();
    //     }
    //     // transform.localScale = characterScale;
        
    // }
    // void FixedUpdate()
    // {
    //     controller.Move(horizontal * Time.fixedDeltaTime, false, jump);
    //     jump = false;
    // }

    // public void OnLanding()
    // {
        
    // }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        
    }

    // public void FireballSpell()
    // {
    //         if(Input.GetMouseButtonDown(1))
    //         {
    //             Instantiate(fireballPrefab, fireballPosition.position, fireballPosition.rotation);
    //         }
    // }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        anim.SetBool("IsJumping", false);
    }

   

    /* Ienumerator Die()
    {
        //If health is 0 or less, restart scene.
    }
    */
}
