using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    private float horizontal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Flip()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 fireScale = transform.localScale;
        // Vector3 fireScale = firePointLocation.transform.localScale;
        if(Input.GetAxis("Horizontal") < 0)
        {
            fireScale.x = -1;
            // transform.Rotate(0f, 180f, 0f);
            // transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        }
        
        if(Input.GetAxis("Horizontal") > 0)
        {
            fireScale.x = 1;
            // transform.Rotate(0f, 180f, 0f);
            // transform.eulerAngles = new Vector3(0, 0, 0); // Normal;
        }
        transform.localScale = fireScale;
        
    }
}
