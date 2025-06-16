using UnityEngine;

public class ButtonHoverFlicker : MonoBehaviour
{
    public Animator animator;

    public void OnHover()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hover");
        }
    }
}

