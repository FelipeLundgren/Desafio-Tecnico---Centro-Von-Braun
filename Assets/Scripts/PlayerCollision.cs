using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Veiculo"))
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}