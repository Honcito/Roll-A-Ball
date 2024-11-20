using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public TMP_Text countText;  // Texto para mostrar los pickups restantes
    public TMP_Text scoreText;  // Texto para mostrar el puntaje
    public TMP_Text wintText;   // Texto de victoria
    public TMP_Text loseText;   // Texto de derrota
    public Button restartButton; // Botón de reinicio

    public float speed = 10.0f;  // Velocidad del jugador
    public float jumpForce = 5.0f; // Fuerza del salto
    private Rigidbody rb;

    private float movementX;
    private float movementY;

    private bool isGrounded;
    private bool isGameOver;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Sincroniza la UI con el estado inicial del GameManager
        UpdateUI();
        restartButton.gameObject.SetActive(false);  // Ocultamos el botón de reinicio al inicio
        wintText.gameObject.SetActive(false);  // Ocultamos el texto de victoria
        loseText.gameObject.SetActive(false);  // Ocultamos el texto de derrota
        isGameOver = false;  // El juego comienza en curso
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

            CheckBounds();  // Verificamos si el jugador está fuera de los límites
        }
    }

    private void CheckBounds()
    {
        float limit = 230.0f;
        if (transform.position.x < -limit || transform.position.x > limit ||
            transform.position.z < -limit || transform.position.z > limit)
        {
            EndGame(false); // Termina el juego si el jugador se sale de los límites
            gameObject.SetActive(false); // Desactiva al jugador
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);  // Desactiva el pickup cuando lo recoge el jugador
            GameManager.Instance.DecrementPickups();  // Decrementa los pickups restantes
            GameManager.Instance.IncrementScore();   // Incrementa el puntaje global
            UpdateUI();  // Actualiza el texto de la UI
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
            EndGame(false);  // Termina el juego si el jugador colisiona con un enemigo
            gameObject.SetActive(false);  // Desactiva al jugador
        }
    }

    private void UpdateUI()
    {
        // Actualiza ambos textos: pickups restantes y puntaje
        SetCountText();
        SetScoreText();
    }

    void SetCountText()
    {
        countText.text = "Pickups left to win: " + GameManager.Instance.pickupsLeft.ToString();

        // Si el jugador ha recogido todos los pickups, avanza a la siguiente fase
        if (GameManager.Instance.CheckIfWon() && !isGameOver)
        {
            EndGame(true);  // Solo muestra la victoria si se ha ganado
            GameManager.Instance.LoadNextLevel();  // Avanza a la siguiente fase
        }
    }


    void SetScoreText()
    {
        scoreText.text = "Score: " + GameManager.Instance.GetTotalScore().ToString();
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Añadir fuerza hacia arriba
        isGrounded = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = true;  // El jugador está tocando el suelo o el terreno
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = false;  // El jugador ya no está tocando el suelo o el terreno
        }
    }

    private void EndGame(bool playerWon)
    {
        if (isGameOver) return;  // Evita que el juego termine más de una vez

        isGameOver = true;  // Indica que el juego ha terminado

        // Si ha ganado, muestra el mensaje de victoria
        if (playerWon)
        {
            wintText.gameObject.SetActive(true);
            wintText.text = "Congrats! You've completed the game!";
            loseText.gameObject.SetActive(false);  // Asegúrate de que el texto de derrota no se muestre
        }
        else
        {
            loseText.gameObject.SetActive(true);
            loseText.text = "You lose!!! Final score: " + GameManager.Instance.GetTotalScore().ToString();
            wintText.gameObject.SetActive(false);  // Asegúrate de que el texto de victoria no se muestre
            restartButton.gameObject.SetActive(true);  // Muestra el botón de reinicio
        }
    }

    public void RestartGame()
    {
        // Aquí reiniciamos la escena actual sin avanzar a la siguiente
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Reinicia los valores globales del GameManager sin alterar el puntaje
        GameManager.Instance.ResetCurrentSceneState();

        // Restablece la UI al estado inicial
        UpdateUI();

        // Reactiva al jugador
        gameObject.SetActive(true);

        // Resetea el estado del juego
        isGameOver = false;

        // Asegura que los textos de victoria y derrota estén ocultos al reiniciar
        wintText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }
}
