using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    // Loading scene name
    [SerializeField] public string nextScene = "";
    // Disable fade-in animation flag
    [SerializeField] private bool disableFadeInAnimation = false;

    private void Start()
    {
        if (disableFadeInAnimation)
        {
            // Move fade-in animation to the end
            Animator animator = gameObject.GetComponent<Animator>();
            animator.Play("Fade Transition_in", 0 , 1);
        }
    }

    public void SetNextScene(string name)
    {
        nextScene = name;
    }

    // Triggered from the end of fade-out animation
    public void FadeOutFinished()
    {
        // Load of the next scene
        SceneManager.LoadScene(nextScene);
    }

}

