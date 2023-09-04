namespace Editor.Utils
{
    using System;
    using System.IO;

    public static class JavaPath
    {
        private const char dirSeparator = '/';
        private const char dirSeparatorAlternative = '\\';

        public static string AddPrefix(string prefix, string dir)
        {
            string text = RemoveSeparators(Prepare(prefix));
            string text2 = RemoveSeparators(Prepare(dir));
            if (!text2.StartsWith(text))
            {
                text2 = Path.Combine(prefix, dir);
            }
            return text2;
        }

        public static bool endsWithSeparator(string path)
        {
            bool flag = false;
            if ((path != null) && (path.Length > 0))
            {
                char ch = path[path.Length - 1];
                flag = (ch == '/') || (ch == '\\');
            }
            return flag;
        }

        public static string Prepare(string path)
        {
            if (path == null)
            {
                path = "";
            }
            path = path.Replace('\\', '/');
            path = path.ToLower();
            return path;
        }

        public static string removeEndSeparator(string path)
        {
            if (((path != null) && (path.Length > 1)) && endsWithSeparator(path))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }

        public static string RemoveSeparators(string path)
        {
            path = removeStartSeparator(path);
            path = removeEndSeparator(path);
            return path;
        }

        public static string removeStartSeparator(string path)
        {
            if (((path != null) && (path.Length > 0)) && startsWithSeparator(path))
            {
                path = path.Substring(1);
            }
            return path;
        }

        public static bool startsWithSeparator(string path)
        {
            bool flag = false;
            if ((path != null) && (path.Length > 0))
            {
                char ch = path[0];
                flag = (ch == '/') || (ch == '\\');
            }
            return flag;
        }
    }
}

