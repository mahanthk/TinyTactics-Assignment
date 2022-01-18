using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonManager : MonoBehaviour
{

    Shop instance;

    public Button _100DiamondsButton;
    public Button _300DiamondsButton;
    public Button _500DiamondsButton;
    public Button _1000CoinsButton;
    public Button _2000CoinsButton;
    public Button _10HealthPotionsButton;

    private void Start()
    {
        instance = Shop.INSTANCE;
        _100DiamondsButton.onClick.AddListener(() => { instance.SetDiamonds(100, 100); });
        _300DiamondsButton.onClick.AddListener(() => { instance.SetDiamonds(300, 250); });
        _500DiamondsButton.onClick.AddListener(() => { instance.SetDiamonds(500, 350); });
        _1000CoinsButton.onClick.AddListener(() => { instance.SetCoins(1000, 100); });
        _2000CoinsButton.onClick.AddListener(() => { instance.SetCoins(2000, 100); });
        _10HealthPotionsButton.onClick.AddListener(() => { instance.SethealthPotions(10, 100); });
    }
}
