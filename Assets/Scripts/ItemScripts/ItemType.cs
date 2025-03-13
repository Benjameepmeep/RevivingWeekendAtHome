using UnityEngine;
using UnityEngine.Timeline;


    [CreateAssetMenu]
    public class ItemType : ScriptableObject
    {
        public string itemName;
        public string itemText;
        public Sprite itemImage;
        public Vector2 itemSize;
        public AudioClip itemAudio;
        public AudioClip cutSceneAudio;
        public TimelineAsset timeline;
    }

