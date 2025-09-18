using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform _target;
    public Vector3 offset;

    public void LateUpdate()
    {

        transform.position = _target.position + offset;
    }
}
