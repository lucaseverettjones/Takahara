using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private GameObject target = null;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start(){
        target = null;
    }
    void OnTriggerStay2D(Collider2D col){
        target = col.gameObject;
        offset = target.transform.position - transform.position;
    }
    void OnTriggerExit2D(Collider2D col){
        target = null;
    }
    void LateUpdate(){
        if (target != null) {
            target.transform.position = transform.position+offset;
        }
    }
}
