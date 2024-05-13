using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class ShowFPS : MonoBehaviour
    {
		private float count;

		private IEnumerator Start()
		{
			Application.targetFrameRate = 60;

			GUI.depth = 2;
			while (true)
			{
				count = 1f / Time.unscaledDeltaTime;
				yield return new WaitForSeconds(0.1f);
			}
		}

		private void OnGUI()
		{
			GUI.Label(new Rect(5, 40, 100, 25), "FPS: " + Mathf.Round(count));
		}
	}
}
