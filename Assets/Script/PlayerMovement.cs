
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;

    private Vector2 input;
    private Vector2 lastMoveDirection = Vector2.down;

    private Animator anim;
    private bool facingLeft = false;

    private bool isAttacking;

    private bool hasHitEnemy = false;

    public int attackDamage = 25;
    public float attackRange = 0.8f;
    public float attackOffset = 0.6f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // New Input System
   void OnMove(InputValue value)
    {
    input = value.Get<Vector2>();

    Debug.Log("Input Movement: " + input);

    input.Normalize();
    }

    void Update()
    {
        Animate();

        if ((input.x < 0 && !facingLeft) ||
            (input.x > 0 && facingLeft))
        {
            Flip();
        }

        if (input == Vector2.zero)
        {
            // Simpan arah terakhir saat berhenti
        }
        else
        {
            lastMoveDirection = input;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Attack();
        }
    }

  void FixedUpdate()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = input * speed;
        }
    void Animate()
    {
        anim.SetFloat("moveX", input.x);
        anim.SetFloat("moveY", input.y);
        anim.SetFloat("moveMag", input.magnitude);

        anim.SetFloat("lastMoveX", lastMoveDirection.x);
        anim.SetFloat("lastMoveY", lastMoveDirection.y);
    }
    void Attack()
    {
        Debug.Log("ATTACK DIPANGGIL");
           if (isAttacking) return;

            isAttacking = true;
            hasHitEnemy = false;

            if (lastMoveDirection.x < 0 && !facingLeft)
            {
                Flip();
            }
            else if (lastMoveDirection.x > 0 && facingLeft)
            {
                Flip();
            }

            anim.SetFloat("attackX", Mathf.Abs(lastMoveDirection.x));
            anim.SetFloat("attackY", lastMoveDirection.y);

            anim.SetBool("isAttacking", true);
    }

    public void AttackHit()
    {
        if (hasHitEnemy) return;

        Vector2 attackDirection = lastMoveDirection.normalized;

        Vector2 attackPosition =
            (Vector2)transform.position +
            attackDirection * attackOffset;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPosition,
            attackRange
        );

        foreach (Collider2D hit in hits)
        {
            EnemyMovement enemy =
                hit.GetComponentInParent<EnemyMovement>();

            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);

                hasHitEnemy = true;

                Debug.Log("Player berhasil menyerang Enemy!");

                break;
            }
        }
    }

    public void FinishAttack()
    {
        Debug.Log("FINISH ATTACK DIPANGGIL");
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
        facingLeft = !facingLeft;
    }
}
