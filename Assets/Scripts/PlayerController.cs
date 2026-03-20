using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float baseSpeed = 5f;

    private float currentSpeed;
    private CharacterController characterController;

    void OnEnable()
    {
        ApiDataLoader.OnDataLoaded += AplicarClima;
        PredictionScheduler.OnClimaAtualizado += AplicarClima;
    }

    void OnDisable()
    {
        ApiDataLoader.OnDataLoaded -= AplicarClima;
        PredictionScheduler.OnClimaAtualizado -= AplicarClima;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) moveX = 1f;
        if (Keyboard.current.upArrowKey.isPressed) moveZ = 1f;
        if (Keyboard.current.downArrowKey.isPressed) moveZ = -1f;

        characterController.Move(new Vector3(moveX, 0, moveZ) * currentSpeed * Time.deltaTime);
    }

    void AplicarClima(TrafficResponse data)
    {
        AplicarMultiplicador(data.current_status.weather);
    }

    void AplicarClima(Status status)
    {
        AplicarMultiplicador(status.weather);
    }

    void AplicarMultiplicador(string weather)
    {
        float multiplicador = weather switch
        {
            "sunny" => 1.0f,
            "clouded" => 0.8f,
            "foggy" => 0.8f,
            "light rain" => 0.6f,
            "heavy rain" => 0.4f,
            _ => 1.0f
        };

        currentSpeed = baseSpeed * multiplicador;
        Debug.Log($"[PLAYER] Velocidade ajustada para clima '{weather}': {currentSpeed} u/s");
    }
}