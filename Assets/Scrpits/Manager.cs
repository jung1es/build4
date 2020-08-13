using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace com.PT.contest
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public string playerPrefab;
        public Transform[] spawnPoint;
        public GameObject pauseMenu;
        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        public Camera MyCamRef;
        public Transform MyTransformRef;
        public void Spawn()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab, spawnPoint[0].position, spawnPoint[0].rotation);
                if (MyCamRef == null)
                {
                    Debug.Log("CAM 1");
                    MyCamRef = obj.GetComponentInChildren<Camera>();
                    MyTransformRef = obj.transform;
                }
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab, spawnPoint[1].position, spawnPoint[1].rotation);
                if (MyCamRef == null)
                {
                    Debug.Log("CAM 2");
                    MyCamRef = obj.GetComponentInChildren<Camera>();
                    MyTransformRef = obj.transform;
                }
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab, spawnPoint[2].position, spawnPoint[2].rotation);
                if (MyCamRef == null)
                {
                    Debug.Log("CAM 3");
                    MyCamRef = obj.GetComponentInChildren<Camera>();
                    MyTransformRef = obj.transform;
                }
            }

        }


        public void Menu()
        {
            SceneManager.LoadScene(0);
        }
    }
}