using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    private const float LINE_HEIGHT = 30;
    private const float CONTENT_X_OFFSET = 391;
    private const float CONTENT_Y_OFFSET = -18;

    #region --- helper ---    
    private class SongData
    {
        public int idx = -1;
        public string fullpath = "";
        public string name = "";
        public Vector3 vec3line;
        public Button btnLine = null;

        public SongData(int _idx, string _fullpath)
        {
            idx = _idx;
            fullpath = _fullpath;
            name = Path.GetFileNameWithoutExtension(fullpath);
            vec3line = new Vector3(CONTENT_X_OFFSET, CONTENT_Y_OFFSET - LINE_HEIGHT * idx, 0);
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", idx.ToString().PadRight(4), vec3line.ToString().PadRight(25), name.PadRight(70), fullpath);
        }
    }
    private struct childObject
    {
        public GameObject Content;
    }
    #endregion

    public string musicPath = @"F:\Omar Music";
    public Button prefabGridLine = null;
    public Button ButtonSong = null;
    private List<SongData> songs = new List<SongData>();
    private childObject obj;
    
    private void Start()
    {
        obj.Content = GetTheChild("Content", this.transform);

        SearchSongs(musicPath);
        AddGridItems();
        Debugging();
    }
    private void SearchSongs(string path, int recurselevel = 0)
    {
        //songs
        foreach (string fullpath in Directory.GetFiles(path, "*.mp3"))
            songs.Add(new SongData(songs.Count, fullpath));

        //sub-directories
        foreach (string directory in Directory.GetDirectories(path))
            SearchSongs(directory, recurselevel + 1);
    }
    private void AddGridItems()
    {
        for (int i = 0; i < songs.Count; i++)
        {
            songs[i].btnLine = Instantiate(prefabGridLine, obj.Content.transform);              //add button to scene
            songs[i].btnLine.name = "btn" + songs[i].idx.ToString();                            //name it
            songs[i].btnLine.transform.localPosition = songs[i].vec3line;                       //position it
            string name = songs[i].idx.ToString().PadRight(5) + songs[i].name;
            songs[i].btnLine.GetComponentInChildren<Text>().GetComponent<Text>().text = name;   //text

            //Note: make a variable and pass the variable, or else onClick paramater will always be the last value of the iterator
            int idx = songs[i].idx;
            songs[i].btnLine.onClick.AddListener(() => GridButton_onClick(idx));
        }        
    }
    public void GridButton_onClick(int idx)
    {
        ButtonSong.GetComponent<Song>().LoadSong(songs[idx].fullpath);
    }
    //
    private GameObject GetTheChild(string name, Transform parent, int recurseLevel = 0)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name.ToLower() == name.ToLower())
                return child.gameObject;
        }

        //this recursively searches for name in the child transforms (turns the child into the parent and recalls this function)
        foreach (Transform child in parent)
        {
            GameObject go = GetTheChild(name, child, recurseLevel + 1);
            if (go != null)
                return go;
        }

        return null;
    }    
    private void Debugging()
    {
        string path = "Assets/Debug.txt";
        StreamWriter sw = new StreamWriter(path, false);
        
        sw.WriteLine("Songs:");
        foreach (SongData itm in songs)
        {
            sw.WriteLine("\t" + itm.ToString());
        }
                
        sw.Close();
        UnityEditor.AssetDatabase.ImportAsset(path);
    }    
}
