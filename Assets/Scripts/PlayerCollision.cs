using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[COLLISION] Colidiu com: {other.gameObject.name} | Tag: {other.tag}");

        if (other.CompareTag("Veiculo"))
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}