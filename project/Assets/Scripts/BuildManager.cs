using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public enum EClassType : int
{
    Warrior=0,
    Wizard=1,
    Archer=2,
    None
}
public enum EPieceType : int
{
    Bishop = 0,
    Knight = 1,
    Pawn = 2,
    Queen = 3,
    Rook = 4,
    King = 5,
    None
}

public class BuildManager : MonoBehaviourPunCallbacks // 체스 말, 기물 종류를 받아서 builded 된 piece를 보드에 넣어주는 역할
{
    public static BuildManager instance;
    [SerializeField]
    private GameObject[] Piece;
    [SerializeField]
    private GameObject SelectTile;
    [SerializeField]
    private GameObject BuildedPiece;
    [SerializeField]
    private EClassType ClassType;
    private EPieceType PieceType;
    private int[] ClassCost = {0, 5, 2}; // 각 Class 에 따른 비용 표시
    
    private int[] PieceTypeCost = {3, 3, 1, 9, 4,0}; // 각 Type 에 따른 비용 표시
    // 상태 확인 코루틴 - 구지
    private IEnumerator SetActiveFalseTime(GameObject gameObject, float WaitSeconds)
    {
        yield return new WaitForSeconds(WaitSeconds);
        Debug.Log("사라져!");
        gameObject.SetActive(false);
    }

    private void Start(){
        ClassType = EClassType.None;
        PieceType = EPieceType.None;
        instance = this;

    }

    public void SetClassType(int Ict) //upcast 
    {
        ClassType = (EClassType)Ict;
        Debug.Log(ClassType);
    }
    public void SetPieceType(int Ict) //upcast 
    {
        PieceType = (EPieceType)Ict;
        Debug.Log(PieceType);
    }
   
    public void InitializeSelectTile()
    {
        SelectTile = null;
    }

    public void SetSelectTile(GameObject GObject)
    {
        SelectTile = GObject;
    }
    
    public GameObject GetSelectTile()
    {
        return SelectTile;
    }
    // 기물의 생성이 가능한가? - 효원
    public bool IsBuild(EPieceType PieceType, EClassType ClassType)
    {   
        int PieceCost = ClassCost[(int)ClassType] + PieceTypeCost[(int)PieceType];
        if (GameManager.instance.GetPlayer())//black player
        {
            if(PieceCost > UIManager.instance.GetBlackTeamCost()) // 돈이 모자라면
            {
                Debug.Log("돈 부족이용");
                GameManager.instance.InitializeTile();
                InitializeSelectTile();
                UIManager.instance.ChooseClassCanvasFalse();
                CameraManager.instance.InitializeCamera();
                return false;
            }
            else
            {
                UIManager.instance.SetBlackTeamCost(UIManager.instance.GetBlackTeamCost() - PieceCost);
                UIManager.instance.SetblackteamcostCanvas();
                return true;
            }
        }
        else
        {
            if(PieceCost > UIManager.instance.GetWhiteTeamCost()) // 돈이 모자라면
            {
                Debug.Log("돈 부족이용");
                GameManager.instance.InitializeTile();
                InitializeSelectTile();
                UIManager.instance.ChooseClassCanvasFalse();
                CameraManager.instance.InitializeCamera();
                return false; 
            }
            else
            {
                UIManager.instance.SetWhiteTeamCost(UIManager.instance.GetWhiteTeamCost() - PieceCost);
                UIManager.instance.SetwhiteteamcostCanvas();
                return true;
            }
        }

    }
    // Onclick에 넣기 위한 함수 최적화 - 재용,효원,구지
    public void BuildPiece()
    {
        // photonView.RPC("BuildPieceRPC",RpcTarget.AllViaServer, SelectTile.transform.position, PieceType, ClassType);
        BuildPieceRPC(SelectTile.transform.position, PieceType, ClassType);
        CameraManager.instance.InitializeCamera();
    }
    
    public void BuildPieceRPC(Vector3 selectpos, EPieceType PieceType, EClassType ClassType) 
    {   
        print("빌드피스까지 들어왔다고1!!");
        if(!IsBuild(PieceType, ClassType))
        {
            UIManager.instance.ChooseCanvasFalse();
            return;
        }

        if (GameManager.instance.GetPlayer() ) //black player
        {
            Debug.Log("블랙이고?");
            if (GameManager.instance.GetBoard((int)selectpos.x, (int)selectpos.z) == null)
            {
                Debug.Log("black instantiate");
                BuildedPiece = PhotonNetwork.Instantiate(Piece[(int)PieceType].name, selectpos, Quaternion.identity);
                BuildedPiece.GetComponent<PhotonView>().RPC("SetPropertyRPC", RpcTarget.All, (int)PieceType, (int)ClassType);
            }

            else
            {
                Debug.Log("이미 있는 자리에 또 만드려고 했어요");
                UIManager.instance.ChooseCanvasFalse();
                return;
            }
            GameManager.instance.SetBoard(BuildedPiece, (int)selectpos.x, (int)selectpos.z);
            GameManager.instance.InitializeTile();
            
            UIManager.instance.ChooseCanvasFalse();
        } 
        else
        {
            Debug.Log("화이트이고?");
            if ( GameManager.instance.GetBoard((int)selectpos.x, (int)selectpos.z) == null)
            {
                Debug.Log("white instantiate");
                BuildedPiece = PhotonNetwork.Instantiate(Piece[(int)PieceType + 5].name, selectpos, Quaternion.Euler(new Vector3(0, 180, 0)));
                BuildedPiece.GetComponent<PhotonView>().RPC("SetPropertyRPC", RpcTarget.All, (int)PieceType, (int)ClassType);
            }

            else
            {
                Debug.Log("이미 있는 자리에 또 만드려고 했어요");
                UIManager.instance.ChooseCanvasFalse();
                return;
            }
            GameManager.instance.SetBoard(BuildedPiece, (int)selectpos.x, (int)selectpos.z);
            GameManager.instance.InitializeTile();
            UIManager.instance.ChooseCanvasFalse();
        }              
    }

    // 기물종류 선택
    public void SelectPiece()
    {
        UIManager.instance.ChooseClassCanvasFalse();
        if (SelectTile == null)
        {
            Debug.Log("위치 선택 안함");
            UIManager.instance.SelectCanvasTrue();
            StartCoroutine(SetActiveFalseTime(UIManager.instance.GetSelectCanvas(), 1.5f));

            UIManager.instance.ChooseCanvasFalse();
        }
        else
        {
            UIManager.instance.SelectCanvasFalse();
            UIManager.instance.ChooseCanvasTrue();
        }

    }
    // 직업 선택
    public void SelectClass()
    {
        if (SelectTile == null)
        {
            Debug.Log("위치 선택 안함");
            UIManager.instance.SelectCanvasTrue(); // choose the tile
            StartCoroutine(SetActiveFalseTime(UIManager.instance.GetSelectCanvas(), 1.5f));

            //UIManager.instance.ChooseClassCanvasFalse(); 필요 없을 듯. 안올 것 같은데
        }
        else
        {
            CameraManager.instance.BuildPieceCamera();
            UIManager.instance.SelectCanvasFalse();
            UIManager.instance.ChooseClassCanvasTrue();


        }
    }


}
