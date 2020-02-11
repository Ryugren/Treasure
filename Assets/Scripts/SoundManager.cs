using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private SoundParameter[] soundParameters = null;
    [System.Serializable]
    public class SoundParameter
    {
        public GameObject Audio = null;
        private AudioSource Source { get { return Audio.GetComponent<AudioSource>(); } }
        [SerializeField]
        public AudioClip Clip = null;
        [SerializeField, Range(0f, 1f)]
        public float Volume = 1f;
        public bool Loop = false;
        public void Changer()
        {
            if (!Audio) return;
            Source.clip = Clip;
            Source.volume = Volume;
            Source.loop = Loop;
        }
    }
    public void ChangedSave()
    {
        for (int i = 0; i < soundParameters.Length; ++i)
        {
            soundParameters[i].Changer();
            //変更情報を保存
            Undo.RegisterCreatedObjectUndo(soundParameters[i].Audio, "ChangedSave");
        }
    }

    public void Search()
    {
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {
            // シーン上に存在するオブジェクトならば処理.
            if (obj.activeInHierarchy && obj.tag.Equals("Sound"))
            {
                gameObjects.Add(obj);
            }
        }
        soundParameters = new SoundParameter[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; ++i)
        {
            Component[] components = gameObjects[i].GetComponentsInChildren<Component>();
            AudioSource source = null;
            foreach (Component component in components)
            {
                if (component.GetType().Name == "AudioSource")
                {
                    source = component as AudioSource;
                }
            }
            if (!source) continue;
            soundParameters[i] = new SoundParameter
            {
                Audio = gameObjects[i],
                Clip = source.clip,
                Loop = source.loop,
                Volume = source.volume
            };
        }
        //変更情報を保存
        Undo.RegisterCreatedObjectUndo(this, "SearchedSave");
    }
    [CustomEditor(typeof(SoundManager))]
    public class ExampleInspector : Editor
    {
        SoundManager rootClass;

        private void OnEnable()
        {
            rootClass = target as SoundManager;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("検索"))
            {
                // 押下時に実行したい処理
                rootClass.Search();
            }
            if (GUILayout.Button("変更"))
            {
                // 押下時に実行したい処理
                rootClass.ChangedSave();
            }

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif