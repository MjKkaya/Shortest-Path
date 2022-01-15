using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace MjKkaya.ShortestPath.Managers
{
    public class UIManager : MonoBehaviour
    {
        public event Action<int> ActionOnChangedMeshDropdown;

        [SerializeField] private TextMeshProUGUI TextNodeCount;
        [SerializeField] private TextMeshProUGUI TextDistance;
        [SerializeField] private TextMeshProUGUI TextDuration;
        [SerializeField] private TMP_Dropdown DropdownMesh;


        public void Initialize()
        {
            GameManager.Instance.MeshManager.ActionOnInitializedMeshDropdown += OnInitializedMeshDropdown;
        }

        public void SetNodeCountTxt(int count)
        {
            TextNodeCount.text = string.Concat("Node Count : ", count);
        }

        public void SetDitanceTxt(float distance)
        {
            TextDistance.text = string.Concat("Distance : ", distance.ToString("f2"), " unit");
        }

        public void SetTimeTxt(long duration)
        {
            TextDuration.text = string.Concat("Duration :  ", duration, " ms");
        }

        private void OnMeshDropdownChanged(TMP_Dropdown change)
        {
            Debug.Log(change.value + "-" + change.captionText.text);
            ActionOnChangedMeshDropdown?.Invoke(change.value);
            //GameManager.Instance.MeshManager.SetActivePickedMesh(change.value);
        }


        #region Events

        private void OnInitializedMeshDropdown(List<string> meshNameList)
        {
            DropdownMesh.ClearOptions();
            DropdownMesh.AddOptions(meshNameList);

            DropdownMesh.onValueChanged.AddListener(delegate
            {
                OnMeshDropdownChanged(DropdownMesh);
            });
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null && GameManager.Instance.MeshManager != null)
                GameManager.Instance.MeshManager.ActionOnInitializedMeshDropdown  -= OnInitializedMeshDropdown;
        }

        #endregion
    }
}