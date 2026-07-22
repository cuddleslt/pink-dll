using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

namespace pink.Extensions {
    public class Client {
        public class currentGorilla {
            public static Vector3 GetPos() {
                return GorillaTagger.Instance.transform.position;
            }

            public static void SetPos(Vector3 position) {
                GorillaTagger.Instance.transform.position = position;
            }

            public static string GetName() {
                return GorillaComputer.instance.currentName;
            }

            public static void SetName(string playerName, bool noColor = false) {
                GorillaComputer.instance.currentName = playerName;

                GorillaComputer.instance.SetLocalNameTagText(GorillaComputer.instance.currentName);
                GorillaComputer.instance.savedName = GorillaComputer.instance.currentName;
                PlayerPrefs.SetString("playerName", GorillaComputer.instance.currentName);
                PlayerPrefs.Save();

                PhotonNetwork.LocalPlayer.NickName = playerName;

                if (noColor) return;

                try {
                    if (!GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(PhotonNetwork.LocalPlayer.UserId) &&
                        !CosmeticWardrobeProximityDetector.IsUserNearWardrobe(PhotonNetwork.LocalPlayer.ActorNumber))
                        return;

                    GorillaTagger.Instance.myVRRig.SendRPC(
                        "RPC_InitializeNoobMaterial",
                        RpcTarget.All,
                        VRRig.LocalRig.playerColor.r,
                        VRRig.LocalRig.playerColor.g,
                        VRRig.LocalRig.playerColor.b);

                    Seralyth.Menu.Main.RPCProtection();
                }
                catch { }
            }

            public static void SetNameColor(Color color, bool enabled) {
                try {
                    if (enabled)
                        VRRig.LocalRig.playerText1.color = color;
                    else
                        VRRig.LocalRig.playerText1.color = Color.white;
                }
                catch { }
            }

            public static Color GetColor() {
                return new Color(
                    PlayerPrefs.GetFloat("redValue", 0f),
                    PlayerPrefs.GetFloat("greenValue", 0f),
                    PlayerPrefs.GetFloat("blueValue", 0f)
                );
            }

            public static void SetColor(Color color) {
                PlayerPrefs.SetFloat("redValue", Mathf.Clamp01(color.r));
                PlayerPrefs.SetFloat("greenValue", Mathf.Clamp01(color.g));
                PlayerPrefs.SetFloat("blueValue", Mathf.Clamp01(color.b));

                GorillaTagger.Instance.UpdateColor(color.r, color.g, color.b);
                PlayerPrefs.Save();

                try {
                    GorillaTagger.Instance.myVRRig.SendRPC(
                        "RPC_InitializeNoobMaterial",
                        RpcTarget.All,
                        color.r,
                        color.g,
                        color.b);
                    Seralyth.Menu.Main.RPCProtection();
                }
                catch { }
            }
        }
    }
}