using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PlayerCore
{
  public class CoreComponent : ComponentBase
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
        {
          OnActivation();
          return;
        }

        OnDeactivation();
      }
    }
    public bool IsPerforming
    {
      get => _isPerforming;
      set
      {
        _targetCharacter.IsPerformingAction = value;
        _isPerforming = value;
      }
    }

    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isPerforming;

    private List<SupplementaryComponent> _supplementaryComponents;
    private CharacterBase _targetCharacter;

    /// <summary>
    /// Always call base!
    /// Function is called OnStart and sets up dependancies
    /// </summary>
    public virtual void Setup()
    {
      _supplementaryComponents = GetComponents<SupplementaryComponent>().ToList();

      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].Setup(this);
    }

    /// <summary>
    /// Always call base!
    /// Function is called everytime the target character changes.
    /// </summary>
    public virtual void OnInitialization(CharacterBase targetCharacter)
    {
      _targetCharacter = targetCharacter;

      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnInitialization();
    }

    public override void OnFixedUpdate()
    {
      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnFixedUpdate();
    }

    public override void OnActiveUpdate()
    {
      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnActiveUpdate();
    }

    public override void OnActivation()
    {
      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnActivation();
    }

    public override void OnDeactivation()
    {
      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnDeactivation();
    }

    public override void ExecuteAction()
    {
      IsPerforming = true;

      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].ExecuteAction();
    }

    public override void OnActionPerform()
    {
      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnActionPerform();
    }

    public override void OnActionPerformed()
    {
      IsPerforming = false;

      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnActionPerformed();
    }

    public override void OnTargetSwitch()
    {
      if (_supplementaryComponents != null)
        for (int i = 0; i < _supplementaryComponents.Count; i++)
          _supplementaryComponents[i].OnTargetSwitch();

      IsPerforming = false;
    }

    #region Context Menus
    [ContextMenu("Remove All Supplementary Components")]
    private void RemoveSupplementaryComponents()
    {
      if (_supplementaryComponents != null)
        foreach (SupplementaryComponent component in _supplementaryComponents)
          DestroyImmediate(component);

      _supplementaryComponents.Clear();
    }

    [ContextMenu("Remove Core Component")]
    private void RemoveCoreComponentWithSupplementarys()
    {
      if (_supplementaryComponents != null)
        foreach (SupplementaryComponent component in _supplementaryComponents)
          DestroyImmediate(component);

      DestroyImmediate(this);
    }
    #endregion
  }
}
