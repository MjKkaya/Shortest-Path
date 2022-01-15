using MjKkaya.ShortestPath.Utilities;


namespace MjKkaya.ShortestPath.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public UIManager UIManager;
        public MeshManager MeshManager;


        void Awake()
        {
            Initialize(this);
            UIManager.Initialize();
            MeshManager.Initialize();
        }
    }
}