using PlayerCore;

namespace GridCore
{
  public class Node
  {
    public Node ParentNode { get => _parentNode; set => _parentNode = value; }

    /// <summary>
    /// Character currently assigned to this Node
    /// </summary>
    public GridCharacter GridCharacter { get => _gridCharacter; set => _gridCharacter = value; }
    public bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }
    public float Elevation { get => _elevation; set => _elevation = value; }
    public int PosX { get => _posX; set => _posX = value; }
    public int PosY { get => _posY; set => _posY = value; }
    public float GValue { get => _gValue; set => _gValue = value; }
    public float HValue { get => _hValue; set => _hValue = value; }

    /// <summary>
    /// Sum of the GValue and HValue
    /// </summary>
    public float FValue { get => _gValue + _hValue; set => FValue = value; }

    private Node _parentNode;
    private GridCharacter _gridCharacter;
    private bool _isWalkable = true;
    private float _elevation;
    private int _posX;
    private int _posY;
    private float _gValue;
    private float _hValue;


    public Node(int posX, int posY)
    {
      _posX = posX;
      _posY = posY;
    }


    /// <summary>
    /// Clears pathfinding related data from Node
    /// </summary>
    public void ClearPathData()
    {
      _gValue = 0;
      _hValue = 0;
      _parentNode = null;
    }
  }
}
