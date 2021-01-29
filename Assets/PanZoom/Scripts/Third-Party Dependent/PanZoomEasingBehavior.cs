using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DentedPixel;

namespace Dossamer.PanZoom
{
    [RequireComponent(typeof(PanZoomBehavior))]
    public class PanZoomEasingBehavior : MonoBehaviour
    {
        PanZoomBehavior pz;
        Camera pzCam;

        List<int> tweenList = new List<int>();

        public LeanTweenType tweenType = LeanTweenType.easeOutCubic;
        public float tweenDuration = 1f;

        // getting instantaneous velocity requires two points
        // at any time cache queue only holds two values

        public float cacheInterval = 0.066f; // 66 milliseconds between position snapshots
        float lastCacheTime = 0f;

        struct DataCache
        {
            public Vector3 panPosition;
            public float panTime;
            public float viewportSize;
        }

        Queue<DataCache> dataQueue = new Queue<DataCache>();

        private void Awake()
        {
            pz = GetComponent<PanZoomBehavior>();
        }

        private void Start()
        {
            pzCam = pz.GetReferenceCamera();
        }

        private void OnEnable()
        {
            pz.PanStarted += HandlePanStart;
            pz.PanEnded += HandlePanEnd;
        }

        private void OnDisable()
        {
            pz.PanStarted -= HandlePanStart;
            pz.PanEnded += HandlePanEnd;
        }

        void UpdateCacheQueue()
        {
            if (Time.time - lastCacheTime >= cacheInterval)
            {
                dataQueue.Enqueue(new DataCache
                {
                    panPosition = pz.transform.position,
                    panTime = Time.time,
                    viewportSize = pz.GetReferenceCamera().orthographicSize
                });

                if (dataQueue.Count > 2)
                {
                    dataQueue.Dequeue();
                }

                lastCacheTime = Time.time; 
            }
        }

        void InitQueue()
        {
            dataQueue.Clear();

            dataQueue.Enqueue(new DataCache
            {
                panPosition = pz.transform.position,
                panTime = Time.time,
                viewportSize = pz.GetReferenceCamera().orthographicSize
            });
        }

        private void HandlePanStart(object sender, EventArgs e)
        {
            // Debug.Log("pan started");

            ClearTweens();

            InitQueue();
        }

        private void LateUpdate()
        {
            if (pz.IsPanning)
            {
                UpdateCacheQueue();
            }
        }

        private void HandlePanEnd(object sender, EventArgs e)
        {
            // Debug.Log("pan ended");

            if (dataQueue.Count >= 2)
            {
                StartEndTween();
            }        
        }

        void StartEndTween()
        {
            DataCache start = dataQueue.Dequeue();
            DataCache end = dataQueue.Dequeue();

            dataQueue.Clear();

            float deltaTime = end.panTime - start.panTime;

            Vector3 velocity = (end.panPosition - start.panPosition) / deltaTime;
            // for minute changes velocity's magnitude will often end up being infinity (e.g. single scroll of scrollwheel)
            if (velocity.magnitude == Mathf.Infinity || float.IsNaN(velocity.magnitude))
            {
                velocity = Vector3.zero;
            }

            // Debug.Log(velocity + ", " + velocity.magnitude);

            float speed = (end.viewportSize - start.viewportSize) / deltaTime;
            if (speed == Mathf.NegativeInfinity || speed == Mathf.Infinity || float.IsNaN(speed))
            {
                speed = 0f;
            }

            ClearTweens();

            // Debug.Log("init vel: " + velocity.magnitude);

            if (velocity.magnitude != 0)
            {
                tweenList.Add(LeanTween.value(velocity.magnitude, 0f, tweenDuration)
                    .setOnUpdate((float value) => {
                        Vector3 translation = velocity.normalized * value * Time.deltaTime;

                        if (pz.GetFocusTarget() != null)
                        {
                            float potentialDot = pz.GetForwardDotToFocusTarget(pz.transform.position + translation);

                            if (pz.GetForwardDotToFocusTarget() <= 0 && potentialDot <= 0 && Mathf.Abs(potentialDot) > pz.GetReferenceCamera().nearClipPlane + pz.GetMinForwardDistFromTarget())
                            {
                                pz.transform.position += translation;
                            }

                            if (potentialDot <= 0 && Mathf.Abs(potentialDot) <= pz.GetReferenceCamera().nearClipPlane + pz.GetMinForwardDistFromTarget())
                            {
                                // close enough that FOV should be tweened
                                pzCam.fieldOfView = pz.GetClampedCameraFOV(pzCam.fieldOfView * 0.8f);
                            }
                        }
                        else
                        {
                            pz.transform.position += translation;
                        }
                    })
                    .setEase(tweenType)
                    .id);
            }

            if (speed != 0)
            {
                tweenList.Add(LeanTween.value(speed, 0f, tweenDuration)
                    .setOnUpdate((float value) => { pzCam.orthographicSize = pz.GetClampedCameraSize(pzCam.orthographicSize + value * Time.deltaTime); })
                    .setEase(tweenType)
                    .id);
            }
        }

        void ClearTweens()
        {
            foreach (int tween in tweenList)
            {
                LeanTween.cancel(tween);
            }

            tweenList.Clear();
        }
    }
}