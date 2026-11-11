using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Diperlukan untuk merestart game
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Image healthBarFill;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return; // Kalau sudah mati, abaikan damage

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();
        Debug.Log("HP Player: " + currentHealth);

        // Panggil animasi Hit/Hurt di sini
        Animator anim = GetComponent<Animator>();
        if (anim != null && currentHealth > 0)
        {
            anim.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

   void Update()
{
    // Tombol tes H untuk mengurangi HP sebanyak 25
    if (Keyboard.current != null &&
        Keyboard.current.hKey.wasPressedThisFrame &&
        !isDead)
    {
        TakeDamage(25);
    }
}

    void Die()
    {
        isDead = true;
        Debug.Log("Player Kalah!");

        // Memanggil animasi mati berdasarkan Trigger di Animator
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("isDead");
        }

        // Delay 2 detik sebelum merestart game agar animasi sempat terlihat
        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        // Memuat ulang scene yang sedang aktif
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}