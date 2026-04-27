using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class HandlePooling : MonoBehaviour
{
    private IObjectPool<HandlePooling> _objectPool;
    public virtual IObjectPool<HandlePooling> ObjectPool { set => _objectPool = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PoolReset"))
        {
            // if _objectPool is null then destroy object on collision
            if (_objectPool == null)
            {
                Destroy(gameObject);
                return;
            }

            // Debug.Log("Releasing to pool");
            _objectPool.Release(this);
        }

    }
}
