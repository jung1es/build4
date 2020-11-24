using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Camera_Controller : MonoBehaviour
{

 

    public float Normal_Speed = 25.0f; //Normal movement speed
   
    public float Shift_Speed = 54.0f; //multiplies movement speed by how long shift is held down.
   
    public float Speed_Cap = 54.0f; //Max cap for speed when shift is held down
  
    public float Camera_Sensitivity = 0.6f; //How sensitive it with mouse
   
    private Vector3 Mouse_Location = new Vector3(255, 255, 255); //Mouse location on screen during play (Set to near the middle of the screen)
    
    private float Total_Speed = 1.0f; //Total speed variable for shift
    public bool useLeftClick = false;

    private PhotonView _pv;
    [SerializeField]
    private GameObject _cameraObject;

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }


    private void Start()
    {
        if(!_pv.IsMine)
            _cameraObject.SetActive(false);
    }

    void Update()
    {
        if(!_pv.IsMine) return;


        if (!useLeftClick&&Input.GetMouseButton(1))
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 150 * Time.deltaTime;
            float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * 150 * Time.deltaTime;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }
        else if (useLeftClick&&Input.GetMouseButton(0))
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 150 * Time.deltaTime;
            float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * 150*Time.deltaTime;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }
        //Camera angles based on mouse position
        




        
        //Keyboard controls
       
        Vector3 Cam = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {


            Total_Speed += Time.deltaTime;
           
            Cam = Cam * Total_Speed * Shift_Speed;
           
            Cam.x = Mathf.Clamp(Cam.x, -Speed_Cap, Speed_Cap);
           
            Cam.y = Mathf.Clamp(Cam.y, -Speed_Cap, Speed_Cap);
           
            Cam.z = Mathf.Clamp(Cam.z, -Speed_Cap, Speed_Cap);



        }
        else
        {
           
            
            Total_Speed = Mathf.Clamp(Total_Speed * 0.5f, 1f, 1000f);
           
            Cam = Cam * Normal_Speed;


        }

        Cam = Cam * Time.deltaTime;
        
        Vector3 newPosition = transform.position;
       
        if (Input.GetKey(KeyCode.Space))
        {


            //If the player wants to move on X and Z axis only by pressing space (good for re-adjusting angle shots)
            transform.Translate(Cam);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;


        }
        else
        {


            transform.Translate(Cam);


        }

    }

    private Vector3 GetBaseInput()
    {   


        Vector3 Camera_Velocity = new Vector3();
        
        float HorizontalInput = Input.GetAxis("Horizontal"); //Input for horizontal movement
        
        float VerticalInput = Input.GetAxis("Vertical"); //Input for Vertical movement
        


        Camera_Velocity += new Vector3(HorizontalInput, 0, 0);

        Camera_Velocity += new Vector3(0, 0, VerticalInput);
       
        return Camera_Velocity;


    }
}