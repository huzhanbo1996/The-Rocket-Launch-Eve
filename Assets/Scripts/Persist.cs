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
    enum TYPE_ITEM { NORMAL, CAMERA}
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
        public TYPE_ITEM mType;
        [System.NonSerialized]
        public GameObject prefab;

        public SerializedItem(
            string picIdle, string picPicked, int objToGive, 
            int objToGive2, int objCarried, bool mKeephAfterUse,
            bool mCarriedSelf, TYPE_ITEM mType = TYPE_ITEM.NORMAL)
        {
            this.picIdle = picIdle;
            this.picPicked = picPicked;
            this.objToGive = objToGive;
            this.objToGive2 = objToGive2;
            this.objCarried = objCarried;
            this.mKeephAfterUse = mKeephAfterUse;
            this.mCarriedSelf = mCarriedSelf;
            this.mType = mType;
        }
    }
    public SpritesPath mSpritePath;
    public GameObject ItemPrefab;
    public GameObject ItemCamera;
    [System.Serializable]
    class SerilaizationData
    {
        public List<int> mUsedSceneObj = new List<int>();
        public List<SerializedItem> mGotItems = new List<SerializedItem>();
        public List<QuizData> mQuizData = new List<QuizData>();
    }

    public Dictionary<int, GameObject> UidVSOBJ { get { return mUidVSOBJ; } }
    public Dictionary<GameObject, int> OBJVSUid { get { return mOBJVSUid; } }
    private Dictionary<int, GameObject> mUidVSOBJ = new Dictionary<int, GameObject>();
    private Dictionary<GameObject, int> mOBJVSUid = new Dictionary<GameObject, int>();
    private List<IQuizSerializable> mQuiz = new List<IQuizSerializable>();
    private SerilaizationData mData = new SerilaizationData();
    private string DATA_FILE = "save.dat";
    // Start is called before the first frame update
    void Start()
    {
        mSpritePath = GetComponent<SpritesPath>();
        var mSE = FindObjectOfType<SoundEffect>();
        mSE.enable = false;
        RegistGameobject();
        RegistQuiz();
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

            // It's possible that during quiz deserialization, quiz give some items
            // back to player, so be sure that DeserializaQuizData() is after 
            // LoadAllItemsGot()
            DeserializeQuizData();
        }
        catch(FileNotFoundException e)
        {
            Debug.Log(e.ToString());
            return;
        }
        finally
        {
            mSE.enable = true;
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
        SerializeQuizData();
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
        string picIdle = mSpritePath.SpriteVSPath[it.picIdle];
        TYPE_ITEM type = it.gameObject.GetComponent<ItemCamera>() == null ? TYPE_ITEM.NORMAL : TYPE_ITEM.CAMERA;
        mData.mGotItems.Add(new SerializedItem(
                                            picIdle,
                                            "",
                                            obj1,
                                            obj2,
                                            carried,
                                            it.mKeephAfterUse,
                                            carriedSelf,
                                            type
                                            ));
    }

    public void RemoveItem(Sprite it)
    {
        for (int idx = 0; idx < mData.mGotItems.Count; idx++)
        {
            if (mData.mGotItems[idx].picIdle == mSpritePath.SpriteVSPath[it])
            {
                mData.mGotItems.RemoveAt(idx);
                return; // only remove the first found
            }
        }
    }
    public string GetResourcePath(Sprite obj)
    {
        return mSpritePath.SpriteVSPath[obj];
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
            GameObject obj = null;
            Item it;
            switch(item.mType)
            {
                case TYPE_ITEM.NORMAL:
                    obj = Instantiate(ItemPrefab);
                    it = obj.GetComponent<Item>();
                    CopyItem(it, item);
                    itemsBox.MoveItemIn(obj, true);
                    break;
                case TYPE_ITEM.CAMERA:
                    it = ItemCamera.GetComponent<Item>();
                    CopyItem(it, item);
                    itemsBox.MoveItemIn(ItemCamera, true);
                    break;
            }
            
        }
    }

    // TODO  QuizCamera is special, item and quiz need special logic (whith QuizWire, Quiz4NPC, QuizMirror)
    // ComputerSwitch related quiz don't need any persistance, 
    // tough should persist ComputerSwitch data
    private void RegistQuiz()
    {
        mQuiz.AddRange(Resources.FindObjectsOfTypeAll<QuizHuaRong>());
        mQuiz.Add(Resources.FindObjectsOfTypeAll<Quiz4Locks>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<Quiz4NPC>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<Quiz9Puzzle>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizBottleLogic>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizB_A>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizJuiceMachine>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizLock>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizPlat>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizPuzzle>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizVendingMachine>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizElec>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizFinal>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizMirror>()[0]);

        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizCamera>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<LevelCtrl>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<ComputerSwitch>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizComputer>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizPointsSupression>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizRotate>()[0]);
        mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizWire>()[0]);
        // mQuiz.Add(Resources.FindObjectsOfTypeAll<QuizIngredient>()[0]);
    }

    private void SerializeQuizData()
    {
        mData.mQuizData.Clear();
        foreach(var quiz in mQuiz)
        {
            mData.mQuizData.Add(quiz.Serialize());
        }
    }

    private void DeserializeQuizData()
    {
        for (int idx = 0; idx < mQuiz.Count; idx++)
        {
            mQuiz[idx].Deserialize(mData.mQuizData[idx]);
        }
    }
}
