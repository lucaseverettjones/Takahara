using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossController : MonoBehaviour
{
    [SerializeField] Transform[] positions;
    bool ranged = true;
    [SerializeField] int minPhaseOneDuration, maxPhaseOneDuration;
    [SerializeField] int minPhaseTwoDuration, maxPhaseTwoDuration;
    [SerializeField] int minTurnDelay, maxTurnDelay;
    [SerializeField] int minStayPutTime, maxStayPutTime;
    [SerializeField] int minFireBallForce, maxFireBallForce;
    [SerializeField] int minFireballs, maxFireballs;

    void Start()
    {
        StartCoroutine("PhaseChange");
        StartCoroutine("Movement");
    }

    void Update()
    {
        if(ranged)
        {
            transform.position = transform.position + Vector3.right * transform.localScale.x * Time.deltaTime;
        }
    }

    IEnumerator Movement()
    {
        if(ranged)
        {
            yield return new WaitForSeconds(Random.Range(minTurnDelay, maxTurnDelay));
        } else
        {
            yield return new WaitForSeconds(Random.Range(minStayPutTime, maxStayPutTime));
            transform.position = positions[Random.Range(0, positions.Length)].position;
        }
        StartCoroutine("Movement");
    }

    IEnumerator Fireballs()
    {
        while(!true) yield return null;
    }

    IEnumerator PhaseChange()
    {
        ranged = !ranged;
        if(ranged)
        {
            yield return new WaitForSeconds(Random.Range(minPhaseOneDuration, maxPhaseOneDuration));
        } else
        {
            yield return new WaitForSeconds(Random.Range(minPhaseTwoDuration, maxPhaseTwoDuration));
        }
        StartCoroutine("PhaseChange");
    }
}
