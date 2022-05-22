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
    public string m_ItemName;                      // 아이템 이름
    public E_ItemType m_ItemType;                  // 아이템 타입 (장비, 소비)
    public Sprite m_ItemSprite;                    // 아이템 스프라이트
    public bool m_IsStackable;                     // 아이템을 쌓을 수 있는지 체크
    public bool m_IsMaterial;                      // 재료 아이템인지 체크
}
