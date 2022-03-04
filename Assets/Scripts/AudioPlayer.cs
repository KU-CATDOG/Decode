using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            AudioManager.Instance.bgmPlaying = false;
            AudioManager.Instance.PlayBGM(AudioType.hpboss);
        } else if(SceneManager.GetActiveScene().buildIndex == 6)
        {
            AudioManager.Instance.bgmPlaying = false;
            AudioManager.Instance.PlayBGM(AudioType.mpboss);
        }else if(SceneManager.GetActiveScene().buildIndex == 8)
        {
            AudioManager.Instance.bgmPlaying = false;
            AudioManager.Instance.PlayBGM(AudioType.speedboss);
        }
        else
        {
            AudioManager.Instance.bgmPlaying = false;
            AudioManager.Instance.PlayBGM(AudioType.stage1);
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
