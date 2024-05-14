using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class MeshCombiner : MonoBehaviour
    {
        [SerializeField] private List<MeshFilter> m_sourceMeshFilters;
        [SerializeField] private MeshFilter m_targetMeshFilter;

		private void Start()
		{
            CombineMesh();
		}

        [ContextMenu("Combine Mesh")]
		private void CombineMesh()
        {
            var combine = new CombineInstance[m_sourceMeshFilters.Count];
            for(var i=0; i< m_sourceMeshFilters.Count; i++)
            {
                combine[i].mesh = m_sourceMeshFilters[i].sharedMesh;
                combine[i].transform = m_sourceMeshFilters[i].transform.localToWorldMatrix;
            }

            m_targetMeshFilter.mesh = new Mesh();
            m_targetMeshFilter.mesh.CombineMeshes(combine);

		}
    }
}
