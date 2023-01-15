using UnityEngine;

namespace PlayerCore
{
  public class PlayerStateController : MonoBehaviour
  {
    private PlayerController _playerController;
    private PlayerGridCharacter _playerGridCharacter;

    private void Start()
    {
      _playerController = PlayerController.Instance;
    }

    private void Update()
    {
      _playerGridCharacter = (PlayerGridCharacter)_playerController.CurrentTargetCharacter.CurrentTargetCharacterTypes[0];
      IsPriorityStateChanging();

      if (Input.GetKeyDown(KeyCode.Tab))
        ChangeCurrentTargetCharacter();

      if (CanChangeCurrentState())
        if (Input.GetKeyDown(KeyCode.Q))
          ChangeCurrentControllerManager(ChangeCurrentState());

      if (Input.GetKeyDown(KeyCode.E))                                                                                                      //TEMPORARY DEBUG PORPUSES
        for (int i = 0; i < GridManager.Instance.GridInfos.Count; i++)
          if (GridManager.Instance.GridInfos[i].IsActive && GridManager.Instance.GridInfos[i] == _playerGridCharacter.CurrentGrid)
            GridManager.Instance.GridInfos[i].IsActive = false;
    }

    #region ChangeCurrentTargetCharacter
    /// <summary>
    /// Cycles through the target character list and attempts to changes the target;
    /// </summary>
    private void ChangeCurrentTargetCharacter()
    {
      if (_playerController.CurrentTargetCharacter.IsPerforming)
        return;

      for (int i = 0; i < _playerController.CurrentTargetCharacterList.Count; i++)
      {
        if (_playerController.CurrentTargetCharacterList[i] == _playerController.CurrentTargetCharacter)
        {
          if ((i + 1) < _playerController.CurrentTargetCharacterList.Count)
          {
            ChangeCurrentTargetCharacter(_playerController.CurrentTargetCharacterList[i + 1]);
            return;
          }

          ChangeCurrentTargetCharacter(_playerController.CurrentTargetCharacterList[0]);
          return;
        }
      }
    }

    /// <summary>
    /// Changes the current target character, calling the due functions related to target switching
    /// </summary>
    /// <param name="newTargetCharacter"></param>
    private void ChangeCurrentTargetCharacter(CharacterManager newTargetCharacter)
    {
      if (newTargetCharacter == null)
      {
        Debug.LogError("Trying to change to null Character!");
        return;
      }

      _playerController.CurrentActiveManager.OnDeactivation();

      _playerController.CurrentTargetCharacter = newTargetCharacter;

      for (int i = 0; i < _playerController.ComponentsManagers.Count; i++)
        _playerController.ComponentsManagers[i].Initialize(_playerController.CurrentTargetCharacter.CurrentTargetCharacterTypes[i]);

      if (!IsPriorityStateChanging())
        if (_playerController.CurrentTargetCharacter.CurrentComponentManager != _playerController.CurrentActiveManager)
          ChangeCurrentControllerManager(newTargetCharacter.CurrentComponentManager);


      _playerController.CurrentActiveManager.OnTargetSwitch();
      _playerController.CurrentActiveManager.OnActivation();
    }
    #endregion

    #region ChangeState
    /// <summary>
    /// Changes the currently active manager to another manager that can be activated.
    /// </summary>
    private void ChangeCurrentControllerManager(ComponentsManager newManager)
    {
      if (_playerController.CurrentTargetCharacter.IsPerforming)
        return;

      _playerController.CurrentActiveManager.IsActive = false;

      _playerController.CurrentActiveManager = newManager;

      _playerController.CurrentActiveManager.IsActive = true;

      _playerController.CurrentTargetCharacter.CurrentComponentManager = _playerController.CurrentActiveManager;
    }
    /// <summary>
    /// If its successfull then it will return the new component manager.
    /// If its not successfull then it will return null by default.
    /// </summary>
    /// <returns></returns>
    private bool CanChangeCurrentState()
    {
      if (ChangeCurrentState() != null)
        return true;

      return false;
    }

    /// <summary>
    /// Returns the new active manager if the specific conditions are met.
    /// If the conditions arent met, it will return null by default
    /// </summary>
    /// <returns></returns>
    private ComponentsManager ChangeCurrentState()
    {
      for (int i = 0; i < _playerController.ComponentsManagers.Count; i++)
      {
        if (_playerController.ComponentsManagers[i] == _playerController.CurrentActiveManager)
          continue;

        if (_playerController.CurrentActiveManager.CanBeSetInactive() != 0)
          if (_playerController.ComponentsManagers[i].CanBeSetActive() == 1)
            return _playerController.ComponentsManagers[i];
      }

      return null;
    }

    private bool IsPriorityStateChanging()
    {
      for (int i = 0; i < _playerController.ComponentsManagers.Count; i++)
      {
        if (_playerController.ComponentsManagers[i] == _playerController.CurrentActiveManager)
          continue;

        if (_playerController.ComponentsManagers[i].CanBeSetActive() == 2)
        {
          ChangeCurrentControllerManager(_playerController.ComponentsManagers[i]);
          return true;
        }

        if (_playerController.CurrentActiveManager.CanBeSetInactive() == 2)
        {
          ChangeCurrentControllerManager(ChangeCurrentState());
          return true;
        }
      }

      return false;
    }
    #endregion
  }
}
