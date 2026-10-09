using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("L1End"))
        {
            LoadScene("Cutscene 2");
        } else if(other.gameObject.CompareTag("L2End"))
        {
            LoadScene("Cutscene 3");
        } else if(other.gameObject.CompareTag("L3End"))
        {
            LoadScene("Cutscene 4");
        }
    }
}
