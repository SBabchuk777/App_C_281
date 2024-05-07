using System.Collections.Generic;
using UnityEngine;
using Screens;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private List<BaseScreen> screensPrefab = new List<BaseScreen>();
        [SerializeField] private List<BaseScreen> screens = new List<BaseScreen>();

        [SerializeField] private Camera camera;

        public bool IsSetup = false;

        protected override void Awake()
        {
            base.Awake();

            SetupScreen();
        }

        private void Start()
        {
            OpenScreen<StartScreen>();

            IsSetup = true;
        }

        private void SetupScreen()
        {
            for (int i = 0; i < screensPrefab.Count; i++)
            {
                var addScreen = Instantiate(screensPrefab[i], gameObject.transform);
                addScreen.SetCamera(camera);
                addScreen.CloseScreen();
                screens.Add(addScreen);
            }
        }

        public void OpenScreen<T>() where T : BaseScreen
        {
            T screen = GetScreen<T>();

            if (screen != null)
            {
                screen.OpenScreen();
            }
            else
            {
                UnityEngine.Debug.LogError($"Screen of type {typeof(T)} is not registered.");
            }
        }

        public void CloseScreen<T>() where T : BaseScreen
        {
            T screen = GetScreen<T>();

            if (screen != null)
            {
                screen.CloseScreen();
            }
            else
            {
                UnityEngine.Debug.LogError($"Screen of type {typeof(T)} is not registered.");
            }
        }

        public T GetScreen<T>() where T : BaseScreen
        {
            foreach (var screen in screens)
            {
                if (screen is T)
                {
                    return (T)screen;
                }
            }
            return null;
        }
    }
}