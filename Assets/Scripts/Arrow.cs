using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject.Find("Player").GetComponent<AudioSource>().Play();
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(15);
        }
        Destroy(gameObject);
    }
}
