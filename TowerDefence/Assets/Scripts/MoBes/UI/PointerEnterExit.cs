using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace nsPointerEnterExit
{
    public class PointerEnterExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event EventHandler OnPointerEnterCustom;
        public event EventHandler OnPointerExitCustom;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterCustom?.Invoke(this, EventArgs.Empty);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitCustom?.Invoke(this, EventArgs.Empty);
        }
    }
}
