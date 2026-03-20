using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Veiculo"))
        {
            Debug.Log("[GAME OVER] A galinha foi atropelada!");
        }
    }
}