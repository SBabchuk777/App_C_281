using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Managers;

namespace UserProfile.UI
{
    [RequireComponent(typeof(Animator))]
    public class SelectPicturePanel : MonoBehaviour
    {
        [SerializeField] private Button cancel;

        private Animator _animator = null;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            cancel.onClick.AddListener(CancelButton);
        }

        private void DestroyPanel()
        {
            Destroy(gameObject);
        }
        private void CancelButton()
        {
            AudioManager.Instance.ButtonClickSound();
        }
        public async void MakeNewPicture()
        {
            AudioManager.Instance.ButtonClickSound();
            await Task.Delay(50);
            NativeCamera.TakePicture(LoadImageByPath);
        }

        public async void SelectPictureFromGallery()
        {
            AudioManager.Instance.ButtonClickSound();
            await Task.Delay(50);
            NativeGallery.GetImageFromGallery(LoadImageByPath);
        }
        
        private void LoadImageByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            
            Texture2D texture = NativeGallery.LoadImageAtPath(path);

            StartCropping(texture);
        }

        private void StartCropping(Texture2D texture)
        {
            ImageCropper.Settings settings = new ImageCropper.Settings();

            settings.markTextureNonReadable = false;
            
            settings.selectionMinAspectRatio = 1f;
            settings.selectionMaxAspectRatio = 1f;
            
            ImageCropper.Instance.Show(texture, SetIconAfterCrop, settings);
        }
        
        private void SetIconAfterCrop(bool result, Texture original, Texture cropped)
        {
            if (result)
            {
                Texture2D texture = (Texture2D)cropped;
                
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    Vector2.one * 0.5f);

                UserProfileStorage.UserIcon = sprite;

                _animator.SetTrigger("Hide");
            }
        }
    }
}
