using com.PT.contest;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public GameObject CanvasPrefab;
    void Start()
    {
        if (FindObjectOfType<Canvas>() == null) 
        {
            myCanvas = Instantiate(CanvasPrefab);
            FindObjectOfType<Launcher>().Switch();
            FindObjectOfType<Launcher>().HideOrShowMenu(false);
            FindObjectOfType<ClockController>().StartClock();
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GetComponent<FreeCamera>().enabled = false;
        }
        if (Manager.Instance.IsSharedControl)
        {
            GetComponent<Camera_Controller>().enabled = true;
            GetComponent<Camera_Controller>().Normal_Speed = 0;
            GetComponent<Camera_Controller>().Shift_Speed = 0;
            GetComponent<Spectator>().m_MoveCamera = false;
            GetComponent<Camera_Controller>().useLeftClick = true;
        }
        else
        {
             GetComponent<Camera_Controller>().useLeftClick = false;
        }

    }
    private GameObject myCanvas;
    private bool showMenu = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Launcher>().HideOrShowMenu(!showMenu);
            showMenu = !showMenu;
        }
    }

}
