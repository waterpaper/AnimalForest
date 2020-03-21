using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    [Header("QuickSlotKind")]
    public QuickSlot_Kind Kind;
    // 포션을 사용하기 위한 기준 퍼센트를 의미합니다.
    [Header("UsingPercent")]
    public float usingPercent = 0.5f;

    private void FixedUpdate()
    {
        if (Kind == QuickSlot_Kind.QuickSlot_Kind_HP)
        {
            //hp포션 아이템이 업는 경우 실행하지 않습니다.
            if (InventoryManager.instance.NowEquipmentHpPotionItem() == false) return;
            //기준 퍼센트이상의 체력시 해당 함수를 실행하지 않습니다.
            if (PlayerManager.instance.Hp / PlayerManager.instance.HpMax > usingPercent) return;

            int hpPotionInventoryNumberTemp = InventoryManager.instance.GetEquipmentPotionItemInventoryNumber(UseItem.UseItemType.HpPotion);
            InventoryManager.instance.UseingItem(hpPotionInventoryNumberTemp);
            SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_PlayerPotion);
        }

        if (Kind == QuickSlot_Kind.QuickSlot_kind_MP)
        {
            //mp포션 아이템이 업는 경우 실행하지 않습니다.
            if (InventoryManager.instance.NowEquipmentMpPotionItem() == false) return;
            if (PlayerManager.instance.Mp / PlayerManager.instance.MpMax > usingPercent) return;

            int mpPotionInventoryNumberTemp = InventoryManager.instance.GetEquipmentPotionItemInventoryNumber(UseItem.UseItemType.MpPotion);
            InventoryManager.instance.UseingItem(mpPotionInventoryNumberTemp);
            SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_PlayerPotion);
        }
    }
}
