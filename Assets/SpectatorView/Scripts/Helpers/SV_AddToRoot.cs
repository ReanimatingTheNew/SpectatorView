using UnityEngine;

namespace SpectatorView
{
    public class SV_AddToRoot : MonoBehaviour
    {
        public bool IsEnabled = true;
        public bool OnlyOnce = true;

        private void Awake()
        {
            MainAction();
        }

        private void Update()
        {
            if (!OnlyOnce
                && transform.parent != SV_Root.Instance.Anchor.transform)
            {
                MainAction();
            }
        }

        private void MainAction()
        {
            if (IsEnabled)
            {
                var sT = GetComponent<SV_TransformSync>();

                if (sT != null) { sT.IgnoreOnce = true; }

                SV_Root.Instance.AddToRoot(gameObject);
            }
        }
	}
}
