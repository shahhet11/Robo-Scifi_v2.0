using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShootWeapons : MonoBehaviour
{
   
   
    public Transform[] ShootPosition;
    public GameObject Bullet;
    public GameObject Shieldprefab;
    public float speed = 1000;
    public float fireRate;
    private GameObject projectile;
    private float nextFire = 0.0f;
    Camera viewCamera;
    //public PhotonView PV;
    public Transform rayOrigin;
    Vector3 RemoteShootLookAt;
    Vector3 RemoteShieldpos;
    Transform RemoteShieldtrans;
    public GameObject Shield;
    public bool ShieldControl;
    private Animation anim;
    public GameObject ShieldGuard;
    public Animation shieldanim;
    string shieldname = "Shield";
    public Image EnergyBar;
    public Text EnergyText;
    public bool fake_var;
    public bool shield_off;
    AsyncOperation ASY;
   public float COUNTER = 100f;
    [Header("FPS")]
    public GameObject FPSgraph;
    // Start is called before the first frame update
    void Start()
    {
        
        viewCamera = Camera.main;
       

    }

    void Update()
    {
        if (PowerManager.isIndicatorOn == false)
        {
            if (Input.GetMouseButton(0) && Time.realtimeSinceStartup > nextFire)
            {
                nextFire = Time.realtimeSinceStartup + fireRate;

                for (int i = 0; i < ShootPosition.Length; i++)
                {
                    projectile = Instantiate(Bullet, ShootPosition[i].position, ShootPosition[i].rotation) as GameObject;
                    projectile.GetComponent<Rigidbody>().velocity = ShootPosition[i].forward * speed;
                }
            }
        }
        //if (Input.GetMouseButton(0) && Time.time > nextFire)
        //{
        //    FIRE();
        //}

        if (Input.GetKey(KeyCode.E) && COUNTER > 0)
        {
            ShieldOpen();
            PlayerHealth.ph_instance.shieldON = true;

        }
        else if (Input.GetKeyUp(KeyCode.E) && PlayerHealth.ph_instance.shieldON)
        {
            ShieldClose();
            PlayerHealth.ph_instance.shieldON = false;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            FPSgraph.SetActive(!FPSgraph.activeSelf);
        }

        ShieldFillBar();
    }

   
    void ShieldOpen()
    {
        
        if (!ShieldControl)
        {
            Debug.Log("ShieldOpen");
            Shield = Instantiate(Shieldprefab, transform.position, transform.rotation) as GameObject;
            anim = Shield.GetComponent<Animation>();
            //anim["ShieldOpen"].speed = 1;
            //anim["ShieldOpen"].time = anim["ShieldOpen"].length;
            anim.Play("ShieldOpen");
            Vector3 temp = Shield.gameObject.transform.position; // transform.position;
            Debug.Log(temp);
            Shield.transform.SetParent(this.transform);
            Shield.transform.localPosition = new Vector3(-0.19f, 0.45f, -1.03f);
            temp = new Vector3(0, 0.72f, -1.1f); //Shield.transform;
            Shield.transform.localPosition = temp;

            ShieldControl = true;

            StartCoroutine(StartCounterUse());
            fake_var = false;
            if (COUNTER <= 0f)
            {
                COUNTER = 0f;
                //ShieldGuard.SetActive(false);
            }
            else
            {
                //ShieldGuard.SetActive(true);
            }

        }
        
    }
    
    void ShieldClose()
    {
        
            anim = Shield.GetComponent<Animation>();
            anim["ShieldOpen"].speed = -1;
            anim["ShieldOpen"].time = anim["ShieldOpen"].length;
            anim.Play("ShieldOpen");
           
            ShieldControl = false;
            Destroy(Shield ,1.0f);

            Debug.Log("ShieldClose");
            //ShieldGuard.SetActive(false);
            //shieldanim[shieldname].speed = -1;
            //shieldanim[shieldname].time = shieldanim[shieldname].length; 
            //shieldanim.Play();
            //Invoke("DisableShield", 0.9f);
            StartCoroutine(StartCounterFill());
            fake_var = true;
            Debug.Log("ShieldClose1");

    }
    void ShieldFillBar()
    {
        

            if (fake_var == true)
            {
                if (COUNTER >= 0 || COUNTER <= 100f)
                {
                    if (COUNTER > 100f)
                    {
                        fake_var = false;
                    }
                    StartCoroutine("StartCounterFill");
                }
            }

            if (Input.GetKeyDown(KeyCode.E))//&& shieldanim[shieldname].speed == 2
            {
                Debug.Log("LOOL");
                //ShieldGuard.SetActive(true);

                //shieldanim[shieldname].speed = 2;
                //shieldanim.Play();
                //ShieldGuard.GetComponent<ParticleSystem>().playbackSpeed = 2.0f;

            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log("LOOL11");
                //ShieldGuard.SetActive(false);
                //shieldanim[shieldname].speed = -1;
                //shieldanim[shieldname].time = shieldanim[shieldname].length; 
                //shieldanim.Play();
                //Invoke("DisableShield", 0.9f);
                StartCoroutine("StartCounterFill");
                fake_var = true;
                shield_off = false;
            }

            if (Input.GetKey(KeyCode.E))
            {
                StartCoroutine("StartCounterUse");
                fake_var = false;
                
                if (COUNTER <= 0f)
                {
                    Debug.Log("COUNTER0<");
                    COUNTER = 0f;
                //ShieldGuard.SetActive(false);
                }
                if (!shield_off && COUNTER == 0)
                {
                    shield_off = true;
                    //Invoke("DisableShield", 0.9f);
                    //StartCoroutine("StartCounterFill");
                    //fake_var = true;
                    Debug.Log("shield_off");
                    ShieldClose();
                    PlayerHealth.ph_instance.shieldON = false;
                    //ShieldGuard.SetActive(false);
            }
            

            }
        
    }
    IEnumerator StartCounterUse()
    {
        yield return new WaitForSeconds(0.00015f);
        Debug.Log("FILlIng1");
        COUNTER -= Time.deltaTime * 15f;
        EnergyBar.fillAmount = COUNTER / 100f;

        EnergyText.text = COUNTER.ToString("00");

    }
    IEnumerator StartCounterFill()
    {
        yield return new WaitForSeconds(0.00015f);
        Debug.Log("FILlIng");
        COUNTER += Time.deltaTime * 10f;
        EnergyBar.fillAmount = COUNTER / 100f;

        EnergyText.text = COUNTER.ToString("00");
    }
    public Animation fireJoyStickAnimation;
    void FIRE()
    {
        Debug.Log("FIRED");
     
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000f) && ShootPosition.Length > 0)
            {
            //PV.RPC("FIREREMOTE", RpcTarget.All, hit.point);
            Debug.Log(Camera.main.ScreenPointToRay(Input.mousePosition)+ "ScreenPointToRay(Input.mousePosition");
                nextFire = Time.time + fireRate;
            fireJoyStickAnimation.Play();
                for (int i = 0; i < ShootPosition.Length; i++)
                {
                    projectile = Instantiate(Bullet, ShootPosition[i].position, ShootPosition[i].rotation) as GameObject;
                    projectile.transform.LookAt(hit.point);
                   
                    projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
                }

            }
        
      
        //RaycastHit hit2;

        //if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit2, 1000))
        //{
        //    nextFire = Time.time + fireRate;

        //        projectile = Instantiate(Bullet, rayOrigin.position, rayOrigin.rotation) as GameObject;
        //        projectile.transform.LookAt(hit.point);
        //        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);

        //}

    }

    //void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(speed);
    //    }
    //    else
    //    {

    //    }
    //}

    //[PunRPC]
    //void FIREREMOTE(Vector3 lookatrpc)
    //{
    //    RemoteShootLookAt = lookatrpc;
    //}
    //[PunRPC]
    //void SHIELDREMOTE(Vector3 lookatrpc)
    //{
    //    RemoteShieldpos = lookatrpc;
    //}

}
