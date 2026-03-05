using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace Antisocial
{
    public sealed class ModConfig
    {
        public int EnergyCost { get; set; } = 2;
        public bool DisableForEvents { get; set; } = false;
        public bool CanPassOut { get; set; } = false;
    }

    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod configuration from the player.</summary>
        private ModConfig Config;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when a menu is changed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            this.Monitor.Log($"{Game1.CurrentEvent}");
            if (e.NewMenu != null)
            {
                if (e.NewMenu.GetType().Name is "DialogueBox")
                {
                    if (Game1.currentSpeaker != null)
                    {
                        if (Game1.CurrentEvent != null && this.Config.DisableForEvents) 
                        {
                            this.Monitor.Log("This is an event, skipping energy cost.", LogLevel.Info);
                            return;
                        }
                        if (Game1.player.stamina - this.Config.EnergyCost <= -15 && !this.Config.CanPassOut)
                        {
                            this.Monitor.Log("The player would pass out, skipping energy cost.", LogLevel.Info);
                            return;
                        }
                        this.Monitor.Log($"NPC: {Game1.currentSpeaker.Name}", LogLevel.Info);
                        Game1.player.stamina -= this.Config.EnergyCost;
                        this.Monitor.Log($"New Player Stamina: {Game1.player.stamina}", LogLevel.Info);
                    }
                }
            }
        }
    }
}