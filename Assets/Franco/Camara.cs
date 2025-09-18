using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
