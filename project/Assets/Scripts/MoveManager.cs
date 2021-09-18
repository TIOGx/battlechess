using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private bool moveflag = false;
    [SerializeField]
    private GameObject selectedObject;
    // 체스기물을 선택하였을 때 상태 UI 갱신
    public void SetStatusUI(){
        GameObject.Find("NicknameText").GetComponent<Text>().text = NetworkManager.instance.NicknameText.text.ToString();
        GameObject.Find("MaxHp").GetComponent<Text>().text =  " / "+ selectedObject.GetComponent<PieceController>().Hp.ToString();
        GameObject.Find("NowHp").GetComponent<Text>().text = selectedObject.GetComponent<PieceController>().nowHp.ToString();
        GameObject.Find("Type").GetComponent<Text>().text = selectedObject.GetComponent<PieceController>().TypeName.ToString();
        GameObject.Find("Class").GetComponent<Text>().text = selectedObject.GetComponent<PieceController>().MyEClass.ToString();
        GameObject.Find("Power").GetComponent<Text>().text = selectedObject.GetComponent<PieceController>().OffensePower.ToString();
        
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
                if (GameManager.instance.GetBoard(x+ dx[selectedObject.GetComponent<PieceController>().Dir], z+dz[selectedObject.GetComponent<PieceController>().Dir]) == null && GameManager.instance.GetBoard(x,z).tag == "BlackTeam")
                {
                    GameManager.instance.GetTile(x+ dx[selectedObject.GetComponent<PieceController>().Dir], z+dz[selectedObject.GetComponent<PieceController>().Dir]).GetComponent<Tile>().SetMovable(true);
                    GameManager.instance.HighlightTile(GameManager.instance.GetTile(x+ dx[selectedObject.GetComponent<PieceController>().Dir], z+dz[selectedObject.GetComponent<PieceController>().Dir]));
                } else if (GameManager.instance.GetBoard(x+ dx[selectedObject.GetComponent<PieceController>().Dir], z+dz[selectedObject.GetComponent<PieceController>().Dir]) == null && GameManager.instance.GetBoard(x, z).tag == "WhiteTeam")
                {
                    GameManager.instance.GetTile(x+ dx[selectedObject.GetComponent<PieceController>().Dir], z+dz[selectedObject.GetComponent<PieceController>().Dir]).GetComponent<Tile>().SetMovable(true);
                    GameManager.instance.HighlightTile(GameManager.instance.GetTile(x+ dx[selectedObject.GetComponent<PieceController>().Dir], z+dz[selectedObject.GetComponent<PieceController>().Dir]));
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
    // 체스기물을 선택하였을 때 체스말의 로테이션 변화 기능
    public void turnSelectedPiece(int d){
        if(GameManager.instance.NowPlayer == EPlayerWho.White){
            d = (d + 2)%4;
        }
        selectedObject.GetComponent<PieceController>().Dir = d;
        selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0,90*d,0));
    }
    // Ray를 쏴 좌표를 일반화하여 기물을 이동하는 Update 함수
    // 언제든 기물을 클릭할 수 있기 때문에 Update로 둠 - 재용
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                
                int x = (int)hit.transform.position.x;
                int z = (int)hit.transform.position.z;
                string tg = hit.transform.tag;
                Debug.Log(tg);
                if (CheckMyTurn(tg))
                {
                    selectedObject = hit.transform.gameObject;
                    SetStatusUI();
                    if (hit.transform.gameObject.GetComponent<PieceController>().GetMovable() == true && moveflag == false)
                    {
                        Debug.Log("x : " + x + " z : " + z);
                        originx = x;
                        originz = z;
                        GameManager.instance.InitializeTile();
                        MovablePieceHighlight(hit.transform.gameObject);
                        CameraManager.instance.MovePieceCamera();
                        moveflag = true;
                    }
                    else if(moveflag == true)
                    {
                        UIManager.instance.ChooseClassCanvasFalse();
                        UIManager.instance.ChooseCanvasFalse();
                        CameraManager.instance.InitializeCamera();
                        GameManager.instance.InitializeTile();
                    }
                }
                else if (tg == "Map" && moveflag == true)
                {
                    moveflag = false;
                    GameManager.instance.GetBoard(originx, originz).GetComponent<PieceController>().Move(x, z, originx, originz);
                }
                else if (tg == "Background")
                {
                    UIManager.instance.ChooseClassCanvasFalse();
                    UIManager.instance.ChooseCanvasFalse();
                    CameraManager.instance.InitializeCamera();
                    GameManager.instance.InitializeTile();
                }
            } 
        }
    }
}


