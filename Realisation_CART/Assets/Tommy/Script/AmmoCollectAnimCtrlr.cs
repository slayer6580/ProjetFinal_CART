using UnityEngine;

namespace DiscountDelirium
{
    public class AmmoCollectAnimCtrlr : MonoBehaviour
    {
		[SerializeField] private Animator m_animator;
		
        public void ActivateCollect()
        {
            if (m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ammo_floating")
            {
				m_animator.SetTrigger("collect");
			}           
        }
    }
}
