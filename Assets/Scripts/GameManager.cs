using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool jogoAtivo = false;
    private bool gameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        ApiDataLoader.OnDataLoaded += IniciarJogo;
    }

    void OnDisable()
    {
        ApiDataLoader.OnDataLoaded -= IniciarJogo;
    }

    void IniciarJogo(TrafficResponse data)
    {
        jogoAtivo = true;
        gameOver = false;
    }

    void Update()
    {
        if (gameOver && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void TriggerGameOver()
    {
        if (!jogoAtivo || gameOver) return;

        gameOver = true;
        jogoAtivo = false;

        Debug.Log("[GAME OVER]");

        // Para o spawn
        VehicleSpawner spawner = FindObjectOfType<VehicleSpawner>();
        if (spawner != null) spawner.PararSpawn();

        // Para o jogador
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.enabled = false;

        // Para todos os carros na cena
        VehicleMove[] carros = FindObjectsOfType<VehicleMove>();
        foreach (VehicleMove carro in carros)
            carro.enabled = false;
    }

    public bool JogoAtivo() => jogoAtivo;


}