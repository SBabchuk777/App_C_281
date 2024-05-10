using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Saves;

public class ButtonBonusController : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] private float shakeInterval = 5f;

    private void OnEnable()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shakeInterval);

            if (GameSaves.Instance.IsAccessAvailable() && !GameSaves.Instance.GetClaimReward())
            {
                button.gameObject.SetActive(true);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
