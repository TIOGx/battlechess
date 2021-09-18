using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // EventSystem 사용하기 위하여

public class MoveManager : MonoBehaviour
{
    [SerializeField]
    private int[] dx = { 0, 1, 0, -1, 1, 1, -1, -1 };
    [SerializeField]
    private int[] dz = { 1, 0, -1, 0, 1, -1, 1, -1 };
    [SerializeField]
    private int[] knightx = { -2, -1, 1, 2, -2, -1, 1, 2 };
    [SerializeField]
    private int[] knightz = { -1, -2, -2, -1, 1, 2, 2, 1 };
    public static MoveManager instance;
    [SerializeField]
    private int originx, originz;
    private bool moveflag;
    [SerializeField]
    private GameObject MoveObject;

    public GameObject GetMoveObject()
    {
        return MoveObject;
    }
    // 체스기물을 선택하였을 때 이동할 수 있는 위치의 타일들을 Highlight - 재용
    public void MovablePieceHighlight(GameObject GObject)
    {
        Debug.Log("type : " + GObject.GetComponent<PieceController>().GetPiecetype());
        int x = (int)GObject.transform.position.x;
        int z = (int)GObject.transform.position.z;
        switch (GObject.GetComponent<PieceController>().GetPiecetype()) {
            case EPieceType.Bishop:
                Debug.Log("비숍이 선택됐어요.");
                for (int i = 4; i < 8; i++)
                {
                    for (int j = 1; j < 8; j++)
                    {
                        int nx = x + j * dx[i]; // 4 
                        int nz = z + j * dz[i]; // 1
                        if (nx < 0 || 8 <= nx || nz < 0 || 8 <= nz) {
                            break;
                        }
                        else if (GameManager.instance.GetBoard(nx, nz) == null) {
                            GameManager.instance.GetTile(nx, nz).GetComponent<Tile>().SetMovable(true);
                            GameManager.instance.HighlightTile(GameManager.instance.GetTile(nx, nz));
                        }
                        else { break; }
                    }
                }
                break;

            case EPieceType.King:
                Debug.Log("킹이 선택됐어요.");
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 1; j < 2; j++)
                    {
                        int nx = x + j * dx[i]; // 4 
                        int nz = z + j * dz[i]; // 1
                        if (nx < 0 || 8 <= nx || nz < 0 || 8 <= nz) break;
                        else if (GameManager.instance.GetBoard(nx, nz) == null)
                        {
                            GameManager.instance.GetTile(nx, nz).GetComponent<Tile>().SetMovable(true);
                            GameManager.instance.HighlightTile(GameManager.instance.GetTile(nx, nz));
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                break;
            case EPieceType.Knight:
                Debug.Log("나이트가 선택됐어요.");
                for (int i = 0; i < 8; i++)
                {
                    int nx = x + knightx[i]; // 4 
                    int nz = z + knightz[i]; // 1
                    if (nx < 0 || 8 <= nx || nz < 0 || 8 <= nz)
                        continue;
                    else if (GameManager.instance.GetBoard(nx, nz) == null)
                    {
                        GameManager.instance.GetTile(nx, nz).GetComponent<Tile>().SetMovable(true);
                        GameManager.instance.HighlightTile(GameManager.instance.GetTile(nx, nz));
                    }
                }
                break;

            case EPieceType.Pawn:
                Debug.Log("폰이 선택됐어요.");
                if (GameManager.instance.GetBoard(x, z+1) == null && GameManager.instance.GetBoard(x,z).tag == "BlackTeam")
                {
                    GameManager.instance.GetTile(x, z+1).GetComponent<Tile>().SetMovable(true);
                    GameManager.instance.HighlightTile(GameManager.instance.GetTile(x, z + 1));
                } else if (GameManager.instance.GetBoard(x, z - 1) == null && GameManager.instance.GetBoard(x, z).tag == "WhiteTeam")
                {
                    GameManager.instance.GetTile(x, z - 1).GetComponent<Tile>().SetMovable(true);
                    GameManager.instance.HighlightTile(GameManager.instance.GetTile(x, z - 1));
                }
                break;

            case EPieceType.Queen:
                Debug.Log("퀸이 선택됐어요.");
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 1; j < 8; j++)
                    {
                        int nx = x + j * dx[i]; // 4 
                        int nz = z + j * dz[i]; // 1
                        if (nx < 0 || 8 <= nx || nz < 0 || 8 <= nz)
                            continue;
                        else if (GameManager.instance.GetBoard(nx, nz) == null)
                        {
                            GameManager.instance.GetTile(nx, nz).GetComponent<Tile>().SetMovable(true);
                            GameManager.instance.HighlightTile(GameManager.instance.GetTile(nx, nz));
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                break;

            case EPieceType.Rook:
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 1; j < 8; j++)
                    {
                        int nx = x + j * dx[i]; // 4 
                        int nz = z + j * dz[i]; // 1
                        if (nx < 0 || 8 <= nx || nz < 0 || 8 <= nz)
                            break;
                        else if (GameManager.instance.GetBoard(nx, nz) == null)
                        {
                            GameManager.instance.GetTile(nx, nz).GetComponent<Tile>().SetMovable(true);
                            GameManager.instance.HighlightTile(GameManager.instance.GetTile(nx, nz));
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                break;

            case EPieceType.None:
                Debug.Log("기물의 타입이 아직 없어요");
                break;
        }
    }
    // 현재 자신의 Turn인지 확인합니다. - 재용 (효원 request)
    public bool CheckMyTurn(string str)
    {
        if((str == "BlackTeam" && GameManager.instance.GetPlayer() == true) || (str == "WhiteTeam" && GameManager.instance.GetPlayer() == false))
        {
            return true;
        } else
        {
            Debug.Log("현재 당신의 턴이 아닙니다.");
            return false;
        }
    }
    public bool CheckClickPiece(string str)
    {
        if (str == "BlackTeam" || str == "WhiteTeam") return true;
        else return false;
    }
    public bool CheckClickMap(string str)
    {
        if (str == "Map") return true;
        else return false;
    }
    
    public void OnClickMove()
    {
        if (MoveObject == null) UIManager.instance.SelectCanvasTrue();
        else
        {
            CameraManager.instance.MovePieceCamera();
            moveflag = true;
            MovablePieceHighlight(MoveObject);
        }
    }

    //  재용
    // !EventSystem.current.IsPointerOverGameObject() 은 터치로 Ray를 쏘는 오브젝트와
    // Button Onclick으로 실행되는 오브젝트가 존재할 때 겹쳐져 있는 경우 한쪽 기능만 실행시키기 위하여
    // 사용하는 메소드입니다. 0918 update.md에 조금 더 작성하겠습니다.
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            
            if (Physics.Raycast(ray, out hit))
            {
                int x = (int)hit.transform.position.x;
                int z = (int)hit.transform.position.z;
                string tagName = hit.transform.tag;

                // Piece를 클릭했을 때
                if (CheckClickPiece(tagName))
                {
                    GameObject ClickGO = hit.transform.gameObject;
                    string Type = ClickGO.GetComponent<PieceController>().Piecetype.ToString();
                    string Class = ClickGO.GetComponent<PieceController>().MyEClass.ToString();
                    string HP = ClickGO.GetComponent<PieceController>().nowHp.ToString();
                    string Power = ClickGO.GetComponent<PieceController>().OffensePower.ToString();

                    UIManager.instance.SetTypeText(Type);
                    UIManager.instance.SetClassText(Class);
                    UIManager.instance.SetHPText(HP);
                    UIManager.instance.SetPowerText(Power);

                    if (CheckMyTurn(tagName) && ClickGO.GetComponent<PieceController>().GetMovable())
                    {
                        MoveObject = ClickGO;
                    } else
                    {
                        MoveObject = null;
                    }
                } 
                
                // Map을 클릭했을 때
                else if (CheckClickMap(tagName) && moveflag == true){
                    
                    moveflag = false;
                    MoveObject.GetComponent<PieceController>().Move(x, z, (int)MoveObject.transform.position.x, (int)MoveObject.transform.position.z);
                    MoveObject = null;
                }
            } 
        }
    }
}


