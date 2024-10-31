using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour 
{
    public TMP_Text countText;
    
    public float speed = 10.0f;
    private Rigidbody rb;
    private int count;

    // Holds movement x and y
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    private void Start()
    {
        count = 0; // Set counter to 0
        SetCountText();
        rb = GetComponent<Rigidbody>(); // Sets rigibody component to rb
        
    }

    private void OnMove(InputValue movementValue)
    {
        
        // Create a vector 2 variable and store the x and y movement values in it
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        // Set the movement to the x and z variables (keep y at 0)
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            // add 1 to the score
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

}
