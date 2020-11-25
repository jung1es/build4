using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
using TMPro;

namespace com.PT.contest
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance;
        
      

        public string playerPrefab;
        public Transform[] spawnPoint;
        public GameObject pauseMenu;
        public bool EnableMovementOnly = false;
        public bool LockObjects = false;
        public bool IsSharedControl = false;
        private bool MakeKinematic = false;

        public Camera MyCamRef;
        public Transform MyTransformRef;
        private GameObject obj;


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

        public void SetKinematic(bool state)
        {
            MakeKinematic = state;
        }

        public bool GetKinematic()
        {
            return MakeKinematic;
        }
        private void Start()
        {
            Spawn();
        }

       
        private IEnumerator SetRotDelay()
        {
            yield return new WaitForSecondsRealtime(0.2f);
            obj.transform.rotation = spawnPoint[0].rotation;
        }



        public void Spawn()
        {

            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                obj = PhotonNetwork.Instantiate(playerPrefab, spawnPoint[0].position, spawnPoint[0].rotation);
                
                if (MyCamRef == null)
                {
                    MyCamRef = obj.GetComponentInChildren<Camera>();
                    MyTransformRef = obj.transform;
                }
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab, spawnPoint[1].position, spawnPoint[1].rotation);
                if (MyCamRef == null)
                {
                    MyCamRef = obj.GetComponentInChildren<Camera>();
                    MyTransformRef = obj.transform;
                }
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab, spawnPoint[2].position, spawnPoint[2].rotation);
                if (MyCamRef == null)
                {
                    MyCamRef = obj.GetComponentInChildren<Camera>();
                    MyTransformRef = obj.transform;
                }
            }
        }

        
        public void Menu()
        {
            //SceneManager.LoadScene(0);
        }

        
    }
}