using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Tooltip("Image (Type=Filled) đại diện thanh máu")]
    public Image fillImage;
    private EnemyHealth eh;

    void Start()
    {
        eh = GetComponent<EnemyHealth>();
        if (eh == null || fillImage == null)
        {
            Debug.LogError("Thiếu EnemyHealth hoặc fillImage!");
            enabled = false;
            return;
        }

        eh.OnHealthChanged += UpdateBar;
        UpdateBar(eh.currentHealth, eh.maxHealth);
    }

    void UpdateBar(int curr, int max)
    {
        fillImage.fillAmount = (float)curr / max;
    }
}