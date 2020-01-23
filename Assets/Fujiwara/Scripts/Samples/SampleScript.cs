using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fujiwara
{
    public class SampleScript : MonoBehaviour
    {

        public static SampleScript Instance { get { return _instance; } }
        static SampleScript _instance = null;


        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                return;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Fujiwara");
            Debug.Log(SampleScript.Instance);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
