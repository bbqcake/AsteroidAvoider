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

    void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }

        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity); // prevent it from going too fast
        
    }
}
