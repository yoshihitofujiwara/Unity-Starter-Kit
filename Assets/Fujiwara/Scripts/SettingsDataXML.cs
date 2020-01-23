using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;


namespace Fujiwara
{
    /*==========================================================================
        @DefaultSettingsData
    ==========================================================================*/
    [System.Serializable]
    public class Settings
    {
        public string ip = "192.168.11.111";
        public int port = 5555;
    }


    /*==========================================================================
        @SettingsDataXML
    ==========================================================================*/
    [ExecuteInEditMode]
    public class SettingsDataXML : MonoBehaviour
    {
        public string fileName = "Settings.xml";
        public Settings settings;

        static SettingsDataXML _instance = null;
        public static SettingsDataXML Instance { get { return _instance; } }


        /*--------------------------------------------------------------------------
            @LifeCycleMethods
        --------------------------------------------------------------------------*/
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                // FIXME:
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                // DestroyImmediate(this.gameObject);
                Destroy(this.gameObject);
                return;
            }

            if (File.Exists(fileName))
            {
                LoadData();
            }
            else
            {
                SaveData();
            }
        }


        /*--------------------------------------------------------------------------
            @Methods
        --------------------------------------------------------------------------*/
        // ファイルを読み込んで設定を更新
        void LoadData()
        {
            if (File.Exists(fileName))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    settings = (Settings)serializer.Deserialize(stream);
                }
            }
        }


        // 現在の設定をファイルに保存
        void SaveData()
        {
            var serializer = new XmlSerializer(typeof(Settings));
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(stream, settings);
            }
        }
    }


    /*==========================================================================
        @SettingsDataXMLEditor
    ==========================================================================*/
    [CustomEditor(typeof(SettingsDataXML))]
    public class SettingsDataXMLEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //元のInspector部分を表示
            base.OnInspectorGUI();

            //targetを変換して対象を取得
            SettingsDataXML settingsXML = target as SettingsDataXML;

            //PrivateMethodを実行する用のボタン
            if (GUILayout.Button("LoadData"))
            {
                settingsXML.SendMessage("LoadData", null, SendMessageOptions.DontRequireReceiver);
            }

            if (GUILayout.Button("SaveData"))
            {
                settingsXML.SendMessage("SaveData", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
