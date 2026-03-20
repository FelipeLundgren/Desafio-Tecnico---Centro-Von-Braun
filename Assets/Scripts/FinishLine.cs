using UnityEngine;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[FINISHLINE] Colidiu com: {other.gameObject.name} | Tag: {other.tag}");

        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TriggerVitoria();
        }
    }
}