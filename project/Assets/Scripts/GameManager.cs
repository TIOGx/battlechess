using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   

    public static GameManager instance;

    [SerializeField]
    private GameObject[,] Board;
    [SerializeField]
    private GameObject[,] Tiles;
    [SerializeField]
    private EPlayerWho Player;
    [SerializeField]
    private GameObject enemy;


    public Queue<GameObject> BlackTeamPiece; // 내가 소환한 말들
    public Queue<GameObject> WhiteTeamPiece;

    public enum EPlayerWho
    {
        Black,
        White
    }

    public void SetBoard(GameObject GObject, int idxX, int idxY)
    {
        Board[idxX, idxY] = GObject;
    }
    public GameObject GetBoard(int idxX, int idxY)
    {
        return Board[idxX, idxY];
    }

    public void SetTile(GameObject GObject, int idxX, int idxY)
    {
        Tiles[idxX, idxY] = GObject;
    }
    public GameObject GetTile(int idxX, int idxY)
    {
        return Tiles[idxX, idxY];
    }


    private void Awake() {
        instance = this;
        BlackTeamPiece = new Queue<GameObject>();
        WhiteTeamPiece = new Queue<GameObject>();
        Board = new GameObject[8, 8];
        Tiles = new GameObject[8, 8];
    }
    private void Start(){
        Player = EPlayerWho.Black;
        UIManager.instance.SetBlackTeamCost(0);
        UIManager.instance.SetWhiteTeamCost(0);
        UIManager.instance.SetblackteamcostCanvas();
        UIManager.instance.SetwhiteteamcostCanvas();
    }

    public void InitializeTile()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject GObject = Tiles[i, j];
                Tiles[i,j].GetComponent<Tile>().SetMovable(false);
                for (int k = 0; k < 4; k++)
                {
                    GObject.transform.GetChild(k).gameObject.SetActive(false);
                    
                }
            }
        }
        BuildManager.instance.InitializeSelectTile();
    }

    public void HighlightTile(GameObject GObject) // 갈 수 있는 타일 highlight
    {
        GObject.GetComponent<Tile>().SetMovable(true);
        for(int i = 0; i < GObject.transform.childCount; i++)
        {
            GObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        GObject.GetComponent<Animator>().Play("TileAnim");
    }

    // 전투 페이즈 담당 함수    
    private void BattlePhase(){
        if (Player == EPlayerWho.Black)
        {
            foreach (GameObject el in BlackTeamPiece)
            {
                if (el != null)
                {
                    el.GetComponent<PieceController>().Attack();
                }
            }
        }
        else if (Player == EPlayerWho.White)
        {
            foreach (GameObject el in WhiteTeamPiece)
            {
                if (el != null)
                {
                    el.GetComponent<PieceController>().Attack();
                }
            }

        }
    }
    public bool GetPlayer()
    {
        if(Player == EPlayerWho.Black)
        {
            return true;
        } else
        {
            return false;
        }
    }
    private void InitializeActive()
    {
        foreach(GameObject el in BlackTeamPiece)
        {
            if (el != null)
            {
                el.GetComponent<PieceController>().SetAttackable(false);
                el.GetComponent<PieceController>().SetMovable(false);

            }
        }
        foreach (GameObject el in WhiteTeamPiece)
        {
            if (el != null)
            {
                el.GetComponent<PieceController>().SetAttackable(false);
                el.GetComponent<PieceController>().SetMovable(false);

            }
        }
    }

    // 턴 종료 페이즈 담당 함수 
    private void NextTurn(){
        CameraManager.instance.InitailizeCamera();
        GameManager.instance.InitializeTile();
        GameManager.instance.InitializeActive();
        Debug.Log(BlackTeamPiece);
        Debug.Log(WhiteTeamPiece);
        Debug.Log("턴종료 다음턴 시작");
        Debug.Log(Player);
        if (Player == EPlayerWho.Black)
        {
            Player = EPlayerWho.White;
            CameraManager.instance.WhiteTeamCameraOn();
            foreach (GameObject el in WhiteTeamPiece)
            {
                if (el != null)
                {
                    el.GetComponent<PieceController>().SetAttackable(true);
                    el.GetComponent<PieceController>().SetMovable(true);

                }
            }
            UIManager.instance.GeteamcolorCanvas().text = "White Turn";
            UIManager.instance.WhiteTeamInterestSystem();
            UIManager.instance.SetwhiteteamcostCanvas();
        }

        else if (Player == EPlayerWho.White)
        {
            
            Player = EPlayerWho.Black;
            CameraManager.instance.BlackTeamCameraOn();
            foreach (GameObject el in BlackTeamPiece)
            {
                if (el != null)
                {
                    el.GetComponent<PieceController>().SetAttackable(true);
                    el.GetComponent<PieceController>().SetMovable(true);
                }
            }
            UIManager.instance.GeteamcolorCanvas().text = "Black Turn";
            UIManager.instance.BlackTeamInterestSystem();
            UIManager.instance.SetblackteamcostCanvas();
        }
    }

}
