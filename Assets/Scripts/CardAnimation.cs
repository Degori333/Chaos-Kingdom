using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Audio;

public class CardAnimation : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    private CardInfo card;
    private string outcomeText = "";
    public string specialText = "";
    public bool unknownOutcome = false;
    public string actionText = "";
    private GameManager gameManager;
    public bool randomize = false;
    private MenuHandler menu;

    private new AudioSource audio;

    public float growthChange = 0.0f;
    public float orderChange = 0.0f;
    public float powerChange = 0.0f;
    public float moneyChange = 0.0f;
    public float entertainmentChange = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("MenuHandler").GetComponent<MenuHandler>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audio = GetComponentInParent<AudioSource>();
        card = GetComponentInParent<CardInfo>();
        animator = GameObject.Find("CardArt").GetComponent<Animator>();
        GenerateOutcomeText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateOutcomeText()
    {
        outcomeText = "";
        if (unknownOutcome)
        {
            outcomeText = "<b>???</b>";
        }
        else if (card.specialCard)
        {
            outcomeText = specialText;
        }
        else
        {
            //Growth
            if (growthChange != 0)
            {
                outcomeText += "Population\n";
            }
            //Order
            if (orderChange != 0)
            {
                outcomeText += "Order\n";
            }
            //Power
            if (powerChange != 0)
            {
                outcomeText += "Power\n";
            }
            //Money
            if (moneyChange != 0)
            {
                outcomeText += "Money\n";
            }
            //Entertainment
            if (entertainmentChange != 0)
            {
                outcomeText += "Happiness\n";
            }
        }
    }

    private void AlterStats()
    {
        if (randomize)
        {
            RandomizeStats();
        }
        card.AlterGrowth(growthChange);
        card.AlterOrder(orderChange);
        card.AlterPower(powerChange);
        card.AlterMoney(moneyChange);
        card.AlterEntertainment(entertainmentChange);
    }

    private void RandomizeStats()
    {
        growthChange += Random.Range(-0.30f,0.30f);
        orderChange += Random.Range(-0.15f, 0.15f);
        powerChange += Random.Range(-0.15f, 0.15f);
        moneyChange += Random.Range(-0.15f, 0.15f);
        entertainmentChange += Random.Range(-0.15f, 0.15f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (name == "Left Option")
        {
            animator.SetBool("TiltLeft", true);
        }
        else if (name == "Right Option")
        {
            this.animator.SetBool("TiltRight", true);
        }
        audio.PlayOneShot(card.cardHover);
        card.action.text = actionText;
        card.description.text = outcomeText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("TiltLeft", false);
        animator.SetBool("TiltRight", false);
        card.action.text = "";
        card.description.text = card.defaultText;
    }


    private IEnumerator WaitForAnimation()
    {

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length ); //+ animator.GetCurrentAnimatorStateInfo(0).normalizedTime
        gameManager.canSpawnCard = true;
        Destroy(card.gameObject);
    }

    public void ChooseThis()
    {
        card.description.text = "";
        card.action.text = "";
        AlterStats();
        if (!card.specialCard) 
        {
            gameManager.DecisionCounter();
        }
        card.leftButton.GetComponent<Button>().interactable = false;
        card.rightButton.GetComponent<Button>().interactable = false;
        card.leftButton.GetComponent<Image>().raycastTarget = false;
        card.rightButton.GetComponent<Image>().raycastTarget = false;
        animator.SetTrigger("Press");
        audio.PlayOneShot(card.cardSelect);
        StartCoroutine(WaitForAnimation());
        
    }

    public void SceneChange(int index)
    {
        menu.SwitchScene(index);
    }

}
