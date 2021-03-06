using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private GameObject SelectedObject;
    [SerializeField]
    private GameObject MoveObject;
    [SerializeField]
    private Image StatusImage;
    [SerializeField]
    private Sprite[] PieceImage;

    public GameObject GetMoveObject()
    {
        return MoveObject;
    }
    public GameObject GetSelectedObject()
    {
        return SelectedObject;
    }

    
    // 체스기물을 선택하였을 때 상태 UI 갱신
    public void SetStatusUI(){
        if (SelectedObject.tag == "WhiteTeam") { StatusImage.sprite = PieceImage[(int)SelectedObject.GetComponent<PieceController>().Piecetype]; }
        else if (SelectedObject.tag == "BlackTeam") { StatusImage.sprite = PieceImage[(int)SelectedObject.GetComponent<PieceController>().Piecetype + 6] ; }

        GameObject.Find("NicknameText").GetComponent<Text>().text = NetworkManager.instance.NicknameText.text.ToString();
        GameObject.Find("MaxHp").GetComponent<Text>().text =  " / "+ SelectedObject.GetComponent<PieceController>().Hp.ToString();
        GameObject.Find("NowHp").GetComponent<Text>().text = SelectedObject.GetComponent<PieceController>().nowHp.ToString();
        GameObject.Find("Type").GetComponent<Text>().text = SelectedObject.GetComponent<PieceController>().TypeName.ToString();
        GameObject.Find("Class").GetComponent<Text>().text = SelectedObject.GetComponent<PieceController>().MyEClass.ToString();
        GameObject.Find("Power").GetComponent<Text>().text = SelectedObject.GetComponent<PieceController>().OffensePower.ToString();
        
    }
    

    public IEnumerator SetActiveFalseTime(GameObject gameObject, float WaitSeconds)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(WaitSeconds);
        Debug.Log("사라져!");
        gameObject.SetActive(false);
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
                        GameManager.instance.HighlightTile(GameManager.instance.GetTile(nx, nz));
                    }
                }
                break;

            case EPieceType.Pawn:
                Debug.Log("폰이 선택됐어요.");
                Debug.Log("x는 " + (x+ dx[MoveObject.GetComponent<PieceController>().Dir]) + " z는 " +(z+dz[MoveObject.GetComponent<PieceController>().Dir]));
                if (GameManager.instance.GetBoard(x+ dx[MoveObject.GetComponent<PieceController>().Dir], z+dz[MoveObject.GetComponent<PieceController>().Dir]) == null && GameManager.instance.GetBoard(x,z).tag == "BlackTeam")
                {
                    Debug.Log("잘 들어갔어요?");
                    GameManager.instance.HighlightTile(GameManager.instance.GetTile(x+ dx[MoveObject.GetComponent<PieceController>().Dir], z+dz[MoveObject.GetComponent<PieceController>().Dir]));
                } else if (GameManager.instance.GetBoard(x+ dx[MoveObject.GetComponent<PieceController>().Dir], z+dz[MoveObject.GetComponent<PieceController>().Dir]) == null && GameManager.instance.GetBoard(x, z).tag == "WhiteTeam")
                {
                    GameManager.instance.HighlightTile(GameManager.instance.GetTile(x+ dx[MoveObject.GetComponent<PieceController>().Dir], z+dz[MoveObject.GetComponent<PieceController>().Dir]));
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
    // 현재 자신의 Turn이면서 자신의 기물을 선택했는지 확인합니다. - 재용 (효원 request)
    public bool CheckMyPiece(string str)
    {
        if((str == "BlackTeam" && GameManager.instance.Player == EPlayerWho.Black) || (str == "WhiteTeam" && GameManager.instance.Player == EPlayerWho.White))
        {
            return true;
        } else
        {
            Debug.Log("현재 당신의 턴이 아닙니다.");
            return false;
        }
    }
    public bool CheckMyTurn(string str){
        if((str == "BlackTeam" && GameManager.instance.NowPlayer == GameManager.instance.Player) || (str == "WhiteTeam" && GameManager.instance.NowPlayer == GameManager.instance.Player)){
            return true;
        } else {
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
    public bool CheckClickBackground(string str)
    {
        if (str == "Background") return true;
        else return false;
    }
    public void OnClickMove()
    {
        if (MoveObject == null) StartCoroutine(SetActiveFalseTime(UIManager.instance.GetSelectCanvas(), 1.5f));
        else
        {
            UIManager.instance.SelectCanvasFalse();
            UIManager.instance.DirButtonFalse();
            CameraManager.instance.MovePieceCamera();
            UIManager.instance.ButtonPanelFalse();
            moveflag = true;
            MovablePieceHighlight(MoveObject);
        }
    }
    // 체스기물을 선택하였을 때 체스말의 로테이션 변화 기능
    public void turnSelectedPiece(int d)
    {
        if (SelectedObject == null)
        {
            StartCoroutine(SetActiveFalseTime(UIManager.instance.GetSelectCanvas(), 1.5f));
            return;
        }
        if (GameManager.instance.NowPlayer == EPlayerWho.White)
        {
            d = (d + 2) % 4;
        }
        SelectedObject.GetComponent<PieceController>().Dir = d;
        SelectedObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90 * d, 0));
    }
    // Update 변경
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
                if (CheckClickPiece(tagName) && moveflag == false)
                {
                    SelectedObject = hit.transform.gameObject;
                    SetStatusUI();
                    if (CheckMyPiece(tagName) && CheckMyTurn(tagName))
                    {
                        if(SelectedObject.GetComponent<PieceController>().GetMovable()) MoveObject = SelectedObject;
                        UIManager.instance.DirButtonTrue();
                    }
                    else
                    {
                        UIManager.instance.DirButtonFalse();
                        MoveObject = null;
                    }
                }
                else if(CheckClickPiece(tagName) && moveflag == true){
                    GameManager.instance.InitializeSetting();
                }

                // Map을 클릭했을 때
                else if (CheckClickMap(tagName) && moveflag == true)
                {
                    MoveObject.GetComponent<PieceController>().Move(x, z, (int)MoveObject.transform.position.x, (int)MoveObject.transform.position.z);
                    moveflag = false;
                    MoveObject = null;
                }
                else if(CheckClickBackground(tagName)){
                    GameManager.instance.InitializeSetting();
                }

            }
            GameManager.instance.InitializeTile();
            BuildManager.instance.InitializeSelectTile();
        }
    }
}


