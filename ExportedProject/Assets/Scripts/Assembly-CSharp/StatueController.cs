using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class StatueController : MonoBehaviourPunCallbacks, IDamageable
{
    

    [SerializeField] GameObject Statue;


    
    PhotonView PV;

    const float maxHealth = 750f;
    float currentHealth = maxHealth;
    bool complete = false;
    bool added = false;
    int prayed = 0;
    PlayerManager playerManager;
    [SerializeField] StatueManager statuecount;

    void Awake()
    {
   
        PV = GetComponent<PhotonView>();

   

    }

    void Start()
    {
        Statue.SetActive(false);

    }
    void Update()
    {
  
        if (prayed == 1 && added == false)
        {
            statuecount.Addition();
            added = true;
            Debug.Log("added");
        }
    }







    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_Takedamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_Takedamage(float damage)
    {


        currentHealth -= damage;

        Debug.Log(currentHealth);
  
        if (currentHealth <= 0 && complete==false)
        {
            Statue.SetActive(true);
            prayed++;
            complete = true;
       
        }
    }

    
}
