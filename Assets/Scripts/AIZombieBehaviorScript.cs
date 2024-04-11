using System.Collections;
using UnityEngine;


public class AIZombieBehaviorScript : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    public Animator animator;

    private bool isAttacking = false;

    public bool isHit = false;
    public bool isHitByExplosion = false;

    public int initialHealth = 100;
    public float deathTimer = 5.0f;
    public float hitStopTimer = 5.0f;

    private int health = 100;
    private bool died = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = initialHealth;
    }

    // Update is called once per frame  
    void Update()
    {
        isAttacking = animator.GetBool("IsAttacking");

        if (isHit)
        {
            health -= 20;
            isHit = false;
            animator.SetTrigger("OnHit");
            StartCoroutine(PauseAgentFollowing(hitStopTimer));

            died = health <= 0;
            if (died)
            {
                health = 0;
                animator.SetFloat("Vertical", 0);
                animator.SetTrigger("Death");
                StartCoroutine(DestroyObject());
            }
        }

        if (isHitByExplosion)
        {
            health = 0;
            died = true;
            isHitByExplosion = false;
            animator.SetTrigger("Death");
            StartCoroutine(DestroyObject());
        }

        if (!died)
        {
            agent.SetDestination(target.position);
            animator.SetFloat("Vertical", Mathf.Clamp(agent.velocity.magnitude / agent.speed, 0, 1));

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!isAttacking)
                {
                    animator.SetTrigger("Attack");
                    animator.SetBool("IsAttacking", true);
                }
            }
            else animator.SetBool("IsAttacking", false);
        }

    }

    private IEnumerator DestroyObject()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
    }

    private IEnumerator PauseAgentFollowing(float seconds)
    {
        agent.enabled = false;
        yield return new WaitForSeconds(seconds);
        agent.enabled = true;
    }
}
