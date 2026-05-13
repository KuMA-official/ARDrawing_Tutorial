using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    public GameObject linePrefab;       // 描画する線のプレハブ
    public OVRHand rightHand;           // 右手のOVRHand
    public Transform indexTip;          // 人差し指の先端のTransform

    private LineRenderer currentLine;   // 現在引いている線
    private Vector3 lastPosition;       // 最後に点を打った位置

    void Update()
    {
        // 手が認識されていない時は処理をしない
        if (!rightHand.IsTracked) return;

        // 【判定】親指と人差し指がくっついているか（ピンチしているか）
        bool isPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (isPinching)
        {
            // 人差し指の先端の位置を取得
            Vector3 indexTipPos = indexTip.position;

            // くっついた瞬間：新しい線を作る
            if (currentLine == null)
            {
                GameObject newLine = Instantiate(linePrefab);
                currentLine = newLine.GetComponent<LineRenderer>();

                currentLine.positionCount = 1;
                currentLine.SetPosition(0, indexTipPos);
                lastPosition = indexTipPos;
            }
            // くっついたまま移動中：指が少し動いたら点を追加して線を伸ばす
            else if (Vector3.Distance(lastPosition, indexTipPos) > 0.005f)
            {
                currentLine.positionCount++;
                currentLine.SetPosition(currentLine.positionCount - 1, indexTipPos);
                lastPosition = indexTipPos;
            }
        }
        else
        {
            // 離した時：現在の線の記憶を消して、次に新しく線を作れるようにする
            currentLine = null;
        }
    }
}