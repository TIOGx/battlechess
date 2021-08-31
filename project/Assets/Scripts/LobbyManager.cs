using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

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
        connectionInfoText.text = "마스터 서버에 접속중 ... ";
    }

    public override void OnConnectedToMaster() // 마스터서버 접속 성공시 실행
    {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음, 접속 재시도 중...";
        PhotonNetwork.ConnectUsingSettings(); // 재접속 시도
    }

    public void Connect()
    {
        joinButton.interactable = false;
        if(PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "룸에 접속 ... ";
            PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
            PhotonNetwork.JoinRandomRoom();
        }
        
        else
        {
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음, 재접속 시도중 ... ";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // 랜덤 룸 참가에 실패한 경우 자동 실행
    {
        connectionInfoText.text = "빈 방이 없음, 새로운 방을 생성";
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.CreateRoom("MyRoom", new RoomOptions{ MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "방 참가 성공";
        PhotonNetwork.LoadLevel("HyowonGameScene");
    }

    bool master()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }
}
