using Photon.Pun;
using pink.Extensions;
using Seralyth.Managers;
using Seralyth.Menu;
using System.Threading.Tasks;

namespace pink {
    public class Plugin {
        public static string Name = "pink";
        public static string Version = "v1.4";
        public static string Description = "discord.gg/so-pink";

        public static string cs = "<color=grey>(</color><color=#f7b4cb>cs</color><color=grey>)</color>"; // Client Sided
        public static string ss = "<color=grey>(</color><color=#f7b4cb>ss</color><color=grey>)</color>"; // Server Sided
        public static string ts = "<color=grey>(</color><color=#f7b4cb>ts</color><color=grey>)</color>"; // Target Sided

        public static async Task OnEnable() {
            GorillaTagger.OnPlayerSpawned(GorillaSpawn);

            LogManager.Log($"[.gg/so-pink] {Name} ({Version}) has loaded!");
            System.Console.Title = $"Gorilla Tag | Seralyth | {Name}";
            System.Console.Beep();

            CategoryChanges.PinkCats();
            CategoryChanges.SettingsCats();
        }

        public static void OnDisable() {
            LogManager.Log($"[.gg/so-pink] {Name} ({Version}) has unloaded!");
            System.Console.Title = $"Game closing... [Gorilla Tag]";

            Buttons.RemoveCategory($"{Name} <color=#f7b4cb>{Version}</color>");
            Buttons.RemoveButton(Buttons.GetCategory("Main"), $"{Name} <color=#f7b4cb>{Version}</color>");
        }

        public static void Update() { }

        private static void GorillaSpawn() {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { $"{Description} | {Name}", Version } });

            AchievementManager.UnlockAchievement(new AchievementManager.Achievement {
                name = "Pink",
                description = "Utilise the <color=#f7b4cb>Pink</color> plugin.",
                icon = "Images/Achievements/popular.png"
            });
        }
    }
}

/*
    + GetIndex(ButtonName); Gets a button's ButtonInfo
    + AddButton(CategoryID, new ButtonInfo); Creates a button in a category
    - RemoveButton(CategoryID, ButtonName); Removes a button in a category

    + GetCategory(CategoryName); Gets a category, you can find the category names in the menu's Buttons.cs file at the bottom
    + AddCategory(CategoryName); Creates a category, does not automatically create the navigation button
    - RemoveCategory(CategoryName); Removes the category created, does not automatically
*/