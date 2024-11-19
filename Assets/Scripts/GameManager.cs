using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int pickupsToWin = 20; // N�mero de pickups necesarios para ganar

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
                    Debug.LogError("No se encontr� un GameManager en la escena.");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Evita que existan m�ltiples instancias si vuelves a esta escena
        }
    }
}
