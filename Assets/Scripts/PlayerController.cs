using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text wintText;
    public TMP_Text loseText;

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

    private void FixedUpdate()
    {
        if (!isGameOver) // Solo mueve al jugador si el juego no ha terminado
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);
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
}
