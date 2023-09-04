namespace Editor.SFS
{
    using System;
    using System.IO;

    internal class KryptoInputFilter : Stream
    {
        private Stream inStream;
        private int[] key;
        private int sw;

        public KryptoInputFilter(Stream inStream)
        {
            this.key = new int[] { 0xff, 170 };
            this.inStream = inStream;
            this.sw = 0;
        }

        public KryptoInputFilter(Stream inStream, int[] key) : this(inStream)
        {
            this.key = key;
            if ((key != null) && (key.Length == 0))
            {
                this.key = null;
            }
            this.sw = 0;
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public int[] kryptoGetKey()
        {
            return this.key;
        }

        public void kryptoResetSwitch()
        {
            this.sw = 0;
        }

        public void kryptoSetKey(int[] newKey)
        {
            this.key = newKey;
            if ((this.key != null) && (this.key.Length == 0))
            {
                this.key = null;
            }
            this.sw = 0;
        }

        public override int Read(byte[] b, int off, int len)
        {
            int num = this.inStream.Read(b, off, len);
            if ((this.key != null) && (num > 0))
            {
                for (int i = 0; i < num; i++)
                {
                    this.sw = (this.sw + 1) % this.key.Length;
                    b[off + i] = (byte) (b[off + i] ^ this.key[this.sw]);
                }
            }
            return num;
        }

        public override int ReadByte()
        {
            int num = this.inStream.ReadByte();
            if (this.key != null)
            {
                this.sw = (this.sw + 1) % this.key.Length;
                if (num != -1)
                {
                    num ^= this.key[this.sw];
                }
            }
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.inStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get
            {
                return this.inStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.inStream.CanSeek;
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
                return this.inStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.inStream.Position;
            }
            set
            {
                this.inStream.Position = value;
            }
        }
    }
}

