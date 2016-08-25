using System;
using TShockAPI;
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

		public ModeSchedule(Main game) : base(game)
		{
			Order = 1;
		}
		#endregion

		#region Initialize/Dispose
		public System.Timers.Timer Timer = new System.Timers.Timer();

		public override void Initialize()
		{
			Commands.ChatCommands.Add(new Command("modeschedule.edit", ModeScheduleCommand, "modeschedule"));
			Timer.Interval = 1800000; //30 minutes
									  //Timer.Interval = 60000; //1 minute
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

		private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
		{
			if (!enabled)
				return;

			DayOfWeek today = DateTime.Now.DayOfWeek;

			if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday)
			{
				TSPlayer.All.SendInfoMessage("[Mode Schedule] It's the weekend! Hard/Expert mode is enabled!");
				Main.hardMode = true;
				Main.expertMode = true;
			}
			else
			{
				if (Main.hardMode || Main.expertMode)
					TSPlayer.All.SendInfoMessage("[Mode Schedule] Today is: " + today.ToString() + ". Hard/Expert mode is only enabled on weekends!");

				Main.hardMode = false;
				Main.expertMode = false;
			}
		}
		#endregion
	}
}
