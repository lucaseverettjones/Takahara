using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    static int enemiesKilled;
    static int rangedEnemiesKilled;
    TextMeshProUGUI enemyCounter;
    TextMeshProUGUI rangedEnemyCounter;
    [SerializeField] Animator meleeAnim;
    [SerializeField] Animator rangedAnim;
    // Start is called before the first frame update
    void Start()
    {
        enemyCounter = GameObject.Find("MeleeKills").GetComponent<TextMeshProUGUI>();
        rangedEnemyCounter = GameObject.Find("RangedKills").GetComponent<TextMeshProUGUI>();
        meleeAnim = GameObject.Find("Melee").GetComponent<Animator>();
        rangedAnim = GameObject.Find("Ranged").GetComponent<Animator>();
        enemyCounter.text = "x " + enemiesKilled;
        rangedEnemyCounter.text = "x " + rangedEnemiesKilled;
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        meleeAnim.Play("Melee_Flash", 0, 0f);
        enemyCounter.text = "x " + enemiesKilled;
    }

    public void RangedEnemyKilled()
    {
        rangedEnemiesKilled++;
        rangedAnim.Play("Ranged_Flash", 0, 0f);
        rangedEnemyCounter.text = "x " + rangedEnemiesKilled;
    }
}
