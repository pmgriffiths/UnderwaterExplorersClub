using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PodTheDog.UEX
{
    public class MenuSceneManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown("s"))
            {
                LoadMainScene();
            }    
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene("UnderwaterScene");
        }

    }
}
