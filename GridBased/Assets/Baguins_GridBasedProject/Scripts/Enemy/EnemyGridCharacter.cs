using UnityEngine;

namespace PlayerCore
{
  public class EnemyGridCharacter : GridCharacter
  {
    private void Start()
    {
      CurrentGrid.TargetGrid.SetGridCharacterPosition(this);
    }
  }
}
