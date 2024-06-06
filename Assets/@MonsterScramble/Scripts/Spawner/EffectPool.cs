using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPool : MonoBehaviour
{
    public static EffectPool instance;

    ObjectPool<GameObject> _hitEffectPool;
    [SerializeField] private GameObject _hitEffectPrefab;
    ObjectPool<GameObject> _deathEffectPool;
    [SerializeField] private GameObject _deathEffectPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InitHitEffectPool();
        InitDeathEffectPool();
    }

    private void InitHitEffectPool()
    {
        _hitEffectPool = new ObjectPool<GameObject>(
            OnCreateHitEffect,  // createFunc
            OnTakeFromPool,        // actionOnGet
            OnReturnedToPool,      // actionOnRelease
            OnDestroyPoolObject,   // actionOnDestroy
            true,                  // collectionCheck
            10,                    // defaultCapacity
            30                     // maxSize
        );
    }

    private void InitDeathEffectPool()
    {
        _deathEffectPool = new ObjectPool<GameObject>(
            OnCreateDeathEffect,  // createFunc
            OnTakeFromPool,        // actionOnGet
            OnReturnedToPool,      // actionOnRelease
            OnDestroyPoolObject,   // actionOnDestroy
            true,                  // collectionCheck
            10,                    // defaultCapacity
            30                     // maxSize
        );
    }

    void OnTakeFromPool(GameObject go)
    {
        go.SetActive(true);
    }
    void OnReturnedToPool(GameObject go)
    {
        go.SetActive(false);
    }
    void OnDestroyPoolObject(GameObject go)
    {
        Destroy(go);
    }
    GameObject OnCreateHitEffect()
    {
        return Instantiate(_hitEffectPrefab, transform);
    }
    public GameObject GetHitEffect(Vector3 position)
    {
        GameObject obj = _hitEffectPool.Get();
        Transform tf = obj.transform;
        tf.position = new Vector3(position.x,2f,position.z);
        StartCoroutine(ReleaseHitEffect(obj));
        return obj;
    }
    public IEnumerator ReleaseHitEffect(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        _hitEffectPool.Release(obj);
    }
    GameObject OnCreateDeathEffect()
    {
        return Instantiate(_deathEffectPrefab, transform);
    }
    public GameObject GetDeathEffect(Vector3 position)
    {
        GameObject obj = _deathEffectPool.Get();
        Transform tf = obj.transform;
        tf.position = new Vector3(position.x, 2f, position.z);
        StartCoroutine(ReleaseDeathEffect(obj));
        return obj;
    }
    public IEnumerator ReleaseDeathEffect(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        _deathEffectPool.Release(obj);
    }
}