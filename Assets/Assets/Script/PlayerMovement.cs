using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 arahGerak;

    public TextMeshProUGUI scoreText;
    public int score = 0;

    void OnMove(InputValue value)
    {
        arahGerak = value.Get<Vector2>();
    }

    void Start()
    {
        ScoreUpdate();
    }

    void Update()
    {
        Vector3 arah = new Vector3(arahGerak.x, arahGerak.y, 0);

        transform.position += arah * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Coin berhasil diambil!");

            Destroy(other.gameObject);

            score += 1;

            ScoreUpdate();

            // Kirim informasi ke GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AmbilKoin();
            }
        }
    }

    void ScoreUpdate()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}