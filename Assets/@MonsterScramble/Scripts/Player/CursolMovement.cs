using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class CursolMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject _cursol;
    [SerializeField]
    private Material _cursolMaterial;

    private void Awake()
    {
        _cursol.SetActive(false);
    }
    /// <summary>
    /// 初期設定
    /// </summary>
    public void Initialize()
    {
        _cursol.transform.parent = null;
        _cursolMaterial.color = Color.black;
        _cursol.transform.localScale = Vector3.one * 0.5f;
        _cursol.SetActive(true);
    }
    /// <summary>
    /// カーソルを移動
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="isAttackTarget"></param>
    public void Move(Vector3 targetPos, bool isAttackTarget)
    {
        _cursol.transform.position = new Vector3(targetPos.x, _cursol.transform.position.y, targetPos.z);
        if (isAttackTarget)
        {
            _cursolMaterial.color = Color.red;
            _cursol.transform.localScale = Vector3.one;
        }
        else
        {
            _cursolMaterial.color = Color.black;
            _cursol.transform.localScale = Vector3.one * 0.5f;
        }
    }
    private void OnDestroy()
    {
        Destroy(_cursol);
    }
}
