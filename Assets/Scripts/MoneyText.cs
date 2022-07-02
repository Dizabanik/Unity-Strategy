using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    private Color _startColor;
    private Text _text;
    
    private void Start()
    {
        _text = GetComponent<Text>();
        _startColor = _text.color;
    }

    public void NotEnough()
    {

    }
}
