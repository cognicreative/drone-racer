using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DroneRacerFpv.Utils;
using DroneRacerFpv.Controller;
using DroneRacerFpv.Settings;
using DroneRacerFpv.Match;

namespace DroneRacerFpv.Ui
{
    public class RaceInfoHud : MonoBehaviour
    {
        static RaceInfoHud instance;

        RectTransform column1;
        RectTransform column2;
        RectTransform column3;
        RectTransform column4;
        RectTransform column5;

        RectTransform rectTransform;
        Vector2 startSizeDelta;

        public float rowHeight = 25;

        public GameObject lapLabelPrefab;
        public GameObject elapsedLapTimePrefab;
        public GameObject raceInfoHudBlankPrefab;

        Text lapLabel;
        Text lapElapsedTime;
        Text elapsedTime;
        Text totalLapsText;

        int lapCount;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("RaceInfoHud has already been created.");
            }

            rectTransform = GetComponent<RectTransform>();
            startSizeDelta = rectTransform.sizeDelta;

            column1 = SceneHelper.FindFirstChildInHierarchy(this, "Column1").GetComponent<RectTransform>();
            column2 = SceneHelper.FindFirstChildInHierarchy(this, "Column2").GetComponent<RectTransform>();
            column3 = SceneHelper.FindFirstChildInHierarchy(this, "Column3").GetComponent<RectTransform>();
            column4 = SceneHelper.FindFirstChildInHierarchy(this, "Column4").GetComponent<RectTransform>();
            column5 = SceneHelper.FindFirstChildInHierarchy(this, "Column5").GetComponent<RectTransform>();

            elapsedTime = SceneHelper.FindFirstChildInHierarchy(this, "ElapsedTime").GetComponent<Text>();

            totalLapsText = SceneHelper.FindFirstChildInHierarchy(this, "TotalLaps").GetComponent<Text>();

        }

        public void StartWatchRaceStatus()
        {
            StartCoroutine(WatchRaceStatus());
        }

        IEnumerator WatchRaceStatus()
        {
            ResetHud();

            totalLapsText.text = RaceManager.GetTotalLaps().ToString();

            while (RaceManager.IsRaceInProgress() == true)
            {
                if (lapCount != RaceManager.GetLapCount())
                {
                    AddLapRow();

                    lapCount = RaceManager.GetLapCount();
                }

                UpdateElapsedTime(RaceManager.GetElapsedSec());
                UpdateLapElapsedTime(RaceManager.GetLapElapsedSec());

                yield return null;
            }
        }

        void UpdateElapsedTime(float elapsedTimeSec)
        {
            if (elapsedTime != null)
            {
                TimeSpan ts = TimeSpan.FromSeconds(elapsedTimeSec);

                elapsedTime.text = string.Format("{0}:{1:00}:{2:0}", ts.Minutes, ts.Seconds, ts.Milliseconds / 100);
            }
        }

        void UpdateLapElapsedTime(float lapElapsedTimeSec)
        {
            if (lapElapsedTime != null)
            {
                TimeSpan ts = TimeSpan.FromSeconds(lapElapsedTimeSec);

                lapElapsedTime.text = string.Format("{0}:{1:00}:{2:0}", ts.Minutes, ts.Seconds, ts.Milliseconds / 100);
            }
        }


        void ResetHud()
        {
            lapCount = 0;

            UpdateElapsedTime(0);
            UpdateLapElapsedTime(0);

            ResetColumn(column1.transform);
            ResetColumn(column2.transform);
            ResetColumn(column3.transform);
            ResetColumn(column4.transform);
            ResetColumn(column5.transform);

            rectTransform.sizeDelta = startSizeDelta;
        }

        void ResetColumn(Transform columnTransform)
        {
            bool skippedFirst = false;

            foreach (Transform child in columnTransform)
            {
                if (skippedFirst == true)
                {
                    Destroy(child.gameObject);
                }

                skippedFirst = true;
            }
        }

        void AddLapRow()
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;

            sizeDelta.y += rowHeight;

            rectTransform.sizeDelta = sizeDelta;

            lapLabel = GameObject.Instantiate(lapLabelPrefab).GetComponent<Text>();
            lapLabel.text = string.Format(Constants.LapLabelFormat, RaceManager.GetLapCount());

            lapLabel.gameObject.transform.SetParent(column1.transform);
            lapLabel.transform.localScale = Vector2.one;

            lapElapsedTime = GameObject.Instantiate(elapsedLapTimePrefab).GetComponent<Text>();
            lapElapsedTime.text = Constants.ElapsedLapTime;

            lapElapsedTime.gameObject.transform.SetParent(column2.transform);
            lapElapsedTime.transform.localScale = Vector2.one;

            GameObject blank = GameObject.Instantiate(raceInfoHudBlankPrefab);

            blank.transform.SetParent(column3.transform);
            blank.transform.localScale = Vector2.one;

            blank = GameObject.Instantiate(raceInfoHudBlankPrefab);

            blank.transform.SetParent(column4.transform);
            blank.transform.localScale = Vector2.one;

            blank = GameObject.Instantiate(raceInfoHudBlankPrefab);

            blank.transform.SetParent(column5.transform);
            blank.transform.localScale = Vector2.one;
        }
    }
}