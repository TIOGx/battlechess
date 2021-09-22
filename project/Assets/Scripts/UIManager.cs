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
    private GameObject ChooseClassCanvas;
    [SerializeField]
    private GameObject ButtonPanel;
    [SerializeField]
    private GameObject StatusPanel;
    [SerializeField]
    private GameObject StartGamePanel;

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
    private int blackteamcost;
    [SerializeField]
    private int whiteteamcost;
    [SerializeField]
    private GameObject StatusPieceCanvas;

    [SerializeField]
    private GameObject[] DirButton;

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

    public void ButtonPanelFalse()
    {
        ButtonPanel.SetActive(false);
    }
    public void ButtonPanelTrue()
    {
        ButtonPanel.SetActive(true);
    }

    public void StartGamePanelFalse()
    {
        StartGamePanel.SetActive(false);
    }
    public void StartGamePanelTrue()
    {
        StartGamePanel.SetActive(true);
    }
    public void DirButtonFalse()
    {
        for(int i=0;i<4;i++) DirButton[i].GetComponent<Button>().interactable = false;
    }
    public void DirButtonTrue()
    {
        for(int i=0;i<4;i++) DirButton[i].GetComponent<Button>().interactable = true;
    }

    public void StatusPanelFalse()
    {
        StatusPanel.SetActive(false);
    }
    public void StatusPanelTrue()
    {
        StatusPanel.SetActive(true);
    }

    public void InitializeUI()
    {
        DirButtonFalse();
        ChooseCanvasFalse();
        SelectCanvasFalse();
        ChooseClassCanvasFalse();
        ButtonPanelTrue();
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
        blackteamcostText.text = blackteamcost.ToString() + " Gold";

    }
    public void SetwhiteteamcostCanvas()
    {
        whiteteamcostText.text = whiteteamcost.ToString() + " Gold";
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
    public void Start()
    {
        instance = this;
        ChoosePieceCanvas.SetActive(false);
        SelectPosCanvas.SetActive(false);
        blackteamcost = 0;
        whiteteamcost = 0;
    }
}