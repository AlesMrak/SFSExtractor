using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using Editor.Utils;
using Editor.SFS;
using Editor.JavaClasses;
using log4net;
namespace SFSExtractor
{
    public class KeyValue
    {
        public string Name;
        public string Value;

        public KeyValue(string n, string v)
        {
            Name = n;
            Value = v;
        }
    }

    public class Category
    {
        private static ILog _log = LogManager.GetLogger(typeof(Category));

        public string Name;

        public Category(string n)
        {
            Name = n;
        }

        public ArrayList aKeys = new ArrayList();
        public Hashtable hKeys = new Hashtable();

        public void AddKey(string Key, string value)
        {
            if (hKeys.ContainsKey(Key.ToLower()) == true)
                return;
            KeyValue kval = new KeyValue(Key, value);

            aKeys.Add(kval);
            hKeys.Add(kval.Name.ToLower(), kval);
        }

        public KeyValue GetValue(string key)
        {
            return (KeyValue)hKeys[key.ToLower()];
        }

        public void ParseLine(string line)
        {
            int index = 0;
            bool flag = false;
            while (index < line.Length)
            {
                if (char.IsWhiteSpace(line, index))
                {
                    flag = true;
                    break;
                }
                index++;
            }
            if (flag)
            {
                string keyName = line.Substring(0, index);
                flag = false;
                while (index < line.Length)
                {
                    if (!char.IsWhiteSpace(line, index))
                    {
                        flag = true;
                        break;
                    }
                    index++;
                }
                string text2 = null;
                if (flag)
                {
                    text2 = line.Substring(index);
                }
                else
                {
                    text2 = keyName;
                }
                this.AddKey(keyName, text2);
            }
            else
            {
                this.AddKey(line, "");
            }
        }

    }

    public class IniReader
    {
        private static ILog _log = LogManager.GetLogger(typeof(IniReader));

        public string FileName;
        public bool IsSFS;
        public ArrayList simpleComments = new ArrayList();

        public ArrayList aCategory = new ArrayList();
        public Hashtable hCategory = new Hashtable();


        public Category GetCategory(string name)
        {
            return (Category)hCategory[name.ToLower()];
        }

        public Category AddCategory(string name)
        {
            if (hCategory.ContainsKey(name.ToLower()) == true)
                return null; ;
            Category cat = new Category(name);

            aCategory.Add(cat);
            hCategory.Add(cat.Name.ToLower(), cat);

            return cat;
        }

        public IniReader()
        {
            simpleComments.Add("//");
        }
        public bool Load(string fileName, bool sfs)
        {
            //_log.InfoFormat("Loading ini file {0}", fileName);
            FileName = fileName;
            IsSFS = sfs;

            if(fileName.EndsWith(".prs")==true)
            {
                int dd = 1 + 1;
                dd++;
            }

            if (sfs == true)
            {
                StreamReader reader = null;
                try
                {
                    reader = new SFSReader(fileName, Encoding.Default);
                }
                catch (Exception exception)
                {
                    _log.Fatal("Exception==>", exception);
                    return false;   
                }

                string line = null;
                Category category = null;
                while ((line = reader.ReadLine()) != null)
                {
                    this.PrepareString(ref line);
                    if ((line.Length == 0) || this.IsComment(line))
                    {
                        continue;
                    }

                    if (this.IsSection(line))
                    {
                        string sectionName = this.ParseSectionName(line);
                        category = AddCategory(sectionName);
                    }
                    else
                    {
                        if (category == null)
                        {
                            category = AddCategory("");
                        }
                        category.ParseLine(line);
                    }
                }
                reader.Close();
            }

            return true;
        }


        public bool IsComment(string line)
        {
            for (int i = 0; i < this.simpleComments.Count; i++)
            {
                if (line.StartsWith((string)this.simpleComments[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsSection(string line)
        {
            if ((line.Length > 2) && (line[0] == '['))
            {
                return (line[line.Length - 1] == ']');
            }
            return false;
        }

        protected virtual string ParseSectionName(string line)
        {
            return line.Substring(1, line.Length - 2);
        }

        protected virtual void PrepareString(ref string line)
        {
            this.TrimCommentsAtEnd(ref line);
            line = line.Trim();
        }

        protected virtual void TrimCommentsAtEnd(ref string line)
        {
            int length = -1;
            for (int i = 0; i < this.simpleComments.Count; i++)
            {
                length = line.IndexOf((string)this.simpleComments[i]);
                if (length > 0)
                {
                    line = line.Substring(0, length);
                    return;
                }
            }
        }

        public string GetValue(string cat, string key)
        {
            Category category = (Category)hCategory[cat.ToLower()];

            if (category != null)
            {
                KeyValue val = category.GetValue(key);
                if (val != null)
                    return val.Value;
            }

            return string.Empty;
        }

        public ArrayList GetValues(string key)
        {
            ArrayList list = new ArrayList();

            for (int i = 0; i < aCategory.Count; i++)
            {
                Category category = (Category)aCategory[i];

                KeyValue kv = category.GetValue(key);
                if (kv != null)
                {
                    list.Add(kv.Value);
                }
            }
            return list;
        }

        public ArrayList GetValuesContain(string partKey)
        {
            ArrayList list = new ArrayList();

            for (int i = 0; i < aCategory.Count; i++)
            {
                Category category = (Category)aCategory[i];
                for (int j = 0; j < category.aKeys.Count; j++)
                {
                    KeyValue kv = (KeyValue)category.aKeys[j];
                    if (kv != null)
                    {
                        if(kv.Name.Contains(partKey)==true)
                            list.Add(kv.Value);
                    }
                }
            }
            return list;
        }
    }
}
