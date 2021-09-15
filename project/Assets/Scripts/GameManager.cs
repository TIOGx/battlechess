using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager instance;

    [SerializeField]
    private GameObject[,] Board;
    [SerializeField]
    private GameObject[,] Tiles;
    public EPlayerWho Player; // id에 따라 결정댐
    [SerializeField]
    private GameObject enemy;
    public EPlayerWho NowPlayer; // 턴 결정

    public Queue<GameObject> BlackTeamPiece; // 내가 소환한 말들
    public Queue<GameObject> WhiteTeamPiece;

    public enum EPlayerWho
    {
        Black = 0,
        White = 1
    }
    public void SetBoard(GameObject GObject, int idxX, int idxY)
    {
        if (GObject != null)
        {
            photonView.RPC("SetBoardRPC", RpcTarget.AllViaServer, GObject.GetComponent<PhotonView>().ViewID, idxX, idxY);
        }
        else photonView.RPC("SetBoardRPC", RpcTarget.AllViaServer, -1, idxX, idxY);

    }
    [PunRPC]
    public void SetBoardRPC(int ID, int idxX, int idxY)
    {
        if (ID == -1) Board[idxX, idxY] = null;
        else Board[idxX, idxY] = PhotonView.Find(ID).gameObject;
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


    private void Awake()
    {
        instance = this;
        BlackTeamPiece = new Queue<GameObject>();
        WhiteTeamPiece = new Queue<GameObject>();
        Board = new GameObject[8, 8];
        Tiles = new GameObject[8, 8];
    }
    private void Start()
    {
        UIManager.instance.SetBlackTeamCost(0);
        UIManager.instance.SetWhiteTeamCost(0);
        UIManager.instance.SetblackteamcostCanvas();
        UIManager.instance.SetwhiteteamcostCanvas();
        NowPlayer = EPlayerWho.Black;
    }
    /*
    public void InitializeTile()
    {
        photonView.RPC("InitializeTileRPC", RpcTarget.AllViaServer);
    }
    [PunRPC]
    */
    // 이동한 후, 생성한 후 Tile animation과 이동 불가상태로 초기화 - 재용
    // 현재는 SelectTile()도 초기화해주고 있음
    public void InitializeTile()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject GObject = Tiles[i, j];
                Tiles[i, j].GetComponent<Tile>().SetMovable(false);
                for (int k = 0; k < 4; k++)
                {
                    GObject.transform.GetChild(k).gameObject.SetActive(false);

                }
            }
        }
        BuildManager.instance.InitializeSelectTile();
    }
    // 갈 수 있는 타일만 Movable을 true로 두고 animation 활성화 - 재용
    public void HighlightTile(GameObject GObject) // 갈 수 있는 타일 highlight
    {
        GObject.GetComponent<Tile>().SetMovable(true);
        for (int i = 0; i < GObject.transform.childCount; i++)
        {
            GObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        GObject.GetComponent<Animator>().Play("TileAnim");
    }

    // 전투 페이즈 담당 함수 - 효원
    private void BattlePhase()
    {
        if (NowPlayer == EPlayerWho.Black)
        {
            foreach (GameObject el in BlackTeamPiece)
            {
                if (el != null)
                {
                    el.GetComponent<PieceController>().Attack();
                }
            }
        }
        else if (NowPlayer == EPlayerWho.White)
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
    // 현재 플레이어는 누구? - 효원
    public bool GetPlayer()
    {
        if (EPlayerWho.Black == NowPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // 턴에 해당하는 플레이어만 이동과 공격을 초기화 - 효원
    private void InitializeActive()
    {
        foreach (GameObject el in BlackTeamPiece)
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
    // 턴 RPC 효원
    public void NextTurn()
    {
        InitializeTile();
        Debug.Log("이번턴 " + NowPlayer);
        photonView.RPC("NextTurnRPC", RpcTarget.AllViaServer, NowPlayer);
    }
    // 턴 종료 페이즈 담당 함수 - 효원
    [PunRPC]
    private void NextTurnRPC(EPlayerWho ThisTurn)
    {
        // CameraManager.instance.InitailizeCamera();
        // GameManager.instance.InitializeTile();
        // GameManager.instance.InitializeActive();
        // Debug.Log("턴종료 다음턴 시작");
        // Debug.Log(Player);
        if (NowPlayer == Player) // 자신 턴에 턴종료를 눌렀을 때
        {
            if (NowPlayer == EPlayerWho.Black)
            {
                foreach (GameObject el in BlackTeamPiece)
                {
                    if (el != null)
                    {
                        el.GetComponent<PieceController>().SetAttackable(true);
                        el.GetComponent<PieceController>().SetMovable(true);
                    }
                }

            }

            else
            {
                foreach (GameObject el in WhiteTeamPiece)
                {
                    if (el != null)
                    {
                        el.GetComponent<PieceController>().SetAttackable(true);
                        el.GetComponent<PieceController>().SetMovable(true);
                    }
                }

            }
            UIManager.instance.DeactivateButton();
        }
        else
        { // 자신 턴이 아닌데 턴종료가 눌릴 경우
            // 버튼 활성화
            UIManager.instance.ActivateButton();
        }

        if (ThisTurn == EPlayerWho.Black)
        {
            UIManager.instance.GeteamcolorCanvas().text = "White Turn";
            UIManager.instance.WhiteTeamInterestSystem();
            NowPlayer = EPlayerWho.White;

        }
        else
        {
            UIManager.instance.GeteamcolorCanvas().text = "Black Turn";
            UIManager.instance.BlackTeamInterestSystem();
            NowPlayer = EPlayerWho.Black;
            UIManager.instance.SetblackteamcostCanvas();
        }

        if (Player == EPlayerWho.Black)
        {
            UIManager.instance.SetblackteamcostCanvas();
        }
        else
        {
            UIManager.instance.SetwhiteteamcostCanvas();
        }
    }

}
