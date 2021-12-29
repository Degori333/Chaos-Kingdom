using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int year = 0;
    public int decisionsPerYear = 5;
    private int decisionsLeft;
    private TextMeshProUGUI yearText;
    private TextMeshProUGUI decisionText;

    private bool spawnSpecialCard = false;
    private int specialCardIndex;
    public bool canSpawnCard = false;
    public GameObject cardSpawnPoint;
    public GameObject[] cards;
    public GameObject[] specialCards;
    private int lastCardIndex;

    private static float growthStart = 1.75f;
    private static float subStart = 0.4f;
    private static float growthCap = 4.0f;
    private static float subCap = 1.0f;

    private bool[] lockValue = new bool[4];
    private Slider growthMarker;
    private Slider orderMarker;
    private Slider powerMarker;
    private Slider moneyMarker;
    private Slider entertainmentMarker;
    private float growthModifier = 0.3f;

    private float _growthRate = growthStart;
    public float GrowthRate
    {
        get
        {
            return _growthRate;
        }
        set
        {
            _growthRate = value;
            if(_growthRate >= growthCap)
            {
                _growthRate = growthCap;
            }
            else if(_growthRate <= 0 && !spawnSpecialCard)
            {
                _growthRate = 0;
                spawnSpecialCard = true;
                specialCardIndex = 0;
            }
            growthMarker.value = _growthRate;
        }
    }

    private float _orderValue = subStart;
    public float OrderValue
    {
        get
        {
            return _orderValue;
        }
        set
        {
            _orderValue = value;
            if (_orderValue >= subCap && !spawnSpecialCard)
            {
                _orderValue = subCap;
                spawnSpecialCard = true;
                specialCardIndex = 1;
                orderMarker.value = _orderValue;
                lockValue[0] = true;
            }
            else if (_orderValue <= 0 && !spawnSpecialCard)
            {
                _orderValue = 0;
                spawnSpecialCard = true;
                specialCardIndex = 0;
            }
            if (!lockValue[0])
            {
            orderMarker.value = _orderValue;
            }
        }
    }

    private float _powerValue = subStart;
    public float PowerValue
    {
        get
        {
            return _powerValue;
        }
        set
        {
            _powerValue = value;
            if (_powerValue >= subCap && !spawnSpecialCard)
            {
                _powerValue = subCap;
                spawnSpecialCard = true;
                specialCardIndex = 1;
                powerMarker.value = _powerValue;
                lockValue[1] = true;
            }
            else if (_powerValue <= 0 && !spawnSpecialCard)
            {
                _powerValue = 0;
                spawnSpecialCard = true;
                specialCardIndex = 0;
            }
            if (!lockValue[1])
            {
                powerMarker.value = _powerValue;
            }
        }
    }

    private float _moneyValue = subStart;
    public float MoneyValue
    {
        get
        {
            return _moneyValue;
        }
        set
        {
            _moneyValue = value;
            if (_moneyValue >= subCap && !spawnSpecialCard)
            {
                _moneyValue = subCap;
                spawnSpecialCard = true;
                specialCardIndex = 1;
                moneyMarker.value = _moneyValue;
                lockValue[2] = true;
            }
            else if (_moneyValue <= 0 && !spawnSpecialCard)
            {
                _moneyValue = 0;
                spawnSpecialCard = true;
                specialCardIndex = 0;
            }
            if (!lockValue[2])
            {
                moneyMarker.value = _moneyValue;
            }
        }
    }

    private float _entertainmentValue = subStart;
    public float EntertainmentValue
    {
        get
        {
            return _entertainmentValue;
        }
        set
        {
            _entertainmentValue = value;
            if (_entertainmentValue >= subCap && !spawnSpecialCard)
            {
                _entertainmentValue = subCap;
                spawnSpecialCard = true;
                specialCardIndex = 1;
                entertainmentMarker.value = _entertainmentValue;
                lockValue[3] = true;
            }
            else if (_entertainmentValue <= 0 && !spawnSpecialCard)
            {
                _entertainmentValue = 0;
                spawnSpecialCard = true;
                specialCardIndex = 0;
            }
            if (!lockValue[3])
            {
                entertainmentMarker.value = _entertainmentValue;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {     
        yearText = GameObject.Find("Year").GetComponent<TextMeshProUGUI>();
        yearText.text = "Year: " + year;
        decisionsLeft = decisionsPerYear;
        decisionText = GameObject.Find("Decisions").GetComponent<TextMeshProUGUI>();
        decisionText.text = "Actions Left: " + decisionsLeft;
        growthMarker = GameObject.Find("Population").GetComponent<Slider>();
        orderMarker = GameObject.Find("Order").GetComponent<Slider>();
        powerMarker = GameObject.Find("Power").GetComponent<Slider>();
        moneyMarker = GameObject.Find("Money").GetComponent<Slider>();
        entertainmentMarker = GameObject.Find("Enjoyment").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Test();
        SpawnCard();
    }

    public void DecisionCounter()
    {
        if(decisionsLeft <= 1)
        {
            year++;
            if(year > 30)
            {
                specialCardIndex = 0;
                spawnSpecialCard = true;
            }
            yearText.text = "Year: " + year;
            decisionsLeft = decisionsPerYear;
            decisionText.text = "Actions Left: " + decisionsLeft;
            growthModifier = (OrderValue + PowerValue + MoneyValue + EntertainmentValue) / 4;
            GrowthRate += growthModifier;
            if (GrowthRate > 3.2 || GrowthRate <= 1.5f)
            {
                OrderValue = YearlyGrowth(OrderValue);
                PowerValue = YearlyGrowth(PowerValue);
                MoneyValue = YearlyGrowth(MoneyValue);
                EntertainmentValue = YearlyGrowth(EntertainmentValue);
            }
            else if (GrowthRate > 2.4)
            {
                OrderValue += YearlyGrowth(OrderValue) * 0.1f;
                PowerValue += YearlyGrowth(PowerValue) * 0.1f;
                MoneyValue += YearlyGrowth(MoneyValue) * 0.1f;
                EntertainmentValue += YearlyGrowth(EntertainmentValue) * 0.1f;
            }
            else if (GrowthRate > 1.5f)
            {
            
                OrderValue -= YearlyGrowth(OrderValue) * 0.1f;
                PowerValue -= YearlyGrowth(PowerValue) * 0.1f;
                MoneyValue -= YearlyGrowth(MoneyValue) * 0.1f;
                EntertainmentValue -= YearlyGrowth(EntertainmentValue) * 0.1f;
            }
        }
        else
        {
            decisionsLeft--;
            GrowthRate += 0.05f;
            decisionText.text = "Actions Left: " + decisionsLeft;
        }
    }

    private float LogisticMapProduct(float value)
    {
        return GrowthRate * value * (1 - value);
    }

    private float YearlyGrowth(float value)
    {
        if (GrowthRate > 3.5f) 
        {
            for (int i = 0; i <= Random.Range(1, 11); i++)
            {
                value = LogisticMapProduct(value);
            }
        }
        else if (GrowthRate > 2.0f)
        {
            for (int i = 0; i <= Random.Range(1, 4); i++)
            {
                value = LogisticMapProduct(value);
            }
        }
        else
        {
            value = LogisticMapProduct(value);
        }

        float maxValueAllowed = 0.86f;
        float minValueAllowed = 0.06f;
        if(value > maxValueAllowed)
        {
            value = maxValueAllowed;
        }
        else if (value < minValueAllowed)
        {
            value = minValueAllowed;
        }
        return value;
        
    }

    private void SpawnCard()
    {
        if (canSpawnCard)
        {
            int index = Random.Range(0, cards.Length);
            if (!spawnSpecialCard && lastCardIndex != index)
            {
                lastCardIndex = index;
                canSpawnCard = false;
                Instantiate(cards[index], cardSpawnPoint.transform);
            }
            else if(spawnSpecialCard)
            {
                SpawnSpecialCard();
            }
        }
    }

    private void SpawnSpecialCard()
    {
        int highscore;
        highscore = PlayerPrefs.GetInt("Highscore", 30);
        if (specialCardIndex == 1 && highscore > year)
        {
            PlayerPrefs.SetInt("Highscore", year);
        }
        spawnSpecialCard = false;
        canSpawnCard = false;
        Instantiate(specialCards[specialCardIndex], cardSpawnPoint.transform);
    }

    /*
    private float testValue = 0.36f;

    private void Test() 
    {
        GrowthRate = 1.6f;
        testValue -= LogisticMapProduct(testValue) * 0.1f;
        Debug.Log(testValue);
    }
    */
}
