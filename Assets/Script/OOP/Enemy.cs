using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int hp = 100;
    public float ms = 2f;

    protected Transform player;
    [Header("Pengaturan State Machine")]
    [SerializeField] private float jarakDeteksi = 5f;
    [SerializeField] private float jarakSerang = 1.2f;
    [SerializeField] private float jedaSerang = 1f;

    // state sekarang
    private StateZombie currentState = StateZombie.IDLE;
    private float waktuSerangTerakhir;
    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {


        PeriksaTransisi();
        switch (currentState)
        {
            case StateZombie.IDLE: PerilakuIdle(); break;
            case StateZombie.PATROL: PerilakuPatrol(); break;
            case StateZombie.CHASE: PerilakuChase(); break;
            case StateZombie.ATTACK: PerilakuAttack(); break;
        }
    }

    public void Kejar()
    {
        if(player == null) return;
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    public virtual void Serang()
    {
        Debug.Log("Enemy Menyerang");
    }

    public void TakeDamage(int jumlah)
    {
        hp -= jumlah;
        Debug.Log($"{gameObject.name} kena damage {jumlah}, Hp sisa : {hp}");
        if(hp <= 0)
        {
            Mati();
        }
    }

    private void Mati()
    {
        Debug.Log($"{gameObject.name} mati!");
        Destroy(gameObject);
    }

    public float JarakKePlayer()
    {
        if(player == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, player.position);
    }

    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();

        if(jarak <= jarakSerang)
        {
            currentState = StateZombie.ATTACK;
        }else if(jarak <= jarakDeteksi)
        {
            currentState = StateZombie.CHASE;
        }
        else
        {
            currentState = StateZombie.PATROL;
        }
    }

    void PerilakuIdle()
    {

    }

    void PerilakuPatrol()
    {
        // Debug.Log("Zombie Sedang Berpatroli");
    }

    void PerilakuChase()
    {
        Kejar();
        Debug.Log("Zombie Sedang Mengejar Player");
    }


    void PerilakuAttack()
    {
        Debug.Log("Zombie Sedang Menyerang Player");
    }
}