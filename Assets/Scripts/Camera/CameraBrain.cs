using System;
using UnityEngine;

public class CameraBrain : MonoBehaviour
{
    [SerializeField] private float rotationSpeedX = 5f; // Velocidade de rotação
    [SerializeField] private float rotationSpeedY = 5f; // Velocidade de rotação
    [SerializeField] private float rotationDamp = 5f;
    [SerializeField] private float minVerticalAngle = -45f; // Ângulo mínimo para olhar para baixo
    [SerializeField] private float maxVerticalAngle = 45f;  // Ângulo máximo para olhar para cima

    private PlayerInputControls inputControls;

    private Vector2 cameraDir;
    private Vector2 currentRotation;
    private Vector2 targetRotation;

    private void Awake()
    {
        inputControls = new PlayerInputControls();

        inputControls.Enable();

        inputControls.CombatMoveset.Camera.performed += ctx => CameraMovePerformed(ctx.ReadValue<Vector2>());
        inputControls.CombatMoveset.Camera.canceled += ctx => CameraMoveCanceled(ctx.ReadValue<Vector2>());
    }

    private void CameraMoveCanceled(Vector2 vector2)
    {
        cameraDir = Vector2.zero;
    }

    private void CameraMovePerformed(Vector2 vector2)
    {
        cameraDir = vector2.normalized;
        Debug.Log($"camera dir: {cameraDir}");
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


    private void Update()
    {
        RotateCamera(cameraDir);
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
