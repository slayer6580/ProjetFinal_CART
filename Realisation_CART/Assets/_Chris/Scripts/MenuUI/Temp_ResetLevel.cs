using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiscountDelirium
{
    public class Temp_ResetLevel : MonoBehaviour
    {
        public void ResetLevel() 
        {
            SceneManager.LoadScene("Main");
        }
    }
}
