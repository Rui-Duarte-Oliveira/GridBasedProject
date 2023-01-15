using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PlayerCore
{
  public class ComponentsManager : MonoBehaviour
  {
    /// <summary>
    /// If set true, will call OnActivation function.
    /// If set false, will call OnDeactivation function.
    /// </summary>
    public bool IsActive
    {
      get => _isActive;
      set
      {
        _isActive = value;

        if (_isActive)
          OnActivation();
        else
          OnDeactivation();
      }
    }

    public CharacterBase TargetCharacter { get => _character; }

    [SerializeField] private bool _isActive;
    [SerializeField] private CoreComponent _currentActiveComponent;

    protected List<SupplementaryComponent> _supplementaryComponents;
    protected List<CoreComponent> _objectCoreComponents;
    private CharacterBase _character;

    /// <summary>
    /// Is used by the State Change functions to determine if the current component manager can be activated.
    /// Returns true (1) by default if only base is called.
    /// Returns false (0).
    /// Returns priority true (2)
    /// </summary>
    /// <returns></returns>
    public virtual int CanBeSetActive() { return 1; }

    /// <summary>
    /// Is used by the State Change functions to determine if the current component manager can be deactivated.
    /// Returns true (1) by default if only base is called.
    /// Returns false (0).
    /// Returns priority true (2)
    /// </summary>
    /// <returns></returns>
    public virtual int CanBeSetInactive() { return 1; }

    /// <summary>
    /// Always call base!
    /// When a Core Component executes an action, this function will be called on Update.
    /// </summary>
    public virtual void OnActionPerform()
    {
      /*for (int i = 0; i < _objectCoreComponents.Count; i++)
        if (_objectCoreComponents[i].IsActive)
          _objectCoreComponents[i].OnActionPerform();*/

      if (!_currentActiveComponent.IsPerforming)
        return;

      _currentActiveComponent.OnActionPerform();

      for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnActionPerform();
    }

    /// <summary>
    /// Always call base!
    /// Is called on Update if the manager is active.
    /// Base function will run through all the designated core components and call their respective ActiveUpdate and InactiveUpdate functions.
    /// </summary>
    public virtual void OnActiveUpdate()
    {
      /*for (int i = 0; i < _objectCoreComponents.Count; i++)
        if (_objectCoreComponents[i].IsActive)
          _objectCoreComponents[i].OnActiveUpdate();*/

      _currentActiveComponent.OnActiveUpdate();

      for (int i = 0; i < _supplementaryComponents.Count; i++)
        _supplementaryComponents[i].OnActiveUpdate();
    }

    /// <summary>
    /// Always call base!
    /// Is called on FixedUpdate if the manager is active.
    /// Base function will run through all the designated core components and call their respective FixedUpdate functions. 
    /// </summary>
    public virtual void OnFixedUpdate()
    {
      /*for (int i = 0; i < _objectCoreComponents.Count; i++)
        if (_objectCoreComponents[i].IsActive)
          _objectCoreComponents[i].OnFixedUpdate();*/

      _currentActiveComponent.OnFixedUpdate();

      for (int i = 0; i < _supplementaryComponents.Count; i++)
        _supplementaryComponents[i].OnFixedUpdate();
    }

    /// <summary>
    /// Always call base!
    /// Is called when the manager is activated.
    /// Base function will run through all the designated core components and call their respective OnActivation functions.
    /// </summary>
    public virtual void OnActivation()
    {
      /*for (int i = 0; i < _objectCoreComponents.Count; i++)
        if (_objectCoreComponents[i].IsActive)
          _objectCoreComponents[i].OnActivation();*/

      _currentActiveComponent.OnActivation();

      for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnActivation();
    }

    /// <summary>
    /// Always call base!
    /// Is called when the manager is deactivated.
    /// Base function will run through all the designated core components and call their respective OnDeactivation functions.
    /// </summary>
    public virtual void OnDeactivation()
    {
      /*for (int i = 0; i < _objectCoreComponents.Count; i++)
        if (_objectCoreComponents[i].IsActive)
          _objectCoreComponents[i].IsActive = false;*/

      _currentActiveComponent.OnDeactivation();

      for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnDeactivation();
    }

    /// <summary>
    /// Always call base!
    /// Is called when target character is switched and if the manager is active.
    /// Base function will run through all the designated core components and call their respective OnTargetSwitch functions. 
    /// </summary>
    public virtual void OnTargetSwitch()
    {
      /*for (int i = 0; i < _objectCoreComponents.Count; i++)
        if (_objectCoreComponents[i].IsActive)
          _objectCoreComponents[i].OnTargetSwitch();*/

      _currentActiveComponent.OnTargetSwitch();

      for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnTargetSwitch();
    }

    /// <summary>
    /// Always call base!
    /// Is called on Start when the managers before the Initialize function.
    /// </summary>
    public virtual void Setup()
    {
      _objectCoreComponents = GetComponentsInChildren<CoreComponent>().ToList();
      _supplementaryComponents = GetComponents<SupplementaryComponent>().ToList();

      for (int i = 0; i < _objectCoreComponents.Count; i++)
        _objectCoreComponents[i].Setup();
    }

    /// <summary>
    /// Always call base!
    /// Base function will run through all the designated core components and call their respective SetTargetCharacter functions. 
    /// </summary>
    /// <param name="targetCharacter"></param>
    public virtual void Initialize(CharacterBase targetCharacter)
    {
      _character = targetCharacter;

      for (int i = 0; i < _objectCoreComponents.Count; i++)
        _objectCoreComponents[i].OnInitialization(targetCharacter);
    }

    public virtual void ChangeCurrentActiveCoreComponent(GameObject coreComponent)
    {
      _currentActiveComponent.IsActive = false;
      _currentActiveComponent = coreComponent.GetComponent<CoreComponent>();
      _currentActiveComponent.IsActive = true;
    }
  }
}
