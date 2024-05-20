using BoxSystem;
using CartControl;
using UnityEngine;
using static Manager.AudioManager;
using static Manager.ScoreManager;

namespace DiscountDelirium
{
    public class CheckoutZone : MonoBehaviour
    {
		[field: SerializeField] public GameObject CheckoutCamera { get; private set; }
        [SerializeField] private GameObject m_counter;
        [SerializeField] private GameObject m_playerDirection;
        [SerializeField] private GameObject m_moneyVfx;
        [SerializeField] private GameObject m_coinsVfx;
        [field: SerializeField] public GameObject BonusText { get; private set; }
		[field: SerializeField] public Animator ThisAnimatior { get; private set; }


		private GameObject m_player;


        private void OnTriggerEnter(Collider other)
        {
            CheckOutTrigger itemTrigger = other.GetComponent<CheckOutTrigger>();

            if (itemTrigger == null)            
                return;
            
            if (itemTrigger.TowerBoxSystem.GetBoxCount() == 0)
                return;

           

            bool isPlayer = other.transform.parent.parent.name == "Character";
            if (isPlayer)
            {
                m_player = other.transform.parent.parent.gameObject;
				Vector3 data = _ScoreManager.EmptyCartAndGetScoreV2(m_counter.transform, this);
				GameStateMachine.Instance.GetScoreFromCart(data);

                //Manage to begin checkout animation
                GameStateMachine.Instance.IsCheckingOut = true;
                other.gameObject.transform.parent.parent.GetComponent<CartStateMachine>().IsPaused = true;
                CheckoutCamera.SetActive(true);
                ThisAnimatior.SetTrigger("MoveCam");				
				m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			else
            {
                _ScoreManager.RemoveAllBoxImpulse(itemTrigger.TowerBoxSystem);
			}

		}

        public void CashExplosion(Vector3 spawnPosition)
        {
            GameObject moneyVfx = Instantiate(m_moneyVfx, spawnPosition, Quaternion.identity);
            GameObject coinVfx = Instantiate(m_coinsVfx, spawnPosition, Quaternion.identity);

            Destroy(moneyVfx, moneyVfx.GetComponent<ParticleSystem>().main.duration);
            Destroy(coinVfx, moneyVfx.GetComponent<ParticleSystem>().main.duration);
        }

        public void CheckoutDone()
        {
			m_player.transform.position = m_playerDirection.transform.position;
			m_player.transform.rotation = m_playerDirection.transform.rotation;

			GameStateMachine.Instance.IsCheckingOut = false;
			m_player.GetComponent<CartStateMachine>().IsPaused = false;
			CheckoutCamera.SetActive(false);
		}
    }
}
