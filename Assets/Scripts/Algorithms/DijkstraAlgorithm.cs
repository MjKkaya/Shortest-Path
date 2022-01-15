// C# program for Dijkstra's  
// single source shortest path  
// algorithm. The program is for  
// adjacency matrix representation  
// of the graph.  
using System.Collections.Generic;
using UnityEngine;
using MjKkaya.ShortestPath.Controllers;


namespace MjKkaya.ShortestPath.Algorithms
{
    public static class DijkstraAlgorithm
    {
        private static readonly int NO_PARENT = -1;
        private static VertexController mTargetVertexController;

        // Function that implements Dijkstra's  
        // single source shortest path  
        // algorithm for a graph represented  
        // using adjacency matrix  
        // representation  
        private static void dijkstra(Dictionary<int, Dictionary<int, float>> graphDict, int startVertex, int endVertex)
        {
            int vertexCount = graphDict.Count;

            // shortestDistances[i] will hold the  
            // shortest distance from src to i  
            Dictionary<int, float> shortestDistanceDict = new Dictionary<int, float>();


            // added[i] will true if vertex i is  
            // included / in shortest path tree  
            // or shortest distance from src to  
            // i is finalized  
            Dictionary<int, bool> added = new Dictionary<int, bool>();



            // Initialize all distances as  
            // INFINITE and added[] as false  
            foreach (var item in graphDict)
            {
                shortestDistanceDict.Add(item.Key, int.MaxValue);
                added.Add(item.Key, false);
            }


            // Distance of source vertex from  
            // itself is always 0  
            shortestDistanceDict[startVertex] = 0;

            // Parent array to store shortest path tree 
            // parent liste mantığı şu şekilde : buluna  en kısa yol ilgili dictionary'indez ine setedilir. Ör: "85" noktasına en yakın nokta "75" ise; 85. index'e 75 atanır.   

            Dictionary<int, int> parents = new Dictionary<int, int>();

            // The starting vertex does not  
            // have a parent  
            parents[startVertex] = NO_PARENT;

            // Find shortest path for all  
            // vertices  
            for (int i = 1; i < vertexCount; i++)
            {

                // Pick the minimum distance vertex  from the set of vertices not yet  processed. nearestVertex is always equal to startNode in first iteration.  

                //Yukarıda shortestDistanceDict[startVertex] = 0; yaıldığı için ilk bu nokta alınır çünkü diğer noktaların mesafesi (distance) int.MaxValue değeridir.
                //Bu nokta başlangıç noktasına en yakın nokta olur diğer tüm noktalar arasında.

                int nearestVertex = -1;
                float shortestDistance = int.MaxValue;

                foreach (var item in shortestDistanceDict)
                {
                    if (!added[item.Key] && item.Value < shortestDistance)
                    {
                        nearestVertex = item.Key;
                        shortestDistance = item.Value;
                    }
                }


                // Mark the picked vertex as processed
                //Bulunan noktanın tekrar taranmaması için set edilir.   
                added[nearestVertex] = true;


                // Update dist value of the adjacent vertices of the picked vertex.  
                //
                //if (nearestVertex != -1)
                //{
                //try
                //{
                if (graphDict.ContainsKey(nearestVertex))
                {
                    foreach (var item in graphDict)
                    {
                        float edgeDistance = graphDict[nearestVertex][item.Key];

                        if (edgeDistance > 0 && ((shortestDistance + edgeDistance) < shortestDistanceDict[item.Key]))
                        {
                            //parents[item.Key] = nearestVertex;
                            if (parents.ContainsKey(item.Key))
                                parents[item.Key] = nearestVertex;
                            else
                                parents.Add(item.Key, nearestVertex);
                            shortestDistanceDict[item.Key] = shortestDistance + edgeDistance;
                        }
                    }
                }
                else
                    Debug.Log("nearestVertex : " + nearestVertex);

                //}
                //catch (Exception ex)
                //{
                //    Debug.Log("ex:" + ex);
                //}
                //}
            }

            //printSolution(startVertex, shortestDistances, parents);
            PrintSolution(startVertex, vertexCount, parents, endVertex);
        }

        // A utility function to print  
        // the constructed distances  
        // array and shortest paths  
        private static void PrintSolution(int startVertex, int distancesLength, Dictionary<int, int> parents)
        {
            int nVertices = distancesLength;
            //Debug.Log("Vertex\t Distance\tPath");

            for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
            {
                if (vertexIndex != startVertex)
                {
                    //Debug.Log("\n" + startVertex + " -> ");
                    //Debug.Log(vertexIndex + " \t\t ");
                    //Debug.Log(distances[vertexIndex] + "\t\t");
                    printPath(vertexIndex, parents);
                }
            }
        }

        private static void PrintSolution(int startVertex, int distancesLength, Dictionary<int, int> parents, int endIndex)
        {
            int nVertices = distancesLength;
            //Debug.Log("Vertex\t Distance\tPath");

            for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
            {
                if (vertexIndex != startVertex && endIndex == vertexIndex)
                {
                    //Debug.Log("\n" + startVertex + " -> " + endIndex + " : " + distances[vertexIndex]);
                    //Debug.Log("printPath : ");
                    printPath(vertexIndex, parents);
                }
            }

            mTargetVertexController.SetShortestPath(foundPathList);
        }

        private static List<int> foundPathList = new List<int>();


        // Function to print shortest path  
        // from source to currentVertex  
        // using parents array  
        private static void printPath(int currentVertex, Dictionary<int, int> parents)
        {
            // Base case : Source node has  
            // been processed  
            if (currentVertex == NO_PARENT)
                return;

            printPath(parents[currentVertex], parents);
            foundPathList.Add(currentVertex);
            //Debug.Log(currentVertex + " ");
        }

        public static void StartDijkstraAlghorithm(VertexController vertexController, Dictionary<int, Dictionary<int, float>> graphDict, int startVertex, int endVertex)
        {
            mTargetVertexController = vertexController;
            foundPathList.Clear();
            dijkstra(graphDict, startVertex, endVertex);
        }
    }
}