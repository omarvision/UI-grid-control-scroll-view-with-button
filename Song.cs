using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Song : MonoBehaviour
{
    public string songfile = "";
    private AudioSource aud;

    private void Start()
    {
        aud = this.GetComponent<AudioSource>();
    }
    public void LoadSong(string file)
    {
        songfile = file;
        this.GetComponentInChildren<Text>().GetComponent<Text>().text = System.IO.Path.GetFileNameWithoutExtension(file);
        StartCoroutine(LoadSongCoroutine());
    }
    private IEnumerator LoadSongCoroutine()
    {
        string url = string.Format("file://{0}", songfile);
        WWW www = new WWW(url);
        yield return www;

        aud.clip = NAudioPlayer.FromMp3Data(www.bytes);
        aud.Play();
    }
}