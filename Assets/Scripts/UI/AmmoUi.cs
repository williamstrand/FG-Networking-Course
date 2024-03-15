using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AmmoUi : MonoBehaviour
    {
        [SerializeField] Text ammoText;
        [SerializeField] Ammo ammo;

        void OnEnable()
        {
            ammo.CurrentAmmo.OnValueChanged += UpdateUI;
        }

        void OnDisable()
        {
            ammo.CurrentAmmo.OnValueChanged -= UpdateUI;
        }

        void UpdateUI(int previousValue, int newValue)
        {
            ammoText.text = newValue.ToString();
        }
    }
}