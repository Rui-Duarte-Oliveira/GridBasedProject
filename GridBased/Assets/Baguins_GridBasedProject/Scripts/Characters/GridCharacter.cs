using GridCore;
using UnityEngine;

namespace PlayerCore
{
  public class GridCharacter : CharacterBase
  {
    [SerializeField] private int _currentActionPoints;
    [SerializeField] private int _totalActionPoints;
    [SerializeField] private int _movementRange;

    public int CurrentActionPoints
    {
      get => _currentActionPoints;
      set
      {
        if (value == 0)
          TurnManager.Instance.UpdateActionPointAmmount(this);

        if (value >= 0)
        {
          _currentActionPoints = value;
          return;
        }

        _currentActionPoints = 0;
      }
    }

    public GridInfo CurrentGrid { get => _currentGrid; set => _currentGrid = value; }
    public Vector2Int CurrentGridPosition { get => _currentGridPosition; set => _currentGridPosition = value; }
    public int TotalActionPoints { get => _totalActionPoints; set => _totalActionPoints = value; }
    public int MovementRange { get => _movementRange; set => _movementRange = value; }


    private GridInfo _currentGrid;
    private Vector2Int _currentGridPosition;
  }
}
