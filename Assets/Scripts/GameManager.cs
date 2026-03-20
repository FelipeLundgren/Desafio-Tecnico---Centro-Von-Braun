using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    

    private bool jogoAtivo = false;
    private bool gameOver = false;
    public int NivelAtual { get; private set; } = 1;

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

        // Mostra tela de Game Over
        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null) hud.MostrarGameOver();
    }

    public bool JogoAtivo() => jogoAtivo;
    public void TriggerVitoria()
    {
        if (!jogoAtivo || gameOver) return;

        jogoAtivo = false;

        // Para o spawn
        VehicleSpawner spawner = FindObjectOfType<VehicleSpawner>();
        if (spawner != null) spawner.PararSpawn();

        // Para o jogador
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.enabled = false;

        // Para todos os carros
        VehicleMove[] carros = FindObjectsOfType<VehicleMove>();
        foreach (VehicleMove carro in carros)
            carro.enabled = false;

        // Mostra tela de vitória
        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null) hud.MostrarVitoria();
    }
    public void AvancarFase()
    {
        NivelAtual++;

        VehicleMove[] carros = FindObjectsOfType<VehicleMove>();
        foreach (VehicleMove carro in carros)
            Destroy(carro.gameObject);

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.Resetar();

        VehicleSpawner spawner = FindObjectOfType<VehicleSpawner>();
        if (spawner != null) spawner.enabled = true;

        ApiDataLoader loader = FindObjectOfType<ApiDataLoader>();
        if (loader != null) loader.LoadData();
    }


}