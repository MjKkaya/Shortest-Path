using UnityEngine;
using MjKkaya.ShortestPath.Controllers;

namespace MjKkaya.ShortestPath.ScriptableObjects
{
    public enum MeshItemType
    {
        None = 0,
        Human = 1,
        Floor1 = 2,
        Floor2 = 3,
        Barel = 4,
        Cube = 5
    }


    [CreateAssetMenu(menuName = "ScriptableObjects/SOMeshItem", fileName = "SOMeshItem_")]
    public class SOMeshItem : ScriptableObject
    {
        public string MeshName;
        public MeshItemType MeshItemType;
        public GameObject MeshPrefab;
        
        private GameObject mMeshInstance;
        private VertexController mVertexController;


        public void SetActive(bool isActve)
        {
            if(mMeshInstance != null)
                mMeshInstance.SetActive(isActve);
        }

        public void InstantiateMeshObject(Transform transformParent)
        {
            if (MeshPrefab != null && mMeshInstance == null)
            {
                mMeshInstance = Instantiate(MeshPrefab, transformParent);
                mVertexController = mMeshInstance.GetComponent<VertexController>();
                mVertexController.Initialize();
            }

            SetActive(true);
        }


        public void SetStartPoint(ref Vector3 hitPoint)
        {
            mVertexController.SetStartPoint(ref hitPoint);
        }

        public void SetEndPoint(ref Vector3 hitPoint)
        {
            mVertexController.SetEndPoint(ref hitPoint);
        }

        public void CheckPointsIndex()
        {
            mVertexController.CheckPointsIndex();
        }
    }
}