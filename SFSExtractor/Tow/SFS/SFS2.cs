namespace Editor.SFS
{
    using System;
    using System.Runtime.InteropServices;

    public sealed class SFS2
    {
        public const string corePath = @"..\core.dll";
        public const int FLAG_INTERNAL_BUFFERING = 2;
        public const int FLAG_MAPPING = 3;
        public const int FLAG_NO_BUFFERING = 1;
        public const int FLAG_SYSTEM_BUFFERING = 0;
        public const string rtsPath = @"..\rts.dll";

        public static void Mount(string path)
        {
            if (-1 == MountExtern(path, 0))
            {
                throw new SFSException(SfsErrorExtern(SfsErrnoExtern()) + ": " + path);
            }
        }

        public static void MountAs(string path, string asPath)
        {
            if (-1 == MountAsExtern(path, asPath,0, 0))
            {
                throw new SFSException(SfsErrorExtern(SfsErrnoExtern()) + ": " + path);
            }
        }

        [DllImport(@"..\rts.dll", EntryPoint = "_SFS_MountAs@16", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int MountAsExtern(string path,string aspath,int x, int flag);
        [DllImport(@"..\rts.dll", EntryPoint="_SFS_mount", CharSet=CharSet.Ansi)]
        private static extern int MountExtern(string path, int flag);
        [DllImport(@"..\rts.dll", EntryPoint="_SFS_errno", CharSet=CharSet.Ansi)]
        private static extern int SfsErrnoExtern();
        [DllImport(@"..\rts.dll", EntryPoint="_SFS_error", CharSet=CharSet.Ansi)]
        private static extern string SfsErrorExtern(int err);
        public static void UnMount(string path)
        {
            if (-1 == UnMountExtern(path))
            {
                throw new SFSException(SfsErrorExtern(SfsErrnoExtern()) + ": " + path);
            }
        }

        [DllImport(@"..\rts.dll", EntryPoint = "_SFS_UnMount@4", CharSet = CharSet.Ansi)]
        private static extern int UnMountExtern(string path);
    }
}

