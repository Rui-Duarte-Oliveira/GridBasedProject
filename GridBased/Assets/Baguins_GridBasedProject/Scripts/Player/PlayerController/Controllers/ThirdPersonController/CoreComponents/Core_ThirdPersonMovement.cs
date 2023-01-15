using UnityEngine;

namespace PlayerCore
{
  public class Core_ThirdPersonMovement : CoreComponent
  {
    [Space(20)]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundDistanceOffset;

    public PlayerThirdPersonCharacter TargetThirdPersonCharacter { get => _targetThirdPersonCharacter; }

    private PlayerThirdPersonCharacter _targetThirdPersonCharacter;

    private Vector3 _movement;
    private Vector3 _slopeMovement;
    private RaycastHit _groundHit;
    private float _currentSpeed;

    public override void OnInitialization(CharacterBase targetCharacter)
    {
      base.OnInitialization(targetCharacter);
      _targetThirdPersonCharacter = (PlayerThirdPersonCharacter)targetCharacter;
    }

    public override void OnActiveUpdate()
    {
      base.OnActiveUpdate();
      CharacterMovement();
    }

    public override void OnFixedUpdate()
    {
      base.OnFixedUpdate();
      ApplyMovementForces();
    }

    private void CharacterMovement()
    {
      CalculatePlayerMovement();
      HandleSpeedStates();
      CalculatePlayerSlopeMovement();
    }

    private void ApplyMovementForces()
    {
      if (IsSlope())
      {
        _targetThirdPersonCharacter.RigidBody.AddForce(_slopeMovement.normalized * _currentSpeed * _targetThirdPersonCharacter.MovementMultiplier, ForceMode.Acceleration);
        return;
      }

      _targetThirdPersonCharacter.RigidBody.AddForce(_movement.normalized * _currentSpeed * _targetThirdPersonCharacter.MovementMultiplier, ForceMode.Acceleration);
      return;
    }

    private void CalculatePlayerMovement()
    {
      Vector2 moveValue = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

      _movement = Vector3.ClampMagnitude((moveValue.y * _targetThirdPersonCharacter.CharacterOrientation.forward) + (moveValue.x * _targetThirdPersonCharacter.CharacterOrientation.right), 1f);
    }

    private void HandleSpeedStates()
    {
      if (Input.GetKey(KeyCode.LeftShift))
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetThirdPersonCharacter.SprintSpeed, _targetThirdPersonCharacter.Accelaration * Time.deltaTime);
      else
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetThirdPersonCharacter.WalkSpeed, _targetThirdPersonCharacter.Accelaration * Time.deltaTime);
    }

    private void CalculatePlayerSlopeMovement() => _slopeMovement = Vector3.ProjectOnPlane(_movement, _groundHit.normal);

    private bool IsSlope()
    {
      if (Physics.Raycast(_targetThirdPersonCharacter.transform.position, Vector3.down, out _groundHit, _targetThirdPersonCharacter.PlayerHeight + _groundDistanceOffset))
        if (_groundHit.normal != Vector3.up)
          return true;
        else
          return false;

      return false;
    }
  }
}
