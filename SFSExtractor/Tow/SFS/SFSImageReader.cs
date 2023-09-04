namespace Editor.SFS
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class SFSImageReader
    {
        [DllImport(@"..\core.dll", EntryPoint="CS_SFSImageReader_Close", CharSet=CharSet.Ansi)]
        private static extern void CloseExtern(int handle);
        [DllImport(@"..\core.dll", EntryPoint="CS_SFSImageReader_GetSize", CharSet=CharSet.Ansi)]
        private static extern int GetSizeExtern(int handle);
        public static Stream Load(string fileName)
        {
            Stream stream;
            int handle = OpenExtern(fileName);
            if (-1 == handle)
            {
                throw new FileNotFoundException("Cannot open the file " + fileName);
            }
            try
            {
                int sizeExtern = GetSizeExtern(handle);
                byte[] buf = new byte[sizeExtern];
                if (sizeExtern > 0)
                {
                    ReadExtern(handle, buf, buf.Length);
                }
                stream = new MemoryStream(buf);
            }
            finally
            {
                CloseExtern(handle);
            }
            return stream;
        }

        [DllImport(@"..\core.dll", EntryPoint="CS_SFSImageReader_Open", CharSet=CharSet.Ansi)]
        private static extern int OpenExtern(string name);
        [DllImport(@"..\core.dll", EntryPoint="CS_SFSImageReader_Read", CharSet=CharSet.Ansi)]
        private static extern int ReadExtern(int handle, byte[] buf, int size);
    }
}

