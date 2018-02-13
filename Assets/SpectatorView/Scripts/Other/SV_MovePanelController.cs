using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectatorView.Other
{
    public class SV_MovePanelController : SV_Singleton<SV_MovePanelController> {

        #region Public Fields

        public List<SV_MovePanel> panelsList = new List<SV_MovePanel>();

        #endregion

        #region Main Methods

        private void Start()
        {
            var arr = FindObjectsOfType<SV_MovePanel>();

            if (arr.Length > 0)
            {
                panelsList.AddRange(arr);
            }
        }

        #endregion

        #region Utility Methods

        public void TogglePanels()
        {
            foreach (var panel in panelsList)
            {
                panel.Toggle();
            }
        }

        #endregion
    }
}