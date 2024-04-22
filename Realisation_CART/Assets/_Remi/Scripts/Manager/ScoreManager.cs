using System.Collections.Generic;
using UnityEngine;
using BoxSystem;
using Box = BoxSystem.Box;

namespace Manager
{
    public class ScoreManager : MonoBehaviour
    {
        [field: SerializeField] public TowerBoxSystem _TowerBoxSystem { get; private set; }

        public static ScoreManager _ScoreManager { get; private set; }

        [field: SerializeField] private int m_cartokenValueMultiplier = 1;

        private void Awake()
        {
            if (_ScoreManager != null)
            {
                Debug.LogWarning("ScoreManager already exists.");
                Destroy(gameObject);
                return;
            }

            _ScoreManager = this;
        }

        /// <summary> Vide le panier et rend le nombre d'items de la tour et le score total </summary>
        public Vector3 EmptyCartAndGetScore()
        {
            CalculateCartokens();

            int totalScore = 0;
            int nbOfItems = 0;
            int nbOfCartokens = CalculateCartokens();
            Debug.Log("Cartokens: " + nbOfCartokens);

            while (_TowerBoxSystem.GetBoxCount() > 0)
            {
                Box topBox = _TowerBoxSystem.GetTopBox();
                List<Box.ItemInBox> itemsInBox = topBox.GetItemsInBox();

                for (int i = 0; i < itemsInBox.Count; i++)
                {
                    nbOfItems++;
                    totalScore += itemsInBox[i].m_item.GetComponent<Item>().m_data.m_cost;
                }
                _TowerBoxSystem.RemoveBoxImpulse();
            }

            return new Vector3(nbOfItems, totalScore, nbOfCartokens);
        }

        public void RemoveAllBoxImpulse(TowerBoxSystem tower)
        {
			while (tower.GetBoxCount() > 0)
			{
				tower.RemoveBoxImpulse();
			}
		}

        private int CalculateCartokens()
        {
            return (int)(Mathf.Pow(_TowerBoxSystem.GetBoxCount(), m_cartokenValueMultiplier));  
        }
    }
}
