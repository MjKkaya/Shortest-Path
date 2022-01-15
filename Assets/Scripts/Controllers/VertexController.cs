using ElapsedTime = System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using MjKkaya.ShortestPath.Managers;
using MjKkaya.ShortestPath.Algorithms;


namespace MjKkaya.ShortestPath.Controllers
{
    public class VertexController : MonoBehaviour
    {
        private Mesh mMesh;
        private Vector3[] mMeshVertices;
        private int[] mMeshTriangles;
        private int[] mModifiedTriangleArr;
        private Dictionary<int, Vector3> mModifiedVerticesDic;
        private Dictionary<int, Dictionary<int, float>> mGraphDict;
        private int mStartPointIndex = -1;
        private int mEndPointIndex = -1;
        private ElapsedTime.Stopwatch mStopwatch;


        public void SetStartPoint(ref Vector3 hitPoint)
        {
            Vector3 localHit = hitPoint - transform.position;
            Vector3 nearestV3Point = FindNearestVertex(localHit, true);
            hitPoint = nearestV3Point + transform.position;
        }

        public void SetEndPoint(ref Vector3 hitPoint)
        {
            Vector3 localHit = hitPoint - transform.position;
            Vector3 nearestV3Point = FindNearestVertex(localHit, false);
            hitPoint = nearestV3Point + transform.position;
        }

        public void CheckPointsIndex()
        {
            if (mStartPointIndex != -1 && mEndPointIndex != -1)
            {
                mStopwatch.Restart();
                DijkstraAlgorithm.StartDijkstraAlghorithm(this, mGraphDict, mStartPointIndex, mEndPointIndex);
            }
        }


        private Vector3 FindNearestVertex(Vector3 localPoint, bool isStartPoint)
        {
            Vector3 vertexPoint = Vector3.zero;
            float minDis = float.MaxValue;
            float distance;

            foreach (var item in mModifiedVerticesDic)
            {
                distance = Vector3.Distance(localPoint, item.Value);
                if (distance < minDis)
                {
                    minDis = distance;
                    vertexPoint = item.Value;

                    if (isStartPoint)
                        mStartPointIndex = item.Key;
                    else
                        mEndPointIndex = item.Key;
                }
            }

            //vertexPoint += transform.position;
            return vertexPoint;
        }

        

        /// <summary>
        /// Sets the shortest path. Called from DijkstraAlgorithm
        /// </summary>
        /// <param name="foundPathList">Found path list.</param>
        public void SetShortestPath(List<int> foundPathList)
        {
            SetPassedTime();
            GameManager.Instance.UIManager.SetNodeCountTxt(foundPathList.Count);

            int firstIndex;
            int secondIndex;
            float distance = 0;
            for (int i = 0; i < foundPathList.Count - 1; i++)
            {
                if (i != (foundPathList.Count - 1))
                {
                    firstIndex = foundPathList[i];
                    secondIndex = foundPathList[i + 1];
                    distance += mGraphDict[firstIndex][secondIndex];
                }
            }
            GameManager.Instance.UIManager.SetDitanceTxt(distance);

            Vector3[] pathVector3Arr = new Vector3[foundPathList.Count];
            for (int i = 0; i < foundPathList.Count; i++)
            {
                pathVector3Arr[i] = mModifiedVerticesDic[foundPathList[i]];
            }
            GameManager.Instance.MeshManager.SetLineRendererPositions(pathVector3Arr);
        }

