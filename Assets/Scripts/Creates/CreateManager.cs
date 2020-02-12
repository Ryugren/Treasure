using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
public static class StageCreate
{
    [MenuItem("Tools/StageCreate")]
    public static void Create()
    {
        CreateManager.CreateStage();
    }
}
public class CreateManager : MonoBehaviour
{
    //ステージ生成全行程
    public static void CreateStage()
    {
        string[][] csvDatas;
        //配置情報読み込み
        csvDatas = LoadCSV();
        //配置開始
        Mapping(csvDatas);
    }
    //CSVデータ読み込み
    private static string[][] LoadCSV()
    {
        TextAsset csvFile = Resources.Load("Mapping") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        List<string[]> csvData = new List<string[]>();
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(','));
        }
        return csvData.ToArray();
    }
    //ステージ生成の実行部
    private static void Mapping(string[][] datas)
    {
        Road[][] objects = new Road[datas.Length][];
        //縦
        for (int y = 0; y < datas.Length; ++y)
        {
            //横
            objects[y] = new Road[datas[y].Length];
            for (int x = 0; x < datas[y].Length; ++x)
            {
                //インスタンス
                objects[y][x] = SetObject(datas[y][x]);
            }
        }
        //配置
        FormMap(objects);
    }
    //道の生成&道の情報収集
    private static Road SetObject(string data)
    {
        if (string.IsNullOrEmpty(data)) return null;
        //反転
        bool reverseFlag = Equals('^', data[0]);
        //オブジェクトの種類
        int objectNumber = reverseFlag ? 
            int.Parse(data.Substring(1, 1)) :
            int.Parse(data.Substring(0, 1));
        GameObject prefab;
        switch (objectNumber)
        {
            case 1:
                prefab = (GameObject)Resources.Load("Prefabs\\Curve");
                break;
            case 2:
                prefab = (GameObject)Resources.Load("Prefabs\\Straight");
                break;
            case 3:
                prefab = (GameObject)Resources.Load("Prefabs\\Square");
                break;
            case 4:
                prefab = (GameObject)Resources.Load("Prefabs\\Branch");
                break;
            case 5:
                prefab = (GameObject)Resources.Load("Prefabs\\DeadEnd");
                break;
            default:
                return null;
        }
        //向き
        Vector3 direction;
        char word = reverseFlag ? data[2] : data[1];
        int dirN = 0;
        switch (word)
        {
            case 'n':
            case 'N':
                direction = new Vector3(0, 0, 0);
                dirN = 0;
                break;
            case 'e':
            case 'E':
                direction = new Vector3(0, 90, 0);
                dirN = 1;
                break;
            case 's':
            case 'S':
                direction = new Vector3(0, 180, 0);
                dirN = 2;
                break;
            case 'w':
            case 'W':
                direction = new Vector3(0, 270, 0);
                dirN = 3;
                break;
            default:
                direction = new Vector3(0, 0, 0);
                break;
        }
        GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.Euler(direction));
        if (reverseFlag)
        {
            obj.transform.localScale = Vector3.Reflect(obj.transform.localScale, Vector3.right);
        }
        Road road = new Road(obj, dirN, reverseFlag);
        return road;
    }
    //道の情報
    public class Road
    {
        public Road(GameObject mySelf, int direction, bool reverse)
        {
            MySelf = mySelf;
            if (mySelf.transform.Find("AnchorN") != null)
                Anchors.Add(new AnchorParameter(mySelf.transform.Find("AnchorN").gameObject, direction, 'N', reverse));
            if (mySelf.transform.Find("AnchorE") != null)
                Anchors.Add(new AnchorParameter(mySelf.transform.Find("AnchorE").gameObject, direction, 'E', reverse));
            if (mySelf.transform.Find("AnchorS") != null)
                Anchors.Add(new AnchorParameter(mySelf.transform.Find("AnchorS").gameObject, direction, 'S', reverse));
            if (mySelf.transform.Find("AnchorW") != null)
                Anchors.Add(new AnchorParameter(mySelf.transform.Find("AnchorW").gameObject, direction, 'W', reverse));
        }
        public bool IsChecked { get; set; } = false;
        public GameObject MySelf { get; private set; }
        public List<AnchorParameter> Anchors { get; private set; } = new List<AnchorParameter>();
    }
    //アンカーの情報
    public class AnchorParameter
    {
        public AnchorParameter(GameObject mySelf, int mySelfDir, char anchorDir, bool reverse)
        {
            int dir = 0;
            switch (anchorDir)
            {
                case 'N':
                    dir = 0;
                    break;
                case 'E':
                    dir = 1;
                    break;
                case 'S':
                    dir = 2;
                    break;
                case 'W':
                    dir = 3;
                    break;
            }
            if (reverse)
            {
                if (mySelfDir % 2 == 1)
                {
                    mySelfDir = (mySelfDir + 2) % 4;
                }
                mySelfDir = (mySelfDir + 3 * dir) % 4;
            }
            else
            {
                mySelfDir = (mySelfDir + dir) % 4;
            }

            MySelf = mySelf;
            Direction = (Directions)mySelfDir;
        }
        public enum Directions
        {
            North,
            East,
            South,
            West
        }
        public GameObject MySelf { get; private set; }
        public Directions Direction { get; private set; }
    }
    //ステージの整理&保存
    private static void FormMap(Road[][] parts)
    {
        //初期化
        GameObject parent;
        parent = GameObject.Find("FieldObject");
        if (parent != null)
        {
            DestroyImmediate(parent);
        }
        parent = new GameObject("FieldObject");
        //親子関係
        for (int y = 0; y < parts.Length; ++y)
        {
            for (int x = 0; x < parts[y].Length; ++x)
            {
                if (parts[y][x] != null)
                    parts[y][x].MySelf.transform.SetParent(parent.transform, false);
            }
        }
        //配置開始
        Setting(Vector3.zero, Vector2Int.zero, parts);
        for (int y = 0; y < parts.Length; ++y)
        {
            for (int x = 0; x < parts[y].Length; ++x)
            {
                if (parts[y][x] != null)
                {
                    if (!parts[y][x].IsChecked)
                    {
                        DestroyImmediate(parts[y][x].MySelf);
                    }
                    else
                    {
                        for (int i = 0; i < parts[y][x].Anchors.Count; ++i)
                        {
                            DestroyImmediate(parts[y][x].Anchors[i].MySelf);
                        }
                    }
                }
            }
        }
        //変更情報を保存
        Undo.RegisterCreatedObjectUndo(parent, "Create FieldObject");
    }
    //道同士の接続
    private static void Setting(Vector3 startPos, Vector2Int id, Road[][]parts)
    {
        Road mySelf = parts[id.y][id.x];
        mySelf.IsChecked = true;
        mySelf.MySelf.transform.position = startPos;
        //アンカーを選ぶ
        foreach (AnchorParameter myAnchor in mySelf.Anchors)
        {
            Road yourSelf = null;
            AnchorParameter yourAnchor = null;
            Vector2Int yourId = Vector2Int.zero;
            //隣の道の反対のアンカーを探す
            switch (myAnchor.Direction)
            {
                case AnchorParameter.Directions.North:
                    if (id.y == 0) break;
                    yourId = new Vector2Int(id.x, id.y - 1);
                    if (parts[yourId.y][yourId.x] != null)
                        yourAnchor = parts[yourId.y][yourId.x].Anchors.Find(a => a.Direction == AnchorParameter.Directions.South);
                    break;
                case AnchorParameter.Directions.East:
                    if (id.x == parts[id.y].Length - 1) break;
                    yourId = new Vector2Int(id.x + 1, id.y);
                    if (parts[yourId.y][yourId.x] != null)
                        yourAnchor = parts[yourId.y][yourId.x].Anchors.Find(a => a.Direction == AnchorParameter.Directions.West);
                    break;
                case AnchorParameter.Directions.South:
                    if (id.y == parts.Length - 1) break;
                    yourId = new Vector2Int(id.x, id.y + 1);
                    if (parts[yourId.y][yourId.x] != null)
                        yourAnchor = parts[yourId.y][yourId.x].Anchors.Find(a => a.Direction == AnchorParameter.Directions.North);
                    break;
                case AnchorParameter.Directions.West:
                    if (id.x == 0) break;
                    yourId = new Vector2Int(id.x - 1, id.y);
                    if (parts[yourId.y][yourId.x] != null)
                        yourAnchor = parts[yourId.y][yourId.x].Anchors.Find(a => a.Direction == AnchorParameter.Directions.East);
                    break;
            }
            //対象が見つかったら
            if (yourAnchor != null)
            {
                yourSelf = parts[yourId.y][yourId.x];
                //移動済みでないなら、そこから伸ばす
                if (!yourSelf.IsChecked)
                {
                    Vector3 mySelfVec = myAnchor.MySelf.transform.position - mySelf.MySelf.transform.position;
                    Vector3 yourSelfVec = yourSelf.MySelf.transform.position - yourAnchor.MySelf.transform.position;
                    Setting(startPos + mySelfVec + yourSelfVec, yourId, parts);
                }
            }
        }
        return;
    }
}
#endif
