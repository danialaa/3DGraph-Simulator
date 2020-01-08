using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Controller : MonoBehaviour
{
    public Material selectionMaterial;
    public List<GameObject> selectedObjects;
    public static Controller Instance;
   public List<GameObject> spheres = new List<GameObject>();
 public   List<GameObject> cubes = new List<GameObject>();
    public List<GameObject> allObjects = new List<GameObject>();
    public bool moveObject = true ;
    public List<Line> lines = new List<Line>();
    public GameObject pointer;
    public GameObject connection;
    public List<Material> materials;
    public int[,] Tiles = new int[10, 10];
    public int scale = 10;
    public float planeY = 0.6f;
    public bool isPaused = false;
    public Button Instructions;
    public Button close;
    public Canvas World;
    public int SelectionIncrement;
    public GameObject Panel;
    public GameObject mainSphere, mainCube;
    List<int> sphereDirection, cubeDirection;
    public GameObject plane;

    float speed = 10f;
    public int horizontalLength = 90, verticalLength = 90;
    public int startYLength = 0, startXLength = 0;
    // Start is called before the first frame update
    void Start()
    {
       if(Instance != null)
        {
            Destroy(Instance);
        }
       else
        {
            Instance = this;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Controller.Instance.spheres = new List<GameObject>();
        Controller.Instance.cubes = new List<GameObject>();
        sphereDirection = new List<int>();
        cubeDirection = new List<int>();
        Randmoize_Positions();
        addObjects(Controller.Instance.Tiles, 5);
        Instructions.onClick.AddListener(OpenInstructions);
        close.onClick.AddListener(CloseInstructions);
    }
    void OpenInstructions()
    {
        Instructions.gameObject.SetActive(false);
        Panel.SetActive(true);
    }
    void CloseInstructions()
    {

        Instructions.gameObject.SetActive(true);
        Panel.SetActive(false);
    }
    void Randmoize_Positions()
    {

        for (int r = 0; r < Controller.Instance.Tiles.GetLength(0); r++)
        {
            for (int c = 0; c < Controller.Instance.Tiles.GetLength(1); c++)
            {
                Controller.Instance.Tiles[r, c] = Random.Range(0, 3);
            }
        }
    }
    void Update_Selection()
    {

        for (int i = 0; i  < lines.Count; i++)
        {

            for (int k = 0; k< lines[i].positions.Count; k++)
            {
                lines[i].line.SetPosition(k, lines[i].positions[k].transform.position);
            
            }
        }

    }
    void RemoveLink()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bool contains = true;
            int pos = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                contains = true;
                for (int k = 0; k < selectedObjects.Count; k++)
                {
                    if (!lines[i].positions.Contains(selectedObjects[k]))
                    {
                        contains = false;

                    }

                }
                if (contains)
                {
                    pos = i;
                    break;
                }
            }
            if (contains)
            {
                Destroy(lines[pos].line);
                lines.RemoveAt(pos);
            }
            RemoveSelectedObjects();
        }
    }
    void Undo_All()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 0; i < spheres.Count; i++)
            {
                spheres[i].SetActive(true);
            }
            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].SetActive(true);
            }
            pointer.SetActive(true);
            SelectionIncrement = 0;
        }

    }
    void UpdatePointer()
    {
        if (pointer.activeSelf)
        {
            pointer.transform.position = new Vector3(allObjects[SelectionIncrement].transform.position.x, pointer.transform.position.y, allObjects[SelectionIncrement].transform.position.z);
        }
        }
    void Change_Inc()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (pointer.activeSelf)
            {
                if (SelectionIncrement + 1 < allObjects.Count)
                {
                    SelectionIncrement++;

                }
                else
                {
                    SelectionIncrement = 0;
                }
                int total = 0;
                while (!allObjects[SelectionIncrement].activeSelf)
                {
                    if (SelectionIncrement + 1 < allObjects.Count)
                    {
                        SelectionIncrement++;

                    }
                    else
                    {
                        SelectionIncrement = 0;
                    }
                    if (total == allObjects.Count)
                    {
                        pointer.SetActive(false);
                        break;
                    }
                    total++;
                }
            }
        }
    }
    void Remove_ActivatedObjects()
    {
        if (Input.GetKey(KeyCode.V))
        {
            Controller.Instance.DestroySelectedObjects();
        }
    }
    public void DestroySelectedObjects()
    {
       // List<int> pos = new List<int>();
        for (int i = 0; i < Controller.Instance.selectedObjects.Count; i++)
        {
            for (int u = 0; u < lines.Count; u++)
            {
                bool shouldDelete = false;

                for (int e = 0; e < lines[u].positions.Count; e++)
                {
                    if (lines[u].positions.Count <= 2)
                    {
                        if (lines[u].positions[e].gameObject == Controller.Instance.selectedObjects[i])
                        {
                            Destroy(lines[u].line.gameObject);
                            shouldDelete = true;
                            break;
                        }
                    }
                    else
                    {
                        if (lines[u].positions[e].gameObject == Controller.Instance.selectedObjects[i])
                        {
                            lines[u].line.positionCount -= 1;
                            lines[u].positions.RemoveAt(e);
                            break;
                        }
                    }
                }

                if (shouldDelete)
                {
                    lines.RemoveAt(u--);
               
                }
            }
            Controller.Instance.selectedObjects[i].GetComponent<MeshRenderer>().material = Controller.Instance.selectedObjects[i].GetComponent<Obstacles>().defaultMaterial;
            Controller.Instance.selectedObjects[i].GetComponent<Obstacles>().isSelected = false;
            Controller.Instance.selectedObjects[i].gameObject.SetActive(false);
        }

        Controller.Instance.selectedObjects.Clear();
    }
    void RemoveLines()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Destroy(lines[i].line.gameObject);
            }
            lines.Clear();
        }
    }
  
    public void RemoveSelectedObjects()
    {
        for (int i = 0; i < Controller.Instance.selectedObjects.Count; i++)
        {
            Controller.Instance.selectedObjects[i].GetComponent<MeshRenderer>().material = Controller.Instance.selectedObjects[i].GetComponent<Obstacles>().defaultMaterial;
            Controller.Instance.selectedObjects[i].GetComponent<Obstacles>().isSelected = false;
        }
        Controller.Instance.selectedObjects.Clear();
    }
    void DrawLines()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {

   
            Debug.Log(lines.Count);
            if (selectedObjects.Count > 0)
            {
                Line l = new Line();

                LineRenderer r = Instantiate(connection, connection.transform.position, Quaternion.identity).GetComponent<LineRenderer>();

                r.positionCount = selectedObjects.Count;
                l.line = r;
                for (int i = 0; i < selectedObjects.Count; i++)
                {
                    l.positions.Add(selectedObjects[i]);
                    r.SetPosition(i, selectedObjects[i].transform.position);
                }
                lines.Add(l);
                RemoveSelectedObjects();
            }
        }
        }
    void ChangeSelectedPosition()
    {
        if (pointer.activeSelf)
        {
            int r, c;
            int type = 0;
            if (cubes.Contains(allObjects[SelectionIncrement]))
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (allObjects[SelectionIncrement].transform.position.x + scale <= horizontalLength)
                {
                    r = (int)allObjects[SelectionIncrement].transform.position.x / scale;
                    c = (int)allObjects[SelectionIncrement].transform.position.z / scale;

                    Controller.Instance.Tiles[r, c] = 0;
                    allObjects[SelectionIncrement].transform.position = new Vector3(allObjects[SelectionIncrement].transform.position.x + scale, allObjects[SelectionIncrement].transform.position.y, allObjects[SelectionIncrement].transform.position.z);
                    r = (int)allObjects[SelectionIncrement].transform.position.x / scale;
                    c = (int)allObjects[SelectionIncrement].transform.position.z / scale;

                    Controller.Instance.Tiles[r, c] = type;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (allObjects[SelectionIncrement].transform.position.x - scale >= 0)
                {
                    r = (int)allObjects[SelectionIncrement].transform.position.x / scale;
                    c = (int)allObjects[SelectionIncrement].transform.position.z / scale;

                    Controller.Instance.Tiles[r, c] = 0;
                    allObjects[SelectionIncrement].transform.position = new Vector3(allObjects[SelectionIncrement].transform.position.x - scale, allObjects[SelectionIncrement].transform.position.y, allObjects[SelectionIncrement].transform.position.z);
                    r = (int)allObjects[SelectionIncrement].transform.position.x / scale;
                    c = (int)allObjects[SelectionIncrement].transform.position.z / scale;

                    Controller.Instance.Tiles[r, c] = type;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (allObjects[SelectionIncrement].transform.position.z + scale <= horizontalLength)
                {
                    r = (int)allObjects[SelectionIncrement].transform.position.x / scale;
                    c = (int)allObjects[SelectionIncrement].transform.position.z / scale;

                    Controller.Instance.Tiles[r, c] = 0;
                    allObjects[SelectionIncrement].transform.position = new Vector3(allObjects[SelectionIncrement].transform.position.x, allObjects[SelectionIncrement].transform.position.y, allObjects[SelectionIncrement].transform.position.z + scale);
                    r = (int)allObjects[SelectionIncrement].transform.position.x / scale;
                    c = (int)allObjects[SelectionIncrement].transform.position.z / scale;

                    Controller.Instance.Tiles[r, c] = type;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (allObjects[SelectionIncrement].transform.position.z - scale >= 0)
                {
                    r = (int)allObjects[SelectionIncrement].transform.position.x / scale;
                    c = (int)allObjects[SelectionIncrement].transform.position.z / scale;

                    Controller.Instance.Tiles[r, c] = 0;
                    allObjects[SelectionIncrement].transform.position = new Vector3(allObjects[SelectionIncrement].transform.position.x, allObjects[SelectionIncrement].transform.position.y, allObjects[SelectionIncrement].transform.position.z - scale);
                    r = (int)allObjects[SelectionIncrement].transform.position.x / scale;
                    c = (int)allObjects[SelectionIncrement].transform.position.z / scale;

                    Controller.Instance.Tiles[r, c] = type;
                }
            }

        }
    }
    void Update()
    {
        if (!isPaused)
        {
            DrawLines();
            RemoveLines();
            Update_Selection();
            Remove_ActivatedObjects();
            Undo_All();
            RemoveLink();
            MoveObjects();
            Change_Inc();
            UpdatePointer();
            ChangeSelectedPosition();
        }
        Pause();
    }
    void Pause()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
           if(isPaused)
            {
                isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Instructions.gameObject.SetActive(false);
                Panel.SetActive(false);

            }
            else
            {
                isPaused = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Instructions.gameObject.SetActive(true);
                Panel.SetActive(false);

            }
        }
    }
    void MoveObjects()
    {
        if (Controller.Instance.moveObject)
        {
            free_Tiles();
            moveObjects(Controller.Instance.spheres, sphereDirection, 0);
            moveObjects(Controller.Instance.cubes, cubeDirection, 1);
        }
    }
    void addObjects(int[,] tile, int size)
    {
        Vector3 position;
        float posX, posZ;
        int increment = 1;
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                posX = (float)c * scale;
                posZ = (float)r * scale;

                switch (tile[r, c])
                {
                    case 1:
                        float sphereSize = (float)(Random.Range(1, 3));

                        GameObject sphere = new GameObject();
                        position = new Vector3(posX, plane.transform.position.y + sphereSize / 2, posZ);
                        sphere = (GameObject)Instantiate(mainSphere, position, Quaternion.identity);
                        sphereDirection.Add(Random.Range(0, 4));
                        sphere.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
                        sphere.GetComponent<Obstacles>().obstacleName = "Sphere " + increment.ToString();
                        increment++;
                        Controller.Instance.spheres.Add(sphere);
                        allObjects.Add(sphere);

                        break;

                    case 2:
                        float cubeSize = (float)(Random.Range(1, 3));

                        GameObject cube = new GameObject();
                        position = new Vector3(posX, plane.transform.position.y + cubeSize / 2, posZ);
                        cube = (GameObject)Instantiate(mainCube, position, Quaternion.identity);
                        cubeDirection.Add(Random.Range(0, 4));
                        cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                   
                        cube.GetComponent<Obstacles>().obstacleName = "Cube " + increment.ToString();
                        increment++;
                        Controller.Instance.cubes.Add(cube);
                        allObjects.Add(cube);
                        break;
                }
            }
        }
    }

    void free_Tiles()
    {
        Controller.Instance.Tiles = new int[10, 10];
        for (int r = 0; r < 10; r++)
        {
            for (int c = 0; c < 10; c++)
            {
                Controller.Instance.Tiles[r, c] = 0;
            }
        }
    }
    void moveObjects(List<GameObject> objects, List<int> objectsDirections, int type)
    {
        for (int i = 0; i < objectsDirections.Count; i++)
        {
            int r = 0;
            int c = 0;
            if (objects[i].activeSelf)
            {
                switch (objectsDirections[i])
                {
                    case 0:
                        if (objects[i].transform.position.x + scale <= horizontalLength)
                        {
                            objects[i].transform.position = new Vector3(objects[i].transform.position.x + scale * Time.deltaTime, objects[i].transform.position.y, objects[i].transform.position.z);
                            r = (int)objects[i].transform.position.x / scale;
                            c = (int)objects[i].transform.position.z / scale;
                            Controller.Instance.Tiles[r, c] = type;

                        }
                        else
                        {
                            objectsDirections[i] = 1;
                        }

                        break;

                    case 1:
                        if (objects[i].transform.position.x - scale >= startXLength)
                        {
                            objects[i].transform.position = new Vector3(objects[i].transform.position.x - scale * Time.deltaTime, objects[i].transform.position.y, objects[i].transform.position.z);
                            r = (int)objects[i].transform.position.x / scale;
                            c = (int)objects[i].transform.position.z / scale;
                            Controller.Instance.Tiles[r, c] = type;
                        }
                        else
                        {
                            objectsDirections[i] = 0;
                        }

                        break;

                    case 2:
                        if (objects[i].transform.position.z + scale <= verticalLength)
                        {
                            objects[i].transform.position = new Vector3(objects[i].transform.position.x, objects[i].transform.position.y, objects[i].transform.position.z + scale * Time.deltaTime);
                            r = (int)objects[i].transform.position.x / scale;
                            c = (int)objects[i].transform.position.z / scale;
                            Controller.Instance.Tiles[r, c] = type;
                        }
                        else
                        {
                            objectsDirections[i] = 3;
                        }

                        break;

                    case 3:
                        if (objects[i].transform.position.z - scale >= startYLength)
                        {
                            objects[i].transform.position = new Vector3(objects[i].transform.position.x, objects[i].transform.position.y, objects[i].transform.position.z - scale * Time.deltaTime);
                            r = (int)objects[i].transform.position.x / scale;
                            c = (int)objects[i].transform.position.z / scale;
                            Controller.Instance.Tiles[r, c] = type;
                        }
                        else
                        {
                            objectsDirections[i] = 2;
                        }

                        break;
                }
            }
        }
    }

}
