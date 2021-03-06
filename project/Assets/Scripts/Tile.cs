using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems; // EventSystem 사용하기 위하여

public class Tile : MonoBehaviourPunCallbacks
{
    private bool Movable; // Tile Movable은 이동할 수 있는 칸인지 체크하기 위하여
    private const int BlackTeamMaxBoundary = 1;
    private const int WhiteTeamMaxBoundary = 1;
    public void SetMovable(bool BMove)
    {
        Movable = BMove;
    }
    public bool GetMovable()
    {
        return Movable;
    }

    private void Start()
    {
        Movable = false;
        GameManager.instance.SetTile(this.gameObject, (int)transform.position.x, (int)transform.position.z);
    }
    // 타일 클릭 시 생성할 수 있는 좌표 받아오기 - 구지 && 효원 && 재용
    private void OnMouseUp()
    {
        int x = (int)gameObject.transform.position.x;
        int z = (int)gameObject.transform.position.z;
        if (GameManager.instance.GetPlayer() && !EventSystem.current.IsPointerOverGameObject())
        {
             if (0 <= z && z <= BlackTeamMaxBoundary)
            {
                if (GameManager.instance.GetBoard(x, z) == null && BuildManager.instance.GetSelectTile() != this.gameObject)
                {
                    GameManager.instance.InitializeTile();
                    BuildManager.instance.SetSelectTile(this.gameObject);
                    GameManager.instance.HighlightTile(this.gameObject);
                }
                else
                {
                    GameManager.instance.InitializeTile();
                    BuildManager.instance.InitializeSelectTile();
                }
            }
        }
        else if (!GameManager.instance.GetPlayer() && !EventSystem.current.IsPointerOverGameObject())
        {
            if (7- WhiteTeamMaxBoundary <= z  && z <= 7 && !EventSystem.current.IsPointerOverGameObject())
            {
                if (GameManager.instance.GetBoard(x, z) == null && BuildManager.instance.GetSelectTile() != this.gameObject)
                {
                    GameManager.instance.InitializeTile();
                    BuildManager.instance.SetSelectTile(this.gameObject);
                    GameManager.instance.HighlightTile(this.gameObject);
                }
                else
                {
                    GameManager.instance.InitializeTile();
                    BuildManager.instance.InitializeSelectTile();
                }
            }
        }

    }
}
