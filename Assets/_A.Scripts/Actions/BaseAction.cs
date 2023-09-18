using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

// Costs of an Action
public enum TypeOfAction { Action, BonusAction, Both }
public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler<int> OnAnySpellCast;
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    [Header("Base Action")]
    [SerializeField] protected string _actionName = "Empty";
    [SerializeField] protected string actionDescription = "Description...";
    [SerializeField] protected TypeOfAction actionCost;
    [SerializeField] protected int cooldownAfterUse = 1;
    [SerializeField] protected int favorCost = 0;
    [SerializeField] protected List<AbilityProperties> _AbilityProperties = new() { AbilityProperties.Basic };
    [SerializeField] private ActionRange range;
    [SerializeField] protected bool canOnlyHitEnemy;
    [Header("AoE")]
    [Tooltip("Leave Empty if AOE is single activation")]
    [SerializeField] protected AOEActive AOEPrefab;
    [SerializeField] protected float AOEActiveTurns = 1;
    [SerializeField] protected bool isFollowingMouse;
    [SerializeField] protected bool isFollowingUnit = true;
    [SerializeField] protected MeshShape actionMeshShape;
    [SerializeField] protected float meshShapeScaleMultiplicator = 1;
    [SerializeField] protected Sprite abilityImage;
    [SerializeField] protected Sprite abilityGrayImage;

    protected Action onActionComplete;
    protected bool _isActive;
    protected bool _usedAction;

    private Unit unit;
    private int cooldown;

    BaseAbility baseAbility;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();

        if (this is BaseAbility)
        {
            print("This is base ability?");
            baseAbility = GetComponent<BaseAbility>();
        }
    }

    protected virtual void Start()
    {
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
    }

    public bool IsBasicAbility() { return _AbilityProperties.Contains(AbilityProperties.Basic); }
    public Unit GetUnit() { return unit; }
    public ActionRange GetRange() { return range; }
    public string GetActionName() { return _actionName; }
    public Sprite GetAbilityImage() { return abilityImage; }
    public Sprite GetAbilityGrayImage() { return abilityGrayImage; }
    public MeshShape GetActionMeshShape() { return actionMeshShape; }
    public float GetMeshScaleMultiplicator() { return meshShapeScaleMultiplicator; }
    public List<AbilityProperties> GetAbilityPropertie() { return _AbilityProperties; }
    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> _enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> _validGridPositionList = GetValidActionGridPositionList();
        if (_validGridPositionList == null)
        {
            return null; // No Possible AI Actions
        }

        if (_validGridPositionList.Count > 0)
        {
            foreach (GridPosition gridPosition in _validGridPositionList)
            {
                EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
                _enemyAIActionList.Add(enemyAIAction);
            }
        }

        if (_enemyAIActionList.Count > 0)
        {
            _enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return _enemyAIActionList[0];
        }
        else
            return null; // No Possible AI Actions
    }

    public bool GetIsFollowingMouse() { return isFollowingMouse; }

    public virtual int GetCurrentCooldown() { return cooldown; }
    public virtual int GetAbilityCooldown() { return cooldownAfterUse; }

    public virtual int GetFavorCost() { return favorCost; }
    public virtual string GetActionDescription() { return actionDescription; }
    public virtual bool GetIfUsedAction() { return _usedAction; }
    /// <summary>
    /// As int: 0 = Action,1 = BonusAction,2 = Both
    /// </summary>
    /// <returns>actionCost</returns>
    public TypeOfAction GetActionCost() { return actionCost; }
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    protected void ActionStart(Action onActionComple)
    {
        GetUnit().GetUnitAnimator().OnActionStarted(GetActionName());
        cooldown += cooldownAfterUse;
        if (baseAbility)
            OnAnySpellCast?.Invoke(this, GetFavorCost());
        _isActive = true;
        this.onActionComplete = onActionComple;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }
    protected void ActionComplete()
    {
        _isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        UnitActionSystem.Instance.InvokeAbilityFinished();
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    public abstract void TakeAction(GridPosition gridPosition, Action actionComplete);

    private void Instance_OnTurnChange(object sender, EventArgs e)
    {
        if (unit.IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
        {
            if (cooldown > 0)
                cooldown--;
        }

        if (!unit.IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            if (cooldown > 0)
                cooldown--;
        }
    }

}