using System;
using System.Management;
using System.Threading;

class YoutubeDlObserver {
	static void processStartedEvent(object sender, EventArrivedEventArgs e) {
		ManagementBaseObject proc = (ManagementBaseObject) (
			(ManagementBaseObject) e.NewEvent
		) ["TargetInstance"];

		if ((string) proc["Name"] != "youtube-dl.exe") return;

		Console.WriteLine(
			DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss ") + proc["CommandLine"]
		);
	}

	static void Main(string[] args) {
		ManagementEventWatcher watcher = new ManagementEventWatcher(
			new WqlEventQuery(
				"__InstanceCreationEvent",
				new TimeSpan(0, 0, 1),
				"TargetInstance ISA \"Win32_Process\""
			)
		);

		watcher.EventArrived += new EventArrivedEventHandler(processStartedEvent);

		watcher.Start();

		while (true) Thread.Sleep(1000);
	}
}

