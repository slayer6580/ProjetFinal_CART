using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiscountDelirium
{
    public class Temp_ResetLevel : MonoBehaviour
    {
        public void ResetLevel() 
        {
            SceneManager.LoadScene("EndGame_Test");
        }
    }
}
