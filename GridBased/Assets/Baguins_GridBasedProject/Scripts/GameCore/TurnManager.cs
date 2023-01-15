using UnityEngine;
using System.Collections.Generic;
using GridCore;

namespace PlayerCore
{
  public class TurnManager : MonoBehaviour
  {
    public static TurnManager Instance { get => _instance; set => _instance = value; }

    private static TurnManager _instance;

    [SerializeField] private List<GridInfo> _currentlyActiveGrids = new List<GridInfo>();

    [SerializeField] private List<GridCharacter> _temp = new();

    private Dictionary<GridInfo, Dictionary<List<GridCharacter>, int>> _totalActionPoints = new Dictionary<GridInfo, Dictionary<List<GridCharacter>, int>>();

    private void Awake()
    {
      if (Instance != null)
      {
        Debug.LogWarning("Error: Trying to instantiate a TurnManager instance when one is already present in scene. " + transform.name);
        Destroy(this);
        return;
      }

      Instance = this;
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.G))
        for (int i = 0; i < _temp.Count; i++)                                       //Very temporary
          _temp[i].CurrentActionPoints = 0;
    }

    public void ActivateTurns(GridInfo targetGrid)
    {
      _currentlyActiveGrids.Add(targetGrid);
      _totalActionPoints.Add(targetGrid, new Dictionary<List<GridCharacter>, int>());
      InitializeSides(targetGrid);
      AddActionPointsToSide(targetGrid, targetGrid.TotalCharacters[targetGrid.TotalCharacters.Count - 1]);
    }

    public void DeActivateTurns(GridInfo targetGrid)
    {
      ResetActionPoints(targetGrid);
      _currentlyActiveGrids.Remove(targetGrid);
      _totalActionPoints.Remove(targetGrid);
      GridManager.Instance.DeleteGrid(targetGrid);
    }

    public void UpdateActionPointAmmount(GridCharacter gridCharacter)
    {
      List<GridCharacter> side;

      side = GetSide(gridCharacter.CurrentGrid, gridCharacter);

      _totalActionPoints[gridCharacter.CurrentGrid][side] -= gridCharacter.TotalActionPoints;

      if (DeactivationCondition(gridCharacter.CurrentGrid))
      {
        DeActivateTurns(gridCharacter.CurrentGrid);
        return;
      }

      if (_totalActionPoints[gridCharacter.CurrentGrid][side] <= 0)
        ChangeTurn(gridCharacter.CurrentGrid, side);
    }

    public bool IsCurrentSideActive(GridCharacter targetCharacter)
    {
      if (!_totalActionPoints.ContainsKey(targetCharacter.CurrentGrid))
        return false;

      return _totalActionPoints[targetCharacter.CurrentGrid][GetSide(targetCharacter.CurrentGrid, targetCharacter)] <= 0;
    }

    private void ChangeTurn(GridInfo targetGrid, List<GridCharacter> currentSide)
    {
      for (int i = 0; i < targetGrid.TotalCharacters.Count; i++)
      {
        if (targetGrid.TotalCharacters[i] != currentSide)
          continue;

        if (i + 1 < targetGrid.TotalCharacters.Count)
        {
          AddActionPointsToSide(targetGrid, targetGrid.TotalCharacters[i + 1]);
          return;
        }

        AddActionPointsToSide(targetGrid, targetGrid.TotalCharacters[0]);
        return;
      }
    }

    private void AddActionPointsToSide(GridInfo targetGrid, List<GridCharacter> targetSide)
    {
      int totalActionPoints = 0;

      for (int i = 0; i < targetSide.Count; i++)
      {
        targetSide[i].CurrentActionPoints = targetSide[i].TotalActionPoints;
        totalActionPoints += targetSide[i].TotalActionPoints;
      }

      _totalActionPoints[targetGrid][targetSide] = totalActionPoints;
    }

    private void InitializeSides(GridInfo targetGrid)
    {
      for (int i = 0; i < targetGrid.TotalCharacters.Count; i++)
        _totalActionPoints[targetGrid].Add(targetGrid.TotalCharacters[i], 0);
    }

    private bool DeactivationCondition(GridInfo targetGrid)
    {
      int sideCount = 0;

      for (int i = 0; i < targetGrid.TotalCharacters.Count; i++)
      {
        if (targetGrid.TotalCharacters[i].Count > 0)
          sideCount++;
      }

      if (sideCount <= 1)
        return true;

      return false;
    }

    private void ResetActionPoints(GridInfo targetGrid)
    {
      for (int i = 0; i < targetGrid.TotalCharacters.Count; i++)
        if (targetGrid.TotalCharacters[i].Count <= 0)
          continue;
        else
          for (int j = 0; j < targetGrid.TotalCharacters[i].Count; j++)
            targetGrid.TotalCharacters[i][j].CurrentActionPoints = -1;
    }

    private List<GridCharacter> GetSide(GridInfo targetGrid, GridCharacter gridCharacter)
    {
      for (int i = 0; i < targetGrid.TotalCharacters.Count; i++)
      {
        if (targetGrid.TotalCharacters[i].Count > 0)
          if (targetGrid.TotalCharacters[i][0].GetType() == gridCharacter.GetType())
            return targetGrid.TotalCharacters[i];
      }

      return null;
    }
  }
}
