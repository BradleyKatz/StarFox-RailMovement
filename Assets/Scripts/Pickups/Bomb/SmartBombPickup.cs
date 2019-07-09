using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SmartBombPickup : PickupBase
{
    protected override void OnPickupGrabbedAnimation(PlayerManager player)
    {
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0.0f, -1.0f, 0.0f);

        Sequence s = DOTween.Sequence();
        s.Append(transform.DORotate(new Vector3(-90.0f, 0.0f, 0.0f), 0.2f));
        s.Append(transform.DORotate(new Vector3(0.0f, 0.0f, -900.0f), 3.0f, RotateMode.LocalAxisAdd)); s.Join(transform.DOScale(0.0f, 0.5f).SetDelay(1.0f));
        s.AppendCallback(() => Destroy(gameObject));
    }

    protected override void OnPickupGrabbedEffect(PlayerManager player)
    {

    }
}
