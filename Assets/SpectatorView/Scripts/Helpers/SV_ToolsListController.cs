using UnityEngine;

namespace SpectatorView
{
    public class SV_ToolsListController : MonoBehaviour
    {
        #region Public Fields

        public GameObject[] tabs;

        #endregion

        #region Public Methods

        public void EnableByName(GameObject obj)
        {
            EnableByName(obj.name);
        }

        public void EnableByName(string name)
        {
            var obj = GetTabByName(name);

            if (obj)
            {
                HideAllTabs();
                obj.SetActive(true);
            }
        }

        public void HideByName(GameObject obj)
        {
            HideByName(obj.name);
        }

        public void HideByName(string name)
        {
            var obj = GetTabByName(name);

            if (obj)
            {
                obj.SetActive(false);
            }
        }

        public void HideAllTabs()
        {
            foreach (var tab in tabs)
            {
                tab.SetActive(false);
            }
        }

        #endregion

        #region Utility Methods

        public GameObject GetTabByName(string name)
        {
            foreach (var tab in tabs)
            {
                if (tab.name == name)
                {
                    return tab;
                }
            }

            return null;
        }

        #endregion
    }
}
