using UnityEngine;

public class EnemyStatement : MonoBehaviour
{
    //에너미의 스텟을 저장합니다.
    public int id = 0;
    public string name = "";
    public string explanation = "";
    public int level = 1;
    public float hp = 100.0f;
    public float hpMax = 100.0f;
    public float mp = 100.0f;
    public float mpMax = 100.0f;
    public float atk = 10.0f;
    public float def = 0.0f;
    public int exp = 0;

    public float patrolMoveSpeed = 3.0f;
    public float traceMoveSpeed = 5.0f;

    public void Start()
    {
        EnemyTable temp = DataManager.instance.EnemyInfo(id);
        Setting(temp);
    }

    private void OnEnable()
    {
        hp = hpMax;
        mp = mpMax;
    }

    void Setting(EnemyTable temp)
    {
        name = temp.Name;
        explanation = temp.Explanation;
        level = temp.Level;
        hp = temp.Hp;
        hpMax = temp.HpMax;
        mp = temp.Mp;
        mpMax = temp.MpMax;
        atk = temp.Atk;
        def = temp.Def;
        exp = temp.Exp;
    }
}
