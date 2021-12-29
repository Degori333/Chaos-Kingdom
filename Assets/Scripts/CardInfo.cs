using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    public string defaultText = "";
    public bool specialCard = false;
    private GameManager gameManager;
    public TextMeshProUGUI action { get; private set; }
    public TextMeshProUGUI description { get; private set; }
    public GameObject leftButton { get; private set; }
    public GameObject rightButton { get; private set; }

    public AudioClip cardHover;
    public AudioClip cardSelect;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leftButton = GameObject.Find("Left Option");
        rightButton = GameObject.Find("Right Option");
        action = GameObject.Find("CardActionText").GetComponent<TextMeshProUGUI>();
        description = GameObject.Find("Card Description").GetComponent<TextMeshProUGUI>();
        description.text = defaultText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AlterGrowth(float value)
    {
        gameManager.GrowthRate += value;
    }

    public void AlterOrder(float value)
    {
        gameManager.OrderValue += value;
    }
    public void AlterPower(float value)
    {
        gameManager.PowerValue += value;
    }

    public void AlterMoney(float value)
    {
        gameManager.MoneyValue += value;
    }

    public void AlterEntertainment(float value)
    {
        gameManager.EntertainmentValue += value;
    }
}
