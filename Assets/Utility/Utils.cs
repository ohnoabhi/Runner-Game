using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace Utility
{
    public static class Input
    {
        public static int TouchCount
        {
            get
            {
#if UNITY_EDITOR
                return FakeTouch.HasValue ? 1 : 0;
#else
                return UnityEngine.Input.touchCount;
#endif
            }
        }

        private static Touch? FakeTouch => SimulateTouchWithMouse.Instance.FakeTouch;

        public static Touch[] Touches
        {
            get
            {
#if UNITY_EDITOR
                return FakeTouch.HasValue ? new[] {FakeTouch.Value} : new Touch[0];
#else
                return UnityEngine.Input.touches;
#endif
            }
        }

        public static bool GetButton(string buttonName)
        {
            return UnityEngine.Input.GetButton(buttonName);
        }

        public static bool GetButtonDown(string buttonName)
        {
            return UnityEngine.Input.GetButtonDown(buttonName);
        }

        public static bool GetButtonUp(string buttonName)
        {
            return UnityEngine.Input.GetButtonUp(buttonName);
        }

        public static bool GetMouseButton(int button)
        {
            return UnityEngine.Input.GetMouseButton(button);
        }

        public static bool GetMouseButtonDown(int button)
        {
            return UnityEngine.Input.GetMouseButtonDown(button);
        }

        public static bool GetMouseButtonUp(int button)
        {
            return UnityEngine.Input.GetMouseButtonUp(button);
        }

        public static Touch GetTouch(int index)
        {
#if UNITY_EDITOR
            Assert.IsTrue(FakeTouch.HasValue && index == 0);
            return FakeTouch.Value;
#else
            return UnityEngine.Input.GetTouch(index);
#endif
        }
    }

    internal class SimulateTouchWithMouse
    {
        private static SimulateTouchWithMouse _instance;
        private Touch? _fakeTouch;
        private float _lastUpdateTime;
        private Vector3 _prevMousePos;


        public static SimulateTouchWithMouse Instance
        {
            get
            {
                if (_instance == null) _instance = new SimulateTouchWithMouse();

                return _instance;
            }
        }

        public Touch? FakeTouch
        {
            get
            {
                Update();
                return _fakeTouch;
            }
        }

        private void Update()
        {
            if (Time.time != _lastUpdateTime)
            {
                _lastUpdateTime = Time.time;

                var curMousePos = UnityEngine.Input.mousePosition;
                var delta = curMousePos - _prevMousePos;
                _prevMousePos = curMousePos;

                _fakeTouch = CreateTouch(GETPhase(), delta);
            }
        }

        private static TouchPhase? GETPhase()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
                return TouchPhase.Began;
            if (UnityEngine.Input.GetMouseButton(0))
                return TouchPhase.Moved;
            if (UnityEngine.Input.GetMouseButtonUp(0))
                return TouchPhase.Ended;
            return null;
        }

        private static Touch? CreateTouch(TouchPhase? phase, Vector3 delta)
        {
            if (!phase.HasValue) return null;

            var curMousePos = UnityEngine.Input.mousePosition;
            return new Touch
            {
                phase = phase.Value,
                type = TouchType.Indirect,
                position = curMousePos,
                rawPosition = curMousePos,
                fingerId = 0,
                tapCount = 1,
                deltaTime = Time.deltaTime,
                deltaPosition = delta
            };
        }
    }
}