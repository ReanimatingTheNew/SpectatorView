using UnityEngine;
using Andy.IdGenerator;
using SpectatorView.InputModule;
using UnityEngine.UI;
using System;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;

namespace SpectatorView.Integration
{
    // Скрипт помогает быстро интегрировать Spectator View в проекты на HoloToolkit,
    // Логика следующая: собираются все объекты в корне, проверяется наличие на них компонентов Button и скриптов
    // реализующих интерфейс IInputClickHandler, далее на них вешаются скрипты IDHandler и SV_Button которые отслеживают нажатия и передают
    // уникальный ID кнопки через SV_Sharing
    public class SV_Integration : SV_Singleton<SV_Integration>
    {
        #region Public Fields

        [Header("AddToRoot Options")]
        public bool EnableAddToRoot = true;

        public List<GameObject> AddToRoot = new List<GameObject>(); // объекты которые мы хотим добавить в корень

        public bool OnlyOnce = true; // значение для поля SV_AddToRoot, проверка наличия объекта в корне только один раз

        [Header("Integration Options")]
        public bool DetectButtons = true; // отслеживание Button
        public bool DetectInputClick = true; // отслеживание IInputClickHandler
        public bool DetectFocusable = true; // отслеживание IFocusable
        public bool DetectInput = true; // отслеживание IInputHandler
        public bool DetectHold = true; // отслеживание IInputHandler
        public bool DetectManipulation = true; // отслеживание IInputHandler
        public bool DetectNavigation = true; // отслеживание IInputHandler
        public bool DetectColliders = false; // отслеживание Collider

        [Header("Deintegration Options")]
        public bool RemoveAddToRoot = false; // удалять скрипты AddToRoot

        #endregion

#if UNITY_EDITOR

        #region Public Methods

        public GameObject[] GetRootGameObjects()
        {
            Transform root = FindObjectOfType<SV_Root>().transform; // получаем корень

            List<GameObject> rootGameObjects = new List<GameObject>(); // объявляем лист

            foreach (Transform child in root.GetComponentsInChildren<Transform>(true)) // получаем все объекты в корне (на любой глубине иерархии)
            {
                rootGameObjects.Add(child.gameObject);
            }

            return rootGameObjects.ToArray();
        }

        #endregion

        #region Main Methods

