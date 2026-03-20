# VBL Smart Crossing

Protótipo desenvolvido como parte do desafio técnico para a vaga de **Game Developer Pleno** no Centro de Pesquisas Avançadas Wernher von Braun.

---

## Sobre o Projeto

VBL Smart Crossing é um simulador inspirado no clássico **Frogger**, onde o jogador controla um personagem que deve atravessar uma via expressa com tráfego dinâmico dirigido por dados de uma API de predição de tráfego e clima.

O tráfego, a velocidade dos veículos e as condições climáticas não são aleatórios — eles refletem o estado atual e as predições da API, agendadas e aplicadas em tempo real durante a simulação.

---

## Requisitos

- **Unity 6** (testado na versão 6000.0)
- **Input System** (pacote incluso no projeto)
- **TextMeshPro** (pacote incluso no projeto)
- Sistema operacional: Windows

---

## Como Rodar o Projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/vbl-smart-crossing.git
```

### 2. Abrir no Unity

1. Abra o **Unity Hub**
2. Clique em **Open > Add project from disk**
3. Selecione a pasta raiz do repositório clonado
4. Aguarde o Unity importar os pacotes e compilar os scripts

### 3. Configurar a cena

1. No painel **Project**, navegue até `Assets/Scenes`
2. Abra a cena `SampleScene`

### 4. Rodar o jogo

Clique em **Play** no editor. Nenhuma configuração adicional é necessária — o mock da API é carregado automaticamente a partir do arquivo `Assets/Resources/vbl_traffic_mock.json`.

---

## Mock da API

O projeto utiliza um arquivo JSON local como mock do endpoint `GET /v1/traffic/status`, conforme especificado no contrato OpenAPI do desafio.

**Localização:** `Assets/Resources/vbl_traffic_mock.json`

```json
{
  "current_status": {
    "vehicleDensity": 0.7,
    "averageSpeed": 60.0,
    "weather": "sunny"
  },
  "predicted_status": [
    {
      "estimated_time": 5000,
      "predictions": {
        "vehicleDensity": 0.8,
        "averageSpeed": 50.0,
        "weather": "foggy"
      }
    },
    {
      "estimated_time": 10000,
      "predictions": {
        "vehicleDensity": 0.9,
        "averageSpeed": 40.0,
        "weather": "light rain"
      }
    },
    {
      "estimated_time": 18000,
      "predictions": {
        "vehicleDensity": 1.0,
        "averageSpeed": 25.0,
        "weather": "heavy rain"
      }
    }
  ]
}
```

O arquivo pode ser substituído por qualquer resposta válida do contrato para testar diferentes cenários. Para usar o Mockoon ou outro servidor de mock, basta substituir o método `LoadData` em `ApiDataLoader.cs` por uma chamada `UnityWebRequest` apontando para o endpoint local.

---

## Controles

| Tecla | Ação |
|-------|------|
| Seta cima | Mover para frente |
| Seta baixo | Mover para trás |
| Seta esquerda | Mover para a esquerda |
| Seta direita | Mover para a direita |
| Espaço | Reiniciar (Game Over) / Avançar fase (Vitória) |

---

## Regras do Jogo

### Spawn de Veículos
O intervalo entre veículos é calculado com base na densidade da via:

```
Intervalo (s) = 1 / vehicleDensity
```

### Velocidade dos Veículos
A velocidade dos carros é mapeada proporcionalmente a partir da API:

```
Velocidade Unity = (averageSpeed / 100) * velocidadeReferencia
```

### Impacto do Clima no Jogador
O clima afeta a velocidade de movimentação do personagem:

| Clima | Multiplicador |
|-------|--------------|
| sunny | 1.0x |
| clouded / foggy | 0.8x |
| light rain | 0.6x |
| heavy rain | 0.4x |

### Predições
O array `predicted_status` é processado no início de cada fase. Cada predição é agendada com `Coroutine` e aplicada automaticamente no tempo definido por `estimated_time` (ms), atualizando o tráfego e o clima em tempo real.

### Condições de Fim de Fase

- **Vitória:** o personagem alcança a calçada do lado oposto da via. O jogo avança de nível e uma nova chamada à API é realizada.
- **Game Over por colisão:** o personagem é atingido por um veículo.
- **Game Over por tempo:** o cronômetro, definido pelo `estimated_time` da última predição recebida, chega a zero com o personagem ainda na pista.

---

## Dificuldade Progressiva

A cada fase concluída, o número de veículos spawnados por intervalo aumenta progressivamente:

| Fases | Veículos por spawn |
|-------|-------------------|
| 1 | 1 |
| 2 | 2 |
| 3 | 3 |
| 4 | 4 |
...

---

## Arquitetura do Código

```
Assets/Scripts/
├── TrafficResponse.cs       # Contrato de dados (Status, PredictedStatus, TrafficResponse)
├── ApiDataLoader.cs         # Lê o JSON e dispara evento OnDataLoaded
├── PredictionScheduler.cs   # Agenda predições com Coroutine e dispara OnClimaAtualizado
├── PlayerController.cs      # Movimento do jogador com multiplicador de clima
├── PlayerCollision.cs       # Detecta colisão com veículos via tag
├── VehicleSpawner.cs        # Spawna veículos com intervalo e velocidade da API
├── VehicleMove.cs           # Movimenta e destrói veículos ao sair da pista
├── FinishLine.cs            # Detecta chegada do jogador ao outro lado
├── GameManager.cs           # Controla estado do jogo (ativo, vitória, game over)
└── HUDManager.cs            # Exibe nível, clima, densidade, velocidade e timer
```

A comunicação entre sistemas é feita via **eventos estáticos** (`static event Action`), mantendo desacoplamento entre a camada de dados e a camada de visualização/gameplay:

- `ApiDataLoader.OnDataLoaded` — disparado ao carregar dados da API
- `PredictionScheduler.OnClimaAtualizado` — disparado quando uma predição é aplicada

---

## Contato

Desenvolvido por **[Seu Nome]**
E-mail: seu@email.com
