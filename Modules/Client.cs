using Photon.Pun;
using Photon.Realtime;

namespace pink.Modules {
    public class Client {
        public static void PinkIdentifiers() {
            foreach (VRRig rig in VRRigCache.ActiveRigs) {
                if (rig == null || rig.playerText1 == null)
                    continue;

                PhotonView view = rig.GetComponent<PhotonView>();
                if (view == null)
                    continue;

                Player player = view.Owner;
                if (player == null)
                    continue;

                string name = player.NickName;

                if (player.UserId == "719D985D50391AE6") {
                    rig.playerText1.richText = true;
                    rig.playerText1.text = $"<color=grey>[<color=#ff69b4>Pink Dev</color>]</color> <b>{name}</b>";
                }
                else if (player.CustomProperties.TryGetValue($"{Plugin.Description} | {Plugin.Name}", out object value) && value is object[] props && props.Length > 1 && props[0] is bool enabled && enabled) {
                    string version = props[1]?.ToString() ?? "Unknown";

                    rig.playerText1.richText = true;
                    rig.playerText1.text = $"<color=grey>[<color=#ff69b4>Pinky {version}</color>]</color> {name}";
                }
                else
                    rig.playerText1.text = name;
            }
        }

        public static void ResetNames() {
            foreach (VRRig rig in VRRigCache.ActiveRigs) {
                if (rig == null || rig.playerText1 == null)
                    continue;

                PhotonView view = rig.GetComponent<PhotonView>();
                if (view == null)
                    continue;

                Player player = view.Owner;
                if (player == null)
                    continue;

                string name = player.NickName;
                rig.playerText1.text = name;
            }
        }
    }
}
