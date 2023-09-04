using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml.Serialization;
using log4net;


namespace SFSExtractor
{
    [Serializable]
    public enum TowTypeDir
    {
        unknown = 0,
        MapDir = 1,
        UnitDir = 2,
        SoundPresent = 3,
        ItemDir=4,
        AmmoDir=5,
    }

    [Serializable]
    public class sfsFile
    {
        public string Name;
        public string FullPath;
        public bool IsText = false;
        public bool Enabled;

        [XmlIgnore]
        public sfsDir Parent = null;
    }

    [Serializable]
    public class sfsDir
    {
        private static ILog _log = LogManager.GetLogger(typeof(sfsDir));
        
        public string Name;
        public TowTypeDir type;
        public ArrayList Dirs = null;
        public ArrayList Files = null;
        public string FullPath;
        public bool Enabled;

        [XmlIgnore]
        public sfsDir Parent = null;


        public void Clear()
        {
            if (Dirs != null)
            {
                for (int i = 0; i < Dirs.Count; i++)
                {
                    sfsDir dir = (sfsDir)Dirs[i];
                    dir.Clear();
                }
                Dirs.Clear();
            }
            
            if(Files!=null) Files.Clear();
        }

        public sfsDir AddDir(string directoryName,TowTypeDir type)
        {
            if (Dirs == null) Dirs = new ArrayList();

            for (int i = 0; i < Dirs.Count; i++)
            {
                sfsDir dir = (sfsDir)Dirs[i];
                if (directoryName.Equals(dir.Name, StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return dir;
                }
            }

           
            sfsDir newDir = new sfsDir();
            newDir.Name = directoryName;
            newDir.type = type;
            newDir.Parent = this;
            newDir.Enabled = true;
            Dirs.Add(newDir);
            return newDir;
        }

        public void AddDirWithPath(string paths, TowTypeDir type)
        {
            char[] sep ={ '\\', '/' };
            string[] dirs = paths.Split(sep);

            if (dirs.Length > 0)
            {
                if (dirs.Length == 1)
                {
                    sfsDir child = AddDir(dirs[0], type);
                }
                else
                {
                    sfsDir newDir = this.AddDir(dirs[0], TowTypeDir.unknown);
                    if (newDir != null)
                    {
                        string[] dirs2 = new string[dirs.Length - 1];
                        for (int i = 1; i < dirs.Length; i++)
                        {
                            dirs2[i - 1] = dirs[i];
                        }
                        newDir.AddDir(dirs2, type);
                    }
                }
            }
        }
        public void AddDir(string[] dirs, TowTypeDir type)
        {
            if (dirs.Length > 0)
            {
                if (dirs.Length == 1)
                {
                    AddDir(dirs[0], type);
                }
                else
                {
                    sfsDir newDir = this.AddDir(dirs[0], TowTypeDir.unknown);
                    if (newDir != null)
                    {
                        string[] dirs2 = new string[dirs.Length-1];
                        for(int i=1;i<dirs.Length;i++)
                        {
                            dirs2[i - 1] = dirs[i];
                        }
                        
                        newDir.AddDir(dirs2, type);
                    }
                }
            }
        }
        public sfsFile AddFile(string filename,bool bin,string ffile)
        {
            if (Files == null) Files = new ArrayList();

            for (int i = 0; i < Files.Count; i++)
            {
                sfsFile file = (sfsFile)Files[i];
                if (filename.Equals(file.Name, StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return file;
                }
            }
            sfsFile newFile = new sfsFile();
            newFile.Name = filename;
            newFile.FullPath = ffile;
            newFile.IsText = !bin;
            newFile.Parent = this;
            newFile.Enabled = true;
            Files.Add(newFile);
            return newFile;
        }

        public void GetFullPath(ref string path)
        {
            if (Parent != null)
            {
                Parent.GetFullPath(ref path);

                if(Parent.Parent!=null)
                 path += "\\"+Name;
                else
                 path += Name;
            }
        }

        public void AddFileWithPath(string path,bool bin,string file)
        {
            char [] sep ={ '\\','/'};
            string[] paths = path.Split(sep);

            AddFile(paths,bin,file);

        }
        public void AddFile(string[] path,bool bin,string file)
        {
            if (path.Length > 0)
            {
                if (path.Length == 1)
                {
                    AddFile(path[0],bin,file);
                }
                else
                {
                    sfsDir dir = AddDir(path[0],TowTypeDir.unknown);
                    if (dir != null)
                    {
                        string[] path2 = new string[path.Length - 1];
                        for (int i = 1; i < path.Length; i++)
                        {
                            path2[i - 1] = path[i];
                        }
                        dir.AddFile(path2,bin,file);
                    }
                }
            }
        }

    }

    [Serializable]
    public class sfsConfig
    {
        public sfsDir Root;
        

        public void Clear()
        {
            if(Root!=null) Root.Clear();
        }
    }

    public class extractConfig
    {
        public string sfsPath;
        public string ExtractDir;
        public List<string> Mounts = new List<string>();
    }
}

