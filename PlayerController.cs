using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    
    [SerializeField] Image healthbarImage;
    [SerializeField] Image speedbarImage;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] GameObject torcher;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;


    [SerializeField] Item[] items;

    [SerializeField] Pose[] poses;
  

    int poseIndex;
    int previousPoseIndex=-1;

    float bowcount;

    bool movesound = false;

         static int statuecount = 0;

    public bool showstatue=false;
   
    bool sworddelay;
    float sworddelaycount;
    int itemIndex;
    int previousItemIndex = -1;

    float sprintamount = 10;
    int stopper = 0;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    const float maxHealth = 300f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;

    StatueManager statuemanage;
    

    public Shootanimation arrow;
    public spartanmove Theseus;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        statuemanage= GameObject.Find("Statuemanager").GetComponent<StatueManager>();

    }

    void Start()
    {
        
        if (PV.IsMine)
        {
            EquipItem(0);
            
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }
    void Update()
    {
       
        stopper--;
        if (sprintamount<10){
            sprintamount+=2*(Time.deltaTime);
            
            speedbarImage.fillAmount = sprintamount / 10;
        }
        if (sworddelay == true)
        {
            sworddelaycount += Time.deltaTime;
        }
        if (sworddelaycount >=0.75)
        {
            sworddelay = false;
            sworddelaycount = 0;
            Theseus.Notattack();
        }
       

        if (!PV.IsMine)
            return;

        Look();
        Move();    
        Jump();
    
        
        for (int i = 0; i<items.Length; i++)
        {
            if (Input.GetKeyDown((i+1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
       if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex>= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
               // EquipItem(itemIndex + 1);
            }
            
        }
       else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
    //EquipItem(itemIndex - 1);
            }
        }
       if (itemIndex == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (sworddelay == false)
                {
                    items[itemIndex].Use();
                
                    sworddelay = true;
                    Theseus.Attack();
                    Debug.Log("hit");
                }
                

            }
            

            
        }
        else
        {
          


                if (Input.GetMouseButton(0))
                {

                    bowcount += Time.deltaTime;
                    Debug.Log(bowcount);
 
                
                    arrow.drawback();

                    
                   

                }
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("backed");
                    if (bowcount >= 0.75)
                    {
                        items[itemIndex].Use();
                        arrow.shoot();
                    }
                    else {
                        arrow.failed();
                    }
                    bowcount = 0;
                    

                }
            }
     

        if (transform.position.y < -10f)
        {
            Die();
        }



        

  
    }

    public void Statuetext()
    {

    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetButton("Vertical") && Input.GetButton("Horizontal"))
        {
            Theseus.Move();
            if (movesound == false)
            {
                FindObjectOfType<MinotaurAudioManager>().Play("footsteps");
                movesound = true;
            }
        }
        if (!Input.GetButton("Horizontal"))
        {
            if (!Input.GetButton("Vertical"))
            {
                Theseus.Notmove();
                FindObjectOfType<MinotaurAudioManager>().StopPlaying("footsteps");
                movesound = false;
            }
        }
        if (sprintamount > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
            sprintamount = sprintamount - 5*(Time.deltaTime);
            stopper = 90;
            speedbarImage.fillAmount = sprintamount / 10;
        }
        else
        {
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? 1 : walkSpeed), ref smoothMoveVelocity, smoothTime);
        }


    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    
    void EquipItem(int _index)
    {
        
        if (_index == previousItemIndex)
            return;
        

        
        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);
        Debug.Log("cheese");

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if(PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
       if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
     
    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_Takedamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_Takedamage(float damage)
    {
        if (!PV.IsMine)
            return;

        currentHealth -= damage;
        FindObjectOfType<MinotaurAudioManager>().Play("hurt");

        healthbarImage.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();
        statuemanage.Minwin();
    }
}
