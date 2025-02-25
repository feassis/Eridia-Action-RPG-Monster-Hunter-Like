using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReciever : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float drag = 0.3f;

    public Vector3 Movement => impact + new Vector3 (0, verticalVelocity, 0);

    public Vector3 impact = Vector3.zero;
    private Vector3 dampingVelocity;

    private float verticalVelocity;

    private void Update()
    {
        if (verticalVelocity < 0f &&  controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null)
        {
            if (impact.sqrMagnitude <= 0.2f* 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }

    }

    public void AddForce(Vector3 force)
    {
        impact += force;

        if(agent != null)
        {
            agent.enabled = false;
        }
    }
}
