using System.Collections;
using UnityEngine;

public class EnemyPatrol : Enemy
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 3f;
    //[SerializeField] private GameObject Scavenger;

    private int currentWaypointIndex = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void StateIdle()
    {
        // Patrulla solo en Idle
        Patrol();

        // Si el jugador está en rango  interrumpe patrulla y sigue al jugador
        if (distancia < distanciaSeguir)
        {
            ChangeState(States.follow);
        }
    }

    public override void StateFollow()
    {
        // Persigue al jugador
        ChasePlayer();

        if (distancia < distanciaAtacar)
        {
            ChangeState(States.atack);
        }
        else if (distancia > distanciaVolver)
        {
            // Si el jugador se aleja vuelve a patrullar
            ChangeState(States.idle);
        }
    }

    public override void StateAtacar()
    {
        Debug.Log("Enemy Patrol ataca (placeholder)");

        //Instantiate(Scavenger, transform.position + Vector3.up, Quaternion.identity);
        // Si se aleja del rango de ataque volver a seguir
        if (distancia > distanciaAtacar + 0.5f)
        {
            ChangeState(States.follow);
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void ChasePlayer()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            Gizmos.color = Color.red;
            foreach (Transform waypoint in waypoints)
            {
                if (waypoint != null)
                {
                    Gizmos.DrawSphere(waypoint.position, 0.3f);
                }
            }

            Gizmos.color = Color.green;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null && waypoints[(i + 1) % waypoints.Length] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[(i + 1) % waypoints.Length].position);
                }
            }
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaSeguir);
    }
}