using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class RingPickupBase : PickupBase
{
    protected override void OnPickupGrabbedAnimation(PlayerManager player)
    {
        transform.parent = player.transform.parent;

        Sequence s = DOTween.Sequence();
        s.Append(transform.DORotate(Vector3.zero, .2f));
        s.Append(transform.DORotate(new Vector3(0, 0, -900), 3, RotateMode.LocalAxisAdd));s.Join(transform.DOScale(0, .5f).SetDelay(1f));
        s.AppendCallback(() => Destroy(gameObject));
    }
}
