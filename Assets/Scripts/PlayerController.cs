using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float baseSpeed = 5f;

    [Header("Limites da pista")]
    public float limiteZMin = -25f;
    public float limiteZMax = 35f;
    public float limiteXMin = -20f;
    public float limiteXMax = 30f;

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

    private Vector3 posicaoInicial;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentSpeed = baseSpeed;
        posicaoInicial = transform.position; // salva posição inicial
    }

    public void Resetar()
    {
        // Desativa o CharacterController para poder mover o personagem
        characterController.enabled = false;
        transform.position = posicaoInicial;
        characterController.enabled = true;
        enabled = true;
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
        // Clamp da posição dentro dos limites da pista
        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z, limiteZMin, limiteZMax);
        pos.x = Mathf.Clamp(pos.x, limiteXMin, limiteXMax);

        if (pos != transform.position)
        {
            characterController.enabled = false;
            transform.position = pos;
            characterController.enabled = true;
        }
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