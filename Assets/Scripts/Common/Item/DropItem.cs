using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    //몬스터를 잡거나 아이템을 떨어뜨리면 생성되는 프리팹에 포함된 클래스입니다.
    //가지고 있는 아이템 정보를 저장합니다.
    public int dropMoney;
    public List<Item> dropItemList;

    private Animation _animation;
    private BoxCollider _boxCollider;
    private bool _isOpened;

    public void Awake()
    {
        dropItemList = new List<Item>();
        _animation = transform.GetChild(0).GetComponent<Animation>();
        _boxCollider = transform.GetComponent<BoxCollider>();
        _isOpened = false;
    }

    public void ItemSetting(int money, List<Item> items)
    {
        //아이템을 세팅합니다.
        Init();
        dropMoney = money;

        for (int i = 0; i < items.Count; i++)
        {
            dropItemList.Add(items[i]);
        }
    }

    private void Init()
    {
        _boxCollider.enabled = true;
        _animation.Stop("woodenchest_large_open");
        _animation.GetClip("woodenchest_large_open").SampleAnimation(_animation.gameObject, 0.0f);
        _isOpened = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (dropMoney > 0)
            {
                PlayerManager.instance.Money += dropMoney;
                UIManager.instance.GetItemUIAdd(string.Format("돈 {00}", dropMoney), 1);
                dropMoney = 0;
            }

            for (int i = 0; i < dropItemList.Count; i++)
            {
                if (!InventoryManager.instance.AddItem(dropItemList[i]))
                {
                    //지금까지 처리된 아이템을 삭제해줍니다.
                    dropItemList.RemoveRange(0, i - 1);
                    break;
                }

                UIManager.instance.GetItemUIAdd(dropItemList[i].itemInfomation.Name, dropItemList[i].count);

                //마지막 아이템 처리시
                if (i == dropItemList.Count - 1)
                    dropItemList.Clear();
            }


            if (!_animation.isPlaying && !_isOpened)
                StartCoroutine(OpenBox());

            if (dropItemList.Count <= 0)
                StartCoroutine(DisableBox());
        }
    }

    IEnumerator OpenBox()
    {
        //상자를 오픈합니다.
        _animation.Play();
        _isOpened = true;
        SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Environment_Boxopen);
        yield return new WaitForSeconds(0.5f);
        ParticleManager.instance.Play(ParticleName.ParticleName_Environment_Boxopen, transform, Vector3.zero);
    }

    IEnumerator DisableBox()
    {
        //모든 갯수가 떨어진 상자를 일정시간 후 비활성화 합니다.
        _boxCollider.enabled = false;
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
    }
}
