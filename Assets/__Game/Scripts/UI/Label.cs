using TMPro;
using UnityEngine;

namespace GMTowerDefense.UI
{
    [RequireComponent(typeof (TextMeshProUGUI))]
    public class Label : MonoBehaviour
    {
        public void SetText(string text)
        {
            GetComponent<TextMeshProUGUI>().text = text;
        }
    }
}
