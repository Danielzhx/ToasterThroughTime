using UnityEngine;

namespace TTT
{
    public class ToastCoin : Collectable
    {
        public float animationDuration;
        private float _elapsedTime;
        private AnimationCurve _bobKeyframes;
        private Vector2 _startPos;

        public override void Initialize()
        {
            _bobKeyframes = new AnimationCurve();
        
            _bobKeyframes.AddKey(0.0f, 0.0f);
            _bobKeyframes.AddKey(0.25f, 0.25f);
            _bobKeyframes.AddKey(0.5f, 0.5f);
            _bobKeyframes.AddKey(0.75f, 0.25f);
            _bobKeyframes.AddKey(1.0f, 0.0f);

            _bobKeyframes.postWrapMode = WrapMode.Loop;

            _startPos = transform.position;
            base.Initialize();
        }

        private void Update()
        {
            float timeStep = (_elapsedTime % animationDuration) / animationDuration;
            transform.position = _startPos + new Vector2(0, -1) * _bobKeyframes.Evaluate(timeStep);
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= animationDuration)
                _elapsedTime = 0;
        }
    }
}
