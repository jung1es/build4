using UnityEngine;
using UnityEngine.UI;

public class KeyOverlay : MonoBehaviour
{
    public Image[] keys;
    public Color[] color;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Image k in keys)
        {

            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), k.name);

            if (Input.GetKey(key))
            {
                k.color = color[1];
            }
            else
            {
                k.color = color[0];
            }
        }
        var inputValue = Input.inputString;
        /* switch (inputValue)
         {
             case "w":
                 Debug.Log("1 key was pressed");
                 break;
             case ("C"):
                 Debug.Log("2 key was pressed");
                 break;
             case ("3"):
                 Debug.Log("3 key was pressed");
                 break;
         }*/


    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {

            //Debug.Log("Detected key code: " + e.keyCode);
        }
    }
}
