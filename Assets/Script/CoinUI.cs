using UnityEngine;
using TMPro; // Usamos TextMeshPro ya que creaste Text (TMP)

public class CoinUI : MonoBehaviour
{
    public static CoinUI instance;

    [Header("Referencia al Texto")]
    [SerializeField] private TextMeshProUGUI coinText;

    private int coinCount = 0;

    private void Awake()
    {
        // Patrón Singleton para acceder directamente desde cualquier moneda
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinDisplay();
    }

    public void AddCoin(int amount = 1)
    {
        coinCount += amount;
        UpdateCoinDisplay();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirSfx(AudioManager.Instance.sonidoMoneda);
        }
    }

    private void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = "Monedas: " + coinCount.ToString();
        }
    }
}