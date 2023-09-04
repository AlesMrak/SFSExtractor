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
    public partial class ExtractManager
    {
        public int GetItemsCount()
        {
            int count = 0;

            

            if (Config.Root!=null && Config.Root.Dirs != null)
            {
                foreach (sfsDir dir in Config.Root.Dirs)
                {
                    Count(dir,ref count);
                }
            }

            return count;
        }

        public void Count(sfsDir Dir, ref int count)
        {
            if (Dir.Files != null)
            {
                foreach (sfsFile file in Dir.Files)
                {
                    if (file.Enabled == true)
                    {
                        count++;
                    }
                }
            }
            if (Dir.Dirs != null)
            {
                foreach (sfsDir dir in Dir.Dirs)
                {
                    if (dir.Enabled == true)
                    {
                        count++;
                    }
                    Count(dir, ref count);
                }
            }
        }

        public void Extract()
        {
            string rootPath = gConfig.ExtractDir;

            if (Directory.Exists(rootPath) == false)
            {
                Directory.CreateDirectory(rootPath);
            }

            if (Config.Root.Dirs != null)
            {
                foreach (sfsDir dir in Config.Root.Dirs)
                {
                    Extract(dir, rootPath);
                }
            }

        }

        public void Extract(sfsDir Dir, string rootPath)
        {
            string fullPath = rootPath + "\\" + Dir.Name;
            if (Dir.Enabled==true && Directory.Exists(fullPath)==false)
            {
                Directory.CreateDirectory(fullPath);
                if (OnProcessItem != null) OnProcessItem();
            }
            if (Dir.Files != null)
            {
                foreach (sfsFile file in Dir.Files)
                {
                    if (file.Enabled == true)
                    {
                        /// Create File
                        ///
                        try
                        {
                            if (file.IsText == true)
                            {
                                CreateText(file, rootPath);
                            }
                            else
                            {
                                CreateBinary(file, rootPath);
                            }
                        }
                        catch (Exception e)
                        {
                            _log.Fatal("Exception==>", e);
                        }
                        if (OnProcessItem != null) OnProcessItem();
                    }
                }
            }
            if (Dir.Dirs != null)
            {
                foreach (sfsDir dir in Dir.Dirs)
                {
                    //if(dir.Enabled == true)
                     Extract(dir, fullPath);
                }
            }

        }

        public void CreateBinary(sfsFile file, string rootPath)
        {
            string err;
            try
            {
                if (File.Exists(gConfig.ExtractDir + "\\" + file.FullPath) == false || Override == true)
                {
                    FileStream outStream = File.Create(gConfig.ExtractDir + "\\" + file.FullPath);

                    SFSStream stream = new SFSStream(@"..\" + file.FullPath);

                    BinaryWriter bw = new BinaryWriter(outStream);
                    
                    long len = stream.Length;

                    byte[] ch = new byte[len];

                    stream.Read(ch, 0, (int)len);
                    
                    bw.Write(ch);

                    stream.Close();
                    bw.Flush();
                    bw.Close();

                    if (outStream != null) outStream.Close();
                }
            }
            catch (Exception exception)
            {
                _log.Fatal("Exception==>", exception);
                err = exception.ToString();
            }


        }

        public void CreateText(sfsFile file, string rootPath)
        {
            string err;
            try
            {
                if (File.Exists(gConfig.ExtractDir + "\\"+ file.FullPath) == false || Override == true)
                {
                    StreamReader reader = new SFSReader(@"..\" + file.FullPath, Encoding.Default); //Info

                    string text = reader.ReadToEnd();
                    StreamWriter fs = new StreamWriter(gConfig.ExtractDir + "\\" + file.FullPath);
                    fs.Write(text);
                    fs.Close();

                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                _log.Fatal("Exception==>", exception);
                err = exception.ToString();
            }


        }
    }
}
