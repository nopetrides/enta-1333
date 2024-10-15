using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class LocalPlayerUI : MonoBehaviour
    {
        [FormerlySerializedAs("_currentPowerAvailable")] [SerializeField] private TMP_Text CurrentPowerAvailable;

        public void Awake()
        {
            CurrentPowerAvailable.text = "Power: 0MW";
        }

        public void SubscribeToPlayerUpdates(Player localPlayer)
        {
            localPlayer.OnPowerChanged += UpdateUI;
        }

        private void UpdateUI(float currentPower)
        {
            CurrentPowerAvailable.text = $"Power: {currentPower}MW";
        }
    }
}