using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalKoin;
    private int koinTerkumpul = 0;

    public TextMeshProUGUI TextMenang;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Hitung jumlah koin di scene saat mulai
        totalKoin = GameObject.FindGameObjectsWithTag("Coin").Length;

        Debug.Log("Total koin: " + totalKoin);

        if (TextMenang != null)
        {
            TextMenang.gameObject.SetActive(false);
        }
    }

    public void AmbilKoin()
    {
        koinTerkumpul++;

        Debug.Log("Koin terkumpul: " + koinTerkumpul + "/" + totalKoin);

        // Jika semua koin sudah diambil
        if (koinTerkumpul >= totalKoin)
        {
            Menang();
        }
    }

    void Menang()
    {
        Debug.Log("KAMU MENANG!");

        if (TextMenang != null)
        {
            TextMenang.gameObject.SetActive(true);
        }
    }
}