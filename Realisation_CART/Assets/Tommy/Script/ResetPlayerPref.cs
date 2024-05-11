using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class ResetPlayerPref : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
			ResetPP();
		}

		public void ResetPP()
		{
			PlayerPrefs.SetInt("Acceleration", 0);
			PlayerPrefs.SetInt("MaxSpeed", 0);
			PlayerPrefs.SetInt("Handling", 0);
			PlayerPrefs.SetInt("Balance", 0);
            PlayerPrefs.SetInt("Melee", 0);
            PlayerPrefs.SetInt("Ranged", 0);

            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.SetInt("Money", 0);
        }
	}
}
