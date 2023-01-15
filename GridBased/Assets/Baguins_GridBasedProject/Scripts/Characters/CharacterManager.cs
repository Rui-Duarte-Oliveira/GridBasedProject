using System.Collections.Generic;
using UnityEngine;

namespace PlayerCore
{
  /// <summary>
  /// Serves the porpuse of grouping all of the character controllers for the player controller system
  /// </summary>
  public class CharacterManager : MonoBehaviour
  {
    public List<CharacterBase> CurrentTargetCharacterTypes { get => _currentTargetCharacterTypes; set => _currentTargetCharacterTypes = value; }
    public bool IsPerforming { get => _isPerformingAction; set => _isPerformingAction = value; }
    public ComponentsManager CurrentComponentManager { get => _currentComponentManager; set => _currentComponentManager = value; }

    [SerializeField] private bool _isPerformingAction;

    private List<CharacterBase> _currentTargetCharacterTypes = new List<CharacterBase>();
    private ComponentsManager _currentComponentManager;


    //Gets all of the character controllers on the designated character(child)
    private void Awake()
    {
      _currentTargetCharacterTypes.AddRange(GetComponentsInChildren<CharacterBase>());

      for (int i = 0; i < _currentTargetCharacterTypes.Count; i++)
        _currentTargetCharacterTypes[i].CharacterControllerManager = this;
    }
  }
}
