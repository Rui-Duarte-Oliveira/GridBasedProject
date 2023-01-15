using UnityEngine;
using Cinemachine;

namespace PlayerCore
{
  public class PlayerThirdPersonCharacter : CharacterBase
  {
    public CinemachineFreeLook FreeLookCam { get => _freeLookCam; set => _freeLookCam = value; }
    public Rigidbody RigidBody { get => _rigidBody; set => _rigidBody = value; }
    public Transform CharacterOrientation { get => _characterOrientation; set => _characterOrientation = value; }
    public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
    public float PlayerHeight { get => _playerHeight; set => _playerHeight = value; }
    public float MovementMultiplier { get => _movementMultiplier; set => _movementMultiplier = value; }
    public float SprintSpeed { get => _sprintSpeed; set => _sprintSpeed = value; }
    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; }
    public float Accelaration { get => _accelaration; set => _accelaration = value; }

    [SerializeField] private CinemachineFreeLook _freeLookCam;
    [SerializeField] private Transform _characterOrientation;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _movementMultiplier;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _accelaration;

    private Rigidbody _rigidBody;

    private void Awake()
    {
      _rigidBody = GetComponent<Rigidbody>();
    }
  }
}
