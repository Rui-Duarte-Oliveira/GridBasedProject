using System.Collections.Generic;
using UnityEngine;

namespace PlayerCore
{
  public class Supp_UserInterfaceDisplay : SupplementaryComponent
  {
    [SerializeField] private List<GameObject> _UIElements = new List<GameObject>();

    public override void OnActivation()
    {
      ActivateUIElements(_UIElements, true);
    }

    public override void OnDeactivation()
    {
      ActivateUIElements(_UIElements, false);
    }

    public override void ExecuteAction()
    {
      ActivateUIElements(_UIElements, false);
    }

    public override void OnActionPerformed()
    {
      ActivateUIElements(_UIElements, true);
    }

    private void ActivateUIElements(List<GameObject> UIElements, bool isActive)
    {
      for (int i = 0; i < UIElements.Count; i++)
        UIElements[i].SetActive(isActive);
    }
  }
}
