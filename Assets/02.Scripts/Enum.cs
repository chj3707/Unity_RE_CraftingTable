using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 타입
public enum EItemType
{
    none = -1,

    building_blocks,       // 건축 블록
    decoration_blocks,     // 장식 블록
    redstone,              // 레드스톤 관련
    transportation,        // 수송 관련
    miscellaneous,         // 기타 아이템
    foodstuffs,            // 식료품
    tools,                 // 도구
    combat,                // 전투

    max
}
