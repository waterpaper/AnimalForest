using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    //몬스터를 잡거나 아이템을 떨어뜨리면 생성되는 프리팹에 포함된 클래스입니다.
    //가지고 있는 아이템 정보를 저장합니다.
    public int dropMoney;
    public List<Item> dropItems;
    public Animation animation;
    public bool isOpened;

    public void Awake()
    {
        dropItems = new List<Item>();
        animation = transform.GetChild(0).GetComponent<Animation>();
        isOpened = false;
    }

    public void ItemSetting(int money, List<Item> items)
    {
        dropMoney = money;

        for (int i = 0; i < items.Count; i++)
        {
            dropItems.Add(items[i]);
        }
        transform.GetComponent<BoxCollider>().enabled = true;
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

            for (int i = 0; i < dropItems.Count; i++)
            {
                if (!InventoryManager.instance.AddItem(dropItems[i]))
                {
                    dropItems.RemoveRange(0, i - 1);
                    break;
                }

                UIManager.instance.GetItemUIAdd(dropItems[i].itemInfomation.Name, dropItems[i].count);

                if (i == dropItems.Count - 1)
                {
                    dropItems.Clear();

                    transform.GetComponent<BoxCollider>().enabled = false;
                }
            }

           
            if (!animation.isPlaying && isOpened != true)
            {
                animation.Play();
                SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Environment_Boxopen);
                StartCoroutine(ParticleEffect());
                isOpened = true;
            }

            if (dropItems.Count <= 0)
            {
                StartCoroutine(ActiveFalseBox());
            }
        }
    }

    IEnumerator ParticleEffect()
    {
        yield return new WaitForSeconds(0.5f);
        ParticleManager.instance.Play( ParticleName.ParticleName_Environment_Boxopen,transform, Vector3.zero);
    }

    IEnumerator ActiveFalseBox()
    {
        yield return new WaitForSeconds(3.0f);
        animation.Stop("woodenchest_large_open");
        animation.GetClip("woodenchest_large_open").SampleAnimation(animation.gameObject,0.0f);
        isOpened = false;
        this.gameObject.SetActive(false);
    }
}
