using System.Collections.Generic;
using PlayerCore;
using UnityEngine;

namespace GridCore
{
  [System.Serializable]
  public class GridInfo
  {
    public bool IsActive
    {
      get => _isActive;
      set
      {
        if (value)
          TurnManager.Instance.ActivateTurns(this);
        else
          TurnManager.Instance.DeActivateTurns(this);

        _isActive = value;
      }
    }

    public GridBehaviour TargetGrid { get => _targetGrid; set => _targetGrid = value; }
    public List<List<GridCharacter>> TotalCharacters { get => _totalCharacters; set => _totalCharacters = value; }

    [SerializeField] private bool _isActive = false;
    private GridBehaviour _targetGrid;
    private List<List<GridCharacter>> _totalCharacters;

    public GridInfo(GridBehaviour grid, List<GridCharacter> enemyCharacters)
    {
      TargetGrid = grid;
      _totalCharacters = new List<List<GridCharacter>> { enemyCharacters };
    }
  }
}
