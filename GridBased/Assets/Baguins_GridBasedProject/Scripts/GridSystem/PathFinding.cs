using UnityEngine;
using System.Collections.Generic;
using PlayerCore;

namespace GridCore
{
  public class PathFinding
  {
    /// <summary>
    /// Returns a List of Nodes if there is a path from the Grid Character to the designated Grid end position.
    /// Returns null if no path is found.
    /// </summary>
    /// <param name="_targetController"></param>
    /// <param name="endX"></param>
    /// <param name="endY"></param>
    /// <returns></returns>
    public List<Node> FindPath(PlayerGridCharacter _targetController, int endX, int endY)
    {
      Vector2Int gridPosition = _targetController.CurrentGrid.TargetGrid.GetGridPosition(_targetController.transform.position);

      Node startNode = _targetController.CurrentGrid.TargetGrid.Grid[gridPosition.x, gridPosition.y];
      Node endNode = _targetController.CurrentGrid.TargetGrid.Grid[endX, endY];

      List<Node> openList = new List<Node>();
      List<Node> closedList = new List<Node>();

      openList.Add(startNode);

      //Starts cycle to eliminate all possible options
      while (openList.Count > 0)
      {
        Node currentNode = openList[0];

        foreach (Node pathNode in openList)
        {
          if (currentNode.FValue > pathNode.FValue)
            currentNode = pathNode;

          if (currentNode.FValue == pathNode.FValue && currentNode.HValue > pathNode.HValue)
            currentNode = pathNode;
        }

        openList.Remove(currentNode);
        closedList.Add(currentNode);

        if (currentNode == endNode)
          return RetracePath(startNode, endNode);


        List<Node> neighbourNodes = GetNeighbours(_targetController, currentNode);

        foreach (Node neighbour in neighbourNodes)
        {
          if (closedList.Contains(neighbour))
            continue;

          if (!_targetController.CurrentGrid.TargetGrid.Grid[neighbour.PosX, neighbour.PosY].IsWalkable)
            continue;

          if (_targetController.CurrentGrid.TargetGrid.Grid[neighbour.PosX, neighbour.PosY].GridCharacter != null)
            continue;

          float movementCost = currentNode.GValue + CalculateDistance(currentNode, neighbour);

          if (!openList.Contains(neighbour) || movementCost < neighbour.GValue)
          {
            neighbour.GValue = movementCost;
            neighbour.HValue = CalculateDistance(neighbour, endNode);
            neighbour.ParentNode = currentNode;

            if (!openList.Contains(neighbour))
              openList.Add(neighbour);
          }
        }
      }

      Debug.Log("No Path Found!");
      return null;
    }

    /// <summary>
    /// Returns a List of Nodes that are inside the designated range.
    /// Same Logic as the FindPath function
    /// </summary>
    /// <param name="_targetController"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<Node> GetReachableNodes(PlayerGridCharacter _targetController, float range)
    {
      Node startNode = _targetController.CurrentGrid.TargetGrid.Grid[_targetController.CurrentGridPosition.x, _targetController.CurrentGridPosition.y];

      List<Node> openList = new List<Node>();
      List<Node> closedList = new List<Node>();

      openList.Add(startNode);

      //Starts cycle to eliminate all possible options
      while (openList.Count > 0)
      {
        Node currentNode = openList[0];
        openList.Remove(currentNode);
        closedList.Add(currentNode);


        List<Node> neighbourNodes = GetNeighbours(_targetController, currentNode);

        foreach (Node neighbour in neighbourNodes)
        {
          if (closedList.Contains(neighbour))
            continue;

          if (!_targetController.CurrentGrid.TargetGrid.Grid[neighbour.PosX, neighbour.PosY].IsWalkable)
            continue;

          if (_targetController.CurrentGrid.TargetGrid.Grid[neighbour.PosX, neighbour.PosY].GridCharacter != null)
            continue;

          float movementCost = currentNode.GValue + CalculateDistance(currentNode, neighbour);

          if (movementCost > range)
            continue;

          if (!openList.Contains(neighbour) || movementCost < neighbour.GValue)
          {
            neighbour.GValue = movementCost;
            neighbour.ParentNode = currentNode;

            if (!openList.Contains(neighbour))
              openList.Add(neighbour);
          }
        }
      }

      ClearNodePathData(_targetController);
      return closedList;
    }

    /// <summary>
    /// Clears the Path data from the character Nodes
    /// </summary>
    /// <param name="_targetController"></param>
    public void ClearNodePathData(PlayerGridCharacter _targetController)
    {
      for (int x = 0; x < _targetController.CurrentGrid.TargetGrid.Width; x++)
        for (int y = 0; y < _targetController.CurrentGrid.TargetGrid.Length; y++)
          _targetController.CurrentGrid.TargetGrid.Grid[x, y].ClearPathData();
    }

    /// <summary>
    /// Returns distance between two Nodes
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public int CalculateDistance(Node currentNode, Node target)
    {
      int distX = Mathf.Abs(currentNode.PosX - target.PosX);
      int distY = Mathf.Abs(currentNode.PosY - target.PosY);

      if (distX > distY)
        return 14 * distY + 10 * (distX - distY);

      return 14 * distX + 10 * (distY - distX);
    }

    /// <summary>
    /// Returns a List of Nodes built by searching the neighbouring Nodes and filtering them through a set of conditions.
    /// </summary>
    /// <param name="_targetController"></param>
    /// <param name="currentNode"></param>
    /// <returns></returns>
    private List<Node> GetNeighbours(PlayerGridCharacter _targetController, Node currentNode)
    {
      List<Node> neighbourNodes = new List<Node>();

      for (int x = -1; x < 2; x++)
        for (int y = -1; y < 2; y++)
        {
          if (x == 0 && y == 0)
            continue;

          if (Mathf.Abs(x) + Mathf.Abs(y) == 2)
            continue;

          if (!_targetController.CurrentGrid.TargetGrid.IsInsideGridBoundry(currentNode.PosX + x, currentNode.PosY + y))
            continue;

          neighbourNodes.Add(_targetController.CurrentGrid.TargetGrid.Grid[currentNode.PosX + x, currentNode.PosY + y]);
        }

      return neighbourNodes;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
      List<Node> path = new List<Node>();

      Node currentNode = endNode;

      while (currentNode != startNode)
      {
        path.Add(currentNode);
        currentNode = currentNode.ParentNode;
      }

      path.Reverse();
      return path;
    }
  }
}
