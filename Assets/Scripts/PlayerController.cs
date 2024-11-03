using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text wintText;
    public TMP_Text loseText;
    public Button restartButton;

    public float speed = 10.0f;
    public float jumpForce = 5.0f;
    private Rigidbody rb;
    private int count = 20;

    private float movementX;
    private float movementY;

    private bool isGrounded;
    private bool isGameOver;

    private void Start()
    {
        SetCountText();
        rb = GetComponent<Rigidbody>();
        restartButton.gameObject.SetActive(false);
        wintText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        isGameOver = false;
    }

    private void FixedUpdate()
    {
        if (!isGameOver)
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);

            if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Jump();
            }

            CheckBounds();
        }
    }

    private void CheckBounds()
    {
        // Asumiendo que el centro del Terrain es el (0,0) y va de -100 a 100 en X y Z
        float limit = 230.0f;
        if (transform.position.x < -limit || transform.position.x > limit ||
            transform.position.z < -limit || transform.position.z > limit)
        {
            EndGame(false); // Termina el juego si el jugador se sale de los límites
            gameObject.SetActive(false); // Desactiva al jugador o puedes usar Destroy(gameObject);
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

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EndGame(false);
            gameObject.SetActive(false);
        }
    }

    void SetCountText()
    {
        countText.text = "Pickups left to win: " + count.ToString();
        if (count <= 0)
        {
            EndGame(true);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = false;
        }
    }

    private void EndGame(bool playerWon)
    {
        isGameOver = true;
        if (playerWon)
        {
            wintText.gameObject.SetActive(true);
            wintText.text = "You Win!!!";
        }
        else
        {
            loseText.gameObject.SetActive(true);
            loseText.text = "You lose!!!";
        }
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        count = 20; // Resetea el contador de pickups
        SetCountText(); // Actualiza la UI
    }
}
