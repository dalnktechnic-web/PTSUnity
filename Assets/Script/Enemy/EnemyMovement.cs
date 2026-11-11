
using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Pengaturan Enemy")]
    public float speed = 2f;
    public int damage = 25;

    [Header("Pengaturan Jarak")]
    public float jarakDeteksi = 5f;
    public float jarakSerang = 1.2f;
    public float attackCooldown = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isAttacking = false;
    private bool isDead = false;
    private bool attackHit = false;

    private Vector2 lastDirection = Vector2.down;

    private EnemySpawner spawner;
    private Transform spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (rb == null)
            Debug.LogError("Rigidbody2D Enemy tidak ditemukan!", this);

        if (anim == null)
            Debug.LogError("Animator Enemy tidak ditemukan!", this);

        FindPlayer();
    }

    public void Initialize(EnemySpawner owner, Transform point)
    {
        spawner = owner;
        spawnPoint = point;
    }

    void FindPlayer()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    void FixedUpdate()
    {
        if (isDead || rb == null || anim == null)
            return;

        if (player == null)
        {
            FindPlayer();
            return;
        }

        // Jika musuh sedang menyerang, abaikan pergerakan atau deteksi jarak lain
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(
            rb.position,
            player.position
        );

        // Player berada di luar jarak deteksi
        if (distance > jarakDeteksi)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("IsMoving", false);
            return;
        }

        // Player sudah masuk jarak serang
        if (distance <= jarakSerang)
        {
            Debug.Log("PLAYER MASUK JARAK SERANG!");

            rb.linearVelocity = Vector2.zero;
            anim.SetBool("IsMoving", false);

            StartCoroutine(AttackPlayer());
            return; // Penting agar tidak mengeksekusi kode di bawahnya
        }

        // Hitung arah menuju Player
        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        // Simpan arah hadap Enemy
        if (direction.sqrMagnitude > 0.01f)
        {
            lastDirection = direction;
            UpdateDirection();
        }

        // Enemy mengejar Player
        rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );

        anim.SetBool("IsMoving", true);
    }

    void UpdateDirection()
    {
        anim.SetFloat("moveX", lastDirection.x);
        anim.SetFloat("moveY", lastDirection.y);
    }

    IEnumerator AttackPlayer()
{
    Debug.Log("ENEMY MULAI MENYERANG!");

    isAttacking = true;
    attackHit = false;

    rb.linearVelocity = Vector2.zero;

    anim.SetBool("IsMoving", false);
    UpdateDirection();

    anim.SetTrigger("Attack");

    yield return new WaitForSeconds(attackCooldown);

    isAttacking = false;
}

    // Dipanggil melalui Animation Event pada animasi Attack
    public void FinishAttack()
    {
        if (isDead || attackHit)
            return;

        attackHit = true;

        Debug.Log("ENEMY BERHASIL MENYERANG!");

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            PlayerHealth health =
                playerObject.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("PlayerHealth tidak ditemukan!");
            }
        }

        // Enemy mati setelah satu kali menyerang
        Die();
    }

    // Dipanggil ketika Player memukul Enemy
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        Die();
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        isAttacking = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (anim != null)
            anim.SetTrigger("Die");
        else
            FinishDie();
    }

    // Dipanggil melalui Animation Event pada frame terakhir animasi Die
    public void FinishDie()
{
    Debug.Log("FINISH DIE TERPANGGIL!");

    if (spawner != null && spawnPoint != null)
    {
        Debug.Log("SPAWNER DAN SPAWN POINT DITEMUKAN!");

        spawner.RespawnEnemy(spawnPoint);
    }
    else
    {
        Debug.LogError("SPAWNER ATAU SPAWN POINT NULL!");
    }

    Destroy(gameObject);
}
}
