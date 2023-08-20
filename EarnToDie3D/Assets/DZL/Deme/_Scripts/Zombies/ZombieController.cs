using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class ZombieController : MonoBehaviour
    {
        class BoneTransform
        {
            public Vector3 Position { get; set; }
            public Quaternion Rotation { get; set; }
        }

        [SerializeField] Transform _ragdollParent;
        [SerializeField] float _standUpTimeAfterKick = 5f;

        ZombieRagdollPart[] _ragdollParts;
        Animator _animator;
        WaitForSeconds _waitForSeconds;


        BoneTransform[] _standUpBoneTransforms;
        BoneTransform[] _ragdollBoneTransforms;
        Transform[] _bones;



        bool _isHitByCar = false; // is on ground / ragdoll position
        void Start()
        {
            _ragdollParts = GetComponentsInChildren<ZombieRagdollPart>();
            _animator = GetComponent<Animator>();
            _waitForSeconds = new WaitForSeconds(_standUpTimeAfterKick);

            _bones = _animator.GetBoneTransform(HumanBodyBones.Hips).GetComponentsInChildren<Transform>();
            _standUpBoneTransforms = new BoneTransform[_bones.Length];
            _ragdollBoneTransforms = new BoneTransform[_bones.Length];

            for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
            {
                _standUpBoneTransforms[boneIndex] = new BoneTransform();
                _ragdollBoneTransforms[boneIndex] = new BoneTransform();
            }

            PopulateAnimationStartBoneTransforms(_standUpBoneTransforms);

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
        void PopulateAnimationStartBoneTransforms(BoneTransform[] boneTransforms)
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
            {
                if(clip.name == AnimationStrings.STAND_UP_BACK)
                {
                    print("FOUND");
                    clip.SampleAnimation(gameObject, 0f);
                    PopulateBoneTransforms(boneTransforms);
                }
            }

            transform.position = pos;
            transform.rotation = rot;
        }
        public void CarHitsZombie()
        {
            if (_isHitByCar) return;

            IEnumerator Routine()
            {
                ZombieIsHitByCar();

                yield return _waitForSeconds;

                yield return StandUp();
            }

            StartCoroutine(Routine());
        }

        [SerializeField] float _resettingBonesTime = 0.5f;
        void ZombieIsHitByCar()
        {
            _isHitByCar = true;

            _animator.enabled = false;
            foreach (var part in _ragdollParts)
            {
                part.EnablePart();
            }

            _ragdollParent.SetParent(null);
        }
        IEnumerator StandUp()
        {
            transform.position = _ragdollParent.position;
            _ragdollParent.SetParent(transform);

            PopulateBoneTransforms(_ragdollBoneTransforms);
            _animator.enabled = true; // this can be interpolated

            foreach (var part in _ragdollParts)
            {
                part.DisablePart();
            }

            

            //

            for (float i = 0f; i < _resettingBonesTime; i+= Time.deltaTime) // 0.5 time
            {
                ResettingBonesBehaviour(i / _resettingBonesTime);
                yield return null;
            }

            //


            bool isFacingUp = _animator.GetBoneTransform(HumanBodyBones.Hips).forward.y > 0f;
            _animator.SetTrigger(AnimationStrings.STAND_UP_BACK);

            _isHitByCar = false;

        }

        void ResettingBonesBehaviour(float elapsedPercentage)
        {
            for (int i = 0; i < _bones.Length; i++)
            {
                _bones[i].localPosition = Vector3.Lerp(_ragdollBoneTransforms[i].Position, _standUpBoneTransforms[i].Position, elapsedPercentage);
                _bones[i].localRotation = Quaternion.Lerp(_ragdollBoneTransforms[i].Rotation, _standUpBoneTransforms[i].Rotation, elapsedPercentage);
            }
        }

    }
}
