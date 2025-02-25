using System;
using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance;

    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("CoroutineManager");
                _instance = go.AddComponent<CoroutineManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    /// <summary>
    /// Starts a coroutine through the manager.
    /// </summary>
    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    /// <summary>
    /// Waits for an animation to finish and then calls the specified method.
    /// </summary>
    public Coroutine WaitForAnimation(Animator animator, string animationName, Action callback)
    {
        return StartCoroutine(WaitForAnimationCoroutine(animator, animationName, callback));
    }

    private IEnumerator WaitForAnimationCoroutine(Animator animator, string animationName, Action callback)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is null. Cannot wait for animation.");
            yield break;
        }

        // Get the animation's length
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait until the animator is in the target animation
        while (!stateInfo.IsName(animationName))
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        // Wait for the animation to finish
        yield return new WaitForSeconds(stateInfo.length);

        // Invoke the callback
        callback?.Invoke();
    }
}
