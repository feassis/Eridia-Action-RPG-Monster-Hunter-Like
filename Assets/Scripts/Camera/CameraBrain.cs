using System;
using UnityEngine;

public class CameraBrain : MonoBehaviour
{
    [SerializeField] private float rotationSpeedX = 5f;  
    [SerializeField] private float rotationSpeedY = 5f; 
    [SerializeField] private float rotationDamp = 5f;
    [SerializeField] private float minVerticalAngle = -45f;
    [SerializeField] private float maxVerticalAngle = 45f;
    [SerializeField] private float smooth = 0.05f;
    [SerializeField] private float normalDistance = 5f;

    [SerializeField] private Camera cam;


    [SerializeField] private DebugSettings debugSettings;

    private PlayerInputControls inputControls;
    [SerializeField]private CameraColisionHandler colisionHandler;

    private Vector2 cameraDir;
    private Vector2 currentRotation;
    private Vector2 targetRotation;
    

    [Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedColisionLines = true;
    }

    private void Awake()
    {
        inputControls = new PlayerInputControls();

        inputControls.Enable();

        inputControls.CombatMoveset.Camera.performed += ctx => CameraMovePerformed(ctx.ReadValue<Vector2>());
        inputControls.CombatMoveset.Camera.canceled += ctx => CameraMoveCanceled(ctx.ReadValue<Vector2>());
        
    }

    private void Start()
    {
        cam.transform.position = transform.position - cam.transform.forward * normalDistance;
        colisionHandler.Initialize(cam, transform);
        colisionHandler.UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref colisionHandler.AdjustedCameraClipPoints);
        
        colisionHandler.UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref colisionHandler.DesiredCameraClipPoints);
    
    }

    private Vector3 GetAdjustedCameraPos()
    {
        float distanceToBeMoved = colisionHandler.GetAdjustedDistanceWithRayFrom(cam.transform.position);

        if(distanceToBeMoved == -1)
        {
            return cam.transform.position;
        }

        Vector3 desiredPosition = cam.transform.position + cam.transform.forward * distanceToBeMoved;
        return desiredPosition;
    }

    public void SetCamera(Camera cam)
    {
        this.cam = cam;
    }

    private void CameraMoveCanceled(Vector2 vector2)
    {
        cameraDir = Vector2.zero;
    }

    private void CameraMovePerformed(Vector2 vector2)
    {
        cameraDir = vector2.normalized;
    }

    private void OnEnable()
    {
        if(inputControls == null)
        {
            return;
        }

        inputControls.Enable();
    }

    private void OnDisable()
    {
        if(inputControls == null)
        {
            return;
        }

        inputControls.Disable();
    }


    private void FixedUpdate()
    {
        RotateCamera(cameraDir);
        ControlCamPosition();

        if (debugSettings.drawDesiredCollisionLines)
        {
            Vector3 camDesiredPos = GetDesiredCameraLocation();
            for (int i = 0; i < colisionHandler.DesiredCameraClipPoints.Length; i++)
            {
                Debug.DrawLine(camDesiredPos, colisionHandler.DesiredCameraClipPoints[i], Color.green);
            }
        }

        if (debugSettings.drawAdjustedColisionLines)
        {
            
            for (int i = 0; i < colisionHandler.AdjustedCameraClipPoints.Length; i++)
            {
                Debug.DrawLine(cam.transform.position, colisionHandler.AdjustedCameraClipPoints[i], Color.yellow);
            }
        }
    }


    private void ControlCamPosition()
    {
        
        Vector3 camDesiredPos = GetDesiredCameraLocation();
        colisionHandler.CheckColliding(camDesiredPos, cam.transform.position);

        colisionHandler.UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref colisionHandler.AdjustedCameraClipPoints);

        colisionHandler.UpdateCameraClipPoints(camDesiredPos, cam.transform.rotation, ref colisionHandler.DesiredCameraClipPoints);

        if (colisionHandler.IsCollidingInDesPos)
        {
            if(colisionHandler.IsCollidingInAdjstPos)
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, GetAdjustedCameraPos(), smooth);
            }
        }
        else
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camDesiredPos, smooth);
        }
    }

    private Vector3 GetDesiredCameraLocation()
    {
        return transform.position - cam.transform.forward * normalDistance;
    }

    public void RotateCamera(Vector2 input)
    {
        if (input == Vector2.zero) return;

        targetRotation.x += input.x * rotationSpeedX * Time.deltaTime;
        targetRotation.y += input.y * rotationSpeedY * Time.deltaTime;

        targetRotation.y = Mathf.Clamp(targetRotation.y, minVerticalAngle, maxVerticalAngle);

        currentRotation.x = Mathf.Lerp(currentRotation.x, targetRotation.x, rotationDamp * Time.deltaTime);
        currentRotation.y = Mathf.Lerp(currentRotation.y, targetRotation.y, rotationDamp * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(-currentRotation.y, currentRotation.x, 0);
    }
}
