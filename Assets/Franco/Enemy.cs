using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody rb;

    [SerializeField] private States states;
    [SerializeField] protected float distanciaSeguir;
    [SerializeField] protected float distanciaAtacar;
    [SerializeField] protected float distanciaVolver;

    [SerializeField] protected float tiempoUltimoAtaque = 0f;
    [SerializeField] protected float cooldownAtaque = 1.5f;

    protected bool isStunned = false;
    [SerializeField] private float stunDuration = 2f;
    


    [SerializeField] protected Transform target;
    protected float distancia;
    protected bool live = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CalcularDistancia());
    }



    private void LateUpdate()
    {
        CheckStates();
    }
    public enum States
    {
        idle = 0,
        follow = 1,
        atack = 2,
        dead = 3
    }


    public void CheckStates()
    {

        switch (states)
        {
            case States.idle:
                StateIdle();
                break;
            case States.follow:
                StateFollow();
                break;
            case States.atack:
                StateAtacar();
                break;
            case States.dead:
                StateDead();
                break;
            default:
                break;

        }

    }


    public void ChangeState(States currentState)
    {
        switch (currentState)
        {
            case States.idle:
                Debug.Log("durmiendo");
                break;
            case States.follow:
                Debug.Log("Siguiendo");
                break;
            case States.atack:
                Debug.Log("atacando");
                break;
            case States.dead:
                Debug.Log("muertando");
                live = false;
                break;
            default:
                break;

        }
        states = currentState;
    }

    public virtual void StateIdle()
    {

        if (distancia < distanciaSeguir)
        {
            ChangeState(States.follow);

        }
    }

    public virtual void StateFollow()
    {
        if (distancia < distanciaAtacar)
        {
            ChangeState(States.atack);
        }
        else if (distancia > distanciaVolver)
        {
            ChangeState(States.idle);
        }
    }
    public virtual void StateAtacar()
    {
        if (distancia > distanciaAtacar + 0.5)
        {
            ChangeState(States.follow);
        }

    }
    public virtual void StateDead()
    {

    }
    public bool PuedeAtacar()
    {
        return Time.time - tiempoUltimoAtaque >= cooldownAtaque;
    }

    public void ReiniciarCooldown()
    {
        tiempoUltimoAtaque = Time.time;
    }

    IEnumerator CalcularDistancia()
    {
        while (live)
        {
            if (target != null)
            {
                distancia = Vector3.Distance(transform.position, target.position);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    protected IEnumerator Stun()
    {
        if (isStunned) yield break;

        isStunned = true;
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;   
       

        yield return new WaitForSeconds(stunDuration);

        rb.isKinematic = false;  
        isStunned = false;
        
    }

    public void ApplyStun()
    {
        if (!isStunned)
        {
            StartCoroutine(Stun());
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtacar);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaSeguir);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaVolver);
    }


}