using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject StartGameBtn;
    PhotonView PV;
    public Text[] NicknameText;
    public int myNum;
    void Start() {
        PV = photonView;

    }
    bool master()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    public void StartGame()
    {
        if(PhotonNetwork.CountOfPlayers != 2 || !master()) return;
        // StartGameBtn.SetActive(false);
        PV.RPC("StartGameRPC", RpcTarget.AllViaServer);
    }
    [PunRPC]
    void StartGameRPC()
    {   
        StartGameBtn.SetActive(false);
        print("게임시작");
        for (int i = 0; i <2 ; i++)
        {
            NicknameText[i].text = PhotonNetwork.PlayerList[i].NickName;
            if( PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer){
                myNum = i;
            }
        }
    }
}
