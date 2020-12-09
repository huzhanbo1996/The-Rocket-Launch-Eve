using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class QuizPointsSupression : MonoBehaviour, IQuizSerializable
{
    public float mZeroX;
    public float mZeroY;
    public float mParamDistance;
    public List<float> mXs;
    public List<float> mYs;
    public List<float> mDs;
    public GameObject mBtnPrefab;
    public GameObject mLinePrefab;
    public List<Sprite> mSpArea;
    public int mStage  = 1;

    private class Point
    {
        public GameObject obj;
        public Vector2Int position;
        public int remainPoints;

        public Point(GameObject obj, Vector2Int p, int r)
        {
            this.obj = obj;
            this.position = p;
            this.remainPoints = r;
        }
    }
    private class Line
    {
        public GameObject lineRender;
        public Vector2 p1;
        public Vector2 p2;

        public Line(Vector2 p1, Vector2 p2, GameObject lineRender)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p1 += (p2 - p1) * 0.1f;
            this.p2 += (p1 - p2) * 0.1f;
            this.lineRender = lineRender;
        }

        private float CrossProduct(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        private Vector2 VectorConstruct(Vector2 A, Vector2 B)
        {
            Vector2 v;
            v.x = B.x - A.x;
            v.y = B.y - A.y;
            return v;
        }

        public bool IsIntersection(Line other)
        {
            Vector2 AC = VectorConstruct(this.p1, other.p1);
            Vector2 CB = VectorConstruct(other.p1, this.p2);
            Vector2 BD = VectorConstruct(this.p2, other.p2);
            Vector2 DA = VectorConstruct(other.p2, this.p1);

            float[] c = new float[4];
            c[0] = CrossProduct(AC, CB);
            c[1] = CrossProduct(CB, BD);
            c[2] = CrossProduct(BD, DA);
            c[3] = CrossProduct(DA, AC);

            int f1 = 0, f2 = 0; // 计算正数，负数的个数
            int i;
            for (i = 0; i < 4; i++)
            {
                if (c[i] > 0) f1++;
                if (c[i] < 0) f2++;
            }

            if (f1 > 0 && f2 > 0)   // 有正，有负，返回无交集
                return false;
            else
                return true;
        }
    }
    private Point mPickedPoint;

    private float mSpriteDistance;
    private string RESOURCES_PATH = "QuizPointsSupression";
    private List<Sprite> mSps = new List<Sprite>();
    private List<Point> mPoints = new List<Point>();
    private List<Line> mLines = new List<Line>();
    // Start is called before the first frame update
    void Start()
    {
        // mStage = 1;
        int i;
        for (i = 0; i <= 6; i++)
        {
            string spriteName = i.ToString().ToUpper();
            Debug.Log(RESOURCES_PATH + "/" + spriteName);
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + spriteName)) as Sprite;
            mSps.Add(sp);
            //mSpriteDistance = sp.textureRect.width / sp.pixelsPerUnit * mParamDistance;
            mSpriteDistance = mParamDistance;
        }
        Debug.Log("mSps" + mSps.Count.ToString());
        Clear();
        Set(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (mStage - 1 < 0 || mStage - 1 >= mXs.Count) return;
        mZeroX = mXs[mStage - 1];
        mZeroY = mYs[mStage - 1];
        mSpriteDistance = mParamDistance = mDs[mStage - 1];
        var mAreaSpR = this.transform.Find("Area").GetComponent<SpriteRenderer>();
        if (mStage - 1 >= 0 && mStage - 1 < mSpArea.Count)
        {
            mAreaSpR.sprite = mSpArea[mStage - 1];
        }
        Render();
        foreach(var p in mPoints)
        {
            if(Common.Utils.ClickedOn(p.obj))
            {
                if (mPickedPoint == null)
                {
                    if(p.remainPoints > 0) mPickedPoint = p;
                    else                   mPickedPoint = null;
                }
                else if(mPickedPoint != p)
                {
                    var lr = Instantiate(mLinePrefab);
                    lr.transform.parent = this.transform;
                    LineRenderer render = lr.GetComponent<LineRenderer>();
                    render.positionCount = 2;
                    render.SetPosition(0, 
                        new Vector3(mZeroX + mPickedPoint.position.x * mSpriteDistance,
                                    mZeroY + mPickedPoint.position.y * mSpriteDistance,
                                    -1));
                    render.SetPosition(1,
                        new Vector3(mZeroX + p.position.x * mSpriteDistance,
                                    mZeroY + p.position.y * mSpriteDistance,
                                    -1));  

                    Line newLine = new Line(mPickedPoint.position, p.position, lr);
                    bool noIntersection = true;
                    foreach(var l in mLines)
                    {
                        if (newLine.IsIntersection(l)) noIntersection = false;
                    }
                    if(noIntersection)
                    {
                        mLines.Add(newLine);
                        mPickedPoint.remainPoints--;
                        p.remainPoints--;
                    }
                    else
                    {
                        Destroy(lr);
                    }
                    mPickedPoint = null;
                }
                if(Common.Utils.ClickedAnywhereOut(p.obj))
                {
                    mPickedPoint = null;
                }
            }
        }
        int cntRemains = 0;
        foreach(var p in mPoints)
        {
            cntRemains += p.remainPoints;
        }
        if(cntRemains == 0)
        {
            mStage++;
            if (mStage <= 3)
            {
                FindObjectOfType<SoundEffect>().Play(SoundEffect.SOUND_TYPE.QUIZ);
                Clear();
                Set(mStage);
            }
            else if(mStage == 4)
            {
                Clear();
                FindObjectOfType<ComputerSwitch>().QuizResolved();
            }
        }
    }

    public void OnEnable()
    {
        if (mStage == 0) return;
        mPickedPoint = null;
        Clear();
        Set(mStage);
    }

    private void Render()
    {
        foreach(var Point in mPoints)
        {
            Point.obj.transform.localPosition =
                new Vector3(mZeroX + Point.position.x * mSpriteDistance,
                            mZeroY + Point.position.y * mSpriteDistance,
                            Point.obj.transform.localPosition.z);
            Point.obj.GetComponent<SpriteRenderer>().sprite = mSps[Point.remainPoints];
        }
    }
    private void Set(int part)
    {
        Debug.Assert(part <= 3);
        if (part == 1)
        {
            mPoints.Add(NewPoint(new Vector2Int(5, 2), 3));
            mPoints.Add(NewPoint(new Vector2Int(5, 4), 2));
            mPoints.Add(NewPoint(new Vector2Int(3, 3), 4));
            mPoints.Add(NewPoint(new Vector2Int(3, 5), 3));
            mPoints.Add(NewPoint(new Vector2Int(1, 2), 2));
            mPoints.Add(NewPoint(new Vector2Int(1, 4), 2));
        }
        else if(part == 2)
        {
            mPoints.Add(NewPoint(new Vector2Int(7, 4), 1));
            mPoints.Add(NewPoint(new Vector2Int(6, 1), 4));
            mPoints.Add(NewPoint(new Vector2Int(3, 5), 2));
            mPoints.Add(NewPoint(new Vector2Int(2, 1), 3));
            mPoints.Add(NewPoint(new Vector2Int(1, 3), 2));
            mPoints.Add(NewPoint(new Vector2Int(1, 6), 4));
        }
        else if(part == 3)
        {
            mPoints.Add(NewPoint(new Vector2Int(7, 3), 6));
            mPoints.Add(NewPoint(new Vector2Int(7, 7), 3));
            mPoints.Add(NewPoint(new Vector2Int(6, 5), 3));
            mPoints.Add(NewPoint(new Vector2Int(4, 2), 3));
            mPoints.Add(NewPoint(new Vector2Int(3, 6), 5));
            mPoints.Add(NewPoint(new Vector2Int(2, 0), 4));
            mPoints.Add(NewPoint(new Vector2Int(2, 4), 5));
            mPoints.Add(NewPoint(new Vector2Int(0, 5), 3));
        }
        
    }
    private void Clear()
    {
        foreach(var p in mPoints)
        {
            DestroyImmediate(p.obj);
        }
        foreach(var l in mLines)
        {
            DestroyImmediate(l.lineRender);
        }
        mPoints.Clear();
        mLines.Clear();
    }

    private Point NewPoint(Vector2Int p, int r)
    { 
        var obj = Instantiate(mBtnPrefab);
        obj.GetComponent<SpriteRenderer>().sprite = mSps[r];
        obj.transform.parent = this.transform;
        return new Point(obj, p, r);
    }

    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mIntData.Add(mStage);
        return ret;
    }

    public void Deserialize(QuizData data)
    {
        mStage = data.mIntData[0];
        
    }
}
