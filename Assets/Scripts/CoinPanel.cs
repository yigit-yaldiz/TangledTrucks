using UnityEngine;
using TMPro;

public class CoinPanel : MonoBehaviour
{
    TMP_Text _coinText;

    private void Awake()
    {
        _coinText = GetComponentInChildren<TMP_Text>();
    }

    public void ChangeCoinText(int coinCount)
    {
        _coinText.text = coinCount.ToString();
    }
}
