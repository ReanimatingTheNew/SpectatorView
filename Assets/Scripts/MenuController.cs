using SpectatorView.InputModule;
using UnityEngine;

namespace SpectatorView.Examples
{
    public class MenuController : MonoBehaviour
    {
        #region Public Fields

        public GameObject target;

        #endregion

        #region Private Fields

        private SV_HandDraggable handDraggable;
        private SV_HandRotatable handRotatable;
        private SV_HandScalable handScalable;

        #endregion

        #region Private Properties

        private SV_HandDraggable SV_HandDraggable
        {
            get
            {
                if (!handDraggable)
                {
                    handDraggable = target.GetComponent<SV_HandDraggable>();
                }

                return handDraggable;
            }
        }

        private SV_HandRotatable SV_HandRotatable
        {
            get
            {
                if (!handRotatable)
                {
                    handRotatable = target.GetComponent<SV_HandRotatable>();
                }

                return handRotatable;
            }
        }

        private SV_HandScalable SV_HandScalable
        {
            get
            {
                if (!handScalable)
                {
                    handScalable = target.GetComponent<SV_HandScalable>();
                }

                return handScalable;
            }
        }

        #endregion

        #region Public Events

        public void OnMove()
        {
            SV_HandDraggable.IsEnabled = true;
            SV_HandRotatable.IsEnabled = false;
            SV_HandScalable.IsEnabled = false;

            //Debug.Log("OnMove");
        }

        public void OnRotate()
        {
            SV_HandDraggable.IsEnabled = false;
            SV_HandRotatable.IsEnabled = true;
            SV_HandScalable.IsEnabled = false;

            //Debug.Log("OnRotate");
        }

        public void OnScale()
        {
            SV_HandDraggable.IsEnabled = false;
            SV_HandRotatable.IsEnabled = false;
            SV_HandScalable.IsEnabled = true;

            //Debug.Log("OnScale");
        }

        #endregion
    }
}
