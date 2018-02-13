using System.Collections.Generic;
using UnityEngine;

namespace Andy.IdGenerator
{
    public class GetObjById : Singleton<GetObjById>
    {
        #region Private Fields

        private IDHolder[] _holders;
        private Dictionary<int, GameObject> _holdersCache = new Dictionary<int, GameObject>();

        #endregion

        #region Private Properties
        /// <summary>
        /// Cache holders in array
        /// </summary>
        private IDHolder[] Holders
        {
            get
            {
                if (_holders == null
                    || _holders.Length == 0)
                {
                    _holders = FindObjectsOfType<IDHolder>();
                }

                return _holders;
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Get Object by IDHolder.ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetObject(int id)
        {
            /*
            // if _holdersCache is empty
            if (_holdersCache == null
                || _holdersCache.Count == 0)
            {
                // cache all holders in dictionary
                foreach (var holder in Holders)
                {
                    if (holder != null)
                    {
                        _holdersCache.Add(holder.ID, holder.gameObject);
                    }
                }
            }
            // get gameobject by id
            if (_holdersCache.ContainsKey(id))
            {
                return _holdersCache[id];
            }
            else
            {
                return null;
            }
            */

            /* OLD VERSION */

            var holders = FindObjectsOfType<IDHolder>();

            foreach (var holder in holders)
            {
                if (holder.ID == id)
                {
                    return holder.gameObject;
                }
            }

            return null;
        }

        #endregion
    }
}
