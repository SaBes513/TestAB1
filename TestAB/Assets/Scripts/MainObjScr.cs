using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class LinkMO
{
    static int NIslP = 0, NEmptyIsl, NEnemOnIsl, SecWayPoint = 0;
    static MainObjScr LinkMain;
    static PlayerScr MainPlayer;
    static WayPointScr WayPoints;
    static List<EnemyScr> AllEnem = new List<EnemyScr>();
    static List<IslWayPointScr> AllWP = new List<IslWayPointScr>();
    public static Transform StartPos;
    public static bool move;

    public static void SetMain(MainObjScr MO)
    {
        LinkMain = MO;
    }
    public static MainObjScr GetMain()
    {
        return LinkMain;
    }
    public static void SetPlayer(PlayerScr MP)
    {
        MainPlayer = MP;
    }
    public static PlayerScr GetPlayer()
    {
        return MainPlayer;
    }
    public static void SetWayPoints(WayPointScr WP)
    {
        WayPoints = WP;
        if (WP != null)
        {
            for (int i = 0; i < WayPoints.ArrayWayPoints.Length; i++)
            {
                AllWP.Add(WayPoints.ArrayWayPoints[i].GetComponent<IslWayPointScr>());
            }
            StartPos = AllWP[0].transform;
        }
    }
    public static void Restart()
    {
        SetPlayer(null);
        SetMain(null);
        SetWayPoints(null);
        AllEnem.Clear();
        AllWP.Clear();
    }

    public static WayPointScr GetWayPoints()
    {
        return WayPoints;
    }

    public static int GetSecWayPoint()
    {
        return SecWayPoint;
    }
    public static void SetSecWayPoint(int Id)
    {
        SecWayPoint = Id;
    }
    public static Transform GetSecPosWayPoint()
    {
        return AllWP[SecWayPoint].transform;
    }

    public static Transform GetNextPosWayPoint()
    {
        if (SecWayPoint + 1 < AllWP.Count)
            return AllWP[SecWayPoint + 1].transform;
        else
        {
            return AllWP[AllWP.Count - 1].transform;
        }
    }

    public static void NextWayPoint()
    {
        SecWayPoint++;
    }

    public static int AddEnem(EnemyScr Enemy)
    {
        AllEnem.Add(Enemy);
        return AllEnem.Count - 1;
    }
    public static void DelEnem(int IdEnemy)
    {
        if (AllEnem.Count > 0 && AllEnem.Count > IdEnemy)
        {
            AllEnem.RemoveAt(IdEnemy);
            for (int i = IdEnemy; i < AllEnem.Count; i++)
            {
                AllEnem[i].ChangeId(i);
            }
        }
        CheckIsl(false);
    }

    public static void CheckIsl(bool AimHelp)
    {
        if (AllWP.Count > 0)
        {
            NIslP = AllWP[SecWayPoint].NIsland;
            NEmptyIsl = 0;
            NEnemOnIsl = 0;
            for (int i = 0; i < AllEnem.Count; i++)
            {
                if (AllEnem[i].NIsland == NIslP) { NEmptyIsl++; NEnemOnIsl = i; }
            }
            if (NEmptyIsl > 0)
            {
                if (!AimHelp)
                {
                    LinkMain.DrawCountEnemy(NEmptyIsl);
                }
                else
                {
                    GetPlayer().Rotate(AllEnem[NEnemOnIsl].transform.position);
                }
                LinkMain.ButtAIM.gameObject.SetActive(true);
            }
            else
            {
                MainPlayer.PlrAnimator.SetTrigger("Walk");
                NextCheckpoint();
                move = true;
                LinkMain.DrawCountEnemy(0);
                LinkMain.ButtAIM.gameObject.SetActive(false);
            }
        }
    }

    public static void NextCheckpoint()
    {
        if (SecWayPoint < AllWP.Count - 2)
        {
            if (AllWP[SecWayPoint + 1].NIsland > NIslP)
            {
                //CheckIsl(false);
            }
            else
            {
                //SecWayPoint++;
            }
        }
        else
        {
            LinkMain.ButtPlay.gameObject.SetActive(false);
            LinkMain.ButtAIM.gameObject.SetActive(false);
            LinkMain.ButtRestart.gameObject.SetActive(true);
        }
    }

    public static void SetPlayerPos(Transform Pos) 
    {
        MainPlayer.transform.position = Pos.position;
    }

    public static void checkRad(Vector3 pos, float Damage, float Dist) 
    {
        List<EnemyScr> DamEnem = new List<EnemyScr>();
        for (int i = 0; i < AllEnem.Count; i++)
        {
            if (Vector3.Distance(pos, AllEnem[i].transform.position) < Dist)
            {
                DamEnem.Add(AllEnem[i]);
            }
        }
        for (int i = 0; i < DamEnem.Count; i++)
        {
            DamEnem[i].TakeDamage(Damage);
        }
        
    }

}

