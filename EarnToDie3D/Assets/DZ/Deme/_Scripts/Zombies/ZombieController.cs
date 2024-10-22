using System.Collections;
using UnityEngine;

namespace DumbRide
{
    public class ZombieController : MonoBehaviour
    {
        [SerializeField] float _minimumSpeedToRagdoll = 5f; // minimum speed to hit by car to ragdoll
        [SerializeField] Transform _ragdollParent;
        [SerializeField] float _standUpTimeAfterKick = 5f;
        [SerializeField] float _resettingBonesTime = 0.5f;
        [SerializeField] LayerMask _groundLayer;

        ZombieRagdollPart[] _ragdollParts;
        Animator _animator;
        WaitForSeconds _waitForSeconds;
        bool _isZombieRagdoll = false; // is on ground / ragdoll position

        ZombieMovement _zombieMovement;

        #region BoneBlendingStuff
        [SerializeField] AnimationClip _standUpBack, _standUpFront;

        class BoneTransform
        {
            public Vector3 Position { get; set; }
            public Quaternion Rotation { get; set; }
        }

        BoneTransform[] _standUpForwardBoneTransforms;
        BoneTransform[] _ragdollBoneTransforms;
        BoneTransform[] _standUpBackBoneTransforms;
        Transform[] _bones;
        #endregion


        void Start()
        {
            _ragdollParts = GetComponentsInChildren<ZombieRagdollPart>();
            _zombieMovement = GetComponent<ZombieMovement>();
            _animator = GetComponent<Animator>();
            _waitForSeconds = new WaitForSeconds(_standUpTimeAfterKick);

            _bones = _animator.GetBoneTransform(HumanBodyBones.Hips).GetComponentsInChildren<Transform>();
            _standUpForwardBoneTransforms = new BoneTransform[_bones.Length];
            _standUpBackBoneTransforms = new BoneTransform[_bones.Length];
            _ragdollBoneTransforms = new BoneTransform[_bones.Length];

            for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
            {
                _standUpForwardBoneTransforms[boneIndex] = new BoneTransform();
                _ragdollBoneTransforms[boneIndex] = new BoneTransform();
                _standUpBackBoneTransforms[boneIndex] = new BoneTransform();
            }

            PopulateAnimationStartBoneTransforms(AnimationStrings.STAND_UP_FRONT, _standUpForwardBoneTransforms);
            PopulateAnimationStartBoneTransforms(AnimationStrings.STAND_UP_BACK, _standUpBackBoneTransforms);

            foreach (var part in _ragdollParts)
            {
                part.DisablePart();
            }
        }

        void PopulateBoneTransforms(BoneTransform[] boneTransforms)
        {
            for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
            {
                boneTransforms[boneIndex].Position = _bones[boneIndex].localPosition;
                boneTransforms[boneIndex].Rotation = _bones[boneIndex].localRotation;
            }
        }
        void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            var clip = clipName == AnimationStrings.STAND_UP_BACK ? _standUpBack : _standUpFront;

            clip.SampleAnimation(gameObject, 0f);
            PopulateBoneTransforms(boneTransforms);

            transform.position = pos;
            transform.rotation = rot;
        }
        bool _isEnoughSpeedToRagdoll = false;
        public void CarTriggerCrashedZombie(float carSpeed)
        {
            _isEnoughSpeedToRagdoll = carSpeed > _minimumSpeedToRagdoll;
            TryToHurtZombie();
        }

        public void TryToHurtZombie()
        {
            if (_isZombieRagdoll || !_isEnoughSpeedToRagdoll) return;

            IEnumerator Routine()
            {
                RagdollizeZombie();

                yield return _waitForSeconds;

                yield return StandUp();
            }

            StartCoroutine(Routine());
        }

        void RagdollizeZombie()
        {
            _isZombieRagdoll = true;

            _zombieMovement.enabled = false;

            if (_animator.enabled)
                EnableRagdoll();

            _ragdollParent.SetParent(null);
        }
        
        public void EnableRagdoll() // called by trigger hit by a car
        {
            _animator.ResetTrigger(AnimationStrings.STAND_UP_FRONT);
            _animator.ResetTrigger(AnimationStrings.STAND_UP_BACK);

            _animator.enabled = false;
            foreach (var part in _ragdollParts)
            {
                part.EnablePart();
            }
        }
        public void OnStandUp() // called by animation event
        {
            _zombieMovement.enabled = true;
        }
        IEnumerator StandUp()
        {
            Vector3 pos;

            if (Physics.Raycast(_ragdollParent.position, Vector3.down, out RaycastHit hit, 100f, _groundLayer))
                pos = hit.point;
            else
                pos = _ragdollParent.position;

            bool isFacingUp = _animator.GetBoneTransform(HumanBodyBones.Hips).forward.y < 0f;


            Vector3 dirToFace = Vector3.ProjectOnPlane(_ragdollParent.up, Vector3.up).normalized;
            transform.position = pos;
            transform.forward = isFacingUp ? dirToFace : -dirToFace;

            _ragdollParent.SetParent(transform);

            PopulateBoneTransforms(_ragdollBoneTransforms);

            foreach (var part in _ragdollParts)
            {
                part.DisablePart();
            }


            for (float i = 0f; i < _resettingBonesTime; i += Time.deltaTime) // 0.5 time
            {
                ResettingBonesBehaviour(isFacingUp, i / _resettingBonesTime);
                yield return null;
            }
            _animator.enabled = true; // this can be interpolated


            _animator.Play(isFacingUp ? AnimationStrings.STAND_UP_FRONT : AnimationStrings.STAND_UP_BACK, 0, 0);
            _isZombieRagdoll = false;
        }

        void ResettingBonesBehaviour(bool isFacingUp, float elapsedPercentage)
        {
            var standUpBoneTransforms = isFacingUp ? _standUpForwardBoneTransforms : _standUpBackBoneTransforms;

            for (int i = 0; i < _bones.Length; i++)
            {
                _bones[i].localPosition = Vector3.Lerp(_ragdollBoneTransforms[i].Position, standUpBoneTransforms[i].Position, elapsedPercentage);
                _bones[i].localRotation = Quaternion.Lerp(_ragdollBoneTransforms[i].Rotation, standUpBoneTransforms[i].Rotation, elapsedPercentage);
            }
        }

    }
}
