using UnityEngine;
using UnityEngine.UI;

namespace SpectatorView.Integration
{
    public class ButtonClicker : MonoBehaviour
    {
        // allow you click on Button component from script
        public void Click()
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
}
