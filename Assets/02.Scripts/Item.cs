using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/*
 *  아이템 클래스 (스크립터블 오브젝트)
 */

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public string item_name;                      // 아이템 이름
    public EItemType item_type;                  // 아이템 타입 (장비, 소비)
    public Sprite item_sprite;                    // 아이템 스프라이트
    public bool is_stackable;                     // 아이템을 쌓을 수 있는지 체크
    public bool is_material;                      // 재료 아이템인지 체크
}
