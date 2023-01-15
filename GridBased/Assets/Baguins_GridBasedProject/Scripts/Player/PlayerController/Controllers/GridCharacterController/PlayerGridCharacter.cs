using UnityEngine;
using Cinemachine;

namespace PlayerCore
{
  public class PlayerGridCharacter : GridCharacter
  {
    public CinemachineFreeLook FreeLookCam { get => _freeLookCam; }
    public CinemachineVirtualCamera VCam { get => _vCam; }
    public Transform CharacterTargetOrientation { get => _characterTargetOrientation; }
    public Rigidbody RigidBody { get => _rigidBody; }

    [SerializeField] private CinemachineFreeLook _freeLookCam;
    [SerializeField] private CinemachineVirtualCamera _vCam;
    [SerializeField] private Transform _characterTargetOrientation;

    private Rigidbody _rigidBody;

    private void Awake()
    {
      _rigidBody = GetComponent<Rigidbody>();
    }
  }
}
