using UnityEngine;

namespace PlayerCore
{
  public class Supp_ThirdPersonCamera : SupplementaryComponent
  {
    private Core_ThirdPersonMovement _parentCoreComponent;

    public override void Setup(CoreComponent parentCoreComponent)
    {
      _parentCoreComponent = (Core_ThirdPersonMovement)parentCoreComponent;
    }

    public override void OnActiveUpdate()
    {
      AlignCharacterOrientation(_parentCoreComponent.TargetThirdPersonCharacter);
    }

    public override void OnActivation()
    {
      SetCinemachineCameraActiveState(true);
    }

    public override void OnDeactivation()
    {
      SetCinemachineCameraActiveState(false);
    }

    private void AlignCharacterOrientation(PlayerThirdPersonCharacter targetThirdPersonCharacter)
    {
      Vector3 viewDirection = targetThirdPersonCharacter.transform.position -
        new Vector3(Camera.main.transform.position.x, targetThirdPersonCharacter.transform.position.y, Camera.main.transform.position.z);

      targetThirdPersonCharacter.CharacterOrientation.forward = viewDirection.normalized;

      float horizontalInput = Input.GetAxis("Horizontal");
      float verticalInput = Input.GetAxis("Vertical");
      Vector3 inputDirection = targetThirdPersonCharacter.CharacterOrientation.forward * verticalInput + targetThirdPersonCharacter.CharacterOrientation.right * horizontalInput;

      if (inputDirection != Vector3.zero)
        targetThirdPersonCharacter.transform.forward = Vector3.Slerp(targetThirdPersonCharacter.transform.forward, inputDirection.normalized, Time.deltaTime * targetThirdPersonCharacter.RotationSpeed);
    }

    private void SetCinemachineCameraActiveState(bool isActive) => _parentCoreComponent.TargetThirdPersonCharacter.FreeLookCam.gameObject.SetActive(isActive);
  }
}
