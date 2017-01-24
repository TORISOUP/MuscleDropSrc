using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities
{
    /// <summary>
    /// 対象のGameObjectを移動させる
    /// </summary>
    public class GameObjectMover : MonoBehaviour
    {
        [SerializeField] private string rootName = "Singletons";

        [SerializeField]
        private bool DontDestroyRootObject = false;

        private void Awake()
        {
            var root = GameObject.Find(rootName) ?? new GameObject(rootName);;
            if (DontDestroyRootObject) { DontDestroyOnLoad(root); }
            transform.SetParent(root.transform);
            Destroy(this);
        }
    }
}
