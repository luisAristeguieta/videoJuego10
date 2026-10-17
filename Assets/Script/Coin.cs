using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Valor")]
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si quien atravesó la moneda tiene el Tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Suma al contador de la UI
            if (CoinUI.instance != null)
            {
                CoinUI.instance.AddCoin(value);
            }

            // Destruye la moneda recogida
            Destroy(gameObject);
        }
    }
}