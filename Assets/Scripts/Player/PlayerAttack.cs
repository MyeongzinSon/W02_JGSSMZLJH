using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private int m_dashPower;
    [SerializeField] private float m_maxStamina;
    [SerializeField] private float m_staminaPerDash;
    [SerializeField] private float m_staminaRegenPerSecond;
    [Header("Smash")]
    [SerializeField] private int m_smashPower;
    [SerializeField] private float m_smashCooldown;
    [Header("Layer")]
    [SerializeField] private LayerMask m_attackableLayer;

    //private UIManager uiManager;

    private float m_currentStamina;
    private float m_currentSmashCooldown;

    private bool m_isAttacking;

    public bool CanDash { get { return m_currentStamina >= m_staminaPerDash; } }
    public bool CanSmash { get { return m_currentSmashCooldown > 0; } }
    public float CurrentStamina { get { return m_currentStamina; } }

    private void Awake()
    {
        // uiManager = FindObjectOfType<UIManager>();
        m_currentStamina = m_maxStamina;
        m_currentSmashCooldown = -1;
    }
    private void Start()
    { 

    }
    private void Update()
    {
        if (m_currentSmashCooldown > 0)
        {
            m_currentSmashCooldown -= Time.deltaTime;
        }
        
        if (m_currentStamina < m_maxStamina)
        {
            m_currentStamina += m_staminaRegenPerSecond * Time.deltaTime;
            if (m_currentStamina > m_maxStamina)
            {
                m_currentStamina = m_maxStamina;
            }
        }
        // uiManager.UpdateUI(m_currentStamina, m_maxStamina, m_currentSmashCooldown, m_smashCooldown);
    }

    public void Dash(Vector2 start, Vector2 end)
    {
        m_currentStamina -= m_staminaPerDash;

        var attackables = Physics2D.LinecastAll(start, end, m_attackableLayer).Select(a => a.transform.GetComponent<IDamagable>()).ToList();
        //Debug.Log($"attackables.count = {attackables.Count}");
        foreach (var attackable in attackables)
        {
            attackable.TakeDamage(m_dashPower);
        }

        Debug.Log($"Dash! Stamina = {m_currentStamina}/{m_maxStamina}");
    }
    public void Attack(Vector2 start, Vector2 end)
    {
        var attackables = Physics2D.LinecastAll(start, end, m_attackableLayer).Select(a => a.transform.GetComponent<IDamagable>()).ToList();
        //Debug.Log($"attackables.count = {attackables.Count}");
        foreach (var attackable in attackables)
        {
            attackable.TakeDamage(m_dashPower);
        }
    }
}
