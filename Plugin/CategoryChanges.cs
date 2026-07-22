using pink.Modules;
using Seralyth.Classes.Menu;
using Seralyth.Menu;
using Seralyth.Patches.Menu;
using UnityEngine;

namespace pink.Extensions {
    public class CategoryChanges {
        public static void PinkCats() {
            const string Pink = "#f7b4cb";
            const string Gold = "#ffd700";

            int pinkyCategory = Buttons.AddCategory($"{Plugin.Name} <color={Pink}>{Plugin.Version}</color>");

            Buttons.AddButton(Buttons.GetCategory("Main"), new ButtonInfo {
                buttonText = $"{Plugin.Name} <color={Pink}>{Plugin.Version}</color>",
                method = () => Buttons.CurrentCategoryIndex = pinkyCategory,
                isTogglable = false,
                toolTip = $"Opens the {Plugin.Name} menu."
            });

            int clientCategory = Buttons.AddCategory($"<color={Pink}>Client</color>");
            int movementCategory = Buttons.AddCategory($"<color={Pink}>Movement</color>");
            int masterCategory = Buttons.AddCategory($"<color={Pink}>Master</color>");
            int miscCategory = Buttons.AddCategory($"<color={Pink}>Misc</color>");
            
            Buttons.AddButtons(pinkyCategory,
                new ButtonInfo[] {
                    new ButtonInfo { buttonText = "Exit", method = () => Buttons.CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main menu." },

                    new ButtonInfo { buttonText = $"<color={Pink}>Client</color>", method = () => Buttons.CurrentCategoryIndex = clientCategory, isTogglable = false, toolTip = "Opens the Client category." },
                    new ButtonInfo { buttonText = $"<color={Pink}>Movement</color>", method = () => Buttons.CurrentCategoryIndex = movementCategory, isTogglable = false, toolTip = "Opens the Movement category." },
                    new ButtonInfo { buttonText = $"<color={Pink}>Master</color>", method = () => Buttons.CurrentCategoryIndex = masterCategory, isTogglable = false, toolTip = "Opens the Master category." },
                    new ButtonInfo { buttonText = $"<color={Pink}>Misc</color>", method = () => Buttons.CurrentCategoryIndex = miscCategory, isTogglable = false, toolTip = "Opens the Misc category." },
                    new ButtonInfo { buttonText = $"Developed by <color={Pink}>Lucky</color>", label = true },
                    new ButtonInfo { buttonText = $"<color={Pink}>Credits</color>", method = () => Misc.ShowCred(), isTogglable = false, toolTip = "Displays the credits." },
                }
            );

            Buttons.AddButtons(clientCategory,
                new ButtonInfo[] {
                    new ButtonInfo { buttonText = "Back", method = () => Buttons.CurrentCategoryIndex = pinkyCategory, isTogglable = false, toolTip = "Returns to the Pinky menu." },

                    new ButtonInfo { buttonText = "Set Name To 'Pinky'", method = () => Client.currentGorilla.SetName("Pinky"), isTogglable = false, toolTip = "Sets your name to Pinky." },
                    new ButtonInfo { buttonText = "Set Color To Pink", method = () => Client.currentGorilla.SetColor(Color.pink), isTogglable = false, toolTip = "Sets your color to Pink." },
                    new ButtonInfo { buttonText = "Pink Name (cs)", overlapText = "Pink Name " + Plugin.cs, method = () => Client.currentGorilla.SetNameColor(Color.pink, true), disableMethod = () => Client.currentGorilla.SetNameColor(Color.white, false), isTogglable = true, toolTip = "Makes your name pink." },
                    new ButtonInfo { buttonText = "Pink Identifiers (cs)", overlapText = "Pink Identifiers " + Plugin.cs, method = () => Modules.Client.PinkIdentifiers(), disableMethod = () => Modules.Client.ResetNames(), isTogglable = true, toolTip = $"Makes {Plugin.Name} users have pink names." },
                }
            );

            Buttons.AddButtons(movementCategory,
                new ButtonInfo[] {
                    new ButtonInfo { buttonText = "Back", method = () => Buttons.CurrentCategoryIndex = pinkyCategory, isTogglable = false, toolTip = "Returns to the Pinky menu." },

                    new ButtonInfo { buttonText = "Stable Bark Fly", method = () => Movement.StableBarkFly(), isTogglable = true, toolTip = "Acts like Bark's fly without making you fall." },
                    new ButtonInfo { buttonText = "Front Flip", method = () => Movement.FrontFlip(), isTogglable = true, toolTip = "Makes you do a front flip in the air." },
                }
            );

            Buttons.AddButtons(masterCategory,
                new ButtonInfo[] {
                    new ButtonInfo { buttonText = "Back", method = () => Buttons.CurrentCategoryIndex = pinkyCategory, isTogglable = false, toolTip = "Returns to the Pinky menu." },

                    new ButtonInfo { buttonText = "vimkickantireport", overlapText = $"<color={Gold}>VIM</color> Kick Anti Report", method = () => Master.VIM.KickAntiReport(), isTogglable = true, toolTip = "Kicks whoever tries to report you. VIM SUBSCRIBERS ONLY." },
                    new ButtonInfo { buttonText = "vimkickontouch", overlapText = $"<color={Gold}>VIM</color> Kick On Touch", method = () => Master.VIM.KickOnTouch(), isTogglable = true, toolTip = "Kicks whoever your hand touches. VIM SUBSCRIBERS ONLY." },
                    new ButtonInfo { buttonText = "vimblockantireport", overlapText = $"<color={Gold}>VIM</color> Block Anti Report", method = () => Master.VIM.BlockAntiReport(), isTogglable = true, toolTip = "Blocks whoever tries to report you. VIM SUBSCRIBERS ONLY." },
                    new ButtonInfo { buttonText = "vimblockontouch", overlapText = $"<color={Gold}>VIM</color> Block On Touch", method = () => Master.VIM.BlockOnTouch(), isTogglable = true, toolTip = "Blocks whoever your hand touches. VIM SUBSCRIBERS ONLY." },
                }
            );

            Buttons.AddButtons(miscCategory,
                new ButtonInfo[] {
                    new ButtonInfo { buttonText = "Back", method = () => Buttons.CurrentCategoryIndex = pinkyCategory, isTogglable = false, toolTip = "Returns to the Pinky menu." },

                    new ButtonInfo { buttonText = "Splash On Touch", overlapText = "Splash On Touch " + Plugin.ss, method = () => Misc.WaterSplashOnTouch(), isTogglable = true, toolTip = "Splashes players you touch." },
                    new ButtonInfo { buttonText = "Complete All Achievements", method = () => Misc.CompleteAdvancements(), isTogglable = false, toolTip = "Completes all achievements." },
                    new ButtonInfo { buttonText = "Hoverboard Mess", method = () => Misc.HoverboardMess(), isTogglable = true, toolTip = "Does random shit with hoverboards around you." },
                    new ButtonInfo { buttonText = "Rig Blind All", method = () => Misc.RigBlindAll(), disableMethod =() => SerializePatch.OverrideSerialization = null, isTogglable = true, toolTip = "Places your rig in front of all the players. [WIP]" },
                }
            );
        }

        public static void SettingsCats() {
            var categories = new[] {
                new { Id = 6, Name = "Room", Legal = true, Description = "room mods" },
                new { Id = 8, Name = "Safety", Legal = false, Description = "safety mods" },
                new { Id = 9, Name = "Movement", Legal = false, Description = "movement mods" },
                new { Id = 10, Name = "Advantage", Legal = true, Description = "advantage mods" },
                new { Id = 11, Name = "Visual", Legal = true, Description = "visual mods" },
                new { Id = 12, Name = "Fun", Legal = true, Description = "fun mods" },
                new { Id = 15, Name = "Projectile", Legal = false, Description = "projectile mods" },
                new { Id = 17, Name = "Overpowered", Legal = false, Description = "overpowered mods" },
                new { Id = 43, Name = "Detected", Legal = false, Description = "detected mods" },
            };

            foreach (var category in categories) {
                string settingsName = $"{category.Name} Settings";

                Buttons.AddButtons(category.Id,
                    new ButtonInfo[] {
                        new ButtonInfo {
                            buttonText = settingsName,
                            overlapText = $"<color=yellow>{settingsName}</color>",
                            method = () => Buttons.CurrentCategoryName = settingsName,
                            isTogglable = false,
                            toolTip = $"Opens the settings for the {category.Description}.",
                            legal = category.Legal
                        }
                    }, 1 // Beginning of the category
                );
            }
        }
    }
}
