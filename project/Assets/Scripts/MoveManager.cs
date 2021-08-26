using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                        GameManager.instance.GetTile(x, z + 1).GetComponent<Tile>().SetMovable(true);
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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("클릭");
            if (Physics.Raycast(ray, out hit))
            {
                int x = (int)hit.transform.position.x;
                int z = (int)hit.transform.position.z;
                if (CheckMyTurn(hit.transform.tag))
                {
                    if (hit.transform.gameObject.GetComponent<PieceController>().GetMovable() == true && moveflag == false)
                    {
                        Debug.Log("x : " + x + " z : " + z);
                        originx = x;
                        originz = z;
                        GameManager.instance.InitializeTile();
                        MovablePieceHighlight(hit.transform.gameObject);
                        CameraManager.instance.ClickPieceCamera();
                        moveflag = true;
                    }
                    else
                    {
                        if (moveflag == true)
                        {
                            Debug.Log("눌렀던거 취소할게요");
                            GameManager.instance.InitializeTile();
                            CameraManager.instance.InitailizeCamera();
                            moveflag = false;
                        }
                        else
                        {
                            Debug.Log("아직 이동할 수 없어요");
                        }
                    }
                }
                else if (hit.transform.tag == "Map" && moveflag == true)
                {
                    moveflag = false;
                    GameManager.instance.GetBoard(originx, originz).GetComponent<PieceController>().Move(x, z, originx, originz);

                }
                else
                {
                    Debug.Log("제 턴이 아닙니다.");
                }
            }
        }
    }
}


