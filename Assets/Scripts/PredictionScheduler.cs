using UnityEngine;

public class PredictionScheduler : MonoBehaviour
{
    // Evento para notificar outros sistemas quando o clima trocar
    public static event System.Action<Status> OnClimaAtualizado;

    void OnEnable()
    {
        ApiDataLoader.OnDataLoaded += AgendarPredicoes;
    }

    void OnDisable()
    {
        ApiDataLoader.OnDataLoaded -= AgendarPredicoes;
    }

    void AgendarPredicoes(TrafficResponse data)
    {
        foreach (PredictedStatus predicao in data.predicted_status)
        {
            float tempoEmSegundos = predicao.estimated_time / 1000f;
            Status status = predicao.predictions;
            StartCoroutine(AguardarETrocar(tempoEmSegundos, status));
        }
    }

    System.Collections.IEnumerator AguardarETrocar(float delay, Status status)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log($"[CLIMA ATUALIZADO] {status.weather} | " +
                  $"Densidade: {status.vehicleDensity} | " +
                  $"Velocidade: {status.averageSpeed} km/h");

        // Notifica o PlayerController e qualquer outro sistema
        OnClimaAtualizado?.Invoke(status);
    }
}