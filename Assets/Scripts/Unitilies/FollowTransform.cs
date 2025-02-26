using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private bool followPosition = true;
    [SerializeField] private bool followRotation = true;

    [SerializeField] private Transform target;

    public void Update()
    {
        if (target != null)
        {
            Follow(target);
        }
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Follow(Transform target)
    {
        if (followPosition)
        {
            transform.position = target.position;
        }
        if (followRotation)
        {
            transform.rotation = target.rotation;
        }
    }
}
