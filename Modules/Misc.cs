using GorillaLocomotion;
using Photon.Realtime;
using Seralyth.Extensions;
using Seralyth.Managers;
using Seralyth.Menu;
using Seralyth.Patches.Menu;
using Seralyth.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace pink.Modules {
    public class Misc {
        public static void CompleteAdvancements() {
            var achievements = new Dictionary<string, (string description, string icon)> {
                ["Veteran"] = ("Use the menu for over a year.", "Images/Achievements/veteran.png"),
                ["Potato"] = ("Have 15 FPS for over a minute.", "Images/Achievements/potato.png"),
                ["EEEEKK!"] = ("Be in the same room as a Console administrator.", "Images/Achievements/eeeekk.png"),
                ["Persistent"] = ("Open the menu 100 times.", "Images/Achievements/persistent.png"),
                ["Dedicated"] = ("Enable 50 mods at the same time.", "Images/Achievements/award.png"),
                ["Too Dedicated"] = ("Enable 100 mods at the same time.", "Images/Achievements/red-award.png"),
                ["Sinister"] = ("Open the \"Detected Mods\" category.", "Images/Achievements/sinister.png"),
                ["Troublemaker"] = ("Evade a player report.", "Images/Achievements/troublemaker.png"),
                ["Purgatory"] = ("Get banned with the menu.", "Images/Achievements/banned.png"),
                ["Not forever alone..."] = ("Make a friend using the friend system.", "Images/Achievements/notforeveralone.png"),
                ["Popular"] = ("Have 25+ friends.", "Images/Achievements/popular.png"),

                ["Pink"] = ("Utilise the <color=#f7b4cb>Pink</color> plugin.", "Images/Achievements/popular.png"),
            };

            foreach (var achievement in achievements) {
                AchievementManager.UnlockAchievement(new AchievementManager.Achievement {
                    name = achievement.Key,
                    description = achievement.Value.description,
                    icon = achievement.Value.icon
                });
            }
        }

        private static bool lastLeftSplash;
        private static bool lastRightSplash;

        public static void WaterSplashOnTouch() {
            bool isTouchingLeft = false;
            bool isTouchingRight = false;

            foreach (VRRig rig in VRRigCache.ActiveRigs) {
                if (rig.isLocal)
                    continue;

                float leftDist = Vector3.Distance(GorillaTagger.Instance.leftHandTransform.position, rig.headMesh.transform.position);
                float rightDist = Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, rig.headMesh.transform.position);

                const float threshold = 0.275f;

                if (!isTouchingLeft)
                    isTouchingLeft = leftDist < threshold;

                if (!isTouchingRight)
                    isTouchingRight = rightDist < threshold;

                if (leftDist < threshold && !lastLeftSplash)
                    Seralyth.Mods.Fun.BetaWaterSplash(rig.headMesh.transform.position + Vector3.up * 0.2f, Quaternion.identity, 1f, 0.5f, true, false);

                if (rightDist < threshold && !lastRightSplash)
                    Seralyth.Mods.Fun.BetaWaterSplash(rig.headMesh.transform.position + Vector3.up * 0.2f, Quaternion.identity, 1f, 0.5f, true, false);
            }

            lastLeftSplash = isTouchingLeft;
            lastRightSplash = isTouchingRight;
        }

        private static float hoverboardDelay;

        public static void HoverboardMess() {
            GTPlayer.Instance.SetHoverAllowed(true);

            if (Time.time > hoverboardDelay) {
                hoverboardDelay = Time.time + 0.25f;

                float offsetA = 0f;
                float offsetB = -10f;

                Vector3 vector3 = new Vector3(MathF.Cos(offsetA + (float)Time.frameCount / 4f) * 2f, 0.5f, MathF.Sin(offsetA + (float)Time.frameCount / 4f) * 2f);
                Vector3 position2 = new Vector3(MathF.Cos(offsetB + (float)Time.frameCount / 4f) * 2f, 0f, MathF.Sin(offsetB + (float)Time.frameCount / 4f) * 2f);

                Seralyth.Mods.Fun.BetaDropBoard(GorillaTagger.Instance.headCollider.transform.position + vector3, Quaternion.Euler((GorillaTagger.Instance.headCollider.transform.position - vector3).normalized), (position2 - vector3).normalized * 6.5f, new Vector3(0f, 360f, 0f), RandomUtilities.RandomColor());

                float offsetC = 1.8f;
                float offsetD = 7.5f;

                Vector3 pos2 = new Vector3(MathF.Sin(offsetC + Time.frameCount / 6f) * 3.5f, 1.25f + MathF.Cos(Time.frameCount / 12f) * 0.4f, MathF.Cos(offsetC + Time.frameCount / 6f) * 1.5f);
                Vector3 vel2 = new Vector3(MathF.Sin(offsetD + Time.frameCount / 5f) * 2f, MathF.Cos(Time.frameCount / 8f), MathF.Cos(offsetD + Time.frameCount / 5f) * 3f);

                Seralyth.Mods.Fun.BetaDropBoard(GorillaTagger.Instance.headCollider.transform.position + pos2, Quaternion.Euler((GorillaTagger.Instance.headCollider.transform.position - pos2).normalized), vel2.normalized * 8f, new Vector3(180f, 180f, 90f), RandomUtilities.RandomColor());
            }
        }

        public static void RigBlindAll() {
            SerializePatch.OverrideSerialization =()=> {
                Main.MassSerialize(true, new[] { VRRig.LocalRig.GetPhotonView() });

                Vector3 originalPos = VRRig.LocalRig.transform.position;
                Quaternion originalRot = VRRig.LocalRig.transform.rotation;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers) {
                    VRRig targetRig = RigUtilities.GetVRRigFromPlayer(Player);
                    if (targetRig == null)
                        continue;

                    Vector3 targetPos = targetRig.transform.position;
                    Vector3 faceDir = targetRig.transform.forward;

                    Vector3 teleportPos = targetPos + faceDir * 0.5f;

                    Vector3 tempPos = VRRig.LocalRig.transform.position;
                    Quaternion tempRot = VRRig.LocalRig.transform.rotation;

                    VRRig.LocalRig.transform.position = teleportPos;
                    VRRig.LocalRig.transform.LookAt(targetPos);

                    Main.SendSerialize(VRRig.LocalRig.GetPhotonView(), new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });

                    VRRig.LocalRig.transform.position = tempPos;
                    VRRig.LocalRig.transform.rotation = tempRot;
                }

                Main.RPCProtection();

                VRRig.LocalRig.transform.position = originalPos;
                VRRig.LocalRig.transform.rotation = originalRot;

                return false;
            };
        }

        public static void ShowCred() {
            Main.PromptSingle("@<color=red>0cool_woopet</color>\n- Stable Bark Fly\n\n@ew70\n- VIM Master Mods ideas");
        }
    }
}
