using UnityEngine;

namespace MjKkaya.ShortestPath.Utilities
{
    [AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
    public class MouseOrbit : MonoBehaviour
    {
        public Transform target;
        public float distance = 5.0f;
        public float xSpeed = 120.0f;
        public float ySpeed = 120.0f;

        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;

        public float distanceMin = .5f;
        public float distanceMax = 15f;

        //public bool isInWireframeMode;

        private Rigidbody rigidbody;

        float x = 0.0f;
        float y = 0.0f;

        // Use this for initialization
        void Start()
        {
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            rigidbody = GetComponent<Rigidbody>();

            // Make the rigid body not change rotation
            if (rigidbody != null)
            {
                rigidbody.freezeRotation = true;
            }
        }

        void LateUpdate()
        {
            if (target && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit, LayerMask.GetMask("Ground")))
                {
                    distance -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }


        //public void SetWireframeMode(bool isActive)
        //{
        //    isInWireframeMode = isActive;
        //}

        //// show wireframe
        //private void OnPreRender()
        //{
        //    if (isInWireframeMode)
        //    {
        //        //GL.sRGBWrite = true;
        //        GL.wireframe = true;        //Not support on (OpenGL ES) 
        //        GL.Color(Color.red);
        //        GL.End();
        //        //GL.invertCulling = true;
        //    }
        //}
        //private void OnPostRender()
        //{
        //    if (isInWireframeMode)
        //    {
        //        GL.wireframe = false;
        //        GL.Color(Color.black);
        //        GL.End();
        //    }
        //}
    }
}