using UnityEngine;

public class PredictionScheduler : MonoBehaviour
{
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
            // Converte milissegundos para segundos
            float tempoEmSegundos = predicao.estimated_time / 1000f;

            // Captura os dados da predição para usar dentro do lambda
            Status status = predicao.predictions;

            Invoke_Schedulado(tempoEmSegundos, status);
        }
    }

    void Invoke_Schedulado(float delay, Status status)
    {
        StartCoroutine(AguardarETrocar(delay, status));
    }

    System.Collections.IEnumerator AguardarETrocar(float delay, Status status)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log($"[CLIMA ATUALIZADO] {status.weather} | " +
                  $"Densidade: {status.vehicleDensity} | " +
                  $"Velocidade: {status.averageSpeed} km/h");
    }
}