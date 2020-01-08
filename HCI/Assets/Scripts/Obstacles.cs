using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Obstacles : MonoBehaviour
{
    public bool isSelected = false;
    public Material defaultMaterial;
    public string obstacleName = "Object";
    public TMP_Text text;
    public Text label;
    // Start is called before the first frame update
    void Start()
    {
        text.text = obstacleName;
        gameObject.transform.parent = Controller.Instance.World.transform;
        int r = Random.Range(0, 6);
        GetComponent<MeshRenderer>().material = Controller.Instance.materials[r];
        defaultMaterial = GetComponent<MeshRenderer>().material;
    }
    public void Select()
    {
        isSelected = true;
        GetComponent<MeshRenderer>().material = Controller.Instance.selectionMaterial;
        Controller.Instance.selectedObjects.Add(gameObject);
    }
    void Update_Name()
    {
        text.text = obstacleName;

    }
    // Update is called once per frame
    void Update()
    {
        Update_Name();        
    }
}
