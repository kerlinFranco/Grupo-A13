using UnityEngine;

public class UpperBodyLookAt : MonoBehaviour
{
    public Transform spineBone;   // Hueso del torso
    public Camera cam;            // Cámara principal
    public float rotationSpeed = 5f;

    void Update()
    {
        if (spineBone == null || cam == null) return;

        // Plano en el suelo a la altura del personaje
        Plane plane = new Plane(Vector3.up, spineBone.position);

        // Ray desde el mouse
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            // Dirección hacia el mouse proyectado en el plano
            Vector3 dir = hitPoint - spineBone.position;
            dir.y = 0; // <- elimina la inclinación hacia arriba/abajo (opcional)

            // Calcula rotación
            Quaternion targetRot = Quaternion.LookRotation(dir);

            // Rotación suave
            spineBone.rotation = Quaternion.Slerp(spineBone.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }
}
