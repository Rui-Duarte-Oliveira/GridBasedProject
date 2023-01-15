using System.Collections.Generic;
using UnityEngine;
using GridCore;

namespace PlayerCore
{
  public class Supp_RangeHighlight : SupplementaryComponent
  {
    [SerializeField] private GameObject _gridSquareObject;
    [SerializeField] private GameObject _objectPool;
    [SerializeField] private float _gridSquareHeightOffset = 0.2f;

    private Core_GridMovement _parentCoreComponent;

    private List<GameObject> _gridSquares = new List<GameObject>();
    private PathFinding _pathFinding = new PathFinding();

    public override void Setup(CoreComponent parentCoreComponent)
    {
      _parentCoreComponent = (Core_GridMovement)parentCoreComponent;
    }

    public override void OnActiveUpdate()
    {
      HighlightMovementRange(_pathFinding.GetReachableNodes(_parentCoreComponent.TargetGridCharacter, _parentCoreComponent.TargetGridCharacter.MovementRange),
        _parentCoreComponent.TargetGridCharacter);
    }

    public override void OnDeactivation() => RemoveHighlight();

    public override void OnTargetSwitch() => RemoveHighlight();

    public override void OnActionPerformed() => RemoveHighlight();


    /// <summary>
    /// Places objects (gridsquares) displaying the movement range of a certain target character.
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="targetCharacter"></param>
    private void HighlightMovementRange(List<Node> nodes, PlayerGridCharacter targetCharacter)
    {
      for (int i = 0; i < nodes.Count; i++)
        SetGridSquarePosition(nodes[i].PosX, nodes[i].PosY, GetGridSquare(i), targetCharacter);
    }

    /// <summary>
    /// Sets the object (gridsquare) designated position.
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="gridSquareObject"></param>
    /// <param name="targetCharacter"></param>
    private void SetGridSquarePosition(int posX, int posY, GameObject gridSquareObject, PlayerGridCharacter targetCharacter)
    {
      Vector3 position = targetCharacter.CurrentGrid.TargetGrid.GetWorldPosition(posX, posY);
      position += Vector3.up * _gridSquareHeightOffset;

      gridSquareObject.transform.position = position;
    }

    /// <summary>
    /// Returns and Instantiates an object (gridsquare) and sets the ObjectPool GameObject as its parent
    /// </summary>
    /// <returns></returns>
    private GameObject CreateGridSquareObject()
    {
      GameObject go = Instantiate(_gridSquareObject);
      _gridSquares.Add(go);
      go.transform.SetParent(_objectPool.transform);
      return go;
    }

    /// <summary>
    /// Returns an object (gridsquare) from the object List
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private GameObject GetGridSquare(int i)
    {
      if (_gridSquares.Count > i)
      {
        _gridSquares[i].SetActive(true);
        return _gridSquares[i];
      }

      return CreateGridSquareObject();
    }

    /// <summary>
    /// Deactivates all highlight objects
    /// </summary>
    private void RemoveHighlight()
    {
      if (_gridSquares.Count > 0)
        if (_gridSquares[0].activeInHierarchy)
          foreach (GameObject gridSquare in _gridSquares)
            gridSquare.SetActive(false);
    }
  }
}
