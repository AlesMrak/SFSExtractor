namespace Editor.SFS
{
    using System;

    internal class SFSException : ApplicationException
    {
        public SFSException()
        {
        }

        public SFSException(string s) : base(s)
        {
        }
    }
}

