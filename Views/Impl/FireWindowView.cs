using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PortfolioViewer.Views.Impl
{
    public class FireWindowView : ViewBase
    {
        [SerializeField] private Image videoImage;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button loadButton;
        [SerializeField] private GameObject loadingIndicator;
        [SerializeField] private GameObject[] hideOnPlay;
        public ProgressBar LoadingIndicator { get; private set; }
        
        private List<Sprite> _videoSprites;
        private bool _isPlaying;
        
        public Action OnLoadButtonClicked;

        public override void Init()
        {
            base.Init();
            _isPlaying = false;
            loadingIndicator.SetActive(false);
            LoadingIndicator = loadingIndicator.GetComponent<ProgressBar>();
            loadButton.gameObject.SetActive(true);
            loadButton.interactable = true;
            statusText.text = "";
            foreach (GameObject o in hideOnPlay)
            {
                o.SetActive(true);
            }
        }
        
        public void LoadButtonClicked()
        {
            OnLoadButtonClicked?.Invoke();
            loadButton.interactable = false;
            loadButton.gameObject.SetActive(false);
            loadingIndicator.SetActive(true);
        }

        public void SetVideo(List<Sprite> videoSprites)
        {
            _videoSprites = videoSprites;
        }

        public void PlayVideo()
        {
            _isPlaying = true;
            loadingIndicator.gameObject.SetActive(false);
            foreach (GameObject obj in hideOnPlay)
            {
                obj.SetActive(false);
            }
            StartCoroutine(PlayFrames());
        }

        private IEnumerator PlayFrames()
        {
            while (_isPlaying)
            {
                foreach (var sprite in _videoSprites)
                {
                    videoImage.sprite = sprite;
                    
                    // play at 60fps (16.67ms per frame)
                    yield return new WaitForSeconds(0.01667f);
                }
            }
        }
        
        public void UpdateStatus(string status) => statusText.text = status;
        public void ShowError()
        {
            loadButton.gameObject.SetActive(true);
            loadingIndicator.gameObject.SetActive(false);
            loadButton.interactable = true;
            statusText.text = "Error loading video frames, please restart the application";
        }
    }
}