using UnityEngine;

namespace PlayerCore
{
  public class ComponentBase : MonoBehaviour
  {
    /// <summary>
    /// Always call base if component is Core!
    /// Is called on Update when the designated Manager is Active and the Core Component Itself is Active.
    /// If component is Supplementary, this function will only be called if parent Core Component is Active.
    /// </summary>
    public virtual void OnActiveUpdate(){}

    /// <summary>
    /// Always call base if component is Core!
    /// Is called on Update when the designated Manager is Active and the Core Component Itself is Active.
    /// If component is Supplementary, this function will only be called if parent Core Component is Active.
    /// </summary>
    public virtual void OnFixedUpdate(){}

    /// <summary>
    /// Always call base if component is Core!
    /// Is called when an Action is executed if the designated Manager is Active and the Core Component Itself is Active.
    /// </summary>
    public virtual void ExecuteAction(){}

    /// <summary>
    /// Always call base if component is Core!
    /// Is called on Update when the designated Manager is Active and the parent Core Component is Performing an Action (IsPerforming).
    /// </summary>
    public virtual void OnActionPerform(){}

    /// <summary>
    /// Always call base if component is Core!
    /// Is called when an Action is done performing.
    /// </summary>
    public virtual void OnActionPerformed(){}

    /// <summary>
    /// Always call base if component is Core!
    /// Is called, if the designated Manager is Active, when the Core Component Itself is Activated.
    /// If component is Supplementary, this function will only be called when parent Core Component is Activated.
    /// </summary>
    public virtual void OnActivation(){}

    /// <summary>
    /// Always call base if component is Core!
    /// Is called, if the designated Manager is Active, when the Core Component Itself is Deactivated.
    /// If component is Supplementary, this function will only be called when parent Core Component is Deactivated. 
    /// </summary>
    public virtual void OnDeactivation(){}

    /// <summary>
    /// Always call base if component is Core!
    /// Is called, if the target has been switched, after the SetTargetCharacter function but before the OnActivation function. (Usefullness still unknown)
    /// </summary>
    public virtual void OnTargetSwitch(){}
  }
}
