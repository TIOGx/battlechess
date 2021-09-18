using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public enum EPlayerWho
{
    Black = 0,
    White = 1
}
public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager instance;

    [SerializeField]
    private GameObject[,] Board;
    [SerializeField]
    private GameObject[,] Tiles;
    public EPlayerWho Player; // id�� ���� ������
    [SerializeField]
    private GameObject enemy;
    public EPlayerWho NowPlayer; // �� ����

    public Queue<GameObject> BlackTeamPiece; // ���� ��ȯ�� ����
    public Queue<GameObject> WhiteTeamPiece;


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
    // �̵��� ��, ������ �� Tile animation�� �̵� �Ұ����·� �ʱ�ȭ - ���
    // ����� SelectTile()�� �ʱ�ȭ���ְ� ����
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
    // �� �� �ִ� Ÿ�ϸ� Movable�� true�� �ΰ� animation Ȱ��ȭ - ���
    public void HighlightTile(GameObject GObject) // �� �� �ִ� Ÿ�� highlight
    {
        GObject.GetComponent<Tile>().SetMovable(true);
        for (int i = 0; i < GObject.transform.childCount; i++)
        {
            GObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        GObject.GetComponent<Animator>().Play("TileAnim");
    }

    // ���� ������ ��� �Լ� - ȿ��
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
    // ���� �÷��̾�� ����? - ȿ��
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
    // �Ͽ� �ش��ϴ� �÷��̾ �̵��� ������ �ʱ�ȭ - ȿ��
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
    // �� RPC ȿ��
    public void NextTurn()
    {
        InitializeTile();
        Debug.Log("�̹��� " + NowPlayer);
        photonView.RPC("NextTurnRPC", RpcTarget.AllViaServer, NowPlayer);
    }
    // �� ���� ������ ��� �Լ� - ȿ��
    [PunRPC]
    private void NextTurnRPC(EPlayerWho ThisTurn)
    {
        // CameraManager.instance.InitailizeCamera();
        // GameManager.instance.InitializeTile();
        // GameManager.instance.InitializeActive();
        // Debug.Log("������ ������ ����");
        // Debug.Log(Player);
        if (NowPlayer == Player) // �ڽ� �Ͽ� �����Ḧ ������ ��
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
        { // �ڽ� ���� �ƴѵ� �����ᰡ ���� ���
            // ��ư Ȱ��ȭ
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
