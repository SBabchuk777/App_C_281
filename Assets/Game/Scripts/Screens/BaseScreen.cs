using UnityEngine;
using Managers;

namespace Screens
{
    public class BaseScreen : MonoBehaviour
    {
        public Canvas Canvas;
        public RectTransform Content;

        public bool IsOpenSound;

        public virtual void OpenScreen()
        {
            if (UIManager.Instance.IsSetup && IsOpenSound)
                AudioManager.Instance.ButtonClickSound();

            if (Content)
                Content.gameObject.SetActive(true);
        }

        public virtual void CloseScreen()
        {
            if (UIManager.Instance.IsSetup && IsOpenSound)
                AudioManager.Instance.ButtonClickSound();

            if (Canvas && Content)
                Content.gameObject.SetActive(false);
        }

        public void SetCamera(Camera camera)
        {
            if (Canvas)
            {
                Canvas.worldCamera = camera;
            }

        }
    }
}
