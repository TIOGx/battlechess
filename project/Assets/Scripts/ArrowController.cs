using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ArrowController : MonoBehaviourPunCallbacks, IPunObservable 
{
    public int dx;
    public int dz;
    public float Damage;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ;
    }
    [PunRPC]
    public void SetPropertyRPC(float getDamage, int getdx, int getdz, string gettag)
    {
        Damage = getDamage;
        dx = getdx;
        dz = getdz;
        tag = gettag;
    }
    void Start()
    {
        this.GetComponent<Rigidbody>().AddForce(new Vector3(dx, 0, dz) * 3, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag != null)
        {
            if (target.tag != this.gameObject.tag)
            {
                target.gameObject.GetComponent<PieceController>().Damaged(Damage);
                Destroy(gameObject);
            }
        }
    }
}
