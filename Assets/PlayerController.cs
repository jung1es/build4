using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
