using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Quiz9Puzzle : MonoBehaviour
{
    public string mInitalOrder;
    public GameObject mBtnPrefab;
    public bool isResolved;

    private string RESOURCES_PATH = "Quiz9Puzzle";
    private List<piece> mPieces = new List<piece>();
    private float mSpriteDistance;
    private class piece
    {
        public GameObject obj;
        public int number;
        public int position;
        public bool isVoid;

        public piece(GameObject obj, int number, bool isVoid = false)
        {
            this.obj = obj;
            this.number = number;
            this.isVoid = isVoid;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int i;
        for (i = 1; i <= 8; i++)
        {
            string spriteName = i.ToString().ToUpper();
            Debug.Log(RESOURCES_PATH + "/" + spriteName);
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + spriteName)) as Sprite;
            var obj = Instantiate(mBtnPrefab);
            obj.GetComponent<SpriteRenderer>().sprite = sp;
            obj.transform.parent = this.transform;
            mPieces.Add(new piece(obj, i));
            mSpriteDistance = sp.textureRect.width / sp.pixelsPerUnit;
        }
        var obj2 = Instantiate(mBtnPrefab);
        obj2.transform.parent = this.transform;
        mPieces.Add(new piece(obj2, i, true));
        for (i = 0; i < mInitalOrder.Length; i++)
        {
            int target = int.Parse(mInitalOrder[i].ToString());
            foreach(var piece in mPieces)
            {
                if(piece.number == target)
                {
                    piece.position = i;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var piece in mPieces)
        {
            if(Common.Utils.ClickedOn(piece.obj))
            {
                var pieceTarget = FindVoid();
                int targetPosition = pieceTarget.position;
                int xNow = piece.position % 3;
                int yNow = piece.position / 3;
                int xTarget = targetPosition % 3;
                int yTarget = targetPosition / 3;
                if((xNow == xTarget && yNow + 1 == yTarget) ||
                   (xNow == xTarget && yNow - 1 == yTarget) ||
                   (yNow == yTarget && xNow + 1 == xTarget) ||
                   (yNow == yTarget && xNow - 1 == xTarget))
                {
                    int tmp = piece.position;
                    piece.position = pieceTarget.position;
                    pieceTarget.position = tmp;
                }
            }
        }
        string ans = "";
        for (int pos = 0; pos <= 7; pos++)
        {
            foreach(var piece in mPieces)
            {
                if (piece.position == pos)
                    ans += piece.number.ToString();
            }
        }
        if(ans == "12345678")
        {
            isResolved = true;
        }
        Render();
    }

    public void OnEnable()
    {
        if (isResolved) return;
        for (int i = 0; i < mInitalOrder.Length; i++)
        {
            int target = int.Parse(mInitalOrder[i].ToString());
            foreach (var piece in mPieces)
            {
                if (piece.number == target)
                {
                    piece.position = i;
                }
            }
        }
    }

    private piece FindVoid()
    {
        foreach(var piece in mPieces)
        {
            if (piece.isVoid) return piece;
        }
        Debug.LogError("No void piece found");
        return null;
    }

    private void Render()
    {
        foreach(var piece in mPieces)
        {
            int x = piece.position % 3 - 1;
            int y = -(piece.position / 3 - 1);
            piece.obj.transform.localPosition = new Vector3(
                x * mSpriteDistance,
                y * mSpriteDistance,
                piece.obj.transform.localPosition.z
                );
        }
    }
}
