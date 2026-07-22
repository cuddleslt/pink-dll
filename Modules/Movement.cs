using GorillaLocomotion;
using Seralyth.Menu;
using UnityEngine;

namespace pink.Modules {
    public class Movement {
        static Vector3 lastPosition;
        static bool hasLastPosition;

        public static void StableBarkFly() {
            Vector3 inputDirection = new Vector3(Main.leftJoystick.x, Main.rightJoystick.y, Main.leftJoystick.y);
            bool hasInput = inputDirection.sqrMagnitude > 0.0001f;

            if (hasInput)
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;

            Vector3 playerForward = GTPlayer.Instance.bodyCollider.transform.forward.X_Z();
            Vector3 playerRight = GTPlayer.Instance.bodyCollider.transform.right.X_Z();
            Vector3 moveDir = inputDirection.x * playerRight + inputDirection.y * Vector3.up + inputDirection.z * playerForward;

            if (hasInput) {
                GorillaTagger.Instance.rigidbody.transform.position +=
                    moveDir * (Seralyth.Mods.Movement.FlySpeed * Time.deltaTime);
                lastPosition = GorillaTagger.Instance.rigidbody.transform.position;
                hasLastPosition = true;
            }
            else if (hasLastPosition) {
                GorillaTagger.Instance.rigidbody.transform.position = lastPosition;
            }
            else {
                lastPosition = GorillaTagger.Instance.rigidbody.transform.position;
                hasLastPosition = true;
            }

            Seralyth.Mods.Movement.ZeroGravity();
        }

        static bool flipping;
        static float flipStart;
        static Quaternion flipFrom;
        static Vector3 flipAxis;
        const float flipDuration = 1f;

        public static void FrontFlip() {
            bool silentFlip = Buttons.GetIndex("Silent Flip").enabled;
            if (!flipping && Main.rightPrimary && VRRig.LocalRig.enabled) {
                if (GTPlayer.Instance.playerRigidBody) {
                    flipping = true;
                    flipStart = Time.time;
                    flipAxis = VRRig.LocalRig.transform.right;
                    flipFrom = GTPlayer.Instance?.playerRigidBody?.rotation ?? Quaternion.identity;
                }
            }
            if (!flipping) return;

            float t = (Time.time - flipStart) / flipDuration;
            if (t >= 1f) {
                flipping = false;
                if (silentFlip)
                    VRRig.LocalRig.transform.rotation = flipFrom;
                else
                    GTPlayerTransform.ApplyRotationOverride(flipFrom, Time.frameCount);

                return;
            }
            var rot = Quaternion.AngleAxis(360f * t, flipAxis) * flipFrom;
            if (silentFlip)
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(0f, GorillaTagger.Instance.bodyCollider.transform.eulerAngles.y, 0f) * rot;
            else
                GTPlayerTransform.ApplyRotationOverride(rot, Time.frameCount);
        }
    }
}
