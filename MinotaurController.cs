using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using TMPro;

public class MinotaurController : MonoBehaviourPunCallbacks, IDamageable
{
        [SerializeField] Image healthbarImage;
    [SerializeField] Image speedbarImage;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] AxeAttack attack;
    StatueManager statuemanage;

    [SerializeField]public GameObject playerwin;
    [SerializeField] public TextMeshProUGUI wintext;
    int poseIndex;
    int previousPoseIndex = -1;

    bool movesound = false;

    float bowcount;

    


   public static bool work=false;
    bool sworddelay;
    float sworddelaycount;

    float sprintamount = 10;
    int stopper = 0;
    bool moving = false;
    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    const float maxHealth = 1500f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;
    public minotauranimation animate;
    public DoubleAxeAnimation axemove;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        statuemanage= GameObject.Find("Statuemanager").GetComponent<StatueManager>();
        playerwin.SetActive(false);
        wintext.enabled=false;

    }

    void Start()
    {
       playerwin.SetActive(false);
        wintext.enabled=false;

        if (PV.IsMine)
        {
            
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }

    public void statuefound(){

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
        if (sworddelaycount >=2.3)
        {
            sworddelay = false;
            sworddelaycount = 0;
            animate.Notattack();
            axemove.notattack();
        }
       

        if (!PV.IsMine)
            return;

        Look();
        Move();    
        Jump();

        if (Input.GetMouseButtonDown(0))
        {
            if (sworddelay == false)
            {
                axemove.attack();
                animate.Attack();
                attack.slash();
                sworddelay = true;
                Debug.Log("axe");
            }


        }




        if (transform.position.y < -10f)
        {
            Die();
        }




        if (work==true){
            playerwin.SetActive(false);
            wintext.enabled=false;

            work=false;
            
        }

    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetButton("Vertical") && Input.GetButton("Horizontal"))
        {
            animate.Move();
            axemove.move();
            moving = true;
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
                animate.Notmove();
                axemove.notmove();
                moving = false;
                FindObjectOfType<MinotaurAudioManager>().StopPlaying("footsteps");
                movesound = false;
            }
        }
        if (moving == false)
        {
            


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
            Debug.Log("minoju,m");
        }
    }




    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
     
    public void SetGroundedStates(bool _grounded)
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

    public void Die()
    {
        playerManager.Die();
        statuemanage.Playerwin();
    }
    public void Foundstatues(){
        playerwin.SetActive(true);
        wintext.enabled=true;
        work=true;
        Debug.Log("bingff");
        

    }
}
