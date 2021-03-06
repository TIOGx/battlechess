using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class PieceController : MonoBehaviourPunCallbacks, IPunObservable
{

    public EPieceType Piecetype;
    public EClassType MyEClass;
    public PhotonView PV;
    public string TypeName;
    public float Hp;
    public float nowHp;
    public float OffensePower;
    public int Dir{set; get;}

    [SerializeField]
    private bool Attackable;
    [SerializeField]
    private bool MoveFlag;

    [SerializeField]
    private bool Movable;
    [SerializeField]
    private GameObject Arrow;
    [SerializeField]
    private GameObject SpawnedArrow;
    [SerializeField]
    private GameObject Explosion;
    [SerializeField]
    private GameObject SpawnedExplosion;
    [SerializeField]
    private int[] dx = new int[]{0,1,0,-1,1,1,-1,-1};
    [SerializeField]
    private int[] dz = new int[]{1,0,-1,0,1,-1,1,-1};
    [SerializeField]
    private GameObject hudDamageText;
    [SerializeField]
    private Transform Hudpos;
    [SerializeField]
    private GameObject HealthBar;
    [SerializeField]
    private int IntPiecetype;

    public void SetMoveFlag(bool BMove)
    {
        MoveFlag = BMove;
    }
    public void SetMovable(bool BMove)
    {
        Movable = BMove;
    }
    public bool GetMovable()
    {
        return Movable;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ;
    }
    public void SetAttackable(bool BAttack)
    {
        Attackable = BAttack;
    }
    public bool GetAttackable()
    {
        return Attackable;
    }

    // 기물의 속성을 Set해줌 - 재용 (효원 req)
    [PunRPC]
    public void SetPropertyRPC(int SetPieceType, int SetClassType)
    {
        Piecetype = (EPieceType)SetPieceType;
        MyEClass = (EClassType)SetClassType;

        if (GameManager.instance.NowPlayer == EPlayerWho.Black)
        {
            GameManager.instance.BlackTeamPiececnt += 1;
        }
        else
        {
            GameManager.instance.WhiteTeamPiececnt += 1;
        }
    }
    void Start(){
        TypeName = Piecetype.ToString();
        if(this.tag == "BlackTeam"){
            GameManager.instance.BlackTeamPiece.Enqueue(this.gameObject);

        
        }
        else if (this.tag == "WhiteTeam")
        {
            GameManager.instance.WhiteTeamPiece.Enqueue(this.gameObject);
        }
        Dir = (int) (gameObject.transform.eulerAngles.y / 90) % 4; // 0,1,2,3
        Attackable = false;
        Movable = false;
        nowHp = Hp;
    }
    void Update(){
        Dir = (int) (gameObject.transform.eulerAngles.y / 90) % 4; // 0,1,2,3
    }
    public EPieceType GetPiecetype()
    {
        return Piecetype;
    }
    [PunRPC]
    // 데미지 받기 - 효원 (PUN으로 수정 - 재용)
    public void Damaged(float damage)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = Hudpos.position;
        hudText.GetComponent<DamageText>().damage = damage;
        nowHp -= damage;
        HealthBar.GetComponent<Image>().fillAmount = nowHp / Hp;
        if (nowHp <= 0)
        {
            Die();
        }
    }
    // 공격 - 효원
    public void Attack()
    {
        if (Attackable)
        {
            int x, nx;
            int z, nz;
            x = (int)transform.position.x;
            z = (int)transform.position.z;
            switch (MyEClass)
            {

                case EClassType.Warrior: // 근거리 공격
                    nx = x + dx[Dir];
                    nz = z + dz[Dir];
                    Debug.Log(x +" "+ z);
                    Debug.Log(nx +" "+ nz);

                    if (GameManager.instance.GetBoard(nx, nz) != null && GameManager.instance.GetBoard(nx, nz).tag != this.tag)
                    {
                        Debug.Log("공격!");
                        GameManager.instance.GetBoard(nx, nz).GetComponent<PhotonView>().RPC("Damaged", RpcTarget.All, OffensePower);
                        Attackable = false;
                    }
                    break;

                case EClassType.Archer: // 원거리 공격
                    for (int i = 1; i < 8; i++)
                    {
                        nx = x + i * dx[Dir];
                        nz = z + i * dz[Dir];
                        if (nx < 0 || 8 <= nx || nz < 0 || 8 <= nz)
                        {
                            break;
                        }
                        if (GameManager.instance.GetBoard(nx, nz) != null)
                        { // 공격하려는 자리에 말이 있는데
                            if (GameManager.instance.GetBoard(nx, nz).tag != this.tag)
                            { // 그 말이 적팀이면
                                Debug.Log("원거리 공격!");
                                // SpawnedArrow = Instantiate(Arrow, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(90, 90 * (1 + Dir), 0));
                                SpawnedArrow = PhotonNetwork.Instantiate(Arrow.name, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(90, 90 * (1 + Dir), 0));
                                SpawnedArrow.GetComponent<PhotonView>().RPC("SetPropertyRPC", RpcTarget.All, OffensePower, dx[Dir], dz[Dir],this.tag );
                                // SpawnedArrow.GetComponent<ArrowController>().Damage = OffensePower;
                                // SpawnedArrow.GetComponent<ArrowController>().dx = dx[Dir];
                                // SpawnedArrow.GetComponent<ArrowController>().dz = dz[Dir];
                                // SpawnedArrow.gameObject.tag = this.tag;
                                Attackable = false;
                                break;
                            }
                            else if (GameManager.instance.GetBoard(nx, nz).tag == this.tag)
                            {
                                Debug.Log("팀이막음");
                                break;
                            }
                        }

                    }
                    break;

                case EClassType.Wizard: // 광역 공격
                    Debug.Log("광역공격!");
                    SpawnedExplosion = PhotonNetwork.Instantiate(Explosion.name, transform.position, Quaternion.Euler(0,0,0));
                    for (int i = 0; i < 8; i++)
                    {
                        nx = x + dx[i];
                        nz = z + dz[i];
                        if (nx < 0 || 8 <= nx || nz < 0 || 8 <= nz)
                        {
                            continue;
                        }
                        if (GameManager.instance.GetBoard(nx, nz) != null && GameManager.instance.GetBoard(nx, nz).tag != this.tag)
                        {
                            GameManager.instance.GetBoard(nx, nz).GetComponent<PhotonView>().RPC("Damaged", RpcTarget.All, OffensePower);
                            Debug.Log(nx);
                            Debug.Log(nz);
                        }
                    }
                    Attackable = false;
                    break;

                    //    default:
            }
        }
    }
    // 이동 - 재용
    
    public void Move(int x, int z, int originx, int originz)
    {
        if (Movable && GameManager.instance.GetTile(x,z).GetComponent<Tile>().GetMovable() == true)
        {
            GameManager.instance.GetBoard(originx, originz).transform.position = new Vector3(x, 0, z);
            GameManager.instance.SetBoard(GameManager.instance.GetBoard(originx, originz), x, z);
            GameManager.instance.SetBoard(null, originx, originz);
            GameManager.instance.InitializeTile();
            Movable = false;
            
        } else
        {
            Debug.Log("Move 불가능합니다");
        }
        CameraManager.instance.InitializeCamera();
        UIManager.instance.ButtonPanelTrue();
    }
    // 제거 - 효원
    public void Die()
    {
        if (gameObject.tag =="BlackTeam")
        {
            GameManager.instance.BlackTeamPiececnt -= 1;
        }
        else 
        {
            GameManager.instance.WhiteTeamPiececnt -= 1;
        }

        if (Piecetype == EPieceType.King) //KING 이 죽었을 때
        {
              UIManager.instance.GetwinTeamText().text = GameManager.instance.NowPlayer.ToString() + " Team";
              UIManager.instance.EndGamePanelTrue();
        }
        GameManager.instance.SetBoard(null,(int)gameObject.transform.position.x, (int)gameObject.transform.position.z);
        Destroy(gameObject);
    }

}
