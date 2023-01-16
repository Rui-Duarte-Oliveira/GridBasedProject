using UnityEngine;

namespace PlayerCore
{
  public class ThirdPersonComponentsManager : ComponentsManager
  {
    public override void OnActivation()
    {
      base.OnActivation();
      Cursor.lockState= CursorLockMode.Locked;
      Cursor.visible = false;
    }
  }
}
