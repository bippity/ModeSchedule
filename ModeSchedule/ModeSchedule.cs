using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using TShockAPI.Hooks;
using Terraria;
using TerrariaApi.Server;

namespace ModeSchedule
{
    [ApiVersion(1, 23)]
    public class ModeSchedule : TerrariaPlugin
    {
        #region Plugin Info
        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override string Name
        {
            get { return "ModeScheduler"; }
        }

        public override string Author
        {
            get { return "Bippity"; }
        }

        public override string Description
        {
            get { return "Schedule hard/expert mode"; }
        }

        public  ModeSchedule(Main game) : base(game)
        {
            Order = 1;
        }
        #endregion

        #region Initialize/Dispose
        public System.Timers.Timer Timer = new System.Timers.Timer();

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("modeschedule.edit", ModeScheduleCommand, "modeschedule"));
            Timer.Interval = 3600000; //60 minutes
            Timer.Enabled = true;
            Timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //stuff?
            }
            base.Dispose(disposing);
        }
        #endregion

        #region ModeSchedule
        bool enabled = true;
        private void ModeScheduleCommand(CommandArgs args)
        {
            enabled = !enabled;

            if (enabled)
            {
                args.Player.SendWarningMessage("[ModeSchedule] Mode scheduling is enabled.");
            }
            else
            {
                args.Player.SendWarningMessage("[ModeSchedule] Mode scheduling is disabled.");
            }
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
        {
            DayOfWeek day = DateTime.Now.DayOfWeek;

        }
    }
}
