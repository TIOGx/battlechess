using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour // 게임에 사용된 UI들을 관리하는 매니저
{
    public static UIManager instance;

    [SerializeField]
    private GameObject ChoosePieceCanvas;
    [SerializeField]
    private GameObject SelectPosCanvas;
    [SerializeField]
    private GameObject LackMoneyCanvas;
    [SerializeField]
    private GameObject ChooseClassCanvas;
    [SerializeField]
    private GameObject ButtonPanel;
    [SerializeField]
    private GameObject StatusPanel;
    [SerializeField]
    private GameObject StartGamePanel;
    [SerializeField]
    private GameObject EndGamePanel;
    [SerializeField]
    private GameObject TabPanel;

    [SerializeField]
    private Text teamcolorCanvas;
    [SerializeField]
    private GameObject MoveButton;
    [SerializeField]
    private GameObject BuildButton;
    [SerializeField]
    private GameObject AttackButton;
    [SerializeField]
    private GameObject NextTurnButton;
    [SerializeField]
    private Text blackteamcostText;
    [SerializeField]
    private Text whiteteamcostText;
    [SerializeField]
    private Text winTeamText;

    [SerializeField]
    private int blackteamcost;
    [SerializeField]
    private int whiteteamcost;

    [SerializeField]
    private GameObject StatusPieceCanvas;
    [SerializeField]
    private GameObject[] DirButton;


    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private GameObject TabPrefab;
    [SerializeField]
    private Sprite[] PieceImage;


    public void ResetTabUI()
    {
        Debug.Log("resetui 실행");
        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            if (scrollRect.content.GetChild(i) == null) { continue; }
            Debug.Log("content안에 들어잇는것" + scrollRect.content.GetChild(i));
            Destroy(scrollRect.content.GetChild(i).gameObject);
        }
       
    }
    //Tab UI 갱신
    public void SetTabUI()
    {
        //scrollRect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        int i = 0;
        
        if (GameManager.instance.GetPlayer()) // blackteampiece
        {
            foreach (GameObject el in GameManager.instance.BlackTeamPiece)
            {       
                if (el == null){continue;}
                else
                {
                    GameObject NewPanel = Instantiate(TabPrefab) as GameObject;
                    NewPanel.name = "Panel"+ i.ToString();
                    NewPanel.transform.SetParent( scrollRect.content);

                    NewPanel.transform.Find("PieceImage").GetComponent<Image>().sprite = PieceImage[(int)el.GetComponent<PieceController>().Piecetype + 6];
                    NewPanel.transform.Find("MaxHp").GetComponent<Text>().text = " / " + el.GetComponent<PieceController>().Hp.ToString();
                    NewPanel.transform.Find("NowHp").GetComponent<Text>().text = el.GetComponent<PieceController>().nowHp.ToString();
                    NewPanel.transform.Find("Type").GetComponent<Text>().text = el.GetComponent<PieceController>().TypeName.ToString();
                    NewPanel.transform.Find("Class").GetComponent<Text>().text = el.GetComponent<PieceController>().MyEClass.ToString();
                    NewPanel.transform.Find("Power").GetComponent<Text>().text = el.GetComponent<PieceController>().OffensePower.ToString();

                    i += 1;
                }
            }
        }
        else // whiteteampiece
        {
            foreach (GameObject el in GameManager.instance.WhiteTeamPiece)
            {
                if (el == null) { continue; }
                else
                {
                    GameObject NewPanel = Instantiate(TabPrefab) as GameObject;
                    NewPanel.name = "Panel" + i.ToString();
                    NewPanel.transform.SetParent(scrollRect.content);

                    NewPanel.transform.Find("PieceImage").GetComponent<Image>().sprite = PieceImage[(int)el.GetComponent<PieceController>().Piecetype];
                    NewPanel.transform.Find("MaxHp").GetComponent<Text>().text = " / " + el.GetComponent<PieceController>().Hp.ToString();
                    NewPanel.transform.Find("NowHp").GetComponent<Text>().text = el.GetComponent<PieceController>().nowHp.ToString();
                    NewPanel.transform.Find("Type").GetComponent<Text>().text = el.GetComponent<PieceController>().TypeName.ToString();
                    NewPanel.transform.Find("Class").GetComponent<Text>().text = el.GetComponent<PieceController>().MyEClass.ToString();
                    NewPanel.transform.Find("Power").GetComponent<Text>().text = el.GetComponent<PieceController>().OffensePower.ToString();

                    i += 1;
                }
            }
        }
    }

    //Canvas SetActive를 관리하는 함수
    public void ActiveFalse(GameObject gameobject)
    {
        gameobject.SetActive(false);
    }
    public void ActiveTrue(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }


    // ButtonPanel
    public void ButtonPanelFalse()
    {
        ButtonPanel.SetActive(false);
    }
    public void ButtonPanelTrue()
    {
        ButtonPanel.SetActive(true);
    }
    // StartGamePanel
    public void StartGamePanelFalse()
    {
        StartGamePanel.SetActive(false);
    }
    public void StartGamePanelTrue()
    {
        StartGamePanel.SetActive(true);
    }
    // EndGamePanel
    public void EndGamePanelFalse()
    {
        EndGamePanel.SetActive(false);
    }
    public void EndGamePanelTrue()
    {
        EndGamePanel.SetActive(true);
    }

    //StatusPanel
    public void StatusPanelFalse()
    {
        StatusPanel.SetActive(false);
    }
    public void StatusPanelTrue()
    {
        StatusPanel.SetActive(true);
    }
    //TabPanel
    public void TabPanelFalse()
    {
        TabPanel.SetActive(false);
    }
    public void TabPanelTrue()
    {
        TabPanel.SetActive(true);
    }
    public bool GetTabPanelStatus()
    {
        if (TabPanel.activeSelf == true) { return true; }
        else return false;
    }


    // ChoosePieceCanvas
    public void ChooseCanvasFalse()
    {
        ChoosePieceCanvas.SetActive(false);
    }
    public void ChooseCanvasTrue()
    {
        ChoosePieceCanvas.SetActive(true);
    }

    //SelectPosCanvas
    public void SelectCanvasFalse()
    {
        SelectPosCanvas.SetActive(false);
    }
    public void SelectCanvasTrue()
    {
        SelectPosCanvas.SetActive(true);
    }
    public GameObject GetSelectCanvas()
    {
        return SelectPosCanvas;
    }
     
    //LackMoneyCanvas
    public void LackMoneyCanvasFalse()
    {
        LackMoneyCanvas.SetActive(false);
    }
    public void LackMoneyCanvasTrue()
    {
        LackMoneyCanvas.SetActive(true);
    }

    public GameObject GetLackMoneyCanvas()
    {
        return LackMoneyCanvas;
    }
    //ChooseClassCanvas
    public void ChooseClassCanvasFalse()
    {
        ChooseClassCanvas.SetActive(false);
    }
    public void ChooseClassCanvasTrue()
    {
        ChooseClassCanvas.SetActive(true);
    }

    //teamcolorCanvas
    public Text GeteamcolorCanvas()
    {
        return teamcolorCanvas;

    }
    //teamcolorCanvas
    public void SetblackteamcostCanvas()
    {
        blackteamcostText.text = blackteamcost.ToString();

    }
    public void SetwhiteteamcostCanvas()
    {
        whiteteamcostText.text = whiteteamcost.ToString();
    }
    public void BlackTeamInterestSystem()
    {
        int interest = blackteamcost / 10;
        if (interest >= 5)
        {
            interest = 5;
        }
        blackteamcost = blackteamcost + 5 + interest;
    }
    public void WhiteTeamInterestSystem()
    {
        int interest = whiteteamcost / 10;
        if (interest >= 5)
        {
            interest = 5;
        }
        whiteteamcost = whiteteamcost + 5 + interest;
    }
    // 재용
    public void OnStatusUI()
    {
        StatusPieceCanvas.SetActive(true);
    }

    // button
    public void DirButtonFalse()
    {
        for (int i = 0; i < 4; i++) DirButton[i].GetComponent<Button>().interactable = false;
    }
    public void DirButtonTrue()
    {
        for (int i = 0; i < 4; i++) DirButton[i].GetComponent<Button>().interactable = true;
    }

    public void DeactivateButton()
    {
        DirButtonFalse();
        MoveButton.GetComponent<Button>().interactable = false;
        BuildButton.GetComponent<Button>().interactable = false;
        AttackButton.GetComponent<Button>().interactable = false;
        NextTurnButton.GetComponent<Button>().interactable = false;

    }
    public void ActivateButton()
    {
        MoveButton.GetComponent<Button>().interactable = true;
        BuildButton.GetComponent<Button>().interactable = true;
        AttackButton.GetComponent<Button>().interactable = true;
        NextTurnButton.GetComponent<Button>().interactable = true;

    }

    // cost
    public void SetBlackTeamCost(int cost)
    {
        blackteamcost = cost;
    }
    public int GetBlackTeamCost()
    {
        return blackteamcost;
    }

    public void SetWhiteTeamCost(int cost)
    {
        whiteteamcost = cost;
    }
    public int GetWhiteTeamCost()
    {
        return whiteteamcost;
    }

    public Text GetwinTeamText()
    {
        return winTeamText;
    }

    public void InitializeUI()
    {
        DirButtonFalse();
        ChooseCanvasFalse();
        SelectCanvasFalse();
        ChooseClassCanvasFalse();
        ButtonPanelTrue();
    }


    public void Start()
    {
        instance = this;
        ChoosePieceCanvas.SetActive(false);
        SelectPosCanvas.SetActive(false);
        blackteamcost = 0;
        whiteteamcost = 0;
    }

}