        public void Integrate()
        {
            int buttonCount = 0;
            int inputClickHandlerCount = 0;
            int focusableCount = 0;
            int inputCount = 0;
            int holdCount = 0;
            int manipulationCount = 0;
            int navigationCount = 0;
            int collidersCount = 0;

            // получаем все объекты содержащие скрипт SV_AddToRoot и добавляем их в коллекцию AddToRoot
            var appendToRoot = FindObjectsOfType<SV_AddToRoot>();

            for (var i = 0; i < appendToRoot.Length; i++)
            {
                if (!AddToRoot.Contains(appendToRoot[i].gameObject))
                {
                    AddToRoot.Add(appendToRoot[i].gameObject);
                }
            }

            if (EnableAddToRoot
                && AddToRoot.Count > 0)
            {
                foreach (var model in AddToRoot)
                {
                    model.transform.parent = FindObjectOfType<SV_Root>().transform; // добавляем все модели из коллекции в корень

                    if (model.GetComponent<SV_AddToRoot>() == null)
                    {
                        var addToRoot = model.AddComponent<SV_AddToRoot>();
                        addToRoot.OnlyOnce = OnlyOnce;

                        Debug.Log("Add SV_AddToRoot on " + GetGameObjectPath(model));
                    }
                    else
                    {
                        Debug.Log("SV_AddToRoot already exist on " + GetGameObjectPath(model));
                    }
                }
            }

            Debug.Log("Spectator View: AUTO INTEGRATION START");

            // цикл через все объекты в корне
            foreach (var obj in GetRootGameObjects())
            {
                // Type: Button
                // если объект содержит Button, на него добавляется IDHolder и SV_Button для синхронизации
                if (DetectButtons)
                {
                    var _btn = obj.GetComponent<Button>();

                    // если объект содержит Button
                    if (_btn != null)
                    {
                        CheckComponent(obj, typeof(IDHolder)); // проверяет наличие IDHolder, или добавляет его
                        CheckComponent(obj, typeof(SV_Button)); // проверяет наличие SV_Button, или добавляет его

                        buttonCount++;
                    }
                }

                // Type: IInputClickHandler
                // если объект содержит IInputClickHandler, на него добавляется IDHolder и SV_Button для синхронизации
                if (DetectInputClick)
                {
                    var _inputClickHandler = obj.GetComponent<IInputClickHandler>();

                    if (_inputClickHandler != null
                        && !(_inputClickHandler is SV_Button))
                    {
                        CheckComponent(obj, typeof(IDHolder)); // проверяет наличие IDHolder, или добавляет его
                        CheckComponent(obj, typeof(SV_Button)); // проверяет наличие SV_Button, или добавляет его

                        inputClickHandlerCount++;
                    }
                }

                // Type: IFocusable
                // если объект содержит IFocusable, на него добавляется IDHolder и SV_Hover для синхронизации
                if (DetectFocusable)
                {
                    var _focusable = obj.GetComponent<IFocusable>();
                    var _inputHandler = obj.GetComponent<IInputHandler>();

                    if (_focusable != null
                        && !(_focusable is SV_Hover)
                        && _inputHandler == null) // IInputHandler == null (не драг)
                    {
                        CheckComponent(obj, typeof(IDHolder));
                        CheckComponent(obj, typeof(SV_Hover)); // проверяет наличие SV_Hover, или добавляет его

                        focusableCount++;
                    }
                }

                // Type: IInputHandler
                // если объект содержит IInputHandler, на него добавляется IDHolder и SV_SyncTransform для синхронизации
                if (DetectInput)
                {
                    var _input = obj.GetComponent<IInputHandler>();

                    if (_input != null)
                    {
                        CheckComponent(obj, typeof(IDHolder)); // проверяет наличие IDHolder, или добавляет его
                        CheckComponent(obj, typeof(SV_TransformSync)); // проверяет наличие SV_SyncTransform, или добавляет его

                        inputCount++;
                    }
                }

                // Type: IHoldHandler
                // если объект содержит IHoldHandler, на него добавляется IDHolder и SV_SyncTransform для синхронизации
                if (DetectHold)
                {
                    var _hold = obj.GetComponent<IHoldHandler>();

                    if (_hold != null)
                    {
                        CheckComponent(obj, typeof(IDHolder)); // проверяет наличие IDHolder, или добавляет его
                        CheckComponent(obj, typeof(SV_TransformSync)); // проверяет наличие SV_SyncTransform, или добавляет его

                        holdCount++;
                    }
                }

                // Type: IManipulationHandler
                // если объект содержит IHoldHandler, на него добавляется IDHolder и SV_SyncTransform для синхронизации
                if (DetectManipulation)
                {
                    var _manipulation = obj.GetComponent<IManipulationHandler>();

                    if (_manipulation != null)
                    {
                        CheckComponent(obj, typeof(IDHolder)); // проверяет наличие IDHolder, или добавляет его
                        CheckComponent(obj, typeof(SV_TransformSync)); // проверяет наличие SV_SyncTransform, или добавляет его

                        manipulationCount++;
                    }
                }

                // Type: INavigationHandler
                // если объект содержит IHoldHandler, на него добавляется IDHolder и SV_SyncTransform для синхронизации
                if (DetectNavigation)
                {
                    var _navigation = obj.GetComponent<INavigationHandler>();

                    if (_navigation != null)
                    {
                        CheckComponent(obj, typeof(IDHolder)); // проверяет наличие IDHolder, или добавляет его
                        CheckComponent(obj, typeof(SV_TransformSync)); // проверяет наличие SV_SyncTransform, или добавляет его

                        navigationCount++;
                    }
                }

                // Type: Collider
                // если объект содержит Collider, на него добавляется IDHolder и SV_SyncTransform для синхронизации 
                // (все объекты содержащие коллайдеры обычно перемещаемые). 
                // Эту опцию следует использовать с осторожностью если в проекте много коллайдеров.
                if (DetectColliders)
                {
                    var _collider = obj.GetComponent<Collider>();
                    var isChildOfMovable = obj.GetComponentInParent<SV_TransformSync>(); // проверка имеет ли родитель объекта синхронизацию Transform

                    if (_collider != null
                        && isChildOfMovable == null)
                    {
                        CheckComponent(obj, typeof(IDHolder)); // проверяет наличие IDHolder, или добавляет его
                        CheckComponent(obj, typeof(SV_TransformSync)); // проверяет наличие SV_SyncTransform, или добавляет его

                        foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
                        {
                            RemoveComponent(child.gameObject, typeof(SV_TransformSync));
                        }

                        collidersCount++;
                    }
                }
            }

            // логирует значения счётчиков
            if (DetectButtons && buttonCount > 0) { Debug.Log("Found: " + buttonCount + " Button"); }
            if (DetectInputClick && inputClickHandlerCount > 0) { Debug.Log("Found: " + inputClickHandlerCount + " InputClickHandler"); }
            if (DetectFocusable && focusableCount > 0) { Debug.Log("Found: " + focusableCount + " IFocusable"); }
            if (DetectInput && inputCount > 0) { Debug.Log("Found: " + inputCount + " IInputHandler"); }
            if (DetectHold && holdCount > 0) { Debug.Log("Found: " + holdCount + " IHoldHandler"); }
            if (DetectManipulation && manipulationCount > 0) { Debug.Log("Found: " + manipulationCount + " IManipulationHandler"); }
            if (DetectNavigation && navigationCount > 0) { Debug.Log("Found: " + navigationCount + " INavigationHandler"); }
            if (DetectColliders && collidersCount > 0) { Debug.Log("Found: " + collidersCount + " Colliders"); }

            IdGenerator.Instance.Generate(); // генерирует уникальные ID для IDHolder

            Debug.Log("Spectator View: AUTO INTEGRATION END");
        }

