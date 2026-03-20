using UnityEngine;

public class VehicleMove : MonoBehaviour
{
    public float speed = 5f;
    public float destroyX = 50f;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

        if (transform.position.x >= destroyX)
            Destroy(gameObject);
    }
}