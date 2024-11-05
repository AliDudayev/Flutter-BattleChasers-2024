using UnityEngine;

public class StartAnimationWhenTracked : DefaultObserverEventHandler
{
    private Animation animator; // Reference to the Animation component

    private string modelname;
    private AnimationManager animationManager;

    protected override void Start()
    {
        base.Start();

        animationManager = FindObjectOfType<AnimationManager>();

        // Get the Animation component attached to the model
        animator = GetComponentInChildren<Animation>();

        if (animator == null)
        {
            Debug.LogError("Animation component not found on the model.");
        }
        else
        {
            modelname = animator.gameObject.name;
        }
    }

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        // Start the idle animation when the image target is found
        if (animator != null)
        {
            // Loop the Idle animation
            var animationName = animationManager.GetIdleAnimation(modelname);
            animator[animationName].wrapMode = WrapMode.Loop;
            animator.Play(animationName);
        }

    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        // Stop the idle animation when the image target is lost
        if (animator != null)
        {
            // Stop the animation
            animator.Stop();
        }
    }
}
