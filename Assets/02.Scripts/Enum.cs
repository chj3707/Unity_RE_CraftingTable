using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 타입
public enum E_ItemType
{
    None = -1,

    BuildingBlocks,        // 건축 블록
    DecorationBlocks,      // 장식 블록
    RedStone,              // 레드스톤 관련
    Transportation,        // 수송 관련
    Miscellaneous,         // 기타 아이템
    Foodstuffs,            // 식료품
    Tools,                 // 도구
    Combat,                // 전투

    Max
}
