using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace SpectatorView.Integration
{
    public class InputClicker : MonoBehaviour
    {
        public Component target;

        public void Click()
        {
            ((IInputClickHandler)target).OnInputClicked(null);
        }
    }
}
