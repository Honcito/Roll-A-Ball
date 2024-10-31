using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text wintText;

    public float speed = 10.0f;
    public float jumpForce = 5.0f;  // Fuerza del salto
    private Rigidbody rb;
    private int count;

    private float movementX;
    private float movementY;

    private bool isGrounded;

    private void Start()
    {
        count = 20;
        SetCountText();
        rb = GetComponent<Rigidbody>();
        wintText.gameObject.SetActive(false);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;  // Evita múltiples saltos
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count--;
            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si la bola está en contacto con el Terrain
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Grounded: " + isGrounded); // Esto debería imprimirse en la consola   
        }
    }

    void SetCountText()
    {
        countText.text = "Pikups left to win: " + count.ToString();
        if (count == 0)
        {
            wintText.gameObject.SetActive(true);
        }
    }
}
