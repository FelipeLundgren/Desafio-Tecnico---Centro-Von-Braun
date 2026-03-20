using UnityEngine;

public class ApiDataLoader : MonoBehaviour
{
    // Evento disparado quando os dados são carregados
    public static event System.Action<TrafficResponse> OnDataLoaded;

    void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        // Carrega o JSON da pasta Resources
        TextAsset mockFile = Resources.Load<TextAsset>("vbl_traffic_mock");

        if (mockFile == null)
        {
            Debug.LogError("Api nao encontrada");
            return;
        }

        TrafficResponse data = JsonUtility.FromJson<TrafficResponse>(mockFile.text);

        if (data == null)
        {
            Debug.LogError("falha ao deserializar o JSON.");
            return;
        }

        Debug.Log($"Clima atual: {data.current_status.weather}");
        Debug.Log($"Densidade: {data.current_status.vehicleDensity}");
        Debug.Log($"Predições carregadas: {data.predicted_status.Length}");

        // Notifica todos os sistemas do jogo que os dados chegaram
        OnDataLoaded?.Invoke(data);
    }
}