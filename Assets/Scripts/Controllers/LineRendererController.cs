using UnityEngine;


namespace MjKkaya.ShortestPath.Controllers
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererController : MonoBehaviour
    {
        [SerializeField] private Transform mStartPointTrans;
        [SerializeField] private Transform mEndPointTrans;

        private LineRenderer mLineRenderer;
        private Transform mBaseTransform;
        private Vector3 startPointDefaultPos;
        private Vector3 endPointDefaultPos;
        private bool isSetDefeultPos = false;


        public void Initialize()
        {
            isSetDefeultPos = true;
            startPointDefaultPos = mStartPointTrans.position;
            endPointDefaultPos = mEndPointTrans.position;
            mLineRenderer = GetComponent<LineRenderer>();
            //mLineRenderer.positionCount = 0;
        }

        public void SetPositions(Vector3[] array)
        {
            //mLineRenderer.positionCount = 0;
            mLineRenderer.positionCount = array.Length;
            Vector3 newVector3;

            for (int i = 0; i < array.Length; i++)
            {
                newVector3 = array[i] + mBaseTransform.position;
                mLineRenderer.SetPosition(i, newVector3);
            }
        }

        public void SetBaseTrans(Transform newTransform)
        {
            mBaseTransform = newTransform;
            mLineRenderer.positionCount = 0;

            if (isSetDefeultPos)
            {
                mStartPointTrans.position = startPointDefaultPos;
                mEndPointTrans.position = endPointDefaultPos;
            }
        }

        public void SetStartPoint(Vector3 vector3)
        {
            mStartPointTrans.position = vector3;
        }

        public void SetEndPoint(Vector3 vector3)
        {
            mEndPointTrans.position = vector3;
        }
    }
}