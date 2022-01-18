using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour
{

    public static Shop INSTANCE;

    public GameObject shop_panel;
    public GameObject challenges_panel;
    public GameObject bottom_panel;
    public GameObject dialogbox_panel;
    public GameObject warningbox_panel;
    public GameObject readme_panel;

    Action<int, int> BuyDiamonds;
    Action<int, int> BuyCoins;
    Action<int, int> BuyHealthPotions;


    public Text coins_Count_Text;
    public Text diamonds_Count_Text;
    public Text healthPotion_Count_Text;
    public Text warning_text;

    [Header("Dialog box Panel")]
    public Button confirm_button;
    public Button cancel_button;

    private int coinsToBuy;
    private int diamondsToBuy;
    private int healthToBuy;
    private int price;

    private void Awake()
    {

        if (INSTANCE == null)
            INSTANCE = this;
        else
        {
            Destroy(this);
            INSTANCE = this;
        }
        coins_Count_Text.text = PlayerPrefs.GetInt(Utils.COINS_PLAYER_PREF, 0).ToString();
        diamonds_Count_Text.text = PlayerPrefs.GetInt(Utils.DIAMONDS_PLAYER_PREF, 0).ToString();
        healthPotion_Count_Text.text = PlayerPrefs.GetInt(Utils.HEALTH_POTION_PLAYER_PREF, 0).ToString();
        //UIPanelManager(bottom_panel.name);
        OnClickCancel(false);
        if (PlayerPrefs.GetString(Utils.IS_GAME_STARTED_PLAYER_PREF, Utils.FALSE) == "false")
        {
            PlayerPrefs.SetString(Utils.IS_GAME_STARTED_PLAYER_PREF, Utils.TRUE);
            UIPanelManager(readme_panel.name);
        }
        else
        {
            UIPanelManager(bottom_panel.name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Methods
    public void UIPanelManager(string panelName)
    {
        shop_panel.SetActive(panelName.Equals(shop_panel.name));
        challenges_panel.SetActive(panelName.Equals(challenges_panel.name));
        bottom_panel.SetActive(panelName.Equals(bottom_panel.name));
        dialogbox_panel.SetActive(panelName.Equals(dialogbox_panel.name));
        readme_panel.SetActive(panelName.Equals(readme_panel.name));
    }

    public void OnClickCancel(bool active)
    {
        dialogbox_panel.SetActive(active);
        BuyCoins = null;
        BuyDiamonds = null;
    }

    public void SetDiamonds(int diamondsCount, int price)
    {
        diamondsToBuy = diamondsCount;
        this.price = price;
        BuyDiamonds += AddDiamonds;
        dialogbox_panel.SetActive(true);
    }

    public void SetCoins(int coinsCount, int price)
    {
        coinsToBuy = coinsCount;
        this.price = price;
        BuyCoins += AddCoins;
        dialogbox_panel.SetActive(true);
    }

    public void SethealthPotions(int healthPotions, int price)
    {
        healthToBuy = healthPotions;
        this.price = price;
        BuyHealthPotions += AddhealthPotions;
        dialogbox_panel.SetActive(true);
    }

    public void OnClickConfirm()
    {
        if (BuyCoins != null)
        {
            BuyCoins?.Invoke(coinsToBuy, price);
        }
        if (BuyDiamonds != null)
        {
            BuyDiamonds?.Invoke(diamondsToBuy, price);
        }
        if (BuyHealthPotions != null)
        {
            BuyHealthPotions?.Invoke(healthToBuy, price);
        }
        ResetAllDelegates();
        UIPanelManager(shop_panel.name);
    }

    public void CloseWarningBox()
    {
        warningbox_panel.SetActive(false);
        UIPanelManager(shop_panel.name);
    }

    public void ExitApplication()
    {
        float remainingTime = (DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(Utils.START_TIME))).Seconds;
        PlayerPrefs.SetFloat(Utils.CHALLENGE_REMAINING_TIME, remainingTime);
        Application.Quit();
    }
    #endregion

    #region Private Methods
    private void AddDiamonds(int diamondsToBuy, int price)
    {
        diamonds_Count_Text.text = (int.Parse(diamonds_Count_Text.text) + diamondsToBuy).ToString();
        PlayerPrefs.SetInt(Utils.DIAMONDS_PLAYER_PREF, int.Parse(diamonds_Count_Text.text));
    }

    private void AddCoins(int coins, int price)
    {
        if (price > PlayerPrefs.GetInt(Utils.DIAMONDS_PLAYER_PREF, 0))
        {
            warning_text.text = "No sufficient diamonds to buy coins. Please add diamonds.";
            warningbox_panel.SetActive(true);
            return;
        }
        coins_Count_Text.text = (int.Parse(coins_Count_Text.text) + coins).ToString();
        PlayerPrefs.SetInt(Utils.COINS_PLAYER_PREF, int.Parse(coins_Count_Text.text));
        diamonds_Count_Text.text = (int.Parse(diamonds_Count_Text.text) - price).ToString();
        PlayerPrefs.SetInt(Utils.DIAMONDS_PLAYER_PREF, int.Parse(diamonds_Count_Text.text));
    }

    public void AddCoinsOnChallenge(int coins)
    {
        coins_Count_Text.text = (int.Parse(coins_Count_Text.text) + coins).ToString();
        PlayerPrefs.SetInt(Utils.COINS_PLAYER_PREF, int.Parse(coins_Count_Text.text));
    }

    private void AddhealthPotions(int healthpotions, int price)
    {
        if (price > PlayerPrefs.GetInt(Utils.COINS_PLAYER_PREF, 0))
        {
            warning_text.text = "No sufficient coins to buy health potions. Please add coins";
            warningbox_panel.SetActive(true);
            return;
        }
        healthPotion_Count_Text.text = (int.Parse(healthPotion_Count_Text.text) + healthToBuy).ToString();
        PlayerPrefs.SetInt(Utils.HEALTH_POTION_PLAYER_PREF, int.Parse(healthPotion_Count_Text.text));
        coins_Count_Text.text = (int.Parse(coins_Count_Text.text) - price).ToString();
        PlayerPrefs.SetInt(Utils.COINS_PLAYER_PREF, int.Parse(coins_Count_Text.text));
    }

    private void ResetAllDelegates()
    {
        BuyCoins = null;
        BuyDiamonds = null;
        BuyHealthPotions = null;
    }
    #endregion
}
