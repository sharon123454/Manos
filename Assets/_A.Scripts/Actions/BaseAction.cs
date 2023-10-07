using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

// Costs of an Action
public enum TypeOfAction { Action, BonusAction, Both }
// Types of Range Actions have
public enum ActionRange
{
    Move/*Set per Unit*/,
    Self/* 0 */,
    Melee/* 0 - 1 */,
    Close/* 0 - 4, 5-9 */,
    Medium/* 2-4, 5 - 9, 10-15 */,
    Long/* 5 - 15 */,
    EffectiveAtAll/* 0 - 15 */,
    InaccurateAtAll/* 0-15 */,
    ResetGrid/*None*/
}
// Ability Properties Actions have
public enum AbilityProperties
{
    Basic,//Ignores Silance Effect
    Finisher,//If the Target has less then 50% before the attack, deal double damage
    Heal,//Give a friendly unit health points
    IgnoreArmor,//The attack damage is done directly to a units health
    CDR,//Reduces your ability's current cooldowns
    AreaOfEffect,// Affects all units within a certain radius of the target point, dealing damage or applying other effects to each unit (physics based)
    Teleport,//The unit instantaneously transport itself to a designated location x tiles away, bypassing obstacles and enemy units that may be in the way
}
public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler<BaseAction> OnAnyActionStarted;
    public static event EventHandler<BaseAction> OnAnyActionCompleted;

    [Header("Base Action")]
    [SerializeField] private string _actionName = "Empty";
    [SerializeField] private string actionDescription = "Description...";
    [SerializeField] private TypeOfAction actionCost;
    [SerializeField] private int cooldownAfterUse = 1;
    [SerializeField] private int favorCost = 0;
    [SerializeField] private Sprite abilityImage;
    [SerializeField] private Sprite abilityGrayImage;
    [SerializeField] private List<AbilityProperties> _AbilityProperties = new List<AbilityProperties>();
    [SerializeField] private ActionRange range;
    [Header("AoE")]
    [Tooltip("Leave Empty if AOE is single activation")]
    [SerializeField] protected AOEActive AOEPrefab;
    [SerializeField] protected int AOEActiveTurns = 1;
    [SerializeField] private bool isFollowingMouse;
    [SerializeField] protected bool isFollowingUnit = true;
    [SerializeField] private MeshShape actionMeshShape;
    [SerializeField] private float meshShapeScaleMultiplicator = 1;

    protected Effectiveness enemyEffectivess;
    protected Action onActionComplete;
    protected bool _isActive;
    protected bool _usedAction;

    private Unit unit;
    private int cooldown;
    private BaseAbility baseAbility;

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

    public Unit GetUnit() { return unit; }
    public ActionRange GetRange() { return range; }
    public string GetActionName() { return _actionName; }
    public Sprite GetAbilityImage() { return abilityImage; }
    public Sprite GetAbilityGrayImage() { return abilityGrayImage; }
    public MeshShape GetActionMeshShape() { return actionMeshShape; }
    public float GetMeshScaleMultiplicator() { return meshShapeScaleMultiplicator; }
    public bool IsXPropertyInAction(AbilityProperties X) { return _AbilityProperties.Contains(X); }
    public List<AbilityProperties> GetAbilityProperties() { return _AbilityProperties; }
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
        GetUnit().GetUnitAnimator().OnActionStarted(GetActionName());//activates action animation
        this.onActionComplete = onActionComple;//setting the end of action Action
        cooldown += cooldownAfterUse;
        _isActive = true;//allowing action update to run (check specific actions/BaseAbility for implementation)
        OnAnyActionStarted?.Invoke(this, this);//updates ActionUI
    }
    protected void ActionComplete()
    {
        _isActive = false;//closing update proccess
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, this);//updates AOE and UnitActionSystem
    }

    public abstract void TakeAction(GridPosition gridPosition, Action actionComplete);
    public abstract List<GridPosition> GetValidActionGridPositionList();
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

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