public class MainObjScr : MonoBehaviour
{
    [System.Serializable]
    public struct Weapon
    {
        public string Name;
        public float Damage, BulletSpeed, BulletMaxSpeed, BulletAccSpeed, BulletBoomRad, BoomDamage;
    }
    public byte CountBulletInPool = 20;
    public Weapon[] Weapons;
    public Text TextEnemyCount;
    public Button ButtRestart;
    public Button ButtPlay;
    public Button ButtAIM;
    public int CountIsl;
    public GameObject BulletPrefab;
    GameObject[] BulletPool;
    BulletScr[] BulletVars;
    int bulletId = 0;
    public float Sec;

    void Start()
    {
        LinkMO.SetMain(this);
        LinkMO.SetPlayerPos(LinkMO.StartPos);
        LinkMO.GetPlayer().Rotate(LinkMO.GetNextPosWayPoint().position);
        BulletPool = new GameObject[CountBulletInPool];
        BulletVars = new BulletScr[CountBulletInPool];
        for (int i = 0; i < CountBulletInPool; i++)
        {
            BulletPool[i] = Instantiate(BulletPrefab,transform);
            BulletVars[i] = BulletPool[i].GetComponent<BulletScr>();
        }
    }

    public void DrawCountEnemy(int Count)
    {
        if(!ButtRestart.gameObject.activeSelf)
            TextEnemyCount.text = "Count: " + Count;
        else
            TextEnemyCount.text = "Score: " + Sec;
    }

    public void CheckIsl()
    {
        Sec += 10;
        LinkMO.CheckIsl(true);
    }

    public void PlayGame()
    {
        LinkMO.SetSecWayPoint(0);
        LinkMO.SetPlayerPos(LinkMO.StartPos);
        LinkMO.GetPlayer().Rotate(LinkMO.GetNextPosWayPoint().position);
        LinkMO.CheckIsl(false);
        ButtPlay.gameObject.SetActive(false);
        ButtAIM.gameObject.SetActive(false);
        ButtRestart.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        LinkMO.Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CreateBullet(Vector3 V3From, Quaternion Dir)
    {
        BulletPool[bulletId].transform.position = V3From;
        BulletPool[bulletId].transform.rotation = Dir;
        BulletVars[bulletId].SetVars(
            Weapons[LinkMO.GetPlayer().WeaponID].Damage,
            Weapons[LinkMO.GetPlayer().WeaponID].BulletSpeed,
            Weapons[LinkMO.GetPlayer().WeaponID].BulletMaxSpeed,
            Weapons[LinkMO.GetPlayer().WeaponID].BulletAccSpeed,
            Weapons[LinkMO.GetPlayer().WeaponID].BulletBoomRad,
            Weapons[LinkMO.GetPlayer().WeaponID].BoomDamage);
        if (++bulletId >= CountBulletInPool)
        {
            bulletId = 0;
        }
    }
}
