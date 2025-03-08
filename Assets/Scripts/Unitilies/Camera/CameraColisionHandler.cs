using System;
using UnityEngine;

[Serializable]
public class CameraColisionHandler 
{
    [field: SerializeField] public LayerMask CollisionLayer {  get; private set; }
    [SerializeField]
    private float cameraCollisionCushionSize = 3.41f;

    public bool IsCollidingInDesPos {  get; private set; }
    public bool IsCollidingInAdjstPos {  get; private set; }
    [HideInInspector] public Vector3[] AdjustedCameraClipPoints;
    [HideInInspector] public Vector3[] DesiredCameraClipPoints;

    Transform ainAtPos;
    Camera camera;

    public void Initialize(Camera cam, Transform ainAtPos)
    {
        camera = cam;
        this.ainAtPos = ainAtPos;
        AdjustedCameraClipPoints = new Vector3[5];
        DesiredCameraClipPoints = new Vector3[5];
    }

    public void UpdateCameraClipPoints(Vector3 camPosition, Quaternion atRotation, ref Vector3[] intoArray)
    {
        if(camera == null)
        {
            return;
        }

        intoArray = new Vector3[5];

        float z = camera.nearClipPlane;
        float x = Mathf.Tan(camera.fieldOfView/cameraCollisionCushionSize) * z;
        float y = x / camera.aspect;

        //top left
        intoArray[0] = (atRotation * new Vector3(-x, y, z)) + camPosition;

        //top right
        intoArray[1] = (atRotation * new Vector3(x, y, z)) + camPosition;

        //bottom left
        intoArray[2] = (atRotation * new Vector3(-x,-y, z)) + camPosition;
        
        //bottom right
        intoArray[3] = (atRotation * new Vector3(x, -y, z)) + camPosition;

        //camera pos
        intoArray[4] = camPosition + camera.transform.forward * Vector3.Distance(camPosition, ainAtPos.position);
    }

    protected bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
    {
        for(int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
            float distance = Vector3.Distance(clipPoints[i], fromPosition);
            if(Physics.Raycast(ray, distance, CollisionLayer))
            {
                return true;
            }
        }

        return false;
    }

    public float GetAdjustedDistanceWithRayFrom(Vector3 from)
    {
        float distance = -1;

        for (int i = 0; i <  DesiredCameraClipPoints.Length; i++)
        {
            Ray ray =  new Ray(from, DesiredCameraClipPoints[i] - from);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if( distance == -1)
                {
                    distance = hit.distance;
                }
                else
                {
                    if(hit.distance < distance)
                    {
                        distance = hit.distance;
                    }
                }
            }
        }

        if (distance == -1)
        {
            return 0;
        }

        return distance;
    }

    public void CheckColliding(Vector3 targetPosition, Vector3 adjustedPos)
    {
        IsCollidingInDesPos = CollisionDetectedAtClipPoints(DesiredCameraClipPoints, targetPosition);
        IsCollidingInAdjstPos = CollisionDetectedAtClipPoints(AdjustedCameraClipPoints, adjustedPos);
    }
}
