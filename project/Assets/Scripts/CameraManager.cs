using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//작성 - 재용 & 구지  수정 - 재용
public class CameraManager : MonoBehaviour 
{
    // Camera Move Manager

    public static CameraManager instance; // singleton
    [SerializeField]
    private GameObject BlackTeamC; // BlackTeamCamera Outlet Connect
    [SerializeField]
    private GameObject WhiteTeamC; // WhiteTeamCamera Outlet Connect
    public bool CameraMoveFlag; // throw Exceptions
    // private float elapsedTime = 0; // Timer for Coroutine
    // private const float waitTime = 0.2f; // Timer for Coroutine

    void Awake()
    {
        CameraMoveFlag = false;
        instance = this;
    }
    public void MovePieceCamera()
    {
        Debug.Log(GameManager.instance.GetPlayer());
        if (GameManager.instance.GetPlayer()) {
            BlackTeamC.GetComponent<Animator>().SetBool("Move", true);
            BlackTeamC.GetComponent<Animator>().SetBool("Init", false);
            BlackTeamC.GetComponent<Animator>().SetBool("Build", false);
        }
        else {
            WhiteTeamC.GetComponent<Animator>().SetBool("Move", true);
            WhiteTeamC.GetComponent<Animator>().SetBool("Init", false);
            WhiteTeamC.GetComponent<Animator>().SetBool("Build", false);
        }
    }
    public void BuildPieceCamera()
    {
        if (GameManager.instance.GetPlayer()) {
            BlackTeamC.GetComponent<Animator>().SetBool("Build", true);
            BlackTeamC.GetComponent<Animator>().SetBool("Init", false);
            BlackTeamC.GetComponent<Animator>().SetBool("Move", false);
        }

        else {
            WhiteTeamC.GetComponent<Animator>().SetBool("Build", true);
            WhiteTeamC.GetComponent<Animator>().SetBool("Init", false);
            WhiteTeamC.GetComponent<Animator>().SetBool("Move", false);
        } 
    }
    public void InitializeCamera()
    {
        if (GameManager.instance.GetPlayer())
        {
            BlackTeamC.GetComponent<Animator>().SetBool("Init", true);
            BlackTeamC.GetComponent<Animator>().SetBool("Build", false);
            BlackTeamC.GetComponent<Animator>().SetBool("Move", false);
        }
        else
        {
            WhiteTeamC.GetComponent<Animator>().SetBool("Init", true);
            WhiteTeamC.GetComponent<Animator>().SetBool("Build", false);
            WhiteTeamC.GetComponent<Animator>().SetBool("Move", false);
        }
    }
    
    public void BlackTeamCameraOn()
    {
        BlackTeamC.GetComponent<Camera>().enabled = true;
        BlackTeamC.GetComponent<AudioListener>().enabled = true;
        WhiteTeamC.GetComponent<Camera>().enabled = false;
        WhiteTeamC.GetComponent<AudioListener>().enabled = false;
    }

    public void WhiteTeamCameraOn()
    {
        WhiteTeamC.GetComponent<Camera>().enabled = true;
        WhiteTeamC.GetComponent<AudioListener>().enabled = true;
        BlackTeamC.GetComponent<Camera>().enabled = false;
        BlackTeamC.GetComponent<AudioListener>().enabled = false;
    }

}