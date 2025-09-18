using UnityEngine;

public class EnemyRange : Enemy
{
    private Vector3 direction;
    private LineRenderer lineRenderer;

    [SerializeField] private float rayDistance = 20f; // alcance máximo
    [SerializeField] private float laserDuration = 0.2f; // tiempo visible
    [SerializeField] private Color laserColor = Color.red;
    [SerializeField] private GameObject startLaserPoint;
    private Vector3 endPoint = new Vector3(0f, 2f, 0f);

    private void Start()
    {
        // Configurar LineRenderer
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
        lineRenderer.enabled = false;
    }

    public override void StateAtacar()
    {
        if (!live) return;
        if (PuedeAtacar())
        {
            Vector3 origin;
            if (startLaserPoint != null)
            {
                origin = startLaserPoint.transform.position;
            }
            else
            {
                origin = transform.position;
            }

            direction = (target.transform.position - origin).normalized;

            RaycastHit hit;

            if (Physics.Raycast(origin, direction, out hit, rayDistance))
            {
                // El rayo se corta en la pared/objeto
                DrawLaser(origin, hit.point);
                Debug.Log("Rayo impactó en: " + hit.collider.name);

                // Si golpea al jugador
                if (hit.collider.CompareTag("Player"))
                {
                   
                }
            }
            else
            {
                // dibuja hasta el alcance máximo
                DrawLaser(origin, origin + direction * rayDistance);
            }

            ReiniciarCooldown();
        }
    }

    private void DrawLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end+endPoint);
        
        lineRenderer.enabled = true;

        // Ocultar después de un tiempo
        Invoke(nameof(DisableLaser), laserDuration);
    }

    private void DisableLaser()
    {
        lineRenderer.enabled = false;
    }
}

