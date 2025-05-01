using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform[] targets;
    int currentTarget = 0;

    [SerializeField] float speed;

    [SerializeField] float attackDistance;

    [SerializeField] Transform punchPivot;

    PlayerLife playerLife;
    Transform playerTransform;

    EnemyPunch punch;

    Animator animator;

    void Start()
    {
        playerLife = FindFirstObjectByType<PlayerLife>();
        playerTransform = playerLife.transform;

        punch = GetComponentInChildren<EnemyPunch>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackDistance)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targets[currentTarget].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targets[currentTarget].position) <= 0.01f)
        {
            currentTarget++;
            currentTarget %= targets.Length;
        }
    }

    void Attack()
    {
        Vector3 direction = playerTransform.position - punchPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        punchPivot.rotation = Quaternion.Euler(0, 0, angle);

        animator.SetTrigger("Attack");
    }

    public void CheckPlayerTouch()
    {
        if (punch.TouchPlayer)
        {
            playerLife.CauseDamage();
        }
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        playerLife.CauseDamage();
    //    }
    //}
}
