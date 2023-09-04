namespace Editor.SFS
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class HomePath
    {
        private static char notSeparator;

        static HomePath()
        {
            if (Path.PathSeparator == '/')
            {
                notSeparator = '\\';
            }
            else
            {
                notSeparator = '/';
            }
            Remove(Path.GetFullPath(Path.GetDirectoryName(@"..\rts.dll")));
            Add(Path.GetFullPath("."));
        }

        public static void Add(string path)
        {
            AddExtern(Path.GetFullPath(path));
        }

        [DllImport(@"..\rts.dll", EntryPoint="CS_SFS_HomePath_add", CharSet=CharSet.Ansi)]
        private static extern void AddExtern(string path);
        public static string concatNames(string parentName, string fileName)
        {
            char ch;
            if (isFileSystemName(fileName))
            {
                return fileName;
            }
            StringBuilder builder = new StringBuilder(parentName);
            int length = builder.Length;
            int num2 = -1;
            for (int i = 0; i < length; i++)
            {
                ch = char.ToLower(builder[i]);
                switch (ch)
                {
                    case '/':
                    case '\\':
                        ch = '/';
                        num2 = i;
                        break;
                }
                builder[i] = ch;
            }
            if ((num2 >= 0) && ((num2 + 1) < length))
            {
                builder.Remove(num2 + 1, length);
            }
            int startIndex = 0;
            length = fileName.Length;
            while (startIndex < length)
            {
                ch = fileName[startIndex];
                if (ch == '.')
                {
                    startIndex++;
                    if (startIndex != length)
                    {
                        ch = fileName[startIndex];
                        if (ch == '.')
                        {
                            startIndex++;
                            if (startIndex == length)
                            {
                                break;
                            }
                            ch = fileName[startIndex];
                            if ((ch == '\\') || (ch == '/'))
                            {
                                num2--;
                                while (num2 >= 0)
                                {
                                    if (builder[num2] == '/')
                                    {
                                        break;
                                    }
                                    num2--;
                                }
                                if (num2 < 0)
                                {
                                    return null;
                                }
                                builder.Remove(num2 + 1, builder.Length);
                            }
                            goto Label_0105;
                        }
                        if ((ch == '\\') || (ch == '/'))
                        {
                            goto Label_0105;
                        }
                        startIndex--;
                    }
                    break;
                }
                if (ch >= ' ')
                {
                    break;
                }
            Label_0105:
                startIndex++;
            }
            if (startIndex == length)
            {
                return null;
            }
            if (startIndex > 0)
            {
                builder.Append(fileName.Substring(startIndex));
            }
            else
            {
                builder.Append(fileName);
            }
            length = builder.Length;
            if (num2 < 0)
            {
                num2 = 0;
            }
            while (num2 < length)
            {
                ch = builder[num2];
                if (ch == '\\')
                {
                    builder[num2] = '/';
                }
                num2++;
            }
            return builder.ToString();
        }

        public static string Get(int idx)
        {
            return GetExtern(idx);
        }

        [DllImport(@"..\rts.dll", EntryPoint="CS_SFS_HomePath_get", CharSet=CharSet.Ansi)]
        private static extern string GetExtern(int idx);
        public static bool isFileSystemName(string fileName)
        {
            switch (fileName[0])
            {
                case '/':
                case '\\':
                    return true;
            }
            if (fileName.Length > 1)
            {
                char ch = fileName[1];
                if (ch == ':')
                {
                    return true;
                }
            }
            return false;
        }

        public static IList list()
        {
            IList list = new ArrayList();
            int idx = 0;
            while (true)
            {
                string text = Get(idx);
                if (text == null)
                {
                    return list;
                }
                list.Add(text);
                idx++;
            }
        }

        public static void Remove(string path)
        {
            RemoveExtern(Path.GetFullPath(path));
        }

        [DllImport(@"..\rts.dll", EntryPoint="CS_SFS_HomePath_remove", CharSet=CharSet.Ansi)]
        private static extern void RemoveExtern(string path);
        private static string subNames(string fsParentName, string fsName, bool bIncludeFullParentName)
        {
            int num4;
            int num = fsParentName.Length - 1;
            while (num >= 0)
            {
                char ch = fsParentName[num];
                if ((ch == '/') || (ch == '\\'))
                {
                    num++;
                    break;
                }
                num--;
            }
            if (num <= 0)
            {
                num = 0;
            }
            int length = fsName.Length - 1;
            while (length >= 0)
            {
                char ch2 = fsName[length];
                if ((ch2 == '/') || (ch2 == '\\'))
                {
                    length++;
                    break;
                }
                length--;
            }
            if (length == fsName.Length)
            {
                return null;
            }
            if (length <= 0)
            {
                length = 0;
            }
            int num3 = 0;
            while ((num3 < num) && (num3 < length))
            {
                char ch3 = char.ToLower(fsParentName[num3]);
                if (ch3 == '\\')
                {
                    ch3 = '/';
                }
                char ch4 = char.ToLower(fsName[num3]);
                if (ch4 == '\\')
                {
                    ch4 = '/';
                }
                if (ch3 != ch4)
                {
                    break;
                }
                num3++;
            }
            if ((num3 != num) && bIncludeFullParentName)
            {
                return null;
            }
            if (num3 < num)
            {
                while (num3 > 0)
                {
                    switch (char.ToLower(fsParentName[num3]))
                    {
                        case '\\':
                        case '/':
                            num3++;
                            goto Label_00F9;
                    }
                    num3--;
                }
            }
        Label_00F9:
            num4 = num3;
            StringBuilder builder = new StringBuilder();
            while (num3 < num)
            {
                switch (char.ToLower(fsParentName[num3]))
                {
                    case '\\':
                    case '/':
                        builder.Append("../");
                        break;
                }
                num3++;
            }
            num3 = num4;
            length = fsName.Length;
            while (num3 < length)
            {
                char ch7 = char.ToLower(fsName[num3]);
                if (ch7 == '\\')
                {
                    ch7 = '/';
                }
                builder.Append(ch7);
                num3++;
            }
            return builder.ToString();
        }

        public static string toFileSystemName(string fileName, int iPath)
        {
            if (isFileSystemName(fileName))
            {
                return fileName.Replace(notSeparator, Path.PathSeparator);
            }
            string parentName = Get(iPath);
            if (parentName == null)
            {
                return null;
            }
            char ch = parentName[parentName.Length - 1];
            string text2 = null;
            if ((ch != '/') && (ch != '\\'))
            {
                text2 = concatNames(parentName + '/', fileName);
            }
            else
            {
                text2 = concatNames(parentName, fileName);
            }
            if (text2 != null)
            {
                text2 = text2.Replace(notSeparator, Path.PathSeparator);
            }
            return text2;
        }

        public static string toFileSystemName(string parentFileName, string fileName, int iPath)
        {
            if (isFileSystemName(fileName))
            {
                return fileName.Replace(notSeparator, Path.PathSeparator);
            }
            if (!isFileSystemName(parentFileName))
            {
                parentFileName = toFileSystemName(parentFileName, iPath);
            }
            if (parentFileName == null)
            {
                return null;
            }
            string text = concatNames(parentFileName, fileName);
            if (text != null)
            {
                text = text.Replace(notSeparator, Path.PathSeparator);
            }
            return text;
        }

        public static string toLocalName(string fsName, int iPath)
        {
            if (!isFileSystemName(fsName))
            {
                return fsName;
            }
            string fsParentName = Get(iPath);
            if (fsParentName == null)
            {
                return null;
            }
            char ch = fsParentName[fsParentName.Length - 1];
            if ((ch != '/') && (ch != '\\'))
            {
                return subNames(fsParentName + '/', fsName, true);
            }
            return subNames(fsParentName, fsName, true);
        }

        public static string toLocalName(string fsParentName, string fsName, int iPath)
        {
            fsParentName = toLocalName(fsParentName, iPath);
            fsName = toLocalName(fsName, iPath);
            if ((fsParentName != null) && (fsName != null))
            {
                return subNames(fsParentName, fsName, false);
            }
            return null;
        }
    }
}

