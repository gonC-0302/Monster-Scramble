using System.Collections;
using Fusion;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(NetworkCharacterController))]
[RequireComponent(typeof(CharacterStateMachine))]
public class SummonMonsterMovement : NetworkBehaviour
{
    [Networked]
    private int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(ColorChanged))]
    public Color NetworkedColor { get; set; }
    [SerializeField]
    private MeshRenderer _teamMarker;
    private Vector3 _direction;
    private bool _isBattle;
    private Transform _attackTargetTran;
    private NetworkCharacterController _characterController;
    private Collider[] _searchCol = new Collider[5];
    private CharacterStateMachine _stateMachine;
    private Transform _targetCrystalTran;   // 敵の拠点の位置は常に保持

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void Spawned()
    {
        _characterController = GetComponent<NetworkCharacterController>();
        _stateMachine = GetComponent<CharacterStateMachine>();
        _teamMarker.gameObject.transform.localScale = Vector3.zero;
        TeamID = Runner.LocalPlayer.PlayerId;
    }
    public void Init(Transform targetTran,int monsterID)
    {
        _targetCrystalTran = targetTran;
        _stateMachine.SwitchState(CharacterState.MoveToTarget);
        _attackTargetTran = _targetCrystalTran;
        StartCoroutine(SearchBattleTarget());
        StartCoroutine(ChangeMarkerColor());
        var maxSpeed = DataBaseManager.instance.dataSO._monsterDatasList.Find(x => x.ID == monsterID).MaxSpeed;
        _characterController.maxSpeed = maxSpeed;
    }
    /// <summary>
    /// チームマーカーの色を変更
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeMarkerColor()
    {
        yield return new WaitForSeconds(0.1f);
        if(Runner.LocalPlayer.PlayerId == 1) NetworkedColor = new Color(1, 0, 0, 0.25f);
        else if(Runner.LocalPlayer.PlayerId == 2) NetworkedColor = new Color(0, 0, 1, 0.25f);
    }
    void ColorChanged()
    {
        _teamMarker.gameObject.transform.DOScale(new Vector3(10,0.1f,10), 5f).SetEase(Ease.InCirc);
        _teamMarker.material.color = NetworkedColor;
    }
    /// <summary>
    /// 周辺に攻撃対象がいないか確認する
    /// </summary>
    /// <returns></returns>
    private IEnumerator SearchBattleTarget()
    {
        yield return new WaitForSeconds(1f);
        _isBattle = false;
        while (!_isBattle)
        {
            int layerMask = LayerMask.GetMask(new string[] { "Character" , "Crystal" });
            var num = Physics.OverlapSphereNonAlloc(gameObject.transform.position, 5f, _searchCol, layerMask);
            if (num >= 1)
            {
                for (int i = 0; i < _searchCol.Length; i++)
                {
                    if (_searchCol[i] == null) continue;
                    if (_searchCol[i].gameObject.TryGetComponent(out IDamagable target))
                    {
                        if (target.TeamID == TeamID) continue;
                        _isBattle = true;
                        _attackTargetTran = _searchCol[i].gameObject.transform;
                    }
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.TryGetComponent(out IDamagable target))
        {
            if (target.TeamID == TeamID) return;
            transform.LookAt(hit.gameObject.transform);
            _stateMachine.SwitchState(CharacterState.PreparateAttack);
        }
    }
    public override void FixedUpdateNetwork()
    {
        Move();
    }
    private void Move()
    {
        if (!Object.HasStateAuthority) return;
        switch (_stateMachine.CurrentState)
        {
            case CharacterState.Idle:
                _attackTargetTran = _targetCrystalTran;
                StartCoroutine(SearchBattleTarget());
                _stateMachine.SwitchState(CharacterState.MoveToTarget);
                break;
            case CharacterState.MoveToTarget:
                if (_attackTargetTran == null)
                {
                    _attackTargetTran = _targetCrystalTran;
                    StartCoroutine(SearchBattleTarget());
                }
                var targetPos = new Vector3(_attackTargetTran.position.x, transform.position.y, _attackTargetTran.position.z);
                if (Vector3.Distance(transform.position, targetPos) < 2)
                {
                    _stateMachine.SwitchState(CharacterState.PreparateAttack);
                }
                _direction = (targetPos - transform.position).normalized;
                _characterController.Move(_direction);
                break;
            case CharacterState.Attack:
                _attackTargetTran = transform;
                break;
        }
    }
}