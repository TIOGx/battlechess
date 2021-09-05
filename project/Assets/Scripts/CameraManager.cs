using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{
    // Camera Move Manager

    public static CameraManager instance; // singleton
    [SerializeField]
    private GameObject BlackTeamC; // BlackTeamCamera Outlet Connect
    [SerializeField]
    private GameObject WhiteTeamC; // WhiteTeamCamera Outlet Connect
    private Vector3 OriginCamerapos; // Black Turn Black pos, White Turn white pos
    private Quaternion OriginCamerarot; // Black Turn Black rotation, White Turn White rotation
    private Vector3 SharedCamerapos; // Detail view pos for Board 
    private Quaternion SharedCamerarot; // Detail view rot for Board
    public bool CameraMoveFlag; // throw Exceptions
    public bool ShiftFlag;
    private float elapsedTime = 0; // Timer for Coroutine
    private const float waitTime = 0.2f; // Timer for Coroutine

    void Start()
    {
        CameraMoveFlag = false;
        ShiftFlag = false;
        instance = this;
    }
    IEnumerator CameraMoveCoroutine(Transform TeamOriginTransform)
    {
        // Camera's sharedcamerapos ����
        while (waitTime > elapsedTime)
        {
            elapsedTime += Time.deltaTime;

            TeamOriginTransform.position = Vector3.Lerp(TeamOriginTransform.position, SharedCamerapos, 0.2f);
            TeamOriginTransform.rotation = Quaternion.Lerp(TeamOriginTransform.rotation, SharedCamerarot, 0.2f);
            yield return null;
        }
        TeamOriginTransform.position = SharedCamerapos;
        TeamOriginTransform.rotation = SharedCamerarot;
        elapsedTime = 0;
        yield break;
    }
    

    IEnumerator InitCamCoroutine(Transform TeamOriginTransform)
    {
        while (waitTime > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            TeamOriginTransform.position = Vector3.Lerp(TeamOriginTransform.position, OriginCamerapos, 0.2f);
            TeamOriginTransform.rotation = Quaternion.Lerp(TeamOriginTransform.rotation, OriginCamerarot, 0.2f);
            yield return null;
        }
        TeamOriginTransform.position = OriginCamerapos;
        TeamOriginTransform.rotation = OriginCamerarot;
        elapsedTime = 0;
        
        yield break;
    }


    public void ClickPieceCamera()
    {
        CameraMoveFlag = true;
        if (GameManager.instance.GetPlayer())
        {
            OriginCamerapos = BlackTeamC.transform.position;
            OriginCamerarot = BlackTeamC.transform.rotation;
            SharedCamerapos = new Vector3(3.5f, 7.05f, 3.5f);
            SharedCamerarot = Quaternion.Euler(90f, 0, 0);
            StartCoroutine(CameraMoveCoroutine(BlackTeamC.transform));

        }
        else
        {
            SharedCamerapos = new Vector3(3.5f, 7.05f, 3.5f);
            SharedCamerarot = Quaternion.Euler(90f, 180f, 0);
            OriginCamerapos = WhiteTeamC.transform.position;
            OriginCamerarot = WhiteTeamC.transform.rotation;
            StartCoroutine(CameraMoveCoroutine(WhiteTeamC.transform));

        }
    }
    // 회전은 필요 없어서. 임시 변수로 잠시 둠
    public void LeftShiftCamera()
    {
        ShiftFlag = true;
        if (!GameManager.instance.GetPlayer())
        {
            OriginCamerapos = BlackTeamC.transform.position;
            OriginCamerarot = BlackTeamC.transform.rotation;
            SharedCamerapos = new Vector3(6f, 6.5f, -2.5f);
            SharedCamerarot = BlackTeamC.transform.rotation;
            StartCoroutine(CameraMoveCoroutine(BlackTeamC.transform));
        }
        else
        {
            SharedCamerapos = new Vector3(1f, 6.5f, 9.5f);
            SharedCamerarot = WhiteTeamC.transform.rotation;
            OriginCamerapos = WhiteTeamC.transform.position;
            OriginCamerarot = WhiteTeamC.transform.rotation;
            StartCoroutine(CameraMoveCoroutine(WhiteTeamC.transform));

        }
    }
    public void InitailizeCamera()
    {
        if(CameraMoveFlag == true)
        {
            CameraMoveFlag = false;
            if (GameManager.instance.GetPlayer())
                StartCoroutine(InitCamCoroutine(BlackTeamC.transform));
            else
                StartCoroutine(InitCamCoroutine(WhiteTeamC.transform));
        } else if(ShiftFlag == true)
        {
            ShiftFlag = false;
            if (GameManager.instance.GetPlayer())
                StartCoroutine(InitCamCoroutine(WhiteTeamC.transform));
            else
                StartCoroutine(InitCamCoroutine(BlackTeamC.transform));
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