using Photon.Pun;
using Seralyth.Extensions;
using Seralyth.Managers;
using Seralyth.Menu;
using Seralyth.Mods;
using Seralyth.Utilities;
using UnityEngine;

namespace pink.Modules {
    public class Master {
        public class VIM {
            public static float antiReportDelay;
            private static bool lastLeftKickOrBlock;
            private static bool lastRightKickOrBlock;

            public static void KickAntiReport() {
                if (!VRRig.LocalRig.IsVIMSubscriber()) {
                    Main.PromptSingle("You are not a VIM subscriber, so this mod will not function.");
                    Buttons.GetIndex("vimkickantireport").SetEnabled(false);
                    return;
                }
                if (PhotonNetwork.IsMasterClient.Equals(false)) {
                    Main.PromptSingle("You are not master client, so this mod will not function.");
                    Buttons.GetIndex("vimkickantireport").SetEnabled(false);
                    return;
                }

                Safety.AntiReport((vrrig, position) => {
                    RoomControls.KickPlayer(vrrig.GetPlayer().ActorNumber);
                    Main.RPCProtection();

                    if (!(Time.time > antiReportDelay)) return;
                    antiReportDelay = Time.time + 1f;
                    NotificationManager.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + RigUtilities.GetPlayerFromVRRig(vrrig).NickName + " attempted to report you, we kicked them for you.");
                });
            }

            public static void BlockAntiReport() {
                if (!VRRig.LocalRig.IsVIMSubscriber()) {
                    Main.PromptSingle("You are not a VIM subscriber, so this mod will not function.");
                    Buttons.GetIndex("vimblockantireport").SetEnabled(false);
                    return;
                }
                if (PhotonNetwork.IsMasterClient.Equals(false)) {
                    Main.PromptSingle("You are not master client, so this mod will not function.");
                    Buttons.GetIndex("vimblockantireport").SetEnabled(false);
                    return;
                }

                Safety.AntiReport((vrrig, position) => {
                    RoomControls.KickAndBlockPlayer(vrrig.GetPlayer().ActorNumber);
                    Main.RPCProtection();

                    if (!(Time.time > antiReportDelay)) return;
                    antiReportDelay = Time.time + 1f;
                    NotificationManager.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + RigUtilities.GetPlayerFromVRRig(vrrig).NickName + " attempted to report you, we kicked them for you.");
                });
            }

            public static void KickOnTouch() {
                if (!VRRig.LocalRig.IsVIMSubscriber()) {
                    Main.PromptSingle("You are not a VIM subscriber, so this mod will not function.");
                    Buttons.GetIndex("vimkickontouch").SetEnabled(false);
                    return;
                }
                if (PhotonNetwork.IsMasterClient.Equals(false)) {
                    Main.PromptSingle("You are not master client, so this mod will not function.");
                    Buttons.GetIndex("vimkickontouch").SetEnabled(false);
                    return;
                }

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

                    if (leftDist < threshold && !lastLeftKickOrBlock)
                        RoomControls.KickPlayer(rig.GetPlayer().ActorNumber);

                    if (rightDist < threshold && !lastRightKickOrBlock)
                        RoomControls.KickPlayer(rig.GetPlayer().ActorNumber);
                }

                lastLeftKickOrBlock = isTouchingLeft;
                lastRightKickOrBlock = isTouchingRight;
            }

            public static void BlockOnTouch() {
                if (!VRRig.LocalRig.IsVIMSubscriber()) {
                    Main.PromptSingle("You are not a VIM subscriber, so this mod will not function.");
                    Buttons.GetIndex("vimblockontouch").SetEnabled(false);
                    return;
                }
                if (PhotonNetwork.IsMasterClient.Equals(false)) {
                    Main.PromptSingle("You are not master client, so this mod will not function.");
                    Buttons.GetIndex("vimblockontouch").SetEnabled(false);
                    return;
                }

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

                    if (leftDist < threshold && !lastLeftKickOrBlock)
                        RoomControls.KickAndBlockPlayer(rig.GetPlayer().ActorNumber);

                    if (rightDist < threshold && !lastRightKickOrBlock)
                        RoomControls.KickAndBlockPlayer(rig.GetPlayer().ActorNumber);
                }

                lastLeftKickOrBlock = isTouchingLeft;
                lastRightKickOrBlock = isTouchingRight;
            }
        }
    }
}