        public void Deintegration()
        {
            Debug.Log("Spectator View: AUTO DEINTEGRATION START");

            int componentsCount = 0;
            int IDHolderCount = 0;
            int TransformSyncCount = 0;
            int SVButtonCount = 0;
            int SVHoverCount = 0;
            int AddToRootCount = 0;

            // loop thru all gameobjects in root
            foreach (var obj in GetRootGameObjects())
            {
                bool removeIDHolder = RemoveComponent(obj, typeof(IDHolder)); // удалить компонент IDHolder

                if (removeIDHolder)
                {
                    IDHolderCount++;
                    componentsCount++;
                }

                bool removeTransformSync = RemoveComponent(obj, typeof(SV_TransformSync)); // удалить компонент SV_TransformSync

                if (removeTransformSync)
                {
                    TransformSyncCount++;
                    componentsCount++;
                }

                var svBtn = obj.GetComponent<SV_Button>();
                if (svBtn
                    && !svBtn.UseAsButton) // UseAsButton подразумевает пользовательскую настройку, такие компоненты удалять не нужно
                {
                    bool removeSVButton = RemoveComponent(obj, typeof(SV_Button));

                    if (removeSVButton)
                    {
                        SVButtonCount++;
                        componentsCount++;
                    }
                }

                var svHover = obj.GetComponent<SV_Hover>();
                if (svHover
                    && !svHover.UseAsHover) // UseAsHover подразумевает пользовательскую настройку, такие компоненты удалять не нужно
                {
                    bool RemoveSVHover = RemoveComponent(obj, typeof(SV_Hover));

                    if (RemoveSVHover)
                    {
                        SVHoverCount++;
                        componentsCount++;
                    }
                }

                if (RemoveAddToRoot)
                {
                    bool RemoveAddToRoot = RemoveComponent(obj, typeof(SV_AddToRoot));

                    if (RemoveAddToRoot)
                    {
                        AddToRootCount++;
                        componentsCount++;
                    }
                }
            }

            if (IDHolderCount > 0) { Debug.Log("Delete: " + IDHolderCount + " IDHolder"); }
            if (TransformSyncCount > 0) { Debug.Log("Delete: " + TransformSyncCount + " SV_TransformSync"); }
            if (SVButtonCount > 0) { Debug.Log("Delete: " + SVButtonCount + " SV_Button"); }
            if (SVHoverCount > 0) { Debug.Log("Delete: " + SVHoverCount + " SV_Hover"); }
            if (AddToRootCount > 0) { Debug.Log("Delete: " + AddToRootCount + " SV_AddToRoot"); }
            if (componentsCount > 0) { Debug.Log("Delete: " + componentsCount + " Components"); }

            Debug.Log("Spectator View: AUTO DEINTEGRATION END");
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Проверяет, содержит ли объект компонент, в противном случае добавляет его
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public Component CheckComponent(GameObject obj, Type type)
        {
            var comp = obj.GetComponent(type);

            if (comp)
            {
                if (!type.ToString().Contains("IDHolder")) // игноририровать тип IDHolder
                {
                    Debug.Log(type.ToString() + " exist on " + GetGameObjectPath(obj));
                }

                return comp;
            }
            else
            {
                comp = obj.AddComponent(type);

                if (!type.ToString().Contains("IDHolder")) // игноририровать тип IDHolder
                {
                    Debug.Log("Add " + type.ToString() + " on " + GetGameObjectPath(obj));
                }

                return comp;
            }
        }

        /// <summary>
        /// Удаляет компонент с объекта
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public bool RemoveComponent(GameObject target, Type type)
        {
            if (target.GetComponent(type))
            {
                DestroyImmediate(target.GetComponent(type));

                if (!type.ToString().Contains("IDHolder")) // игноририровать тип IDHolder
                {
                    Debug.Log("Remove " + type.ToString() + " on " + GetGameObjectPath(target));
                }

                return true;
            }

            return false;
        }

        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        #endregion

#endif
    }
}