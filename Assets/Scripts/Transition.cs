using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHandler : MonoBehaviour
{
    // Loading scene name
    [SerializeField] private string nextScene = "";
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

    // Triggered from the end of fade-out animation
    void FadeOutFinished()
    {
        // Load of the next scene
        Application.LoadLevel(1);
    }

}

