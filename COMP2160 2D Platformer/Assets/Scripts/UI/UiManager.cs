/**
 *
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

#region Parameters
    [SerializeField] private string coinFormat = "Coins: {0}";
#endregion

#region Other objects
    [SerializeField] private TextMeshProUGUI coinsLabel;
#endregion

#region Init & Destroy
    void OnEnable()
    {
        // subscribe to coin collection events
        GameManager.Instance.OnCoinCollected += OnCoinCollected;
    }

    void OnDisable()
    {
        // unsubscribe from coin collection events
        GameManager.Instance.OnCoinCollected -= OnCoinCollected;
    }

    void Start()
    {
        coinsLabel.text = string.Format(coinFormat, GameManager.Instance.CoinsCollected);
    }
#endregion 

#region Events
    private void OnCoinCollected(int coinsCollected)
    {
        coinsLabel.text = string.Format(coinFormat, coinsCollected);
    }
#endregion 

}
