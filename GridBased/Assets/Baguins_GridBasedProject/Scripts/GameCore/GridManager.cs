using GridCore;
using PlayerCore;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
  [SerializeField] private List<GridInfo> _gridInfos = new List<GridInfo>();

  public static GridManager Instance { get => _instance; set => _instance = value; }
  public List<GridInfo> GridInfos { get => _gridInfos; set => _gridInfos = value; }

  private static GridManager _instance;

  private List<GridBehaviour> _grids;

  private void Awake()
  {
    if (Instance != null)
    {
      Debug.LogWarning("Error: Trying to instantiate a TurnManager instance when one is already present in scene. " + transform.name);
      Destroy(this);
      return;
    }

    Instance = this;


    _grids = GetComponentsInChildren<GridBehaviour>().ToList();

    for (int i = 0; i < _grids.Count; i++)
      _gridInfos.Add(new GridInfo(_grids[i], _grids[i].GetComponentsInChildren<GridCharacter>().ToList()));

    for (int i = 0; i < _grids.Count; i++)
    {
      if (_grids[i].GetComponentsInChildren<GridCharacter>().Length != 0)
        for (int j = 0; j < _grids[i].GetComponentsInChildren<GridCharacter>().Length; j++)
          _grids[i].GetComponentsInChildren<GridCharacter>()[j].CurrentGrid = _gridInfos[i];
    }
  }

  public void UpdateGridInfoStatus(GridInfo targetGrid, GridCharacter gridCharacter, bool isInGrid = true)
  {
    for (int i = 0; i < targetGrid.TotalCharacters.Count; i++)
      if (targetGrid.TotalCharacters[i].Count > 0)
        if (targetGrid.TotalCharacters[i][0].GetType() == gridCharacter.GetType())
        {
          if (!isInGrid)
          {
            targetGrid.TotalCharacters[i].Remove(gridCharacter);
            return;
          }

          if (!targetGrid.TotalCharacters[i].Contains(gridCharacter))
            targetGrid.TotalCharacters[i].Add(gridCharacter);

          return;
        }

    targetGrid.TotalCharacters.Add(new List<GridCharacter> { gridCharacter });
  }

  public void DeleteGrid(GridInfo gridToDelete)
  {
    _gridInfos.Remove(gridToDelete);
    Destroy(gridToDelete.TargetGrid);
  }
}
