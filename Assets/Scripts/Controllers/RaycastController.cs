using UnityEngine;
using MjKkaya.ShortestPath.Managers;


namespace MjKkaya.ShortestPath.Controllers
{
    public class RaycastController : MonoBehaviour
    {
        Ray ray;
        RaycastHit hit;


        void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log(hit.collider.name);
                    //Debug.Log("Input.mousePosition : " + Input.mousePosition);
                    //Debug.Log("ray.origin : " + ray.origin.x + "," + ray.origin.y + "," + ray.origin.z);
                    //Debug.Log("ray.direction : " + ray.direction.x + "," + ray.direction.y + "," + ray.direction.z);
                    //Debug.Log("hit.triangleIndex : " + hit.triangleIndex);
                    //Debug.Log("hit.point : " + hit.point);
                    //Debug.Log("hit.local point : " +  (hit.point - hit.transform.position));
                    //Debug.Log("-----------------------------------");

                    GameManager.Instance.MeshManager.SetStartPoint(hit.point);

                    //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
                }
                else if (Input.GetMouseButtonDown(1))   //Click with right mouse button.
                {
                    GameManager.Instance.MeshManager.SetEndPoint(hit.point);
                }
            }
        }
    }
}