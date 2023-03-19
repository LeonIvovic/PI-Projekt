using System.Collections; // Importing the System.Collections library
using System.Collections.Generic; // Importing the System.Collections.Generic library
using UnityEngine; // Importing the UnityEngine library

public class Igrac : MonoBehaviour // Defining a public class called "Igrac" that inherits from "MonoBehaviour"
{

    private float horizontal; // Declaring a private variable "horizontal" of type float
    private bool isFacingRight; // Declaring a private variable "isFacingRight" of type bool
    [SerializeField] Rigidbody2D rb; // Declaring a private Rigidbody2D component "rb" and making it serializable in the Unity Editor
    [SerializeField] Transform groundCheck; // Declaring a private Transform component "groundCheck" and making it serializable in the Unity Editor
    [SerializeField] LayerMask groundLayer; // Declaring a private LayerMask variable "groundLayer" and making it serializable in the Unity Editor

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Setting "horizontal" to the value of the "Horizontal" axis input
        Flip(); // Calling the "Flip" function
        if (Input.GetButtonDown("Jump") && IsGrounded()) // If the "Jump" button is pressed and the player is on the ground...
        {
            rb.velocity = new Vector2(rb.velocity.x, 30f); // ... set the y-velocity of "rb" to 16f
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) // If the "Jump" button is released and the player is still ascending...
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // ... reduce the y-velocity of "rb" by half
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer); // Check if "groundCheck" is overlapping with an object in the "groundLayer" and return true or false
    }

    private void Flip()
    {
        if (isFacingRight && horizontal > 0f || !isFacingRight && horizontal < 0f) // If the player is facing right and moving left, or facing left and moving right...
        {
            isFacingRight = !isFacingRight; // ... flip the direction the player is facing
            Vector3 localScale = transform.localScale; // Get the current scale of the player
            localScale.x *= -1f; // Flip the x-axis of the scale by multiplying it by -1
            transform.localScale = localScale; // Set the scale of the player to the new flipped scale
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * 10f, rb.velocity.y); // Move the player horizontally based on the "horizontal" input and the current y-velocity
    }
}





