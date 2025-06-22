using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TTT
{
    public class CoinUI : MonoBehaviour
    {
        public TextMeshProUGUI coinText;
        public Image coinImage;
        public float animDuration;
        private float animPlayTime;
        private Vector3 startScale;
        private AnimationCurve scaleCurve;
        private CancellationTokenSource cts;

        private void Start()
        {
            cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy(),CancellationToken.None);
            scaleCurve = new();
            scaleCurve.AddKey(0f, 0f);
            scaleCurve.AddKey(0.25f, 0.25f);
            scaleCurve.AddKey(0.5f, 0.5f);
            scaleCurve.AddKey(0.75f, 0.25f);
            scaleCurve.AddKey(1f, 0f);

            startScale = coinImage.rectTransform.localScale;
        
            CollectablesManager.Instance.SubscribeToAllCoins(AddScore);
        }

        private async UniTaskVoid GrowCoin(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                float timeStep = animPlayTime / animDuration;
                coinImage.rectTransform.localScale = startScale + new Vector3(1,1,0) * scaleCurve.Evaluate(timeStep);
                animPlayTime += Time.deltaTime;
                if (animDuration <= animPlayTime)
                {
                    animPlayTime = 0;
                    return;
                }


                await UniTask.NextFrame();
            }
        }

        private void AddScore(Guid id)
        {
            GrowCoin(cts.Token).Forget();
            coinText.text = $"{CollectablesManager.Instance.coinsCollected}";
        }
    }
}
