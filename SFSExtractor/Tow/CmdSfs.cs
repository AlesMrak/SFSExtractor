namespace Editor.Utils
{
    using Editor.SFS;
    using System;
    using System.IO;

    internal static class CmdSfs
    {
        private const string CMD_MOUNT = "mount";
        private const string CMD_UNMOUNT = "unmount";

        public static bool Exec(string[] cmdParams)
        {
            bool flag = true;
            string cmd = null;
            string text2 = null;
            string text3 = null;
            for (int i = 1; i < cmdParams.Length; i++)
            {
                string text4 = cmdParams[i];
                if (text4.Equals("mount", StringComparison.InvariantCultureIgnoreCase) || text4.Equals("unmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    if ((cmd != null) && !ExecCommand(cmd, text2, text3))
                    {
                        flag = false;
                    }
                    cmd = text4;
                    text2 = null;
                    text3 = null;
                }
                else if ((cmd != null) && (text2 == null))
                {
                    text2 = text4;
                }
                else if ((cmd != null) && (text3 == null))
                {
                    text3 = text4;
                }
            }
            if ((cmd != null) && !ExecCommand(cmd, text2, text3))
            {
                flag = false;
            }
            return flag;
        }

        public static bool ExecCommand(string cmd, string param1, string param2)
        {
            if (param1 == null)
            {
                return false;
            }
            string text = @"..\";
            string path = Path.GetFullPath(Path.Combine(text, param1));
            if (cmd.Equals("mount", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    if (param2 != null)
                    {
                        Editor.SFS.SFS.MountAs(path, Path.Combine(text, param2));
                    }
                    else
                    {
                        Editor.SFS.SFS.MountAs(path, text);
                    }
                }
                catch (SFSException)
                {
                    return false;
                }
                return true;
            }
            if (!cmd.Equals("unmount", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            try
            {
                Editor.SFS.SFS.UnMount(path);
            }
            catch (SFSException)
            {
                return false;
            }
            return true;
        }
    }
}

