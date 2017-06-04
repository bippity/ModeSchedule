using System;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;

namespace ModeSchedule
{
	[ApiVersion(2, 1)]
	public class ModeSchedule : TerrariaPlugin
	{
		#region Plugin Info
		public override Version Version
		{
			get { return new Version("1.1"); }
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

		public ModeSchedule(Main game) : base(game)
		{
			Order = 1;
		}
		#endregion

		#region Initialize/Dispose
		public System.Timers.Timer Timer = new System.Timers.Timer();
		public const int TIMER_INTERVAL_WEEKDAY = 1800000; //30 minutes, 60000 = 1 min
		public const int TIMER_INTERVAL_WEEKEND = 7200000; //2 hours

		public override void Initialize()
		{
			Commands.ChatCommands.Add(new Command("modeschedule.edit", ModeScheduleCommand, "modeschedule"));
			Timer.Interval = TIMER_INTERVAL_WEEKDAY;
			Timer.Enabled = true;
			Timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
		}

		protected override void Dispose(bool disposing)
		{
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
				args.Player.SendSuccessMessage("[Mode Schedule] Mode scheduling is enabled.");
			}
			else
			{
				args.Player.SendWarningMessage("[Mode Schedule] Mode scheduling is disabled.");
			}
		}

		DateTime oldDate;
		private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
		{
			if (!enabled)
				return;

			DayOfWeek today = DateTime.Now.DayOfWeek;

			if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday) //Every 2 hours, disable hardmode and make it expertmode
			{
				TSPlayer.All.SendInfoMessage("[Mode Schedule] It's the weekend! Expert mode is enabled!");
				Main.hardMode = false;
				Main.expertMode = true;
				Timer.Interval = TIMER_INTERVAL_WEEKEND;

				//Change between Crimson/Corruption
				if (today == DayOfWeek.Saturday)
				{
					if (oldDate == null)
					{
						oldDate = DateTime.UtcNow;
					}
					else
					{
						//Check if 7 days passed (if it's a new saturday)
						if (DateTime.UtcNow.Subtract(oldDate) >= TimeSpan.FromMinutes(10080)) //7 days passed
						{
							WorldGen.crimson = !WorldGen.crimson;
							TSPlayer.All.SendData(PacketTypes.WorldInfo);

							oldDate = DateTime.UtcNow;
							TSPlayer.All.SendInfoMessage("[Mode Schedule] It's a new week! The world is now: " + ((WorldGen.crimson) ? "Crimson" : "Corruption"));
						}
					}
				}
			}
			else //it's a weekday
			{
				if (Main.hardMode || Main.expertMode)
					TSPlayer.All.SendInfoMessage("[Mode Schedule] Today is: " + today.ToString() + ". Hard/Expert mode is only enabled on weekends!");

				Main.hardMode = false;
				Main.expertMode = false;
				Timer.Interval = TIMER_INTERVAL_WEEKDAY;
			}
		}
		#endregion
	}
}
