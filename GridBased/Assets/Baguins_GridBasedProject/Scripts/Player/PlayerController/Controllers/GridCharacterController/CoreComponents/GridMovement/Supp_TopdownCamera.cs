namespace PlayerCore
{
  public class Supp_TopdownCamera : SupplementaryComponent
  {
    private Core_GridMovement _parentCoreComponent;

    public override void Setup(CoreComponent parentCoreComponent)
    {
      _parentCoreComponent = (Core_GridMovement)parentCoreComponent;
    }

    public override void OnActivation()
    {
      SetCinemachineCameraActiveState(true);
    }

    public override void OnDeactivation()
    {
      SetCinemachineCameraActiveState(false);
    }

    private void SetCinemachineCameraActiveState(bool isActive) => _parentCoreComponent.TargetGridCharacter.FreeLookCam.gameObject.SetActive(isActive);
  }
}
