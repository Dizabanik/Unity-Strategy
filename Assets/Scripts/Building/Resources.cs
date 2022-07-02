using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    private int _money;
    public int StartMoneyBonus = 50;
    public Text MoneyText;

    public int Money
    {
        get
        {
            return _money;
        }

        set
        {
            _money = value;
            MoneyText.text = Money.ToString();
        }
    }
    
    private void Start()
    {
        _money += StartMoneyBonus;
        MoneyText.text = Money.ToString();
    }
}
