using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;

    private Camera mainCamera;
    private Rigidbody rb;

    private Vector3 movementDirection;
    
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        ProcessInput();

        KeepPlayerOnScreen();
    }

    void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }

        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity); // prevent it from going too fast
        
    }

    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
           Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
          

           Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

           movementDirection = transform.position - worldPosition; // moves away from the finger
           movementDirection.z = 0f; // only care about x and y
           movementDirection.Normalize(); // no matter how far away we touch it will always be 1  
        }
        else
        {
            movementDirection = Vector3.zero;
        }
    }

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if(viewportPosition.x > 1) // right side of screen
        {
            newPosition.x = -newPosition.x + 0.1f; // add on a slight value so we dont teleport back
        }

        else if(viewportPosition.x < 0) // left
        {
            newPosition.x = -newPosition.x - 0.1f; // add on a slight value so we dont teleport back
        }

        if(viewportPosition.y > 1) // top
        {
            newPosition.y = -newPosition.y + 0.1f; // add on a slight value so we dont teleport back
        }

        else if(viewportPosition.y < 0) // bottom of screen
        {
            newPosition.y = -newPosition.y - 0.1f; // add on a slight value so we dont teleport back
        }

        transform.position = newPosition;
    }

}
