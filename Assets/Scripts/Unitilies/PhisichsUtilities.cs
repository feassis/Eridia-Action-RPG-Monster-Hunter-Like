using UnityEngine;

public static class PhisichsUtilities
{
    /// <summary>
    /// Checks if the angle between two vectors is less than the given threshold.
    /// </summary>
    /// <param name="vectorA">The first vector.</param>
    /// <param name="vectorB">The second vector.</param>
    /// <param name="threshold">The angle threshold in degrees.</param>
    /// <returns>True if the angle is less than the threshold; otherwise, false.</returns>
    public static bool IsAngleLessThan(Vector3 vectorA, Vector3 vectorB, float threshold)
    {
        // Ensure the vectors are normalized to get accurate results
        vectorA.Normalize();
        vectorB.Normalize();

        // Calculate the angle between the vectors
        float angle = Vector3.Angle(vectorA, vectorB);

        // Check if the angle is less than the threshold
        return angle < threshold;
    }

    /// <summary>
    /// Calculates the velocity needed to throw an object to hit a target position.
    /// </summary>
    /// <param name="start">The starting position of the object.</param>
    /// <param name="target">The target position to hit.</param>
    /// <param name="timeToTarget">The desired time for the projectile to reach the target.</param>
    /// <param name="gravity">The gravity affecting the projectile (default is -Physics.gravity.y).</param>
    /// <returns>The initial velocity vector required to hit the target.</returns>
    public static Vector3 CalculateThrowVelocity(Vector3 start, Vector3 target, float timeToTarget, float gravity = -1f)
    {
        // Use default gravity if not provided
        if (gravity < 0f)
        {
            gravity = Physics.gravity.y;
        }

        // Calculate the horizontal and vertical displacement
        Vector3 displacement = target - start;

        // Separate displacement into horizontal and vertical components
        Vector3 horizontalDisplacement = new Vector3(displacement.x, 0f, displacement.z);
        float verticalDisplacement = displacement.y;

        // Calculate horizontal and vertical velocities
        float horizontalVelocity = horizontalDisplacement.magnitude / timeToTarget;
        float verticalVelocity = (verticalDisplacement - 0.5f * gravity * timeToTarget * timeToTarget) / timeToTarget;

        // Combine horizontal and vertical velocities into a single vector
        Vector3 initialVelocity = horizontalDisplacement.normalized * horizontalVelocity;
        initialVelocity.y = verticalVelocity;

        return initialVelocity;
    }
}