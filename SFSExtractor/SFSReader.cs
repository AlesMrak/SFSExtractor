namespace Editor.SFS
{
    using System;
    using System.IO;
    using System.Text;

    internal class SFSReader : StreamReader
    {
        public SFSReader(string path) : base(new SFSStream(path))
        {
        }

        public SFSReader(string path, Encoding encoding) : base(new SFSStream(path), encoding)
        {
        }

        public SFSReader(string path, int[] kryptoKey) : base(new KryptoInputFilter(new SFSStream(path), kryptoKey))
        {
        }

        public SFSReader(string path, Encoding encoding, int[] kryptoKey) : base(new KryptoInputFilter(new SFSStream(path), kryptoKey), encoding)
        {
        }
    }
}

