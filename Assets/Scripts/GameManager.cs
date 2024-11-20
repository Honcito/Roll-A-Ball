using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int pickupsToWin = 17;     // Número total de pickups necesarios para ganar el juego en la fase actual
    public int pickupsLeft;           // Número de pickups restantes en la fase actual
    public int score;                 // Puntuación global acumulada

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    Debug.LogError("No se encontró un GameManager en la escena.");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Garantizar una única instancia de GameManager persistente
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGame(); // Inicializa los valores al cargar la escena
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de cambio de escena
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Desuscribirse del evento de cambio de escena
    }

    // Inicializa los valores al comienzo de cada escena
    private void InitializeGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Configuración de pickups y score según la fase actual
        switch (currentSceneIndex)
        {
            case 0: // Fase 1
                pickupsToWin = 5;
                pickupsLeft = pickupsToWin;
                score = 0; // Reseteamos el score solo en la primera fase
                break;
            case 1: // Fase 2
                pickupsToWin = 5;
                pickupsLeft = pickupsToWin;
                break;
            case 2: // Fase 3
                pickupsToWin = 7;
                pickupsLeft = pickupsToWin;
                break;
            default:
                Debug.LogError("Índice de escena no configurado en GameManager.");
                break;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Inicializa el juego cada vez que se carga una nueva escena
        InitializeGame();
    }

    // Incrementa la puntuación cuando se recoge un pickup
    public void IncrementScore()
    {
        score++;
    }

    // Decrementa los pickups restantes cuando se recoge un pickup
    public void DecrementPickups()
    {
        pickupsLeft--;
    }

    // Comprueba si el jugador ha ganado la fase actual
    public bool CheckIfWon()
    {
        return pickupsLeft <= 0;
    }

    // Llama al siguiente nivel si el jugador ha ganado la fase actual
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            // Carga la siguiente escena
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            // Si no hay más fases, muestra un mensaje y permite reiniciar
            Debug.Log("Has completado todas las fases!");
            PlayerController.Instance.ShowRestartButton();  // Muestra el botón de reinicio
        }
    }

    // Reinicia el juego desde la primera fase
    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Vuelve a cargar la primera fase
        score = 0;
        pickupsLeft = pickupsToWin;
    }

    // Resetea el estado global del juego
    public void ResetGame()
    {
        score = 0;
        pickupsToWin = 17;
        pickupsLeft = 0;
    }

    // Devuelve la puntuación total acumulada
    public int GetTotalScore()
    {
        return score;
    }

    // Resetea el estado de la escena actual sin afectar el progreso global
    public void ResetCurrentSceneState()
    {
        pickupsLeft = pickupsToWin; // Reinicia el contador de pickups de la escena actual
    }
}
