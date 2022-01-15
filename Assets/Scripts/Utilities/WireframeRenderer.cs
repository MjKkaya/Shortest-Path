using System.Collections.Generic;
using UnityEngine;


namespace MjKkaya.ShortestPath.Utilities
{
    [RequireComponent(typeof(MeshFilter))]
    public class WireframeRenderer : MonoBehaviour
    {
        private const float distance = 1.0001f;
        private static readonly Color color = Color.black;
        private List<Vector3> renderingQueue;

        // ReSharper disable once MemberCanBePrivate.Global
        public Material WireMaterial;


        private void InitializeOnDemand()
        {
            if (renderingQueue != null)
                return;

            var meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogError("No mesh detected at" + gameObject.name, gameObject);
                return;
            }
            var mesh = meshFilter.mesh;

            renderingQueue = new List<Vector3>();
            foreach (var point in mesh.triangles)
            {
                renderingQueue.Add(mesh.vertices[point] * distance);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public void OnPreRender()
        {
            GL.wireframe = true;
        }

        // ReSharper disable once UnusedMember.Global
        public void OnRenderObject()
        {
            InitializeOnDemand();

            if (WireMaterial != null)
                WireMaterial.SetPass(0);
            else
                GL.Color(color);

            GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.LINES);

            for (var i = 0; i < renderingQueue.Count; i += 3)
            {
                var vertex1 = renderingQueue[i];
                var vertex2 = renderingQueue[i + 1];
                var vertex3 = renderingQueue[i + 2];
                GL.Vertex3(vertex1.x, vertex1.y, vertex1.z);
                GL.Vertex3(vertex2.x, vertex2.y, vertex2.z);
                GL.Vertex3(vertex2.x, vertex2.y, vertex2.z);
                GL.Vertex3(vertex3.x, vertex3.y, vertex3.z);
                GL.Vertex3(vertex3.x, vertex3.y, vertex3.z);
                GL.Vertex3(vertex1.x, vertex1.y, vertex1.z);
            }
            GL.End();
        }

        // ReSharper disable once UnusedMember.Global
        public void OnPostRender()
        {
            GL.wireframe = false;
        }
    }
}