namespace Editor.SFS
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public class SFSStream2 : Stream, IDisposable
    {
        private int _fd = -1;
        private FileStream _fis;
        private readonly string _name;
        private const int EXTERN_SEEK_CUR = 1;
        private const int EXTERN_SEEK_END = 2;
        private const int EXTERN_SEEK_SET = 0;

        public SFSStream2(string name)
        {
            this._name = name;
            if (name == null)
            {
                throw new FileNotFoundException("file name == null");
            }
            int length = name.Length;
            for (int i = 0; i < length; i++)
            {
                if (name[i] >= '\x0080')
                {
                    this._fis = new FileStream(HomePath.toFileSystemName(name, 0), FileMode.Open, FileAccess.Read);
                    return;
                }
            }
            this._fd = Open(name,null);
            if (this._fd < 0)
            {
                throw new FileNotFoundException(name);
            }
        }

        public override void Close()
        {
            if (this._fis != null)
            {
                this._fis.Close();
                this._fis = null;
            }
            if (this._fd != -1)
            {
                Close(this._fd);
                this._fd = -1;
            }
        }

        [DllImport(@"..\rts.dll", EntryPoint="CS_SFSInputStream_Close", CharSet=CharSet.Ansi)]
        private static extern void Close(int f);
        public static bool Exists(string name)
        {
            int f = Open(name,null);
            if (f < 0)
            {
                return false;
            }
            Close(f);
            return true;
        }

        ~SFSStream2()
        {
            this.Close();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        [DllImport(@"..\rts.dll", EntryPoint="CS_SFSInputStream_GetLength", CharSet=CharSet.Ansi)]
        private static extern int GetLength(int f);
        [DllImport(@"..\rts.dll", EntryPoint = "_SFS_open@8", CharSet = CharSet.Ansi)]
        private static extern int Open(string name,string x);
        [DllImport(@"..\rts.dll", EntryPoint="CS_SFSInputStream_Read", CharSet=CharSet.Ansi)]
        private static extern int Read(int f);
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._fis != null)
            {
                return this._fis.Read(buffer, offset, count);
            }
            if (-1 == this._fd)
            {
                throw new ObjectDisposedException(this._name);
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset + count) > buffer.Length)
            {
                throw new ArgumentException(string.Format("buffer.Length > offset + count: bufferLength = {0}, offset = {1}, count = {2}", buffer.Length, offset, count));
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", count, "argument is negative");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "argument is negative");
            }
            return ReadBytes(this._fd, buffer, offset, count);
        }

        public override int ReadByte()
        {
            if (this._fis != null)
            {
                return this._fis.ReadByte();
            }
            if (-1 == this._fd)
            {
                throw new ObjectDisposedException(this._name);
            }
            return Read(this._fd);
        }

        [DllImport(@"..\rts.dll", EntryPoint="CS_SFSInputStream_ReadBytes", CharSet=CharSet.Ansi)]
        private static extern int ReadBytes(int f, byte[] b, int off, int len);
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (this._fis != null)
            {
                return this._fis.Seek(offset, origin);
            }
            if (-1 == this._fd)
            {
                throw new ObjectDisposedException(this._name);
            }
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset < 0)
                    {
                        throw new ArgumentException("offset < 0");
                    }
                    return (long) Seek(this._fd, (int) offset, 0);

                case SeekOrigin.Current:
                    return (long) Seek(this._fd, (int) offset, 1);

                case SeekOrigin.End:
                    return (long) Seek(this._fd, (int) offset, 2);
            }
            throw new NotSupportedException("Unknown origin " + origin);
        }

        [DllImport(@"..\rts.dll", EntryPoint="CS_SFSInputStream_Seek", CharSet=CharSet.Ansi)]
        private static extern int Seek(int f, int offset, int origin);
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                if (this._fis != null)
                {
                    return this._fis.Length;
                }
                return (long) GetLength(this._fd);
            }
        }

        public override long Position
        {
            get
            {
                return this.Seek((long) 0, SeekOrigin.Current);
            }
            set
            {
                this.Seek(value, SeekOrigin.Begin);
            }
        }
    }
}

