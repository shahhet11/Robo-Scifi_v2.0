using Devdog.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerMove : MonoBehaviour
{
  
    public ParticleSystem dashParticles;
    [Header("JETPACK")]
    public float speed = 1f;
    public float timetoInit = 0f;
    //public float timetoInit = 0f;
    public float colorDuration = 2f;
    public Color blackColor = new Color32(0, 0, 0, 255);
    public Color greenColor = new Color32(0, 108, 5, 255);
    public Color redColor = new Color32(212, 6, 0, 255);
    public bool repeatable = false;
    public float startTime;
    public float timeLeft = 20f;
    public Material jetPackDesign;
    public bool JetPackHit = false;
    public bool JetPackTimerStart = false;
    public bool JetPackAllowed = true;
    public ParticleSystem[] JetParticles;
    public UnityEngine.UI.Slider JetPackSlider;
    //public GameObject JetFlame;
    //public GameObject[] ignitefrom;
    public bool isJetPackFlameOn = false;
    public AudioSource JetSfx;
    public AudioClip[] JetTransitionClips;
    public Transform PlayerHead;
    public Transform PlayerHeadOriginal;

    //public PhotonView PV;
    private CharacterController characterController;
	private Camera cam;

    public Transform compassReferenceDirection; // North of Compass
    public Text CompassAngle;
    public float gravity = 14.0f;
	public float movementSpeed;
    public float additionalSpeed;
	public float jumpForce = 10.0f;
	public float rotationSpeed = 450f;
    public float dashEffectRate;

    public float dashSpeed;
    public float maxDashTime;
    public float dashStoppingSpeed;
    public bool isHoldingShift;
    private float verticalVelocity;
    private float currentDashTime;
    private float defaultMoveSpeed;

	private Vector3 moveVector;
	private Vector3 movementTemp;
	private Quaternion targetRotation;
	private int floorMask;
    public Rigidbody Playerrig;
    bool Inair;
    string sceneName;
    public Transform playersObjectsPositionsRoot;
    public ShootWeapons ShootWeapons;
    
    void Start ()
	{
        Scene currentScene = SceneManager.GetActiveScene();

         sceneName = currentScene.name;

        if (sceneName == "MultiPlayerScene")
        {
           
        //PV = this.GetComponentInParent<PhotonView>();
           
           
            Invoke("SetValues",1);
           
        }

        characterController = GetComponent<CharacterController>();
		cam = Camera.main;
		floorMask = LayerMask.GetMask ("Ground");

        defaultMoveSpeed = movementSpeed;
        currentDashTime = maxDashTime;
    }

    void SetValues()
    {

      //  PlayerHead = playersObjectsPositionsRoot.transform.GetChild(4).Find("Head");
        dashParticles = playersObjectsPositionsRoot.transform.GetChild(4).Find("Dash Particles").gameObject.transform.GetComponent<ParticleSystem>();

        //if (this.PV.IsMine)
        //{
        WeaponLookAt.weaponLook.PlayerGuns[0] = this.playersObjectsPositionsRoot.transform.GetChild(4).Find("AllGuns");
        //}
        CameraController.CamContr.transform.GetComponent<CrosshairDemoPlayerRecoil>().Projectile = playersObjectsPositionsRoot.transform.GetChild(4).Find("Guns").gameObject;
        CameraController.CamContr.transform.GetComponent<CrosshairDemoPlayerRecoil>().Gun = playersObjectsPositionsRoot.transform.GetChild(4).Find("Guns");
        CameraController.CamContr.transform.GetComponent<CrosshairDemoPlayerRecoil>().Aim = playersObjectsPositionsRoot.transform.GetChild(4).Find("Guns");
        ShootWeapons.ShootPosition[0] = playersObjectsPositionsRoot.transform.GetChild(4).Find("Head").GetChild(1).GetChild(4).GetChild(0);
        ShootWeapons.rayOrigin = ShootWeapons.ShootPosition[0];
       // PlayerHeadOriginal = playersObjectsPositionsRoot.transform.GetChild(4).Find("Head");
    }

	private void Update()
	{
        if (isHoldingShift == false)
        {
            Move_Character();
        }
        if(!GameManager.Instance.isPaused)
            Mouse_Turning();
        CompassAngleCalculator();
        JetPackNew();
        Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.yellow);
        //Debug.DrawRay(JetFlame.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.red);
    }

   
     void Move_Character()
	{
       
        if (!characterController.isGrounded)
        {
            
            verticalVelocity -= gravity * Time.deltaTime;
            movementSpeed = 20;
        }
        else
        {
            movementSpeed = 10;
        }

        //JetPack Purpose
        //if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && transform.position.y < 20.01f)
        //{
        //    Vector3 JetVelocity = new Vector3(Playerrig.velocity.x,  15 , Playerrig.velocity.z);
        //    //JetVelocity.y += 0.002f;
        //    Playerrig.velocity = JetVelocity;
        //    verticalVelocity = JetVelocity.y;
        //    moveVector.y = verticalVelocity;
        //    //moveVector = Vector3.zero;
        //    //moveVector.y = verticalVelocity;
            
        //    if (!isJetPackFlameOn) {

        //        for (int i = 0; i < JetParticles.Length; i++)
        //        {
        //            JetParticles[i].Play();
        //        }
        //        JetStartSound();
        //        //GameObject Flame = Instantiate(JetFlame, ignitefrom[0].transform.position, Quaternion.identity);
        //        //Flame.transform.rotation = Quaternion.Euler(180, Flame.transform.rotation.y, Flame.transform.rotation.z);
        //        //Flame.transform.SetParent(ignitefrom[0].transform);
        //        //GameObject Flame1 = Instantiate(JetFlame, ignitefrom[1].transform.position, Quaternion.identity);
        //        //Flame1.transform.rotation = Quaternion.Euler(180, Flame1.transform.rotation.y, Flame1.transform.rotation.z);
        //        //Flame1.transform.SetParent(ignitefrom[1].transform);

        //        isJetPackFlameOn = true;
        //    }

        //}
        //else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
        //    //moveVector.y = verticalVelocity;
        //    Vector3 JetVelocity = new Vector3(Playerrig.velocity.x, -5f, Playerrig.velocity.z);
        //    //JetVelocity.y += 0.002f;
        //    Playerrig.velocity = JetVelocity;
        //    verticalVelocity = JetVelocity.y;
        //    moveVector.y = verticalVelocity;
        //}
        //else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) {
        //    for (int i = 0; i < JetParticles.Length; i++)
        //    {
        //        JetParticles[i].Stop();
        //    }
            
        //    JetStopSound();
        //}
        //else
        //{
        //    moveVector = Vector3.zero;
        //    moveVector.y = verticalVelocity;
        //    isJetPackFlameOn = false;
            
        //}
       

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentDashTime = 0.0f;
        }

        if(currentDashTime < maxDashTime)
        {
            currentDashTime += dashStoppingSpeed;

            var em = dashParticles.emission;
            em.rateOverDistance = dashEffectRate;

            moveVector.x = Input.GetAxisRaw("Horizontal") * dashSpeed * Time.deltaTime * 2f;
            moveVector.z = Input.GetAxisRaw("Vertical") * dashSpeed * Time.deltaTime * 2f;

            characterController.Move(moveVector);
        }
        else
        {
            var em = dashParticles.emission;
            em.rateOverDistance = 0f;

            moveVector.x = Input.GetAxis("Horizontal") * movementSpeed;
            moveVector.z = Input.GetAxis("Vertical") * movementSpeed;
            

            characterController.Move(moveVector * Time.deltaTime);
        }

		movementTemp = new Vector3(moveVector.x, 0.0f, moveVector.z);
	}

    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] public Transform aimTransform;
    [SerializeField] public Transform joystickRelative;
    void Mouse_Turning()
	{

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, 100f))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            
            PlayerHead.rotation = newRotation;
            //Debug.Log(PlayerHead.localRotation.y+ "PlayerHead.rotation");
            //joystickRelative.position = Input.mousePosition;
            //joystickRelative.rotation = Quaternion.Euler(0, 0, newRotation.z);
            // PlayerHeadOriginal.localRotation = newRotation;
        }

        //Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        //Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        //if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        //{
        //    aimTransform.position = raycastHit.point;
        //}



    }
    IEnumerator ChangeColor()
    {

        while (timetoInit < colorDuration)
        {
            timetoInit += Time.deltaTime;
            //Debug.Log("ChangeColor");
            if (colorDuration == 30)
            {
                jetPackDesign.color = Color.Lerp(greenColor, redColor, timetoInit / colorDuration);
            }
            else
            {
                jetPackDesign.color = Color.Lerp(blackColor, greenColor, timetoInit / colorDuration);
            }
            yield return null;
        }
    }
    void JetPackNew()
    {

        if (timeLeft < 0)
        {
            Debug.Log("TIMELEFT OVER");
            //JetPackHit = false;
            //float t = (Time.time - startTime) * speed;
            //jetPackDesign.color = Color.Lerp(greenColor, blackColor, 1f);
            StopAllCoroutines();
            //timetoInit = 0f;
            JetPackAllowed = false;
            JetPackTimerStart = false;
            JetPackHit = false;
            float t = (Time.time - startTime) * speed;
            jetPackDesign.color = Color.Lerp(redColor, blackColor, 1f);
            timetoInit = 0f;
            colorDuration = 2f;
            timeLeft = 5f;
        }

        if (!JetPackAllowed)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                //Debug.Log("afefegewsrgewwa");
                JetPackAllowed = true;
                timeLeft = 30;
            }
        }

        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            //movementSpeed = 20;
            movementSpeed = 5;
        }
        else
        {
            movementSpeed = 2;
            if (jetPackDesign.color != blackColor)
            {
                //Debug.Log("afefewa");
                float t = (Time.time - startTime) * speed;
                jetPackDesign.color = Color.Lerp(jetPackDesign.color, blackColor, t);
                timetoInit = 0f;
                timeLeft = 30;
                
            }

            //movementSpeed = 10;
        }

        //JetPack Purpose
        if (Input.GetKey(KeyCode.LeftShift) && JetPackAllowed)//&& characterController.isGrounded
        {
            if (!JetPackHit)
            {
                StartCoroutine(ChangeColor());
                JetPackHit = true;
            }
            if (timetoInit > colorDuration && !JetPackTimerStart)
            {
                timetoInit = 0;
                colorDuration = 30;
                Debug.Log("aferwgtrwetrewfewa");
                //timeLeft = colorDuration;
                JetPackTimerStart = true;
            }
            if (timetoInit > colorDuration && JetPackTimerStart)
            {
                JetPackHit = false;
                JetPackTimerStart = false;
                float t = (Time.time - startTime) * speed;
                jetPackDesign.color = Color.Lerp(redColor, blackColor, 1f);
                timetoInit = 0f;
                colorDuration = 2f;

            }
            timeLeft -= Time.deltaTime;
            JetPackSlider.value = timeLeft/30;
            
            if (transform.position.y > 10f)
            {
                moveVector = Vector3.zero;
                moveVector.y = verticalVelocity * 0.05f;
                //Debug.Log("moveVectorIF" + verticalVelocity);
            }
            else
            {
                //Debug.Log("moveVectorELSE" + transform.position.y);
                //Vector3 JetVelocity = new Vector3(Playerrig.velocity.x, 11, Playerrig.velocity.z);
                //Debug.Log("JetVelocity " + JetVelocity);
                //Playerrig.velocity = JetVelocity;
                verticalVelocity = 5;
                moveVector.y = verticalVelocity;



                moveVector = Vector3.zero;
                moveVector.y = verticalVelocity;
            }
            if (!isJetPackFlameOn)
            {

                for (int i = 0; i < JetParticles.Length; i++)
                {
                    JetParticles[i].Play();
                }
                JetStartSound();

                isJetPackFlameOn = true;
            }


        }
        //else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        //{
        //    //moveVector.y = verticalVelocity;
        //    Vector3 JetVelocity = new Vector3(Playerrig.velocity.x, -5f, Playerrig.velocity.z);
        //    //JetVelocity.y += 0.002f;
        //    Playerrig.velocity = JetVelocity;
        //    verticalVelocity = JetVelocity.y;
        //    moveVector.y = verticalVelocity;
        //}
        else
        {
            moveVector = Vector3.zero;
            moveVector.y = verticalVelocity;
            if (isJetPackFlameOn)
            {
                for (int i = 0; i < JetParticles.Length; i++)
                {
                    JetParticles[i].Stop();
                }
                StopCoroutine(ChangeColor());
                JetStopSound();
                isJetPackFlameOn = false;
                JetPackHit = false;
                //float t = (Time.time - startTime) * speed;
                //timetoInit = 0;
                //colorDuration = 2;
                //startTime = 0;
                //timeLeft = 20;
                Debug.Log("JETBLACK");
                JetPackTimerStart = false;
                colorDuration = 2;
                //timetoInit = 0f;
                //timeLeft = 30;
                jetPackDesign.color = blackColor;
            }
            if (timeLeft <= 30f)
            {
                timeLeft += Time.deltaTime;
                JetPackSlider.value = timeLeft / 30;
                timetoInit -= Time.deltaTime;
            }
           

        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentDashTime = 0.0f;
        }

        if (currentDashTime < maxDashTime)
        {
            //Debug.Log("MOVE2");

            currentDashTime += dashStoppingSpeed;


            moveVector.x = Input.GetAxisRaw("Horizontal") * dashSpeed * Time.deltaTime;
            moveVector.z = Input.GetAxisRaw("Vertical") * dashSpeed * Time.deltaTime;

            characterController.Move(moveVector);
        }
        else
        {
            //Debug.Log("MOVE3");

            moveVector.x = Input.GetAxis("Horizontal") * movementSpeed;
            moveVector.z = Input.GetAxis("Vertical") * movementSpeed;


            characterController.Move(moveVector * Time.deltaTime);
        }

        movementTemp = new Vector3(moveVector.x, 0.0f, moveVector.z);


    }
    void CompassAngleCalculator()
    {
        //Debug.Log("rotation:" + PlayerHead.rotation.y);
        //Debug.Log("localRotation:" + PlayerHead.localRotation.y);
        float angle = PlayerHead.rotation.eulerAngles.y;
        string compassDirection = GetCompassDirection(angle);
        CompassAngle.text = PlayerHead.rotation.eulerAngles.y.ToString("00")+ "° " + compassDirection;
    }
    string GetCompassDirection(float angle)
    {
        if (angle >= 294 && angle < 337)
        {
            return "NW";
        }
        else if ((angle >= 338 && angle < 359) || (angle >= 0 && angle < 22))
        {
            return "N";
        }
        else if (angle >= 23 && angle < 67)
        {
            return "NE";
        }
        else if (angle >= 68 && angle < 112)
        {
            return "E";
        }
        else if (angle >= 113 && angle < 156)
        {
            return "SE";
        }
        else if (angle >= 157 && angle < 201)
        {
            return "S";
        }
        else if (angle >= 157 && angle < 201)
        {
            return "S";
        }
        else if (angle >= 202 && angle < 246)
        {
            return "SW";
        }
        else if (angle >= 247 && angle < 292)
        {
            return "W";
        }
        return "";
       
    }
    /// <summary>
    /// Experimentation for Compass Angle degree calculator
    /// </summary>
    //private void CompassDirection2()
    //{
    //    // Get the mouse position in screen coordinates
    //    Vector3 mouseScreenPosition = Input.mousePosition;

    //    // Convert the mouse position to world coordinates
    //    Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, cam.nearClipPlane));

    //    // Calculate the direction vector from the compass center to the mouse position
    //    Vector3 directionToMouse = mouseWorldPosition - compassReferenceDirection.position;

    //    // Calculate the angles between the direction vector and the reference directions
    //    float angleN = Vector3.SignedAngle(Vector3.forward, directionToMouse, Vector3.up);
    //    float angleE = Vector3.SignedAngle(Vector3.right, directionToMouse, Vector3.up);
    //    float angleS = Vector3.SignedAngle(-Vector3.forward, directionToMouse, Vector3.up);
    //    float angleW = Vector3.SignedAngle(-Vector3.right, directionToMouse, Vector3.up);

    //    // Adjust angles to be positive and fit within [0, 360] degrees
    //    angleN = (angleN + 360) % 360;
    //    angleE = (angleE + 360) % 360;
    //    angleS = (angleS + 360) % 360;
    //    angleW = (angleW + 360) % 360;

    //    // Print or use the angles as needed
    //    Debug.Log("Angle N: " + angleN);
    //    Debug.Log("Angle E: " + angleE);
    //    Debug.Log("Angle S: " + angleS);
    //    Debug.Log("Angle W: " + angleW);
    //}
    //public Vector3 referenceDirection = Vector3.forward;
    //void CompassDirection1()
    //{
    //    // Get mouse position in screen coordinates
    //    Vector3 mouseScreenPosition = Input.mousePosition;

    //    // Convert mouse position to world coordinates
    //    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    //    mouseWorldPosition.y = 0; // Assuming your compass is flat on the ground

    //    // Calculate direction from reference to mouse
    //    Vector3 directionToMouse = mouseWorldPosition - transform.position;

    //    // Calculate angle between reference direction and mouse direction
    //    float angle = Vector3.SignedAngle(referenceDirection, directionToMouse, Vector3.up);

    //    // Ensure the angle is positive
    //    if (angle < 0)
    //    {
    //        angle += 360;
    //    }

    //    // Map the angle to compass directions
    //    string compassDirection = GetCompassDirection(angle);

    //    Debug.Log("Angle: " + angle + " degrees, Compass Direction: " + compassDirection);
    //}

    //private void newCompassDegree()
    //{

    //    Vector3 mousePosition = Input.mousePosition;
    //    mousePosition.z = cam.nearClipPlane; // Set the depth to the near clip plane

    //    Vector3 worldMousePosition = cam.ScreenToWorldPoint(mousePosition);

    //    // Calculate the angle relative to the positive Y-axis (North)
    //    float angleRadians = Mathf.Atan2(worldMousePosition.x, worldMousePosition.z);
    //    float angleDegrees = angleRadians * Mathf.Rad2Deg;
    //    angleDegrees = (angleDegrees + 360) % 360; // Adjust the range to 0° to 360°

    //    Debug.Log("Compass Degree relative to North: " + angleDegrees);

    //}
    //void newCompass()
    //{
    //    Vector3 mouseScreenPosition = Input.mousePosition;
    //    Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, cam.transform.position.y));

    //    Vector3 playerToMouse = mouseWorldPosition - PlayerHead.position;
    //    Vector3 northDirection = Vector3.forward; // Assuming your north direction is along the Z-axis

    //    float angle = Vector3.SignedAngle(northDirection, playerToMouse, Vector3.up);
    //    CompassAngle.text = angle.ToString();
    //    // The 'angle' variable now contains the angle between the north direction and the direction from the player to the mouse.
    //    //Debug.Log("Angle: " + angle);
    //}
    //void CompassDirection()
    //{
    //    // Get the mouse position in screen coordinates
    //    Vector3 mouseScreenPosition = Input.mousePosition;

    //    // Convert the mouse position to world coordinates
    //    Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, cam.nearClipPlane));

    //    // Calculate the angle between the reference direction and the mouse position
    //    Vector3 directionToMouse = mouseWorldPosition - compassReferenceDirection.position;
    //    float angle = Mathf.Atan2(directionToMouse.x, directionToMouse.y) * Mathf.Rad2Deg;

    //    // Adjust the angle to be positive and fit within [0, 360] degrees
    //    if (angle < 0)
    //    {
    //        angle += 360;
    //    }
    //    CompassAngle.text = angle.ToString();
    //    // Print or use the angle as needed
    //    //Debug.Log("Angle: " + angle);
    //}

    //public float offsetAngle = 0f; // Offset angle if needed

    //void compassneww()
    //{
    //    Vector3 mouseScreenPos = Input.mousePosition;

    //    // Convert mouse screen position to a ray in world space
    //    Ray mouseRay = cam.ScreenPointToRay(mouseScreenPos);

    //    // Calculate the angle between the mouse direction and world north direction
    //    float angle = Vector3.SignedAngle(Vector3.forward, mouseRay.direction, Vector3.up) + offsetAngle;
    //    CompassAngle.text = angle.ToString();
    //    // Rotate the player character to face the mouse direction
    //    //PlayerHead.rotation = Quaternion.Euler(0f, angle, 0f);

    //}
    void JetStartSound() {
        if(JetSfx.loop)
            JetSfx.loop = false;
        JetSfx.clip = JetTransitionClips[0];
        JetSfx.Play();
        Invoke("JetFlameContinuity",0.9f);
    }
    void JetFlameContinuity() {
        JetSfx.clip = JetTransitionClips[2];
        JetSfx.loop = true;
        JetSfx.Play();
    }
    void JetStopSound() {
        if (JetSfx.loop)
            JetSfx.loop = false;
        CancelInvoke("JetFlameContinuity");
        JetSfx.clip = JetTransitionClips[1];
        JetSfx.Play();
    }
    private void Keys_Turning()
	{
		if(movementTemp != Vector3.zero)
		{
			targetRotation = Quaternion.LookRotation(movementTemp);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		}
	}
}
