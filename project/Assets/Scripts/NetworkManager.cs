using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance; // singleton
    public GameObject StartGameBtn;
    public PhotonView PV;
    public Text[] NicknameText;
    public int IDNum;
    void Start()
    {
        PV = photonView;
        instance = this;

    }
    bool master()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    public void StartGame()
    {
        if (!master()) return; // 방장만 게임 시작 버튼 누를 수 있음
        // StartGameBtn.SetActive(false);
        PV.RPC("StartGameRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void StartGameRPC()
    {
        StartGameBtn.SetActive(false);
        print("게임시작");
        for (int i = 0; i < 2; i++)
        {
            NicknameText[i].text = PhotonNetwork.PlayerList[i].NickName;
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                IDNum = i; // 0 : 1번째로 들어온 사람(방장) Black, 1 : 2번째로 들어온사람 White
                if (IDNum == 0)
                {
                    CameraManager.instance.BlackTeamCameraOn(); // First turn is Black Turn
                    GameManager.instance.Player = GameManager.EPlayerWho.Black;
                    UIManager.instance.SetblackteamcostCanvas();
                }
                else
                {
                    CameraManager.instance.WhiteTeamCameraOn();
                    GameManager.instance.Player = GameManager.EPlayerWho.White;
                    UIManager.instance.SetwhiteteamcostCanvas();
                }
            }
        }
    }
}