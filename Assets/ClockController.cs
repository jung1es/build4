using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ClockController : MonoBehaviour
{
    public Image myClock;
    public int clockSeconds = 300;
    
    public void StartClock()
    {
        myClock.gameObject.SetActive(true);
        myClock.fillAmount = 1;
    }

    private void Update()
    {
        myClock.fillAmount -= 1.0f / clockSeconds * Time.deltaTime;
    }
}
