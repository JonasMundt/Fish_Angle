using UnityEngine;

public class FishingCatchBarController : MonoBehaviour
{
    [Header("Bewegungsbereich")]
    [SerializeField] private RectTransform fishingArea;

    [Header("Fangzone")]
    [SerializeField] private RectTransform catchBar;

    [Header("Bewegung")]
    [SerializeField] private float upwardAcceleration = 900f;
    [SerializeField] private float gravity = 700f;
    [SerializeField] private float maximumUpwardSpeed = 500f;
    [SerializeField] private float maximumFallingSpeed = 450f;

    private float verticalVelocity;

    private void Update()
    {
        HandleInput();
        MoveCatchBar();
    }

    private void HandleInput()
    {
        bool buttonHeld =
            Input.GetKey(KeyCode.Space) ||
            Input.GetMouseButton(0);

        if (buttonHeld)
        {
            verticalVelocity +=
                upwardAcceleration * Time.deltaTime;
        }
        else
        {
            verticalVelocity -=
                gravity * Time.deltaTime;
        }

        verticalVelocity = Mathf.Clamp(
            verticalVelocity,
            -maximumFallingSpeed,
            maximumUpwardSpeed
        );
    }

    private void MoveCatchBar()
    {
        if (fishingArea == null || catchBar == null)
        {
            return;
        }

        Vector2 position =
            catchBar.anchoredPosition;

        position.y +=
            verticalVelocity * Time.deltaTime;

        float minimumY = 0f;

        float maximumY =
            fishingArea.rect.height -
            catchBar.rect.height;

        if (position.y <= minimumY)
        {
            position.y = minimumY;

            if (verticalVelocity < 0f)
            {
                verticalVelocity = 0f;
            }
        }

        if (position.y >= maximumY)
        {
            position.y = maximumY;

            if (verticalVelocity > 0f)
            {
                verticalVelocity = 0f;
            }
        }

        catchBar.anchoredPosition =
            position;
    }
}