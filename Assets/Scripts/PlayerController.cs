using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
//using UnityEngine.UI;
//using System.Threading;

public class PlayerController : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text wintText;
    public TMP_Text loseText;
    //public TMP_InputField playerNameInput;
    //public Button startButton;

    private string playerName;

    //private bool isTiming;
    //private float timer;

    public float speed = 10.0f;
    public float jumpForce = 5.0f;
    private Rigidbody rb;
    private int count = 20;

    private float movementX;
    private float movementY;

    private bool isGrounded;
    private bool isGameOver; // Para manejar el estado del juego

    private void Start()
    {
        SetCountText();
        rb = GetComponent<Rigidbody>();
        wintText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        isGameOver = false; // Inicializa el estado del juego

      
    }

   /* public void StartGame()
    {
        // Obtener el nombre del jugador en el Input Field
        playerName = playerNameInput.text;

        // Ocultar el Input Field y el botón de inicio
        playerNameInput.gameObject.SetActive(false );
        startButton.gameObject.SetActive(false);

        // Iniciar el cronómetro
        isTiming = true;
        // Reiniciar el tiempo
        timer = 0.0f;
    }*/
    
    private void FixedUpdate()
    {
        if (!isGameOver) // Solo mueve al jugador si el juego no ha terminado
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);

            if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame) // Verifica si está en el suelo y si se presionó la tecla de espacio
            {
                Jump();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count--;
            SetCountText(); // Actualiza el contador
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
            // Marca que el juego ha terminado y muestra el mensaje de derrota
            isGameOver = true;
            loseText.gameObject.SetActive(true);
            loseText.text = "You lose!!!";
            // Desactivar el player
            Destroy(gameObject);
        }
    }

    void SetCountText()
    {
        countText.text = "Pickups left to win: " + count.ToString();
        if (count <= 0)
        {
            isGameOver = true; // Marca que el juego ha terminado
            wintText.gameObject.SetActive(true);
            wintText.text = "You Win!!!";
            // Aquí podrías añadir lógica para reiniciar el juego o mostrar un menú después
        }
    }

    // Método para hacer que el jugador salte
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Aplica una fuerza hacia arriba
        isGrounded = false; // Marca que ya no está en el suelo
    }

    private void OnCollisionStay(Collision collision)
    {
        // Verificar si el jugador está tocando el suelo
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = true; // El jugador está en el suelo
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Cuando el jugador deja de tocar el suelo
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = false; // El jugador ya no está en el suelo
        }
    }
 }
