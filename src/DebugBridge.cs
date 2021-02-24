﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MonqueModder
{
    public class DebugBridge
    {
        private const string APP_ID = "com.AnotherAxiom.GorillaTag";

        private string handlePlaceholders(string command)
        {
            command = command.Replace("{app-id}", APP_ID);
            return command;
        }

        public string runCommand(string command)
        {
            return runCommandAsync(command).Result;
        }

        public async Task<string> runCommandAsync(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "adb.exe";
            process.StartInfo.Arguments = handlePlaceholders(command);
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            await process.WaitForExitAsync();

            if(output.Contains("error"))
            {
                throw new Exception(output);
            }

            return output;
        }
    }
}