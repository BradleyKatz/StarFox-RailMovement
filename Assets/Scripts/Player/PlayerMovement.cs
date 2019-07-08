using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    private Transform playerModel;

    [Header("Settings")]
    public bool joystick = true;

    [Space]

    [Header("Player Input Parameters")]
    public float xySpeed = 18;
    public float lookSpeed = 340;
    public float forwardSpeed = 6;

    [Space]
    [Range(0.0f, 1.0f), Tooltip("The amount of time the player has to input the double tap sequence for Barrel Roll")]
    public float barrelRollInputWindow = 0.3f;
    private float barrelRollInputWindowTimeElapsed = 0.0f;
    private int lTapCount = 0;
    private int rTapCount = 0;

    [Range(0.0f, 1.0f), Tooltip("The speed at which the player rotates into their half-spin")]
    public float halfSpinSpeed = 0.6f;
    private bool isInHalfSpin = false;

    [Header("Public References")]
    public Transform aimTarget;
    public CinemachineDollyCart dolly;
    public Transform cameraParent;

    [Space]

    [Header("Particles")]
    public ParticleSystem trail;
    public ParticleSystem circle;
    public ParticleSystem barrel;
    public ParticleSystem stars;

    private Tween currentHalfRotationTween = null;

    void Start()
    {
        playerModel = transform.GetChild(0);
        SetSpeed(forwardSpeed);
    }

    void Update()
    {
        float h = joystick ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
        float v = joystick ? Input.GetAxis("Vertical") : Input.GetAxis("Mouse Y");

        LocalMove(h, v, xySpeed);

        if (!isInHalfSpin)
        {
            RotationLook(h, v, lookSpeed);
            HorizontalLean(playerModel, h, 80, .1f);
        }

        if (Input.GetButtonDown("Action"))
            Boost(true);

        if (Input.GetButtonUp("Action"))
            Boost(false);

        if (Input.GetButtonDown("Fire3"))
            Break(true);

        if (Input.GetButtonUp("Fire3"))
            Break(false);

        // Barrel Roll Update
        {
            if (lTapCount > 0 || rTapCount > 0)
            {
                barrelRollInputWindowTimeElapsed += Time.deltaTime;
            }

            if (Input.GetButtonDown("TriggerL") || Input.GetButtonDown("TriggerR"))
            {
                int dir = Input.GetButtonDown("TriggerL") ? -1 : 1;

                if (dir == -1)
                {
                    ++lTapCount;
                }
                else
                {
                    ++rTapCount;
                }

                // INVALID INPUT - Barrel Roll direction input can't be mixed, so reset Barrel Roll state
                if ((barrelRollInputWindowTimeElapsed > barrelRollInputWindow) || (lTapCount > 0 && rTapCount > 0))
                {
                    lTapCount = 0;
                    rTapCount = 0;
                    barrelRollInputWindowTimeElapsed = 0.0f;
                }
                else if (lTapCount >= 2 || rTapCount >= 2)
                {
                    QuickSpin(dir);
                }
            }
            else if (!isInHalfSpin && (Input.GetButton("TriggerL") || Input.GetButton("TriggerR")))
            {
                int dir = Input.GetButton("TriggerL") ? -1 : 1;
                isInHalfSpin = true;

                StartCoroutine(HalfSpin(dir));
            }
            else if (isInHalfSpin && (Input.GetButtonUp("TriggerL") || Input.GetButtonUp("TriggerR")))
            {
                isInHalfSpin = false;
            }
        }
    }

    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void RotationLook(float h, float v, float speed)
    {
        aimTarget.parent.position = Vector3.zero;
        aimTarget.localPosition = new Vector3(h, v, 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * speed * Time.deltaTime);
    }

    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTarget.position, .5f);
        Gizmos.DrawSphere(aimTarget.position, .15f);

    }

    public void QuickSpin(int dir)
    {
        playerModel.DOLocalRotate(new Vector3(playerModel.localEulerAngles.x, playerModel.localEulerAngles.y, 360 * -dir), .4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
        barrel.Play();

        barrelRollInputWindowTimeElapsed = 0.0f; 
        lTapCount = 0;
        rTapCount = 0;
        isInHalfSpin = false;
    }

    public IEnumerator HalfSpin(int dir)
    {
        if (!DOTween.IsTweening(playerModel))
        {
            currentHalfRotationTween = playerModel.DOLocalRotate(new Vector3(playerModel.localEulerAngles.x, playerModel.localEulerAngles.y, 90.0f * -dir), halfSpinSpeed, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);

            while (currentHalfRotationTween.IsPlaying())
            {
                yield return new WaitForSeconds(halfSpinSpeed * 0.5f);

                if (dir == -1 && !Input.GetButton("TriggerL"))
                {
                    currentHalfRotationTween.Kill();
                    isInHalfSpin = false;
                    break;
                }
                else if (dir == 1 && !Input.GetButton("TriggerR"))
                {
                    currentHalfRotationTween.Kill();
                    isInHalfSpin = false;
                    break;
                }
            }
        }
    }

    void SetSpeed(float x)
    {
        dolly.m_Speed = x;
    }

    void SetCameraZoom(float zoom, float duration)
    {
        cameraParent.DOLocalMove(new Vector3(0, 0, zoom), duration);
    }

    void DistortionAmount(float x)
    {
        Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<LensDistortion>().intensity.value = x;
    }

    void FieldOfView(float fov)
    {
        cameraParent.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
    }

    void Chromatic(float x)
    {
        Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>().intensity.value = x;
    }


    void Boost(bool state)
    {

        if (state)
        {
            cameraParent.GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();
            trail.Play();
            circle.Play();
        }
        else
        {
            trail.Stop();
            circle.Stop();
        }
        trail.GetComponent<TrailRenderer>().emitting = state;

        float origFov = state ? 40 : 55;
        float endFov = state ? 55 : 40;
        float origChrom = state ? 0 : 1;
        float endChrom = state ? 1 : 0;
        float origDistortion = state ? 0 : -30;
        float endDistorton = state ? -30 : 0;
        float starsVel = state ? -20 : -1;
        float speed = state ? forwardSpeed * 2 : forwardSpeed;
        float zoom = state ? -7 : 0;

        DOVirtual.Float(origChrom, endChrom, .5f, Chromatic);
        DOVirtual.Float(origFov, endFov, .5f, FieldOfView);
        DOVirtual.Float(origDistortion, endDistorton, .5f, DistortionAmount);
        var pvel = stars.velocityOverLifetime;
        pvel.z = starsVel;

        DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
        SetCameraZoom(zoom, .4f);
    }

    void Break(bool state)
    {
        float speed = state ? forwardSpeed / 3 : forwardSpeed;
        float zoom = state ? 3 : 0;

        DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
        SetCameraZoom(zoom, .4f);
    }
}
