using System.Collections.Generic;
using UnityEngine;
using PlayerCore;

namespace GridCore
{
  public class GridBehaviour : MonoBehaviour
  {
    [Header("GridBehaviour Settings")]
    [SerializeField] private bool _isDisplayingGizmos = true;
    [SerializeField] private bool _isEditorRunTimeEditing = true;
    [SerializeField] private int _width = 25;
    [SerializeField] private int _length = 25;
    [SerializeField] private float _cutOffAngle = 65f;
    [SerializeField] private float _cellSize;
    [SerializeField, Tooltip("Obstacle Detection Layer")] private LayerMask _obstacleMask;
    [SerializeField, Tooltip("Terrain Detection Layer")] private LayerMask _terrainMask;


    public Node[,] Grid { get => _grid; set => _grid = value; }
    public int Width { get => _width; set => _width = value; }
    public int Length { get => _length; set => _length = value; }

    private Node[,] _grid;

    private void Awake()
    {
      _isEditorRunTimeEditing = false;

      GenerateGrid();
    }


    /// <summary>
    /// Gets the current Grid Character grid position, assigns it, assigns the character to the designated Node 
    /// and places the Grid Character at the world position of the selected node. 
    /// </summary>
    /// <param name="gridCharacter"></param>
    public void SetGridCharacterPosition(GridCharacter gridCharacter)
    {
      gridCharacter.CurrentGridPosition = GetGridPosition(gridCharacter.transform.position);
      PlaceGridCharacter(gridCharacter, gridCharacter.CurrentGridPosition);

      gridCharacter.transform.position = GetWorldPosition(gridCharacter.CurrentGridPosition.x, gridCharacter.CurrentGridPosition.y);
    }


    /// <summary>
    /// Assigns to the designated Node a character.
    /// </summary>
    /// <param name="gridCharacter"></param>
    /// <param name="currentGridPosition"></param>
    public void PlaceGridCharacter(GridCharacter gridCharacter, Vector2Int currentGridPosition)
    {
      if (IsInsideGridBoundry(currentGridPosition.x, currentGridPosition.y))
        Grid[currentGridPosition.x, currentGridPosition.y].GridCharacter = gridCharacter;
      else
        Debug.Log("Object is Outside of the Grid!");
    }


    /// <summary>
    /// Takes a Node position and verifies if there is a character already designated for it. 
    /// If there is a character then it will return it.
    /// If there isnt a character then it will return null.
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public GridCharacter GetPlacedCharacter(Vector2Int gridPosition)
    {
      if (IsInsideGridBoundry(gridPosition.x, gridPosition.y))
        return Grid[gridPosition.x, gridPosition.y].GridCharacter;

      return null;
    }


    /// <summary>
    /// Returns a List of World Positions from a List of Grid Nodes
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public List<Vector3> GetNodesWorldPositions(List<Node> path)
    {
      List<Vector3> worldPositions = new List<Vector3>();

      foreach (Node pathNode in path)
        worldPositions.Add(GetWorldPosition(pathNode.PosX, pathNode.PosY));

      return worldPositions;
    }

    /// <summary>
    /// Gets the relative Grid Position from a World Position.
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
      worldPosition -= transform.position;
      return new Vector2Int(Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize));
    }

    /// <summary>
    /// Gets Node world position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(int x, int y)
    {
      return new Vector3(transform.position.x + (x * _cellSize), Grid[x, y].Elevation, transform.position.z + (y * _cellSize));
    }

    /// <summary>
    /// Compares the values with the width and length of the Grid.
    /// Returns true if the values are between 0 and the designated length.
    /// For the parameter values, use "GetGridPosition" for an accurate return result.
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public bool IsInsideGridBoundry(int posX, int posY)
    {
      if (posX < 0 || posX >= _width)
        return false;

      if (posY < 0 || posY >= _length)
        return false;

      return true;
    }

    /// <summary>
    /// Instantiates a grid with the designated width and length.
    /// As each Grid Node is instantiated, it will also calculate its elevation, and check if its walkable.
    /// </summary>
    private void GenerateGrid()
    {
      Grid = new Node[_width, _length];

      for (int x = 0; x < _width; x++)
        for (int y = 0; y < _length; y++)
        {
          Grid[x, y] = new Node(x, y);

          CalculateElevation(x, y);
          CheckWalkableTerrain(x, y);
        }
    }

    /// <summary>
    /// Uses Physics CheckBox to define an area the same size as the designated Grid Node to check for obstacles.
    /// If an obstacle is detected then the GridNode will be marked as unwalkable.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CheckWalkableTerrain(int x, int y)
    {
      if (!Grid[x, y].IsWalkable)
        return;

      Grid[x, y].IsWalkable = !Physics.CheckBox(GetWorldPosition(x, y), new Vector3(0.95f, 0.95f, 0.95f) / 2 * _cellSize, Quaternion.identity, _obstacleMask);
    }

    /// <summary>
    /// Calculates Grid Node(x, y) elevation through RayCast.
    /// If inclination is superior to the designated cut off angle, the node will be marked as UnWalkable.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CalculateElevation(int x, int y)
    {
      Ray ray = new Ray(GetWorldPosition(x, y) + Vector3.up * 100f, Vector3.down);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit, float.MaxValue, _terrainMask))
      {
        Grid[x, y].Elevation = hit.point.y;

        if (Vector3.Angle(hit.normal, Vector3.up) > _cutOffAngle)
          Grid[x, y].IsWalkable = false;
      }
    }


    private void OnDrawGizmos()
    {
      if (_isDisplayingGizmos)
        GenerateGUIGrid();
    }

    /// <summary>
    /// OnDrawGizmos function to display the grid during Editing
    /// </summary>
    private void GenerateGUIGrid()
    {
      if (_isEditorRunTimeEditing)
        Grid = new Node[_width, _length];

      for (int x = 0; x < _width; x++)
        for (int y = 0; y < _length; y++)
        {
          if (_isEditorRunTimeEditing)
          {
            Grid[x, y] = new Node(x, y);
            CalculateElevation(x, y);
            CheckWalkableTerrain(x, y);
          }

          Gizmos.color = Grid[x, y].IsWalkable ? Color.green : Color.red;
          Gizmos.DrawCube(GetWorldPosition(x, y), Vector3.one / 5);
        }
    }
  }
}
