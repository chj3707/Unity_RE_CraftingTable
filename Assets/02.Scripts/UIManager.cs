using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  UI 관리용 스크립트
 */

public class UIManager : Singleton_Mono<UIManager>
{
    // 사용처 :: 아이템 추가 버튼
    public void GenerateObject(GameObject p_object, Sprite p_sprite, string p_name, Transform p_parent)
    {
        GameObject copyObj = GameObject.Instantiate(p_object);        // 복사
        copyObj.SetActive(true);                                      // 활성화
        copyObj.GetComponent<Image>().sprite = p_sprite;              // 스프라이트 설정
        copyObj.name = string.Format($"{p_name}");                    // 이름 설정
        copyObj.transform.parent = p_parent;                          // 부모 오브젝트 설정
    }

    // 사용처 :: 슬롯
    public void GenerateObject(GameObject p_object, int p_copyCnt, string p_name, Transform p_parent)
    {
        for (int i = 0; i < p_copyCnt; i++)
        {
            GameObject copyObj = GameObject.Instantiate(p_object);    // 복사
            copyObj.SetActive(true);                                  // 활성화
            copyObj.name = string.Format($"{p_name}_{i + 1} ");       // 이름 설정
            copyObj.transform.parent = p_parent;                      // 부모 오브젝트 설정
        }
    }
}
