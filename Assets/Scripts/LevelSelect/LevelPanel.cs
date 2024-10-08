using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    int stageIndex;
    public Image StageThumb;
    public TMP_Text TextTitle;

    public void SetLevelInformation(int stageIndex, Sprite thumbnail, string title) 
    {
        StageThumb.sprite = thumbnail;
        TextTitle.text = title;
        this.stageIndex = stageIndex;
    }

    public void StageStart() // ��ư Ŭ�� �� Ȱ��ȭ OnClick
    {
        LevelManager.Instance.StartLevel(stageIndex);
    }
}
