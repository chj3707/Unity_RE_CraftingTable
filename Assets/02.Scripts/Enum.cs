using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ Ÿ��
public enum EItemType
{
    none = -1,

    building_blocks,       // ���� ���
    decoration_blocks,     // ��� ���
    redstone,              // ���彺�� ����
    transportation,        // ���� ����
    miscellaneous,         // ��Ÿ ������
    foodstuffs,            // �ķ�ǰ
    tools,                 // ����
    combat,                // ����

    max
}

public enum ECommonMaterial
{
    none = -1,

    Log,
    Plank,

    max
}

public enum ENonConsumeMaterialItem
{
    none = -1,

    Bucket,

    max
}