using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EClassType : int
{
    Warrior=0,
    Wizard=1,
    Archer=2,
    None
}


public class BuildManager : MonoBehaviour // 체스 말, 기물 종류를 받아서 builded 된 piece를 보드에 넣어주는 역할
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

    public bool IsBuild()
    {   
        int PieceCost = ClassCost[(int)ClassType] + PieceTypeCost[(int)PieceType];
        if (GameManager.instance.GetPlayer())//black player
        {
            if(PieceCost > UIManager.instance.GetBlackTeamCost()) // 돈이 모자라면
            {
                Debug.Log("돈 부족이용");
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
    public void BuildPiece() 
    {
        Vector3 selectpos = SelectTile.transform.position;
        if(!IsBuild())
        {
            UIManager.instance.ChooseCanvasFalse();
            return;
        }
        if (GameManager.instance.GetPlayer()) //black player
        {
            if (GameManager.instance.GetBoard((int)selectpos.x, (int)selectpos.z) == null)
            {
                Debug.Log("black instantiate");
                BuildedPiece = (GameObject)Instantiate(Piece[(int)PieceType], selectpos, Quaternion.identity);
                BuildedPiece.GetComponent<PieceController>().MyEClass = ClassType;
                BuildedPiece.GetComponent<PieceController>().Piecetype = (EPieceType)PieceType;
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
            if (GameManager.instance.GetBoard((int)selectpos.x, (int)selectpos.z) == null)
            {
                Debug.Log("white instantiate");
                BuildedPiece = (GameObject)Instantiate(Piece[(int)PieceType + 5], selectpos, Quaternion.Euler(new Vector3(0,180,0)));
                BuildedPiece.GetComponent<PieceController>().MyEClass = ClassType;
                BuildedPiece.GetComponent<PieceController>().Piecetype = (EPieceType)PieceType;
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

    public void SelectClass()
    {
        if (SelectTile == null)
        {
            Debug.Log("위치 선택 안함");
            UIManager.instance.SelectCanvasTrue();
            StartCoroutine(SetActiveFalseTime(UIManager.instance.GetSelectCanvas(), 1.5f));

            UIManager.instance.ChooseClassCanvasFalse();
        }
        else
        {
            UIManager.instance.SelectCanvasFalse();
            UIManager.instance.ChooseClassCanvasTrue();
        }
    }


}
