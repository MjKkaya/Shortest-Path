using System;
using System.Collections.Generic;
using UnityEngine;
using MjKkaya.ShortestPath.Controllers;
using MjKkaya.ShortestPath.ScriptableObjects;


namespace MjKkaya.ShortestPath.Managers
{
    public class MeshManager : MonoBehaviour
    {
        public event Action<List<string>> ActionOnInitializedMeshDropdown;

        public Transform TransformMeshContainer;

        [SerializeField] private LineRendererController mLineRendererController;
        [SerializeField] private List<SOMeshItem> mSOMeshItemList;

        private SOMeshItem mPickedSOMeshItem;


        public void Initialize()
        {
            GameManager.Instance.UIManager.ActionOnChangedMeshDropdown += SetActivePickedMesh;
            mLineRendererController.Initialize();
            SetMeshDropdown();
        }

        public void SetStartPoint(Vector3 hitPoint)
        {
            mPickedSOMeshItem.SetStartPoint(ref hitPoint);
            mLineRendererController.SetStartPoint(hitPoint);
            mPickedSOMeshItem.CheckPointsIndex();
        }

        public void SetEndPoint(Vector3 hitPoint)
        {
            mPickedSOMeshItem.SetEndPoint(ref hitPoint);
            mLineRendererController.SetEndPoint(hitPoint);
            mPickedSOMeshItem.CheckPointsIndex();
        }

        public void SetLineRendererPositions(Vector3[] positions)
        {
            mLineRendererController.SetPositions(positions);
        }

        public void SetLineRendererBaseTransform(Transform transform)
        {
            mLineRendererController.SetBaseTrans(transform);
        }


        /// <summary>
        /// MeshItemType is used in the loop, as it is desired to list in enum order.
        /// </summary>
        private void SetMeshDropdown()
        {
            List<string> meshNameList = new List<string>();
            foreach (MeshItemType meshItemType in System.Enum.GetValues(typeof(MeshItemType)))
            {
                foreach (var item in mSOMeshItemList)
                {
                    if(item.MeshItemType.Equals(meshItemType))
                    {
                        meshNameList.Add(item.MeshName);
                        break;
                    }
                }
            }

            ActionOnInitializedMeshDropdown?.Invoke(meshNameList);
        }


        #region Events

        private void SetActivePickedMesh(int itemIndex)
        {
            foreach (SOMeshItem meshItem in mSOMeshItemList)
            {
                if (itemIndex.Equals((int)meshItem.MeshItemType))
                {
                    if (mPickedSOMeshItem != null)
                        mPickedSOMeshItem.SetActive(false);
                    mPickedSOMeshItem = meshItem;
                    mPickedSOMeshItem.InstantiateMeshObject(TransformMeshContainer);
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            if(GameManager.Instance != null && GameManager.Instance.UIManager != null)
                GameManager.Instance.UIManager.ActionOnChangedMeshDropdown -= SetActivePickedMesh;
        }

        #endregion
    }
}