using UnityEngine;
using UnityEngine.Assertions;

namespace Common
{
    public abstract class ContextBase : MonoBehaviour
    {
        public float Tick => Time.time;
        public float DeltaTime => Time.deltaTime;
        public Vector3 Position => transform.position;

        protected virtual void Awake()
        {
            ResolveDependencies();
        }

        protected virtual void Update()
        {

        }

        protected virtual void OnDestroy()
        {
            
        }

        protected virtual void ResolveDependencies()
        {
        }
    }
}