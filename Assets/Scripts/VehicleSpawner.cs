using UnityEngine;
using System.Collections;

public class VehicleSpawner : MonoBehaviour
{
    [Header("Prefabs dos veículos")]
    public GameObject[] vehiclePrefabs;

    [Header("Configuração da pista")]
    public float spawnX = -50f;   // onde o carro nasce (esquerda)
    public float spawnY = 0f;     // altura do spawn
    public float destroyX = 50f;  // onde o carro é destruído (direita)
    public float[] lanePositionsZ = { -20f, -10f, 0f, 10f, 20f, 30f };

    [Header("Velocidade de referência")]
    public float referenceSpeed = 10f;

    private float spawnInterval = 1f;
    private float vehicleSpeed = 5f;
    private Coroutine spawnCoroutine;

    void OnEnable()
    {
        ApiDataLoader.OnDataLoaded += AplicarDados;
        PredictionScheduler.OnClimaAtualizado += AplicarPredicao;
    }

    void OnDisable()
    {
        ApiDataLoader.OnDataLoaded -= AplicarDados;
        PredictionScheduler.OnClimaAtualizado -= AplicarPredicao;
    }

    void AplicarDados(TrafficResponse data)
    {
        AplicarStatus(data.current_status);
    }

    void AplicarPredicao(Status status)
    {
        AplicarStatus(status);
    }

    void AplicarStatus(Status status)
    {
        if (!enabled) return; // ignora se o spawner estiver parado

        // Fórmula do desafio: intervalo = 1 / densidade
        spawnInterval = 1f / status.vehicleDensity;

        // Fórmula do desafio: velocidade = (averageSpeed / 100) * referencia
        vehicleSpeed = (status.averageSpeed / 100f) * referenceSpeed;

        Debug.Log($"[SPAWNER] Intervalo: {spawnInterval:F2}s | Velocidade: {vehicleSpeed:F2} u/s");

        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnVeiculo();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnVeiculo()
    {
        int nivel = GameManager.Instance != null ? GameManager.Instance.NivelAtual : 1;

        // A cada 2 níveis spawna um carro a mais, máximo de 4
        int quantidadeCarros = Mathf.Min(1 + (nivel - 1) / 1, 8);

        for (int i = 0; i < quantidadeCarros; i++)
        {
            GameObject prefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
            float laneZ = lanePositionsZ[Random.Range(0, lanePositionsZ.Length)];

            Vector3 spawnPos = new Vector3(spawnX, spawnY, laneZ);
            GameObject veiculo = Instantiate(prefab, spawnPos, Quaternion.Euler(0f, 90f, 0f));

            VehicleMove mover = veiculo.AddComponent<VehicleMove>();
            mover.speed = vehicleSpeed;
            mover.destroyX = destroyX;
        }
        Debug.Log($"[SPAWNER] Nível {nivel} | Carros por spawn: {quantidadeCarros}");
    }
    public void PararSpawn()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        enabled = false; // garante que AplicarStatus não reinicie
    }
}