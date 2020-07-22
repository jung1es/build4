using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace com.PT.contest
{
    public class Manager : MonoBehaviour
    {
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
        public void Spawn()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.Instantiate(playerPrefab, spawnPoint[0].position, spawnPoint[0].rotation);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.Instantiate(playerPrefab, spawnPoint[1].position, spawnPoint[1].rotation);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
            {
                PhotonNetwork.Instantiate(playerPrefab, spawnPoint[2].position, spawnPoint[2].rotation);
            }

        }


        public void Menu()
        {
            SceneManager.LoadScene(0);
        }
    }
}