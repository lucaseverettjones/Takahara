using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutController : MonoBehaviour
{
    public GameObject space;
    public GameObject redSpace;
    public GameObject spaceText;
    public GameObject fire;
    public GameObject redFire;
    public GameObject fireText;
    public GameObject melee;
    public GameObject redMelee;
    public GameObject meleeText;
    public GameObject heal;
    public GameObject healText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Tut1")
        {
            space.SetActive(true);
            redSpace.SetActive(true);
            spaceText.SetActive(true);
        }
        if(col.gameObject.tag == "Tut2")
        {
            fire.SetActive(true);
            redFire.SetActive(true);
            fireText.SetActive(true);
        }
        if(col.gameObject.tag == "Tut3")
        {
            melee.SetActive(true);
            redMelee.SetActive(true);
            meleeText.SetActive(true);
        }
        if(col.gameObject.tag == "Tut4")
        {
            heal.SetActive(true);
            healText.SetActive(true);
            
        }
    }
}
