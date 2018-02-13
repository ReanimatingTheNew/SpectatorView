using HoloToolkit.Unity;
using UnityEngine;

namespace SpectatorView
{
    public class SV_ToolsController : Singleton<SV_ToolsController>
    {
        public bool IsOpen = true;

        public GameObject container;

        private void Start()
        {
            if (IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public void Open()
        {
            IsOpen = true;
            container.SetActive(true);
        }

        public void Close()
        {
            IsOpen = false;
            container.SetActive(false);

            SV_DragCube.Instance.Hide();
        }
    }
}