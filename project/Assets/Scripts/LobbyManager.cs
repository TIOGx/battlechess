using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

// 효원 작성
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1"; // 게임 버전

    public Text connectionInfoText; // 네트워크 상태를 표시 할 텍스트
    public Button joinButton; // 룸 접속 버튼
    public InputField NicknameInput;

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion; // 접속에 필요한 게임 버전 설정
        PhotonNetwork.ConnectUsingSettings(); // 설정한 정보를 가지고 마스터 서버 접속 시도

        joinButton.interactable = false; // 룸 접속 버튼 비활성화
        connectionInfoText.text = "Connecting... ";
    }

    public override void OnConnectedToMaster() // 마스터서버 접속 성공시 실행
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected...";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "Offline : Retry...";
        PhotonNetwork.ConnectUsingSettings(); // 재접속 시도
    }

    public void Connect()
    {
        joinButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Joined Room.... ";
            PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
            PhotonNetwork.JoinRandomRoom();
        }

        else
        {
            connectionInfoText.text = "Offline : Retry... ";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // 랜덤 룸 참가에 실패한 경우 자동 실행
    {
        connectionInfoText.text = "Auto Make Room...";
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.CreateRoom("MyRoom", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Loading...";
        PhotonNetwork.LoadLevel("jaeyong");
    }

    bool master()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }
}