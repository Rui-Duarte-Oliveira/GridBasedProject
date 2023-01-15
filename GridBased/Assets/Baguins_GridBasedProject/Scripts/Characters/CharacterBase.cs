using UnityEngine;

namespace PlayerCore
{
  public abstract class CharacterBase : MonoBehaviour
  {
    /// <summary>
    /// Debug Purposes
    /// </summary>
    public bool IsPerformingAction
    {
      get => _isPerformingAction;

      set
      {
        _characterControllerManager.IsPerforming = value;
        _isPerformingAction = value;
      }
    }

    /// <summary>
    /// Debug Purposes
    /// </summary>
    public CharacterManager CharacterControllerManager { get => _characterControllerManager; set => _characterControllerManager = value; }


    [SerializeField] private bool _isPerformingAction;

    private CharacterManager _characterControllerManager;
  }
}
