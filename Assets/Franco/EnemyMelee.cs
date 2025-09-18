using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

public class EnemyMelee : Enemy
{
    [SerializeField] private float forceAtack = 50f;
    [SerializeField] private Animator animator;
    private Vector3 direction;
    [SerializeField]  private float retroceso=5f;

    private void Start()
    {

    }


    public override void StateAtacar()
    {
        if (!live) return;
        if (PuedeAtacar())
        {

           
            direction = (target.transform.position - transform.position).normalized;

            rb.AddForce((direction * forceAtack), ForceMode.Impulse);



            ReiniciarCooldown();
            
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            target.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
            Debug.Log("Jugador golpeado por el enemigo cuerpo a cuerpo");
        }
        else if (collision.gameObject.CompareTag("Pared"))
        {
            Debug.Log("Enemigo cuerpo a cuerpo tocó pared");

            Vector3 normal = collision.contacts[0].normal;

            Vector3 bounceDirection = Vector3.Reflect(direction, normal).normalized;

            rb.linearVelocity = Vector3.zero;

            rb.AddForce(bounceDirection * retroceso, ForceMode.Impulse);

            ChangeState(States.idle);

            StartCoroutine(DelayStun(0.1f));
        }
    }
    private IEnumerator DelayStun(float delay)
    {
        yield return new WaitForSeconds(delay);
        ApplyStun();
    }


}