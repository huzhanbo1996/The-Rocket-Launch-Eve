using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizHuaRong : MonoBehaviour
{
    public float pixPerUnit = 100.0f;
    public GameObject mPiecePrefab;
    public bool mStage2 = false;
    public GameObject mResetBtn;
    public GameObject mSceneObj;
    public GameObject mObjTOGive;

    private ItemsBox mItemsBox;
    private Vector2Int mAnsPosition;
    private class Piece
    {
        public GameObject obj;
        public int[][] points;
        public Vector2Int position;
        public bool verticle;
        public int lenght;
        public bool isSp;

        public Piece(GameObject obj, int[][] points, Vector2Int position, bool verticle, bool isSp = false)
        {
            this.obj = obj;
            this.points = points;
            this.position = position;
            this.verticle = verticle;
            this.isSp = isSp;
            int cnt = 0;
            for (int i = 0; i < points.Length; i++)
                for (int j = 0; j < points[0].Length; j++)
                    cnt += points[i][j];
            this.lenght = cnt;
        }
    }

    private Dictionary<string, Sprite> mSps = new Dictionary<string, Sprite>();
    private List<Piece> mPieces = new List<Piece>();
    private int[][] mTable = new int[][]
        {
            new int[]{0, 0, 0, 0 , 0, 0 },
            new int[]{0, 0, 0, 0 , 0, 0 },
            new int[]{0, 0, 0, 0 , 0, 0 },
            new int[]{0, 0, 0, 0 , 0, 0 },
            new int[]{0, 0, 0, 0 , 0, 0 },
            new int[]{0, 0, 0, 0 , 0, 0 }
        };
    private const string RESOURCES_PATH = "QuizHuarong";
    // Start is called before the first frame update
    void Start()
    {
        mItemsBox = FindObjectOfType<ItemsBox>();
        string[] rsName = { "1x3", "1x2", "3x1", "2x1", "target"};
        foreach(var name in rsName)
        {
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + name)) as Sprite;
            mSps.Add(name, sp);
        }
        if (mStage2)
        {
            Clear();
            Set2();
        }
        else
        {
            Clear();
            Set1();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update table
        ClearTable();
        foreach (var piece in mPieces)
        {
            int biasI = piece.position.x;
            int biasY = piece.position.y;
            for (int i = 0; i < piece.points.Length; i++)
            {
                for (int j = 0; j < piece.points[0].Length; j++)
                {
                    if (biasI + i < mTable.Length &&
                        biasI + i >=0 &&
                        biasY + j < mTable[0].Length &&
                        biasY + j >=0
                        )
                        mTable[biasI + i][biasY + j] |= piece.points[i][j];
                }
            }
        }

        foreach (var piece in mPieces)
        {
            var sc = piece.obj.GetComponent<Draguable>();
            // if piece not being drag, adjust it render position
            if (!sc.isMouseDown)
            {
                piece.obj.transform.localPosition =
                    new Vector3(piece.position.x, piece.position.y, 
                                piece.obj.transform.localPosition.z);

                // update drag limitation piece unpicked
                sc.limit = CaculateLimitation(piece);
            }
            else
            {
                // if piece being drag, update its table position
                var newX = Mathf.RoundToInt(piece.obj.transform.localPosition.x);
                var newY = Mathf.RoundToInt(piece.obj.transform.localPosition.y);
                piece.position = new Vector2Int(newX, newY);
            }
            
            // check ans
            if(piece.isSp && piece.position == mAnsPosition)
            {
                mObjTOGive.SetActive(true);
                //mItemsBox.MoveItemIn(mObjTOGive);
                mSceneObj.GetComponent<SceneObj>().QuizResolved();
            }
        }
        if(Common.Utils.ClickedOn(mResetBtn))
        {
            Clear();
            if(mStage2)
            {
                Clear();
                Set2();
            }
            else
            {
                Clear();
                Set1();
            }
        }

    }

    private void Clear()
    {
        foreach (var piece in mPieces)
        {
            Destroy(piece.obj);
        }
        mPieces.Clear();
    }
    private void Set1()
    {
        mAnsPosition = new Vector2Int(4, 3);
        AddPiece(mPiecePrefab, mSps["1x3"], POINTS1x3, new Vector2Int(0, 5), true);
        AddPiece(mPiecePrefab, mSps["1x2"], POINTS1x2, new Vector2Int(0, 0), true);
        AddPiece(mPiecePrefab, mSps["1x2"], POINTS1x2, new Vector2Int(4, 1), true);
        AddPiece(mPiecePrefab, mSps["1x2"], POINTS1x2, new Vector2Int(2, 4), true);
        AddPiece(mPiecePrefab, mSps["1x2"], POINTS1x2, new Vector2Int(4, 4), true);
        AddPiece(mPiecePrefab, mSps["3x1"], POINTS3x1, new Vector2Int(2, 0), false);
        AddPiece(mPiecePrefab, mSps["3x1"], POINTS3x1, new Vector2Int(3, 0), false);
        AddPiece(mPiecePrefab, mSps["2x1"], POINTS2x1, new Vector2Int(1, 1), false);
        AddPiece(mPiecePrefab, mSps["2x1"], POINTS2x1, new Vector2Int(1, 3), false);
        AddPiece(mPiecePrefab, mSps["2x1"], POINTS2x1, new Vector2Int(4, 2), false);
        AddPiece(mPiecePrefab, mSps["target"], POINTS1x2, new Vector2Int(2, 3), true, true);
    }
    private void Set2()
    {
        mAnsPosition = new Vector2Int(4, 3);
        AddPiece(mPiecePrefab, mSps["1x3"], POINTS1x3, new Vector2Int(2, 4), true);
        AddPiece(mPiecePrefab, mSps["1x3"], POINTS1x3, new Vector2Int(3, 5), true);
        AddPiece(mPiecePrefab, mSps["1x2"], POINTS1x2, new Vector2Int(3, 1), true);
        AddPiece(mPiecePrefab, mSps["3x1"], POINTS3x1, new Vector2Int(2, 1), false);
        AddPiece(mPiecePrefab, mSps["2x1"], POINTS2x1, new Vector2Int(0, 1), false);
        AddPiece(mPiecePrefab, mSps["2x1"], POINTS2x1, new Vector2Int(1, 4), false);
        AddPiece(mPiecePrefab, mSps["2x1"], POINTS2x1, new Vector2Int(3, 2), false);
        AddPiece(mPiecePrefab, mSps["2x1"], POINTS2x1, new Vector2Int(5, 1), false);
        AddPiece(mPiecePrefab, mSps["2x1"], POINTS2x1, new Vector2Int(5, 3), false);
        AddPiece(mPiecePrefab, mSps["target"], POINTS1x2, new Vector2Int(0, 3), true, true);
    }

    private void ClearTable()
    {
        for (int i = 0; i < mTable.Length; i++)
        {
            for (int j = 0; j < mTable[0].Length; j++)
            {
                mTable[i][j] = 0;
            }
        }
    }
    private void AddPiece(GameObject obj, Sprite sp, int[][] points, Vector2Int position, bool verticle, bool isSp = false)
    {
        var objInstance = Instantiate(obj);
        objInstance.transform.parent = this.transform.Find("Area").transform;
        objInstance.transform.localPosition = new Vector3(position.x, position.y,
                                                          objInstance.transform.localPosition.z);
        objInstance.GetComponent<Draguable>().Initialize(sp);
        objInstance.GetComponent<Draguable>().allowVerticle = verticle;
        objInstance.GetComponent<Draguable>().allowHorizental = !verticle;
        mPieces.Add(new Piece(objInstance, points, position, verticle, isSp));
    }
    private Vector2Int CaculateLimitation(Piece p)
    {
        Vector2Int ret = new Vector2Int(0, 0);
        if (p.verticle)
        {
            int x = p.position.x;
            int i = 0;
            int j = p.position.y;
            for (i = x - 1; i >= 0; i--)
            {
                if (mTable[i][j] == 1) break;
            }
            i = Mathf.Max(-1, i);
            ret.x = - (p.position.x - i) + 1;
            for (i = x + p.lenght; i < mTable.Length; i++)
            {
                if (mTable[i][j] == 1) break;
            }
            i = Mathf.Min(mTable.Length, i);
            ret.y = i - p.position.x - p.lenght;
        }
        else
        {
            int y = p.position.y;
            int i = p.position.x;
            int j = 0;
            for (j = y - 1; j >= 0; j--)
            {
                if (mTable[i][j] == 1) break;
            }
            j = Mathf.Max(-1, j);
            ret.x = -(p.position.y - j) + 1;
            for (j = y + p.lenght; j < mTable.Length; j++)
            {
                if (mTable[i][j] == 1) break;
            }
            j = Mathf.Min(mTable.Length, j);
            ret.y = j - p.position.y - p.lenght;
        }
        return ret;
    }
    private int[][] POINTS1x3 = new int[][]
        {
            new int[]{1, 0, 0},
            new int[]{1, 0, 0},
            new int[]{1, 0, 0},
        };
    private int[][] POINTS1x2 = new int[][]
        {
            new int[]{1, 0, 0},
            new int[]{1, 0, 0},
            new int[]{0, 0, 0},
        };
    private int[][] POINTS2x1 = new int[][]
        {
            new int[]{1, 1, 0},
            new int[]{0, 0, 0},
            new int[]{0, 0, 0},
        };
    private int[][] POINTS3x1 = new int[][]
        {
            new int[]{1, 1, 1},
            new int[]{0, 0, 0},
            new int[]{0, 0, 0},
        };

}
