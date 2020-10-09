using com.PT.contest;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    private GameObject MyArea;
    public void SetMyArea(GameObject _area)
    {
        MyArea = _area;
    }
    public GameObject GetMyArea()
    {
        return MyArea;
    }
    // Start is called before the first frame update
    GameObject[] DragableObjects;
    private int dragableIndexSelect = 0;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GetComponent<FreeCamera>().enabled = false;
        }
        DragableObjects = new GameObject[FindObjectsOfType<DraggingObs>().Length];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Manager.Instance.IsSharedControl)
            {

            }
        }
    }

}
