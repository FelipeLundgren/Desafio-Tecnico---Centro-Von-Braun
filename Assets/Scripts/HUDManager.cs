using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUDManager : MonoBehaviour
{
    [Header("Textos do HUD")]
    public TMP_Text textNivel;
    public TMP_Text textClima;
    public TMP_Text textDensidade;
    public TMP_Text textVelocidade;
    public TMP_Text textTimer;
    public TMP_Text textGameOver;

    private float timerTotal = 0f;
    private float timerAtual = 0f;
    private bool timerRodando = false;
    private int nivelAtual = GameManager.Instance.NivelAtual;

    void OnEnable()
    {
        ApiDataLoader.OnDataLoaded += IniciarHUD;
        PredictionScheduler.OnClimaAtualizado += AtualizarDadosClima;
    }

    void OnDisable()
    {
        ApiDataLoader.OnDataLoaded -= IniciarHUD;
        PredictionScheduler.OnClimaAtualizado -= AtualizarDadosClima;
    }

    void Start()
    {
        textGameOver.gameObject.SetActive(false);
    }

    void IniciarHUD(TrafficResponse data)
    {
        int ultimaPredicao = data.predicted_status.Length - 1;
        timerTotal = data.predicted_status[ultimaPredicao].estimated_time / 1000f;
        timerAtual = timerTotal;
        timerRodando = true;
        aguardandoInput = false;

        AtualizarDadosStatus(data.current_status);
        textNivel.text = $"Nível: {GameManager.Instance.NivelAtual}"; // atualizado
        textGameOver.gameObject.SetActive(false);
    }

    void AtualizarDadosStatus(Status status)
    {
        textClima.text = $"Clima: {status.weather}";
        textDensidade.text = $"Densidade: {status.vehicleDensity:F1}";
        textVelocidade.text = $"Velocidade: {status.averageSpeed} km/h";
    }

    void AtualizarDadosClima(Status status)
    {
        AtualizarDadosStatus(status);
    }

    void Update()
    {
        if (aguardandoInput)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (ehVitoria)
                    GameManager.Instance.AvancarFase();
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene(
                        UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        if (!timerRodando) return;

        timerAtual -= Time.deltaTime;

        if (timerAtual <= 0f)
        {
            timerAtual = 0f;
            timerRodando = false;
            textTimer.text = "Tempo: 0s";
            GameManager.Instance.TriggerGameOver();
            return;
        }

        textTimer.text = $"Tempo: {timerAtual:F1}s";
    }

    public void AvancarNivel()
    {
        textNivel.text = $"Nível: {GameManager.Instance.NivelAtual}";
    }
    private bool aguardandoInput = false;
    private bool ehVitoria = false;

    public void MostrarVitoria()
    {
        timerRodando = false;
        aguardandoInput = true;
        ehVitoria = true;
        textGameOver.gameObject.SetActive(true);
        textGameOver.text = "FASE COMPLETA!\nPressione ESPAÇO para continuar";

        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null) hud.AvancarNivel();
    }

    public void MostrarGameOver()
    {
        timerRodando = false;
        aguardandoInput = true;
        ehVitoria = false;
        textGameOver.gameObject.SetActive(true);
        textGameOver.text = "GAME OVER\nPressione ESPAÇO para recomeçar";
    }
}