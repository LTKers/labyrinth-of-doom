using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axegun : Gun
{
    [SerializeField] Camera cam;

    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void Use()
    {
        Shoot();
    }
    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(30);
            if (hit.transform.tag == "entity")
            {

                Debug.Log("hit");
                PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
            }
            else
            {
                PV.RPC("RPC_HitObject", RpcTarget.All, hit.point, hit.normal);
            }

        }

    }
    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj3 = Instantiate(bulletImpactPrefab, hitPosition, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj3, 10f);
            bulletImpactObj3.transform.SetParent(colliders[0].transform);
        }
    }
    [PunRPC]
    void RPC_HitObject(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj2 = Instantiate(ObjectImpact, hitPosition, Quaternion.LookRotation(hitNormal, Vector3.up) * ObjectImpact.transform.rotation);
            Destroy(bulletImpactObj2, 10f);
            bulletImpactObj2.transform.SetParent(colliders[0].transform);
        }
    }
}
