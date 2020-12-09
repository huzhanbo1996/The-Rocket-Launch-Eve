using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Text;
using System;
using UnityEditor;
using System.Linq;

public class Persist : MonoBehaviour
{
    [System.Serializable]
    class SerializedItem
    {
        public string picIdle;
        public string picPicked;
        public int objToGive;
        public int objToGive2;
        public int objCarried;
        public bool mKeephAfterUse;
        public bool mCarriedSelf;
        [System.NonSerialized]
        public GameObject prefab;

        public SerializedItem(
            string picIdle, string picPicked, int objToGive, 
            int objToGive2, int objCarried, bool mKeephAfterUse,
            bool mCarriedSelf)
        {
            this.picIdle = picIdle;
            this.picPicked = picPicked;
            this.objToGive = objToGive;
            this.objToGive2 = objToGive2;
            this.objCarried = objCarried;
            this.mKeephAfterUse = mKeephAfterUse;
            this.mCarriedSelf = mCarriedSelf;
        }
    }
    public GameObject ItemPrefab;

    [System.Serializable]
    class SerilaizationData
    {
        public List<int> mUsedSceneObj = new List<int>();
        public List<SerializedItem> mGotItems = new List<SerializedItem>();
    }
    private Dictionary<int, GameObject> mUidVSOBJ = new Dictionary<int, GameObject>();
    private Dictionary<GameObject, int> mOBJVSUid = new Dictionary<GameObject, int>();
    private SerilaizationData mData = new SerilaizationData();
    private string DATA_FILE = "save.dat";
    // Start is called before the first frame update
    void Start()
    {
        RegistGameobject();
        try
        {
            Debug.Log("open" + Application.persistentDataPath
                     + DATA_FILE);
            FileStream file = File.Open(Application.persistentDataPath
                     + DATA_FILE, FileMode.Open, FileAccess.Read);
            BinaryFormatter data = new BinaryFormatter();
            mData = (SerilaizationData)data.Deserialize(file);
            file.Close();
            Debug.Log("DATA Read");

            DeleteSceneObjs();
            LoadAllItemsGot();
        }
        catch(FileNotFoundException e)
        {
            Debug.Log(e.ToString());
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveToFile()
    {

        FileStream file = File.Open(Application.persistentDataPath
                     + DATA_FILE, FileMode.Create, FileAccess.ReadWrite);
        BinaryFormatter data = new BinaryFormatter();
        data.Serialize(file, mData);
        file.Close();
        Debug.Log("Game data saved!");
    }

    public void AddDestroy(GameObject obj)
    {
        mData.mUsedSceneObj.Add(obj.GetComponent<GameObjectGUID>().gameObjectID);
    }

    public void AddItem(Item it)
    {
        int carried = it.objCarried!=null && mOBJVSUid.ContainsKey(it.objCarried) ? mOBJVSUid[it.objCarried] : -1;
        int obj1 = it.objToGive == null ? -1 : mOBJVSUid[it.objToGive];
        int obj2 = it.objToGive2 == null ? -1 : mOBJVSUid[it.objToGive2];
        bool carriedSelf = it.objCarried == it.gameObject;
        string picIdle = GetResourcePath(it.picIdle);
        mData.mGotItems.Add(new SerializedItem(
                                         picIdle,
                                         "",
                                         obj1,
                                         obj2,
                                         carried,
                                         it.mKeephAfterUse,
                                         carriedSelf
                                         ));
    }

    public void RemoveItem(Sprite it)
    {
        for (int idx = 0; idx < mData.mGotItems.Count; idx++)
        {
            if (mData.mGotItems[idx].picIdle == GetResourcePath(it))
            {
                mData.mGotItems.RemoveAt(idx);
                return; // only remove the first found
            }
        }
    }
    private string GetResourcePath(Sprite obj)
    {
        var ret = AssetDatabase.GetAssetPath(obj);
        Debug.Assert(ret.IndexOf("Assets/Resources/") >= 0);
        ret = new string(ret.Skip("Assets/Resources/".Length).ToArray());
        ret = ret.Split('.')[0];
        return ret;
    }
    private void CopyItem(Item dest, SerializedItem orig)
    {
        Debug.Log(orig.picIdle);
        dest.picIdle = Resources.Load<Sprite>(orig.picIdle);
        Debug.Log(dest.picIdle);
        dest.picPicked = null;
        dest.objToGive = mUidVSOBJ[orig.objToGive];
        dest.objToGive2 = mUidVSOBJ[orig.objToGive2];
        dest.objCarried = orig.mCarriedSelf ? dest.gameObject : mUidVSOBJ[orig.objCarried];
        dest.mKeephAfterUse = orig.mKeephAfterUse;
    }
    
    private void DeleteSceneObjs()
    {
        var sceneObjs = Resources.FindObjectsOfTypeAll<SceneObj>();
            foreach (var uid in mData.mUsedSceneObj)
            {
                foreach(var obj in sceneObjs)
                {
                    var obj_uid = obj.gameObject.GetComponent<GameObjectGUID>();
                    if(obj_uid!= null && obj_uid.gameObjectID == uid)
                    {
                        Destroy(obj.gameObject);
                    }
                }
            }
    }

    private void RegistGameobject()
    {
        var allObjs = new List<GameObject>(Resources.FindObjectsOfTypeAll<GameObject>());
        foreach (var obj in allObjs)
        {
            // Debug.Log(obj.name);
            var scriptUid = obj.GetComponent<GameObjectGUID>();
            if (scriptUid != null)
            {
                if (scriptUid.gameObjectID == 0)
                {
                    Debug.Log(scriptUid.gameObject.name + " : " + scriptUid.gameObject.transform.parent.gameObject.name);
                }

                mUidVSOBJ.Add(scriptUid.gameObjectID, obj);
                mOBJVSUid.Add(obj, scriptUid.gameObjectID);
                Debug.Log("Found : " + obj.name);
            }
        }
        mUidVSOBJ.Add(-1, null);
    }
    private void LoadAllItemsGot()
    {
        var itemsBox = FindObjectOfType<ItemsBox>();
        foreach (var item in mData.mGotItems)
        {
            var obj = Instantiate(ItemPrefab);
            var it = obj.GetComponent<Item>();
            CopyItem(it, item);
            itemsBox.MoveItemIn(obj, true);
        }
    }
}
