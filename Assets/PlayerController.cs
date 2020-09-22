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
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GetComponent<FreeCamera>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
