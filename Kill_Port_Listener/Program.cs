using System.Diagnostics;

namespace Kill_Port_Listener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool restartApp;

            do
            {
                PrintProgramTitleAfterClearingConsole();

                Console.Write("\t\t\t\tEnter port number: ");
                string portNumber = Console.ReadLine();

                Process findListeningPortProcess = new Process();
                findListeningPortProcess.StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = String.Format("/C netstat -aon | find \":{0}\" | find \"LISTENING\"", portNumber),//"/C netstat -aon | findstr " + portNumber + "\r\n /c",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                };

                findListeningPortProcess.Start();

                PrintProgramTitleAfterClearingConsole();

                string commandOutput = findListeningPortProcess.StandardOutput.ReadToEnd();
                string appId = commandOutput.Split(' ').Last().Trim();
                Console.WriteLine(commandOutput);

                if (String.IsNullOrWhiteSpace(commandOutput))
                {
                    Console.WriteLine("An application listening to port: {0} has NOT been found", portNumber);
                }
                else
                {
                    Console.WriteLine("\nAn application listening to port: {0} has been found", portNumber);
                    Console.Write("\nWould you like to terminate the application with id: {0} (Y/N) ", appId);

                    if (Console.ReadLine().ToUpper().Equals("Y")) 
                    {
                        Process processIDKiller = new Process();
                       
                        processIDKiller.StartInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = String.Format("/C taskkill /f -pid " + appId),
                            RedirectStandardError = true,
                            RedirectStandardOutput = true
                        };
                        processIDKiller.Start();
                        string output = processIDKiller.StandardOutput.ReadToEnd();

                        if (output.ToUpper().Contains("SUCCESS"))
                            Console.WriteLine("\n\n"+output);
                        else
                            Console.WriteLine("An error occured... \n" + output);
                    }


                }

                
                Console.Write("\nWould you like to restart the application? (Y/N) ");
                if (Console.ReadLine().ToUpper().Equals("Y"))
                    restartApp= true;
                else
                    restartApp= false;
                
            }while(restartApp);
        }

        private static void PrintProgramTitleAfterClearingConsole() 
        {
            Console.Clear();
            Console.WriteLine("\t\t\t|----- Kill Port Listener -----|");
            Console.WriteLine("\n");
        }

    }
}