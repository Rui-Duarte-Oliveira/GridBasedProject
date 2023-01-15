using UnityEngine;
using GridCore;
using System.Collections.Generic;
using TMPro;

namespace PlayerCore
{
  public class GridComponentsManager : ComponentsManager
  {
    [Space(20)]
    [SerializeField] private List<GameObject> _UIElements = new List<GameObject>();
    [SerializeField] TMP_Text _statusText;

    private List<GridInfo> _gridInfos;
    private PlayerGridCharacter _targetCharacter;
    private string _statusTextString;

    public override void Initialize(CharacterBase targetCharacter)
    {
      base.Initialize(targetCharacter);
      _targetCharacter = (PlayerGridCharacter)targetCharacter;
    }

    public override void OnActivation()
    {
      base.OnActivation();
      UIandCursorStateChange(true);

      if (_gridInfos == null || _targetCharacter.CurrentGrid == null)
        return;

      _targetCharacter.CurrentGrid.TargetGrid.SetGridCharacterPosition(_targetCharacter);                                //All of this is temporary
      _targetCharacter.GetComponent<Rigidbody>().velocity = Vector3.zero;

      if (!_targetCharacter.CurrentGrid.IsActive)
        _targetCharacter.CurrentGrid.IsActive = true;
    }

    public override void OnDeactivation()
    {
      base.OnDeactivation();
      UIandCursorStateChange(false);
    }

    public override void OnActiveUpdate()
    {
      base.OnActiveUpdate();
      ActionPointTextUpdate();
    }

    public override void OnTargetSwitch()
    {
      base.OnTargetSwitch();
      IsInsideGridBoundaries();
    }

    public override int CanBeSetActive()
    {
      return IsInsideGridBoundaries();
    }

    public override int CanBeSetInactive()
    {
      if (_targetCharacter.CurrentGrid == null) return 2;

      if (_targetCharacter.CurrentGrid.TargetGrid == null) return 2;

      return base.CanBeSetInactive();
    }

    /// <summary>
    /// Checks if target character is inside of any boundary of any Grid in the level.
    /// Then it will assign the grid to the character and return true if this condition is met.
    /// If the condition isnt met then it will reset character grid.
    /// </summary>
    /// <returns></returns>
    private int IsInsideGridBoundaries()
    {
      _gridInfos = GridManager.Instance.GridInfos;

      for (int i = 0; i < _gridInfos.Count; i++)
      {
        if (_gridInfos[i] == null)
          continue;

        if (_gridInfos[i] == _targetCharacter.CurrentGrid)
          if (GridManager.Instance.GridInfos[i].IsActive)
            return 2;
          else
            return 1;

        Vector2Int gridPosition = _gridInfos[i].TargetGrid.GetGridPosition(_targetCharacter.transform.position);

        if (_gridInfos[i].TargetGrid.IsInsideGridBoundry(gridPosition.x, gridPosition.y))
        {
          GridManager.Instance.UpdateGridInfoStatus(_gridInfos[i], _targetCharacter);
          _targetCharacter.CurrentGrid = _gridInfos[i];

          if (GridManager.Instance.GridInfos[i].IsActive)
            return 2;

          return 1;
        }
      }

      if (_targetCharacter.CurrentGrid != null)
      {
        GridManager.Instance.UpdateGridInfoStatus(_targetCharacter.CurrentGrid, _targetCharacter, false);
        _targetCharacter.CurrentGrid = null;
      }

      return 0;
    }

    private void UIandCursorStateChange(bool isActive)
    {
      Cursor.visible = isActive;

      foreach (GameObject element in _UIElements)
        element.SetActive(isActive);

      if (isActive)
        Cursor.lockState = CursorLockMode.None;
      else
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void ActionPointTextUpdate()
    {
      _statusTextString = "Current Action Points: " + _targetCharacter.CurrentActionPoints;

      if (TurnManager.Instance.IsCurrentSideActive(_targetCharacter))
        _statusTextString += "\n Player Turn";
      else
        _statusTextString += "\n Enemy Turn";

      _statusText.text = _statusTextString;
    }
  }
}
