using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SummonMonsterMovement : NetworkBehaviour
{
    private Transform _targetCrystalTran;
    private int _teamID;
    public int TeamID => _teamID;
    private Vector3 _direction;
    private Transform _targetTran;
    private NetworkCharacterController _characterController_;
    private Collider[] searchCol = new Collider[5];
    private bool _isBattle;
    private CharacterStateMachine _stateManager;
    [SerializeField]
    private MeshRenderer _teamMarker;
  

    private void Awake()
    {
        _characterController_ = GetComponent<NetworkCharacterController>();
        _stateManager = GetComponent<CharacterStateMachine>();
    }

    public void Init(int playerID, Transform targetTran,Material mat)
    {
        _teamID = playerID;
        _targetCrystalTran = targetTran;
        _stateManager.SwitchState(CharacterState.MoveToTarget);
        _targetTran = _targetCrystalTran;
        _teamMarker.material = mat;
        StartCoroutine(SearchBattleTarget());
    }

    private IEnumerator SearchBattleTarget()
    {
        yield return new WaitForSeconds(1f);
        _isBattle = false;
        while (!_isBattle)
        {
            int layerMask = LayerMask.GetMask(new string[] { "Character" , "Crystal" });
            var num = Physics.OverlapSphereNonAlloc(gameObject.transform.position, 5f, searchCol, layerMask);
            if (num >= 1)
            {
                for (int i = 0; i < searchCol.Length; i++)
                {
                    if (searchCol[i] == null) continue;
                    if (searchCol[i].gameObject.TryGetComponent(out IDamagable target))
                    {
                        if (target.TeamID == _teamID) continue;
                        _isBattle = true;
                        _targetTran = searchCol[i].gameObject.transform;
                    }
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            switch (_stateManager.CurrentState)
            {
                case CharacterState.Idle:
                    _targetTran = _targetCrystalTran;
                    StartCoroutine(SearchBattleTarget());
                    _stateManager.SwitchState(CharacterState.MoveToTarget);
                    break;
                case CharacterState.MoveToTarget:
                    if(_targetTran == null)
                    {
                        _targetTran = _targetCrystalTran;
                        StartCoroutine(SearchBattleTarget());
                    }
                    var targetPos = new Vector3(_targetTran.position.x, transform.position.y, _targetTran.position.z);
                    _direction = (targetPos - transform.position).normalized;
                    _characterController_.Move(_direction);
                    break;
                case CharacterState.PreparateAttack:
                    break;
                case CharacterState.Attack:
                    _targetTran = transform;
                    break;
            }
        }
    }
}