        /// /Move to Dijkstra algoritm
        private void SetGraphArray(Dictionary<int, Vector3> verticesDic)
        {
            mGraphDict.Clear();

            Dictionary<int, float> distanceOfVertexToOtherVertexDict = new Dictionary<int, float>();

            List<int> vertexKeyList = new List<int>();
            foreach (var item in mModifiedVerticesDic)
            {
                vertexKeyList.Add(item.Key);
            }
            vertexKeyList.Sort();

            int vertexNoInTriangle;

            List<int> chedkedIndexList = new List<int>();
            Dictionary<int, float> subDict;

            //foreach (var item in modifiedVerticesDic)
            foreach (int vertexNo in vertexKeyList)
            {
                //don't check to same otherVertexNo
                chedkedIndexList.Clear();

                //set current Vertexno distance to other vertoxNo
                distanceOfVertexToOtherVertexDict.Clear();

                for (int i = 0; i < mModifiedTriangleArr.Length; i += 3)
                {
                    //if vertexNo is in the triangle points. 
                    if (vertexNo.Equals(mModifiedTriangleArr[i]) || vertexNo.Equals(mModifiedTriangleArr[i + 1]) || vertexNo.Equals(mModifiedTriangleArr[i + 2]))
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            vertexNoInTriangle = mModifiedTriangleArr[i + k];
                            if (!vertexNo.Equals(vertexNoInTriangle) && !chedkedIndexList.Contains(vertexNoInTriangle))
                            {
                                chedkedIndexList.Add(vertexNoInTriangle);
                                float distanceOfTwoVertices = Vector3.Distance(verticesDic[vertexNo], verticesDic[vertexNoInTriangle]);

                                distanceOfVertexToOtherVertexDict.Add(vertexNoInTriangle, distanceOfTwoVertices);
                            }
                        }
                    }
                }

                // Set sub dictionary to graph.
                subDict = new Dictionary<int, float>();
                foreach (int indexNo in vertexKeyList)
                {
                    if (distanceOfVertexToOtherVertexDict.ContainsKey(indexNo))
                        subDict.Add(indexNo, distanceOfVertexToOtherVertexDict[indexNo]);
                    else
                        subDict.Add(indexNo, 0);
                }
                mGraphDict[vertexNo] = subDict;
            }
        }


        public void Initialize()
        {
            mStopwatch = new ElapsedTime.Stopwatch();
            mGraphDict = new Dictionary<int, Dictionary<int, float>>();
            mMesh = GetComponent<MeshFilter>().mesh;
            mMeshVertices = mMesh.vertices;
            mMeshTriangles = mMesh.triangles;   // dizi uzunluğu 3'ün katı olmalı. Triangle olmasından ötürü sanırım.
            SetModifiedTriangleArr();
            SetModifiedVerticesDic();
            SetGraphArray(mModifiedVerticesDic);

            Debug.Log("meshVertices length : " + mMeshVertices.Length);
        }


        private void SetModifiedTriangleArr()
        {
            mModifiedTriangleArr = new int[mMeshTriangles.Length];
            for (int i = 0; i < mMeshTriangles.Length; i++)
            {
                mModifiedTriangleArr[i] = mMeshTriangles[i];
            }

            int firstVertexIndex;
            int nextVertexIndex;

            for (int i = 0; i < mModifiedTriangleArr.Length - 1; i++)
            {
                firstVertexIndex = mModifiedTriangleArr[i];

                for (int k = i + 1; k < mModifiedTriangleArr.Length; k++)
                {
                    nextVertexIndex = mModifiedTriangleArr[k];

                    //if vertices point not equal own and vertex poibt vector3 equal eachother.
                    if (firstVertexIndex != nextVertexIndex && mMeshVertices[firstVertexIndex].Equals(mMeshVertices[nextVertexIndex]))
                        mModifiedTriangleArr[k] = firstVertexIndex;
                }
            }
        }

        private void SetModifiedVerticesDic()
        {
            mModifiedVerticesDic = new Dictionary<int, Vector3>();
            int vertexIndex;

            for (int i = 0; i < mModifiedTriangleArr.Length; i++)
            {
                vertexIndex = mModifiedTriangleArr[i];
                if (!mModifiedVerticesDic.ContainsKey(vertexIndex))
                    mModifiedVerticesDic.Add(vertexIndex, mMeshVertices[vertexIndex]);
            }
        }

        private void SetPassedTime()
        {
            mStopwatch.Stop();
            GameManager.Instance.UIManager.SetTimeTxt(mStopwatch.ElapsedMilliseconds);
        }


        #region Events

        private void OnEnable()
        {
            GameManager.Instance.MeshManager.SetLineRendererBaseTransform(gameObject.transform);
        }

        private void OnDisable()
        {
            mStartPointIndex = -1;
            mEndPointIndex = -1;
        }

        #endregion
    }
}