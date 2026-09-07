using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Ziel")]
    [SerializeField] private Transform target;

    [Header("Bewegung")]
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float horizontalOffset = 0f;
    [SerializeField] private float verticalOffset = 0f;

    [Header("Kameragrenzen")]
    [SerializeField] private BoxCollider2D playerCameraBounds;
    [SerializeField] private BoxCollider2D hookCameraBounds;

    private Camera cameraComponent;
    private BoxCollider2D activeCameraBounds;

    private Vector3 velocity;

    private bool followX = true;
    private bool followY;

    private float fixedY;

    private float minCameraX;
    private float maxCameraX;
    private float minCameraY;
    private float maxCameraY;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
        fixedY = transform.position.y;
    }

    private void Start()
    {
        activeCameraBounds = playerCameraBounds;
        CalculateCameraBounds();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        float desiredX = followX
            ? target.position.x + horizontalOffset
            : transform.position.x;

        float desiredY = followY
            ? target.position.y + verticalOffset
            : fixedY;

        if (activeCameraBounds != null)
        {
            desiredX = Mathf.Clamp(
                desiredX,
                minCameraX,
                maxCameraX
            );

            desiredY = Mathf.Clamp(
                desiredY,
                minCameraY,
                maxCameraY
            );
        }

        float smoothX = Mathf.SmoothDamp(
            transform.position.x,
            desiredX,
            ref velocity.x,
            smoothTime
        );

        float smoothY = Mathf.SmoothDamp(
            transform.position.y,
            desiredY,
            ref velocity.y,
            smoothTime
        );

        transform.position = new Vector3(
            smoothX,
            smoothY,
            transform.position.z
        );
    }

    public void FollowPlayer(Transform playerTarget)
    {
        if (playerTarget == null)
        {
            return;
        }

        target = playerTarget;

        followX = true;
        followY = false;

        activeCameraBounds = playerCameraBounds;

        fixedY = transform.position.y;
        velocity = Vector3.zero;

        CalculateCameraBounds();
    }

    public void FollowHook(Transform hookTarget)
    {
        if (hookTarget == null)
        {
            return;
        }

        target = hookTarget;

        followX = true;
        followY = true;

        activeCameraBounds = hookCameraBounds;

        velocity = Vector3.zero;

        CalculateCameraBounds();
    }

    private void CalculateCameraBounds()
    {
        if (
            activeCameraBounds == null ||
            cameraComponent == null
        )
        {
            return;
        }

        Bounds bounds = activeCameraBounds.bounds;

        float cameraHalfHeight =
            cameraComponent.orthographicSize;

        float cameraHalfWidth =
            cameraHalfHeight * cameraComponent.aspect;

        minCameraX = bounds.min.x + cameraHalfWidth;
        maxCameraX = bounds.max.x - cameraHalfWidth;

        minCameraY = bounds.min.y + cameraHalfHeight;
        maxCameraY = bounds.max.y - cameraHalfHeight;

        if (minCameraX > maxCameraX)
        {
            Debug.LogWarning(
                "Die aktiven CameraBounds sind schmaler als der Kamerabereich."
            );

            float centerX = bounds.center.x;
            minCameraX = centerX;
            maxCameraX = centerX;
        }

        if (minCameraY > maxCameraY)
        {
            Debug.LogWarning(
                "Die aktiven CameraBounds sind niedriger als der Kamerabereich."
            );

            float centerY = bounds.center.y;
            minCameraY = centerY;
            maxCameraY = centerY;
        }
    }
}