using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    float speedc = 3f;
    float starty;
    public GameObject camera;
    public GameObject cam1;
    public GameObject clone;
    public GameObject Target;
    public LineRenderer line;
    public float CameraXrotation = 15f;
    Vector3 start;
    Vector3 end;
    Vector3 tmp;
    Vector3 offset;
    public bool first = true;
    public GameObject santa;
    Vector3 center;
    Rigidbody rgd;
 
   public GameObject cam2;
    public static Player Instance;
    public GameObject mainSphere ,mainCube, plane;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
        starty = transform.position.y;
        clone.transform.position = this.transform.position;
        start = Input.mousePosition;
        center = new Vector3(Screen.width/2, Screen.height/2, 0);
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!Controller.Instance.isPaused)
        {
            Transform_Player();
            Rotate_Player();
            Select_Object();
            Remove_Selection();
            Switch_Camera();
            Change_ObjectState();
        }
    }
  
    void Select_Object()
    {
        if (Input.GetKey(KeyCode.Space))
        {
             RaycastHit hit;
            line.SetPosition(0,new Vector3(transform.position.x, Target.transform.position.y, transform.position.z));
            line.SetPosition(1, new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z));
            line.enabled = true;
            if (Physics.Linecast(new Vector3 (transform.position.x, Target.transform.position.y, transform.position.z) ,new Vector3 (Target.transform.position.x, Target.transform.position.y, Target.transform.position.z), out hit))
            {
                if (hit.transform.gameObject.tag == "Obstacle")
                {

                    //Debug.Log("Oh Yeah");
                    GameObject selected = hit.transform.gameObject;
                    if (!selected.GetComponent<Obstacles>().isSelected)
                    {
                        selected.GetComponent<Obstacles>().Select();
                    }
               }
            }
        }
        else
        {
            line.enabled = false;
        }
    }
    void Remove_Selection()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Controller.Instance.RemoveSelectedObjects();
        }
    }
   
    void First_Person(float v, float h)
    {
        camera.transform.Rotate(new Vector3(-v, h, 0));
     
        float z = camera.transform.eulerAngles.z;

        if (z > 0)
        {
            camera.transform.Rotate(new Vector3(0, 0, -z));

        }

        else
        {
            camera.transform.Rotate(new Vector3(0, 0, z));

        }
           transform.rotation = camera.transform.rotation;
        float x =transform.eulerAngles.x;

        if (x > 0)
        {
          transform.Rotate(new Vector3(-x, 0, 0));

        }

        else
        {
          transform.Rotate(new Vector3(x, 0, 0));

        }
     
        z = camera.transform.eulerAngles.z;
     
            clone.transform.rotation = transform.rotation;
        z = clone.transform.eulerAngles.z;

        if (z > 0)
        {
            clone.transform.Rotate(new Vector3(0, 0, -z));
        }
        else
        {
            clone.transform.Rotate(new Vector3(0, 0, z));

        }

    }
    void Third_Person(float v  , float h)
    {


        cam1.transform.Rotate(new Vector3(0, h, 0));
  
        transform.Rotate(new Vector3(0, h, 0));

        float x = transform.eulerAngles.x;

        if (x > 0)
        {
            transform.Rotate(new Vector3(-x, 0, 0));

        }

        else
        {
            transform.Rotate(new Vector3(x, 0, 0));

        }
        camera.transform.rotation = santa.transform.rotation;
        camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x + CameraXrotation, camera.transform.eulerAngles.y, camera.transform.eulerAngles.z);
       camera.transform.position = new Vector3(transform.position.x - santa.transform.forward.x*3 ,camera.transform.position.y*3 , transform.position.z - santa.transform.forward.z*3);
        float w = camera.transform.eulerAngles.z;

        if (w> 0)
        {
            camera.transform.Rotate(new Vector3(0, 0, -w));

        }
        else
        {
            camera.transform.Rotate(new Vector3(0, 0, w));

        }
        clone.transform.rotation = transform.rotation;
        float z = clone.transform.eulerAngles.z;

        if (z > 0)
        {
            clone.transform.Rotate(new Vector3(0, 0, -z));
        }
        else
        {
            clone.transform.Rotate(new Vector3(0, 0, z));

        }


    }
    void Change_ObjectState()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Controller.Instance.moveObject)
            {
                Controller.Instance.moveObject = false;
            }
            else
            {
                Controller.Instance.moveObject = true;

            }
        }
    }
    void Rotate_Player()
    {

       
        float h = Input.GetAxis("Mouse X") * speedc;
        float v = Input.GetAxis("Mouse Y") * speedc;
      
        if (first)
        {
            First_Person(v, h);
        }
        else
        {
            Third_Person(v, h);
        }
                   
    }
    void FixedUpdate()
    {

     }
    void Switch_Camera()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {

            if (first)
            {
                first = false;


                cam2.SetActive(true);
                cam2.transform.eulerAngles = new Vector3(cam2.transform.eulerAngles.x, camera.transform.eulerAngles.y, camera.transform.eulerAngles.z);
               camera = cam2;
                cam1.SetActive(false);
            }
            else
            {
                first = true;
                cam1.SetActive(true);
                cam1.transform.eulerAngles = new Vector3(cam1.transform.eulerAngles.x, camera.transform.eulerAngles.y, camera.transform.eulerAngles.z);
               camera = cam1;
                cam2.SetActive(false);
            }
        }

    }
    void Transform_Player()
    {


            if (Input.GetKey(KeyCode.W))
        {

            clone.gameObject.GetComponent<CharacterController>().Move(clone.transform.forward*speed*Time.deltaTime);
            transform.position = new Vector3(clone.transform.position.x, starty, clone.transform.position.z);

        }
        if (Input.GetKey(KeyCode.A))
        {


            clone.gameObject.GetComponent<CharacterController>().Move(-clone.transform.right * speed*Time.deltaTime);
            Debug.Log(santa.transform.right * speed * Time.deltaTime);
            transform.position = new Vector3(clone.transform.position.x, starty, clone.transform.position.z);

        }
        if (Input.GetKey(KeyCode.D))
        {

            clone.gameObject.GetComponent<CharacterController>().Move(clone.transform.right * speed * Time.deltaTime);
            Debug.Log(santa.transform.right * speed * Time.deltaTime);
            transform.position = new Vector3(clone.transform.position.x, starty, clone.transform.position.z);
         
        }
        if (Input.GetKey(KeyCode.S))
        {


            clone.gameObject.GetComponent<CharacterController>().Move(-clone.transform.forward * speed * Time.deltaTime);

            transform.position = new Vector3(clone.transform.position.x, starty, clone.transform.position.z);

        }
        clone.transform.position = transform.position;

        if (first)
        {
            camera.transform.position = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
        }
        else
        {
            camera.transform.position = new Vector3(santa.transform.position.x - santa.transform.forward.x, santa.transform.position.y - santa.transform.forward.y, santa.transform.position.z - santa.transform.forward.z);
          
        }

    }
}
