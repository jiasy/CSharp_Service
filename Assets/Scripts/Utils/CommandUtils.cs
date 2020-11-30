using System.Diagnostics;
using UnityEngine;

namespace Utils {
    public class CommandUtils {
        //[MenuItem ("MyGame/Downscale Reference Textures")]
        public static void DownscaleRefTextures () {
            // using System.Diagnostics;
            Process p = new Process ();
            p.StartInfo.FileName = "python";
            p.StartInfo.Arguments = "resize_reference_sprites.py";
            // Pipe the output to itself - we will catch this later
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            // Where the script lives
            p.StartInfo.WorkingDirectory = Application.dataPath + "/SpriteCollections/";
            p.StartInfo.UseShellExecute = false;

            p.Start ();
            // Read the output - this will show is a single entry in the console - you could get  fancy and make it log for each line - but thats not why we're here
            UnityEngine.Debug.Log (p.StandardOutput.ReadToEnd ());
            p.WaitForExit ();
            p.Close ();
        }
    }
}