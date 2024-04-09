using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DiscountDelirium
{
    public class ManageGrindVfx : MonoBehaviour
    {
        [SerializeField] private VisualEffect m_grindVfx;
        private const int SPAWN_RATE = 50;
        private const int UPDATERATE = 1000;

        public void PlayVfx()
        {
            m_grindVfx.SendEvent("OnPlay");
            m_grindVfx.SetInt("SpawnRate", SPAWN_RATE);
			m_grindVfx.SetInt("UpdateRate", UPDATERATE);
		}

        public void StopVfx()
        {
			m_grindVfx.SendEvent("OnStop");
			m_grindVfx.SetInt("SpawnRate", 0);
			m_grindVfx.SetInt("UpdateRate", 0);
		}
    }
}
