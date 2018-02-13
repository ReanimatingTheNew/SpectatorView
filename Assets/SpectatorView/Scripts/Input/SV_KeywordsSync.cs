using Andy.IdGenerator;
using HoloToolkit.Unity.InputModule;
using SpectatorView.Sharing;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpectatorView
{
    public class SV_KeywordsSync : MonoBehaviour
    {
        #region Public Fields

        #endregion

        #region Private Fields

        private int _objectId;

        private string _keyword;

        private KeywordManager _manager;

        private KeywordReferenses _references = new KeywordReferenses();

        #endregion

        private void Start()
        {
            var _idHolder = GetComponent<IDHolder>();

            if (_idHolder)
            {
                _objectId = _idHolder.ID; // присваиваем id объекта
            }

            _manager = GetComponent<KeywordManager>(); 

            if (_manager != null)
            {
                // индексриуем все ключевые слова
                foreach (var item in _manager.KeywordsAndResponses)
                {
                    _references.References.Add(new KeywordReference(
                        new Keyword(_objectId, item.Keyword),
                        item.Response));
                }
            }
        }

        public void ShareKeyword(string keyword)
        {
            SV_Sharing.Instance.SendJson(keyword, "recognize_keyword");
            Debug.Log("Share keyword " + keyword);
        }

        public void InvokeKeywordResponse(string keyword)
        {
            if (_manager)
            {
                var reference = _references.GetByKeyword(keyword);

                if (reference != null)
                {
                    reference.Response.Invoke();
                }
            }
        }

        #region Nested Classes

        public class KeywordReference
        {
            public Keyword Keyword;
            public UnityEvent Response;

            public KeywordReference(Keyword keyword, UnityEvent response)
            {
                Keyword = keyword;
                Response = response;
            }
        }

        public class KeywordReferenses
        {
            private List<KeywordReference> _references = new List<KeywordReference>();

            public List<KeywordReference> References
            {
                get { return _references; }
            }

            public KeywordReference GetByKeyword(string keyword)
            {
                foreach (var _reference in _references)
                {
                    if (_reference.Keyword.keyword == keyword)
                    {
                        return _reference;
                    }
                }

                return null;
            }
        }

        [Serializable]
        public class Keyword
        {
            public int id;
            public string keyword;

            public Keyword(int id, string keyword)
            {
                this.id = id;
                this.keyword = keyword;
            }
        }

        #endregion
    }
}
