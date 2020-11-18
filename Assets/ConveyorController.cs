using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ConveyorController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Button1, Button2;
    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            Button1.SetActive(true);
            Button2.SetActive(true);
        }
    }
    public void SetConveyorMoving(bool _move)
    {
        Conveyor.Instance.StartStopMoving(_move);
    }
}
