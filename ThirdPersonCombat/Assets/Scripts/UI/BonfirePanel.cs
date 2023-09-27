using States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonfirePanel : MonoBehaviour
{
    [SerializeField] Button _firstSelectedButton;
    [SerializeField] TextMeshProUGUI _soulCounterOnPanel;
    [SerializeField] GameObject _soulCounterUI;
    [Header("TeleportBonfires")]
    [SerializeField] private Button[] _buttons;
    [Header("AttackPanel")]
    [SerializeField] GameObject _atkDmgUpgradeBarsPanel;
    [SerializeField] TextMeshProUGUI _atkUpgSoulInfoText;
    [Header("HealthPanel")]
    [SerializeField] GameObject _healthUpgradeBarsPanel;
    [SerializeField] TextMeshProUGUI _healthUpgSoulInfoText;
    [SerializeField] RectTransform _healthSlider;
    [Header("StaminaPanel")]
    [SerializeField] GameObject _staminaUpgradeBarsPanel;
    [SerializeField] TextMeshProUGUI _staminaUpgSoulInfoText;
    [SerializeField] RectTransform _staminaSlider;
    [Header("ArmorPanel")]
    [SerializeField] GameObject _armorUpgradeBarsPanel;
    [SerializeField] TextMeshProUGUI _armorUpgSoulInfoText;
    [Header("Config - Upgrades")]
    [SerializeField] private float _atkDmgPercentPerUpg = 10f;
    [SerializeField] private int _healthAddUpg = 40;
    [SerializeField] private int _staminaAddUpg = 30;
    [SerializeField] private int _armorPercentPerUpg = 60;
    [Header("Config - Soul Costs")]
    [SerializeField] private int[] _atkDmgSoulCosts = new int[5];
    [SerializeField] private int[] _healthSoulCosts = new int[5];
    [SerializeField] private int[] _staminaSoulCosts = new int[5];
    [SerializeField] private int[] _armorSoulsCosts = new int[5];

    private Image[] _atkDmgUpgradeBars;
    private Image[] _healthUpgradeBars;
    private Image[] _staminaUpgradeBars;
    private Image[] _armorUpgradeBars;


    private int _attackDamageUpgrade = 0;
    private int _healthUpgrade = 0;
    private int _staminaUpgrade = 0;
    private int _armorUpgrade = 0;
    
    private const int maxUpgrade = 5;
    private readonly Color _greenColor = new Color(0.54f, 0.91f, 0.61f);
    private void Awake()
    {
        _atkDmgUpgradeBars = _atkDmgUpgradeBarsPanel.GetComponentsInChildren<Image>();
        _healthUpgradeBars = _healthUpgradeBarsPanel.GetComponentsInChildren<Image>();
        _staminaUpgradeBars = _staminaUpgradeBarsPanel.GetComponentsInChildren<Image>();
        _armorUpgradeBars = _armorUpgradeBarsPanel.GetComponentsInChildren<Image>();
    }
    private void OnEnable()
    {
        _soulCounterUI.SetActive(false);
        _firstSelectedButton.Select();
        UpdatePanel();
        CheckKindledBonfires();
    }

    private void CheckKindledBonfires()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i > BonfiresManager.Instance.kindledBonfiresList.Count - 1)
                _buttons[i].gameObject.SetActive(false);
            else
                _buttons[i].gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        _soulCounterUI.SetActive(true);
    }
    private void HandleOnInvalid()
    {

    }
    
    private void UpdatePanel()
    {
        _soulCounterOnPanel.text = SoulsManager.Instance.CurrentSouls.ToString();
        ///////////////////////////
        if(_attackDamageUpgrade == maxUpgrade)
            _atkUpgSoulInfoText.text = "";
        else
            _atkUpgSoulInfoText.text = $"Required souls: {_atkDmgSoulCosts[_attackDamageUpgrade]}";
        ///////////////////////////
        if (_healthUpgrade == maxUpgrade)
            _healthUpgSoulInfoText.text = "";
        else
            _healthUpgSoulInfoText.text = $"Required souls: {_healthSoulCosts[_healthUpgrade]}";
        ///////////////////////////
        if (_staminaUpgrade == maxUpgrade)
            _staminaUpgSoulInfoText.text = "";
        else
            _staminaUpgSoulInfoText.text = $"Required souls: {_staminaSoulCosts[_staminaUpgrade]}";
    }
       
    //Button events
    public void IncreaseAttackDamage()
    {
        if (_attackDamageUpgrade == maxUpgrade) return;
        if (!SoulsManager.Instance.SpendSoul(_atkDmgSoulCosts[_attackDamageUpgrade]))
        {
            HandleOnInvalid();
            return;
        }
        _attackDamageUpgrade++;
        _atkDmgUpgradeBars[_attackDamageUpgrade - 1].color = _greenColor;
        PlayerStateMachine.Instance.combatController.attackDamageBoostPercent = _attackDamageUpgrade * _atkDmgPercentPerUpg;
        UpdatePanel();
    }
    public void IncreaseHealth()
    {
        if (_healthUpgrade == maxUpgrade) return;
        if (!SoulsManager.Instance.SpendSoul(_healthSoulCosts[_healthUpgrade]))
        {
            HandleOnInvalid();
            return;
        }
        _healthUpgrade++;
        _healthUpgradeBars[_healthUpgrade - 1].color = _greenColor;
        PlayerStateMachine.Instance.health.IncreaseMaxHealth(_healthAddUpg);
        _healthSlider.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400 + _healthUpgrade * _healthAddUpg); //400 is inital health slider witdh.
        UpdatePanel();
    }
    public void IncreaseStamina()
    {
        if (_staminaUpgrade == maxUpgrade) return;
        if (!SoulsManager.Instance.SpendSoul(_staminaSoulCosts[_staminaUpgrade]))
        {
            HandleOnInvalid();
            return;
        }
        _staminaUpgrade++;
        _staminaUpgradeBars[_staminaUpgrade - 1].color = _greenColor;
        PlayerStateMachine.Instance.stamina.IncreaseMaxStamina(_staminaAddUpg);
        _staminaSlider.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300 + _staminaUpgrade * _staminaAddUpg); //400 is inital health slider witdh.
        UpdatePanel();
    }
    public void TeleportToBonfire(int bonfireVal)
    {
        if (bonfireVal > BonfiresManager.Instance.kindledBonfiresList.Count - 1)
        {
            HandleOnInvalid();
            return;
        }
        BonfiresManager.Instance.ExitRest();
        Vector3 motion = BonfiresManager.Instance.kindledBonfiresList[bonfireVal].respawnPoint.position - PlayerStateMachine.Instance.transform.position;
        PlayerStateMachine.Instance.movement.CharacterController.Move(motion);
        PlayerStateMachine.Instance.ChangeState(PlayerStateMachine.Instance.PreviousState);
    }
    public void IncreaseArmor()
    {
        if (_armorUpgrade == maxUpgrade) return;
        if (!SoulsManager.Instance.SpendSoul(_armorSoulsCosts[_armorUpgrade]))
        {
            HandleOnInvalid();
            return;
        }
        _armorUpgrade++;
        _armorUpgradeBars[_armorUpgrade - 1].color = _greenColor;
        PlayerStateMachine.Instance.health.IncreaseArmor(_armorPercentPerUpg);
        UpdatePanel();
    }
}
