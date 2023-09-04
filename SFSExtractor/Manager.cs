using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

using Editor.Utils;
using Editor.SFS;
using Editor.JavaClasses;
using log4net;

namespace SFSExtractor
{
    public delegate void ProcessedItem();

    public partial class ExtractManager
    {
        private static ILog _log = LogManager.GetLogger(typeof(ExtractManager));

        public event ProcessedItem OnProcessItem = null;

        public static ExtractManager Global = new ExtractManager();

        public sfsConfig Config = new sfsConfig();
        public extractConfig gConfig = new extractConfig();

        private static readonly string _itemPrefix = JavaPath.Prepare("data/items/");

        public FilesID filesId = null;
        public string OldCurrent = string.Empty;
        public bool Override = false;


        public void Refresh()
        {
            Config.Clear();
            Search();
        }
        public void Init()
        {
            _log.InfoFormat("Initing Manager");
            
            LoadConfig("Config.xml");

            //LoadXML("Config.xml");
            //Config.sfsPath = "D:\\Games\\Theatre of War\\MissionEditor";
            //Config.ExtractDir = "d:\\TOW";

            //Config.sfsPath = "c:\\Users\\Ales\\Theatre of War\\MissionEditor";
            //Config.ExtractDir = "c:\\Users\\Ales\\TOW";
                       

            OldCurrent = Directory.GetCurrentDirectory();

            if (gConfig.sfsPath != null && gConfig.sfsPath !=string.Empty)
            {
                Directory.SetCurrentDirectory(gConfig.sfsPath);
                _log.InfoFormat("Set working dir at {0}", gConfig.sfsPath);
                Mount();

                foreach (string s in gConfig.Mounts)
                {
                    try
                    {
                        MountFile(s);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }

                try
                {
                    Directory.SetCurrentDirectory(OldCurrent);
                    LoadFiles();
                    Directory.SetCurrentDirectory(gConfig.sfsPath);
                }
                catch (Exception e)
                {
                    _log.Fatal("Exception==>", e);
                }

                try
                {
                    filesId = FilesID.getInstance();

                    Search();
                }
                catch (Exception e)
                {
                    _log.FatalFormat("Exception==>{0}", e.ToString());
                }

            }

            
        }

        public void LoadFiles()
        {
            _log.InfoFormat("Loading  Filelist.txt");
            if (File.Exists("Filelist.txt") == false) return;

            StreamReader reader = File.OpenText("Filelist.txt");
            if (reader == null) return;

            if (Config.Root == null)
            {
                Config.Root = new sfsDir();
            }

            string line="";

            do
            {
                line = reader.ReadLine();
                Add2(line.Trim());

            }
            while (reader.EndOfStream == false);
                   

        }

        public void DeInit()
        {
            UnMount();

            Directory.SetCurrentDirectory(OldCurrent);
            SaveConfig("Config.xml");
        }

        public void UnMount()
        {
            try
            {


                _log.InfoFormat("Deiniting sfs's");
                bool res = false;
                res = CmdSfs.ExecCommand("UNMOUNT", "maps.sfs", null);
                res = CmdSfs.ExecCommand("UNMOUNT", "maps2.sfs", null);
                res = CmdSfs.ExecCommand("UNMOUNT", "3dobj.sfs", null);
                res = CmdSfs.ExecCommand("UNMOUNT", "data.sfs", null);
                res = CmdSfs.ExecCommand("UNMOUNT", "models.sfs", null);
                res = CmdSfs.ExecCommand("UNMOUNT", "sounds.sfs", null);
                res = CmdSfs.ExecCommand("UNMOUNT", "patch1.sfs", null);
                res = CmdSfs.ExecCommand("UNMOUNT", "patch2.sfs", null);
            }
            catch(Exception ex)
            {
                _log.FatalFormat("Exception==>{0}",ex);
            }
        }

        public bool UnMountFile(string file)
        {
            _log.InfoFormat("Initing sfs's");
            bool res = false;
            res = CmdSfs.ExecCommand("UNMOUNT", file, null);
            return res;
        }

        public void Mount()
        {
            _log.InfoFormat("Deiniting sfs's");
            bool res = false;

            try
            {
                res = CmdSfs.ExecCommand("MOUNT", "maps.sfs", null);
                _log.InfoFormat("mounting maps.sfs {0}", res);
                res = CmdSfs.ExecCommand("MOUNT", "maps2.sfs", null);
                _log.InfoFormat("mounting maps2.sfs {0}", res);
                res = CmdSfs.ExecCommand("MOUNT", "3dobj.sfs", null);
                _log.InfoFormat("mounting 3dobj.sfs {0}", res);
                res = CmdSfs.ExecCommand("MOUNT", "data.sfs", null);
                _log.InfoFormat("mounting data.sfs {0}", res);
                res = CmdSfs.ExecCommand("MOUNT", "models.sfs", null);
                _log.InfoFormat("mounting models.sfs {0}", res);
                res = CmdSfs.ExecCommand("MOUNT", "sounds.sfs", null);
                _log.InfoFormat("mounting sounds.sfs {0}", res);
                res = CmdSfs.ExecCommand("MOUNT", "patch1.sfs", null);
                _log.InfoFormat("mounting patch1sfs {0}", res);
                res = CmdSfs.ExecCommand("MOUNT", "patch2.sfs", null);
                _log.InfoFormat("mounting patch2.sfs {0}", res);
            }
            catch (Exception e)
            {
                _log.Fatal(String.Format("CurrentDir:{0}\r\n{1}",Directory.GetCurrentDirectory(), e.ToString()));
                //throw e;
            }
        }

        public bool MountFile(string file)
        {
            _log.InfoFormat("Initing sfs");
            bool res = false;


            try
            {
                Directory.SetCurrentDirectory(gConfig.sfsPath);
                
                res = CmdSfs.ExecCommand("MOUNT", file, null);
                _log.InfoFormat("mounting {1} {0}", res, file);
                return res;
            }
            catch (Exception e)
            {
                _log.Fatal(String.Format("CurrentDir:{0}\r\n{1}", Directory.GetCurrentDirectory()), e);
                throw e;
            }
        }


        public bool LoadXML(string fileName)
        {
            _log.InfoFormat("Loading XML from {0}", fileName);
            FileStream dataFile = null;
            bool res = true;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(sfsConfig), new Type[] { typeof(sfsDir), typeof(sfsFile) });
                dataFile = new FileStream(fileName, FileMode.Open);
                Config = (sfsConfig)serializer.Deserialize(dataFile);

                return true;
            }
            catch (Exception ex)
            {
                _log.Fatal("Exception==>", ex);
                res = false;
            }
            finally
            {
                if (null != dataFile)
                {
                    dataFile.Close();
                    dataFile = null;
                }
            }
            return res;
        }

        public void SaveXML(string fileName)
        {
            _log.InfoFormat("Saving XML to {0}", fileName);
            FileStream dataFile = null;
            try
            {
                XmlSerializer serializer =
                    new XmlSerializer(typeof(sfsConfig), new Type[] { typeof(sfsDir), typeof(sfsFile) });

                dataFile = new FileStream(fileName, FileMode.Create);
                serializer.Serialize(dataFile, Config);
            }
            catch (Exception ex)
            {
                _log.Fatal("Exception==>", ex);
            }
            finally
            {
                if (dataFile != null)
                {
                    dataFile.Close();
                    dataFile = null;
                }
            }
        }

        public bool LoadConfig(string fileName)
        {
            _log.InfoFormat("Loading config from {0}", fileName);
            FileStream dataFile = null;
            bool res = true;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(extractConfig));
                dataFile = new FileStream(fileName, FileMode.Open);
                gConfig = (extractConfig)serializer.Deserialize(dataFile);

                return true;
            }
            catch (Exception ex)
            {
                _log.Fatal("Exception==>", ex);
                res = false;
            }
            finally
            {
                if (null != dataFile)
                {
                    dataFile.Close();
                    dataFile = null;
                }
            }
            return res;
        }

        public void SaveConfig(string fileName)
        {
            _log.InfoFormat("Saving config to {0}",fileName);
            FileStream dataFile = null;
            try
            {
                XmlSerializer serializer =
                    new XmlSerializer(typeof(extractConfig));

                dataFile = new FileStream(fileName, FileMode.Create);
                serializer.Serialize(dataFile, gConfig);
            }
            catch (Exception ex)
            {
                _log.Fatal("Exception==>", ex);
            }
            finally
            {
                if (dataFile != null)
                {
                    dataFile.Close();
                    dataFile = null;
                }
            }
        }

        public void Search()
        {
            _log.InfoFormat("Started to search for files in SFS's");
            if (Config.Root == null)
            {
                Config.Root = new sfsDir();
            }
            if (filesId == null) return;

            Add("samples");
            Add("maps");

            foreach (string text in filesId)
            {
                bool isdir = !IsFile(text);

                TowTypeDir type = TowTypeDir.unknown;

                if (isdir == false)
                {
                    AddFile(text);
                }
                else
                {
                   
                    if (FilesID.IsMapDir(text))
                    {
                        type = TowTypeDir.MapDir;
                    }
                    else if(FilesID.IsAmmoDir(text))
                    {
                        type = TowTypeDir.AmmoDir;
                    }
                    else if (FilesID.IsSoundAmbientPreset(text))
                    {
                        type = TowTypeDir.SoundPresent;
                    }
                    else if (FilesID.IsUnitDir(text))
                    {
                        type = TowTypeDir.UnitDir;
                    }
                    else if (IsItem(text))
                    {
                        type = TowTypeDir.ItemDir;
                    }

                    
                    /// Post Adding ///

                    if (type == TowTypeDir.AmmoDir)
                    {
                        Add(text.Trim() + ".ini");
                        continue;
                    }

                    AddDir(text, type);

                    if (type == TowTypeDir.MapDir)
                    {
                        AddMapFiles(text);
                    }
                    else if (type == TowTypeDir.UnitDir)
                    {
                        AddUnitFiles(text);
                    }
                    else if (type == TowTypeDir.SoundPresent)
                    {

                    }
                    else if (type == TowTypeDir.ItemDir)
                    {
                        AddItemFiles(text);
                    }
                }
                  
            }

            AddDefaultFiles();

        }

        public bool IsFile(string file)
        {
            try
            {
                file = file.Trim();
                return SFSStream.Exists(Path.Combine(@"..\", file));
            }
            catch (Exception e)
            {
                _log.FatalFormat("Exception==>{0}", e.ToString());
                throw e;
            }
            return false;
        }

        public void AddFile(string file)
        {
            file = file.Trim();
            string[] dirs = SplitDirs(file);

            if (dirs.Length > 0)
            {
                bool binary = true;
                if (file.EndsWith(".ini", StringComparison.InvariantCultureIgnoreCase) == true ||
                    file.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase) == true ||
                    file.EndsWith(".mat", StringComparison.InvariantCultureIgnoreCase) == true ||
                    file.EndsWith(".prs", StringComparison.InvariantCultureIgnoreCase) == true ||
                    file.EndsWith(".eff", StringComparison.InvariantCultureIgnoreCase) == true 
                    )
                {
                    binary = false;
                }
                Global.Config.Root.AddFile(dirs, binary,file);

                if (file.EndsWith(".mat", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    AddMaterial(file);
                }
                else if (file.EndsWith(".prs", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    AddPrsFile(file);
                }
            }
        }

        public void AddDir(string dir, TowTypeDir type)
        {
            dir = dir.Trim();
            string[] dirs = SplitDirs(dir);
            
            if(dirs.Length>0)
            {
                Global.Config.Root.AddDir(dirs, type);
            }
        }


        public string[] SplitDirs(string dir)
        {
            string[] str = null;
            char [] sep = {'\\','/'};
            str = dir.Split(sep);
            return str;
        }

        public void AddMapFiles(string dir)
        {
            /*
            Add(dir + "/maintex.tga");
            Add(dir + "/nearHF.raw");
            Add(dir + "/TypesAI.tga");
            Add(dir + "/GameTypeMapIndex.tga");
            Add(dir + "/TreeTypeMap.bin");
            /**/
            Add(dir + "/load.ini");

            IniReader ini = new IniReader();
            string path = @"..\" + dir;
            ini.Load(path + "/load.ini", true);

            AddTexture(ini.GetValue("MAP", "HeightMap").Replace("=", " ").Trim(), dir);
            AddTexture(ini.GetValue("MAP", "HeightSubMap").Replace("=", " ").Trim(), dir);
            AddTexture(ini.GetValue("MAP", "HeightMap1").Replace("=", " ").Trim(), dir);
            AddTexture(ini.GetValue("MAP", "HeightSubMap1").Replace("=", " ").Trim(), dir);
            AddTexture(ini.GetValue("MAP", "TypeMapAI").Replace("=", " ").Trim(), dir);
            AddTexture(ini.GetValue("MAP", "TypeMap").Replace("=", " ").Trim(), dir);

            AddTexture(ini.GetValue("WWMAP", "MainTex").Replace("=", " ").Trim(), dir);
            AddTexture(ini.GetValue("WWMAP", "FarMainTex").Replace("=", " ").Trim(), dir);
            AddTexture(ini.GetValue("WWMAP", "BumpTex").Replace("=", " ").Trim(), dir);

            AddFile(dir + "\\" + ini.GetValue("WWMAP", "NoiseConfig").Replace("=", " ").Trim());
            AddFile(dir + "\\" + ini.GetValue("WWMAP", "CommonNoiseFile").Replace("=", " ").Trim());

            AddFile("maps\\_TEX\\" + ini.GetValue("GRASS", "VerticalGrassTexture").Replace("=", " ").Trim());
            AddFile(dir+"\\" + ini.GetValue("GRASS", "VerticalGrassTexture").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("GRASS", "HorizontalGrassTexture").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("GRASS", "FarGrass_HorizontalGrassTexture").Replace("=", " ").Trim());

            AddFile("maps\\_TEX\\" + ini.GetValue("TREES", "TextureSprite").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("TREES", "TextureSpriteBump").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("TREES", "TextureTrunk").Replace("=", " ").Trim());
            AddFile(dir + "\\" + ini.GetValue("TREES", "TreeTypeMap").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("TREES", "TextureSprite").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("TREES", "TextureSpriteBump").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("TREES", "TextureTrunk").Replace("=", " ").Trim());
            AddFile(dir + "\\" + ini.GetValue("TREES", "TreesMat").Replace("=", " ").Trim());


            AddFile(dir + "\\" + ini.GetValue("FARTREES", "FarTreeTypeMap").Replace("=", " ").Trim());
            AddTexture(ini.GetValue("FARTREES", "FarTreeMat").Replace("=", " ").Trim(), dir);

            AddFile(dir + "\\" + ini.GetValue("ROADS", "RoadsConfig").Replace("=", " ").Trim());

            AddFile("maps\\_TEX\\" + ini.GetValue("LIGHT", "LandLight").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("LIGHT", "CloudShadows").Replace("=", " ").Trim());

            AddFile(dir + "\\" + ini.GetValue("DEFORMATIONS", "PresetDynamicDeformations").Replace("=", " ").Trim());
            AddFile("maps\\" + ini.GetValue("DEFORMATIONS", "DeformTexture").Replace("=", " ").Trim());
            AddFile("maps\\" + ini.GetValue("DEFORMATIONS", "DeformBumpTexture").Replace("=", " ").Trim());

            AddFile(dir + "\\" + "actors.static");

            
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "HighClouds").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "HighCloudsNoise").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "Moon").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "ShadeNoise").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "WaterNoise").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "WaterNoiseAnimStart").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "ForestNoise").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "BeachFoam").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "BeachSurf").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "BeachLand").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "Tree0").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "TreeTrunk").Replace("=", " ").Trim());
            AddFile("maps\\_TEX\\" + ini.GetValue("APPENDIX", "TreeLightMask").Replace("=", " ").Trim());

            
        }

        public void AddItemFiles(string dir)
        {
            Add(dir + "/unit.ini");
            Add2(dir + "/hier.ini");
        }

        public void AddUnitFiles(string dir)
        {
            Add(dir + "/hier.ini");
            Add(dir + "/parts.ini");
            Add(dir + "/unit.ini");

                        
            IniReader ini = new IniReader();
            IniReader part = new IniReader();

            string path = @"..\" + dir;

            ini.Load(path + "/unit.ini", true);
            part.Load(path + "/parts.ini", true);

            ///Get Skeletons 
            ArrayList Skeletons = part.GetValues("SkelName");

            foreach (string skl in Skeletons)
            {
                Add(skl);
            }

            


            ArrayList rootMesh = ini.GetValues("MeshName");
            ArrayList rootMesh2 = ini.GetValues("Mesh");
            ArrayList rootMeshP = part.GetValues("MeshName");
            ArrayList rootMeshP2 = part.GetValues("Mesh");

            if(rootMesh.Count>0)
            {
                string name = GetName(dir);
                string d3dir = RemoveName((string)rootMesh[0]);
                Add(d3dir + "\\"+name + "_big.rect");
                Add(d3dir + "\\" + name + ".mat");
                Add(d3dir + "\\" + name + "_big.mat");
            }

            ArrayList comb = new ArrayList();
            comb.AddRange(rootMesh);
            comb.AddRange(rootMesh2);
            comb.AddRange(rootMeshP);
            comb.AddRange(rootMeshP2);

            if (comb.Count>0)
            {
                foreach (string meshname in comb)
                {
                    
                    Add(meshname);



                    string dirmesh = RemoveName(meshname) + "\\";

                    Add(dirmesh + "skins.ini");

                    IniReader meshIni = new IniReader();
                    meshIni.Load(@"..\" + meshname, true);
                    ///Add msh files
                    ArrayList list = meshIni.GetValues("Mesh");
                    foreach (string mesh in list)
                    {
                        Add(dirmesh + mesh + ".msh");
                    }

                    ///Read Skins
                    ///
                    IniReader skinIni = new IniReader();
                    skinIni.Load(@"..\" + dirmesh + "skins.ini", true);
                    ArrayList mats = skinIni.GetValuesContain("Material");
                    foreach (string mat in mats)
                    {
                        string matn = RepairMaterial(mat);
                        if (matn != string.Empty)
                        {
                            Add(matn);
                            /// Read textures
                            /// 

                            /*
                            string matdir = RemoveName(matn);

                            IniReader matIni = new IniReader();
                            matIni.Load(@"..\" + matn, true);

                            ArrayList texts = matIni.GetValues("TextureName");
                            foreach (string tex in texts)
                            {
                                string ttex = tex;
                                int updir = HowManyUp(ref ttex);
                                string texdir = RemoveDirs(matdir, updir);
                                Add(texdir + "\\" +  ttex);
                            }
                            /**/
                        }
                    }
                }
            }

        }

        public void AddMaterial(string matn)
        {
            if (matn == null || matn == string.Empty) return;

            string matdir = RemoveName(matn);

            IniReader matIni = new IniReader();
            matIni.Load(@"..\" + matn, true);

            ArrayList texts = matIni.GetValues("TextureName");
            foreach (string tex in texts)
            {
                AddTexture(tex, matdir);
            }
        }

        public void AddTexture(string tex, string rootdir)
        {
            string ttex = tex;
            int updir = HowManyUp(ref ttex);
            string texdir = RemoveDirs(rootdir, updir);
            Add(texdir + "\\" + ttex);
        }

        public string RemoveDirs(string path, int count)
        {
            string newpath = path;

            int cc = 0;
            int rc = 0;
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '\\' || path[i] == '/')
                {
                    rc++;
                    if (rc >= count) break;
                }
                else
                {
                    cc++;
                }
            }

            if (count > 0)
            {
                newpath = new string(path.ToCharArray(), 0, path.Length - cc-1);
            }
            else
                newpath = path;



            return newpath;
        }

        public int HowManyUp(ref string path)
        {
            string[] sep = { "..\\", "../" };
            string[] count1 = path.Split(sep,StringSplitOptions.None);

            path = count1[count1.Length - 1];

            return count1.Length-1;
        }


        public string RemoveName(string path)
        {
            int count = 0;
            for (int i = path.Length - 1; i >= 0; i--)
            {
                count++;
                if (path[i] == '\\' || path[i] == '/')
                {
                    break;
                }
            }
            if (count > 0)
            {
                return path.Remove(path.Length - count);
            }
            return string.Empty;
        }

        public string GetName(string path)
        {
            int count = 0;
            for (int i = path.Length - 1; i >= 0; i--)
            {
                
                if (path[i] == '\\' || path[i] == '/')
                {
                    break;
                }
                count++;
            }
            if (count > 0 && count <path.Length)
            {
                return path.Remove(0,path.Length-count);
            }
            return string.Empty;
        }

        public string RepairMaterial(string mat)
        {
            bool start = false;
            string ret = string.Empty;
            for (int i = 0; i < mat.Length; i++)
            {
                if (start == true)
                {
                    ret += mat[i];
                }
                if (mat[i] == ' ')
                {
                    start = true;
                }
            }
            ret = ret.TrimStart();
            return ret;
        }

        public void Add(string file)
        {
            bool isdir = !IsFile(file);

            if (isdir == true)
            {
                AddDir(file, TowTypeDir.unknown);
            }
            else
            {
                AddFile(file);
            }

            

        }


        public void Add2(string file)
        {
            bool isdir = !IsFile(file);

            if (isdir ==false)
            {
                AddFile(file);
            }
        }

        public void AddPrsFile(string file)
        {
            if (file == null || file == string.Empty) return;

            IniReader prsIni = new IniReader();
            prsIni.Load(@"..\"+ file, true);

            Category samples = prsIni.GetCategory("samples");
            if (samples != null)
            {
                foreach (KeyValue key in samples.aKeys)
                {
                    string name = key.Name.Trim();
                    if (name[0] == ';') continue;
                    Add("samples\\" + name);
                }
            }

        }

        public void AddDefaultFiles()
        {
            Add("Maps/_Roads");

            Add("data/local/en/strings.utf8");
            Add("data/local/en/units.utf8");
            Add("data/local/en/items.utf8");
            Add("data/local/en/ammo.utf8");
            Add("data/local/en/guns.utf8");
            Add("data/local/en/ranks.utf8");
            Add("data/local/en/names.utf8");
            Add("data/local/en/hkeys.utf8");
            Add("data/local/en/awards.utf8");
            Add("data/local/en/months.utf8");
            Add("data/local/en/messages.utf8");
            Add("data/local/en/award_desc.utf8");
            Add("data/local/en/skills_desc.utf8");
            Add("data/local/en/credits.utf8");

            Add("data/local/en/encdescription_armoredcar.utf8");
            Add("data/local/en/encdescription_spg.utf8");
            Add("data/local/en/encdescription_flak.utf8");
            Add("data/local/en/encdescription_car.utf8");
            Add("data/local/en/encdescription_tanks.utf8");
            Add("data/local/en/encdescription_artillery.utf8");
            Add("data/local/en/encdescription_hmg.utf8");

            Add("data/guns/Throw.ini");

            Add("Data/Units/Units_Backpacks.ini");
            Add("Data/Units/Units_Classes.ini");
            Add("Data/Units/Units_CrewSets.ini");
            Add("Data/Units/Units_Faces.ini");
            Add("Data/Units/Units_PersonLastNames.ini");
            Add("Data/Units/Units_PersonNames.ini");

            Add("data/Ai/MoraleSystem.ini");
            Add("data/Ai/BattleModel.ini");
            Add("data/Ai/Detour.ini");
            Add("data/Ai/DangerLevel.ini");
            Add("Data/Ai/AmmoTypeProp.ini");
            Add("Data/Ai/AmmoRating.ini");
            Add("data/Ai/aggro.ini");
            Add("data/Ai/RatingTypes.ini");
            Add("data/Ai/RetreatSystem.ini");
            Add("data/Ai/AITypes.ini");
            Add("Data/AI/messages.xml");
            Add("data/ai/accuracy.ini");
            Add("data/ai/awards.ini");
            Add("data/Ai/SkillsImpact/Dispersion.ini");
            Add("data/Ai/visibility.ini");
            Add("data/Ai/weather.ini");
            Add("Ai/Animator/Animator.rc");
            Add("Ai/Animator/artillery.rc");
            Add("Ai/Animator/HMesh/Animations.ini");
            Add("data/ai/experience.ini");
            Add("data/ai/skills.ini");
            Add("data/ai/levels.ini");
            Add("data/Ai/SkillsImpact/NoiseRange.ini");
            Add("data/Ai/SkillsImpact/ObservableRange.ini");
            Add("data/ai/ranks.ini");
            Add("data/Ai/SkillsImpact/ReloadSpeed.ini");
            Add("data/settings/shoulders.ini");
            Add("data/Ai/SkillsImpact/TransferSpeed.ini");
            Add("Ai/Animator/HMesh/SwitchTables.ini");
            Add("Ai/Animator/SMesh/SwitchTables.ini");
            Add("Ai/Animator/human.rc");
            Add("Ai/Animator/SMesh/Markers.ini");
            Add("Ai/Animator/SMesh/Animations.ini");


            Add("models/Human/sk_0/GreenDude");
            Add("models/Human/sk_0");
            Add("models/Human/sk_1");
            Add("models/Human/sk_1/woman1");

            Add("effects/SunFlare/Flare0.mat");
            Add("effects/SunFlare/Flare1.mat");
            Add("effects/SunFlare/Flare2.mat");
            Add("effects/SunFlare/Flare3.mat");


            Add("users/sound.ini");

            Add("data/Settings/DieEffects.ini");
            Add("data/Settings/DieselEffects.ini");
            Add("data/Settings/DustEffects.ini");
            Add("data/Settings/GroundHoles.ini");
            Add("Data/Settings/EncyclopaediaCont.xml");
            Add("data/Settings/DamageEffects.ini");
            Add("data/Settings/static.ini");
            Add("data/Settings/StaticTypePresets.ini");
            Add("data/settings/filesid.ini");
            Add("Data/Settings/gui.ini");
            Add("data/Settings/damage.ini");
            Add("data/Settings/Debug/decal_camera.ini");
            Add("data/settings/transmission.ini");
            Add("data/Settings/DecalPreset.ini");
            Add("data/Settings/DamageSounds.ini");
            Add("Data/Settings/camera.properties");
            Add("data/Settings/navigation.ini");
            Add("Data/Settings/video_presets.ini");
            Add("data/settings/GunEffects.ini");
            Add("data/settings/playerindicators.ini");
            Add("Data/Settings/sound_defaults.ini");
            Add("data/Settings/mp_default.ini");
            Add("Data/Settings/SpeedPresets.ini");
            Add("data/settings/shoulders.ini");
            Add("data/Settings/support.ini");


            Add("3do/primitive/Marker/mono.sim");

            Add("3do/Animations");

            Add("DATA/GUI/Cursors/Attack.mat");
            Add("DATA/GUI/Cursors/attack_air.mat");
            Add("DATA/GUI/Cursors/attack_artillery.mat");
            Add("DATA/GUI/Cursors/AttackGround.mat");
            Add("DATA/GUI/Cursors/AttackStorm.mat");
            Add("DATA/GUI/Cursors/Guard.mat");
            Add("DATA/GUI/Cursors/CameraDown.mat");
            Add("DATA/GUI/Cursors/CameraLeftDown.mat");
            Add("DATA/GUI/Cursors/CameraRightDown.mat");
            Add("DATA/GUI/Cursors/attach_vehicle.mat");
            Add("DATA/GUI/Cursors/Impossible.mat");
            Add("DATA/GUI/Cursors/CameraLeft.mat");
            Add("DATA/GUI/Cursors/MoveToTrench.mat");
            Add("DATA/GUI/Cursors/Normal.mat");
            Add("DATA/GUI/Cursors/InUnit.mat");
            Add("DATA/GUI/Cursors/reconair.mat");
            Add("DATA/GUI/Cursors/Retreat.mat");
            Add("DATA/GUI/Cursors/CameraRight.mat");
            Add("DATA/GUI/Cursors/Rotate.mat");
            Add("DATA/GUI/Cursors/RotateCamera.mat");
            Add("DATA/GUI/Cursors/Normal.mat");
            Add("DATA/GUI/Cursors/CameraUp.mat");
            Add("DATA/GUI/Cursors/CameraLeftUp.mat");
            Add("DATA/GUI/Cursors/CameraRightUp.mat");
            Add("data/GUI/animator/elements.mat");
            Add("data/GUI/animator/elementss.mat");
            Add("intro_FMV_gen_v02.avi");
            Add("data/gui/clear_set_z.mat");
            Add("Data/Gui/Ingame/Icons/Detachment/default.mat");
            Add("Data/GUI/Ingame/dispositionground.mat");
            Add("data/gui/Ingame/Faces/");
            Add("data/gui/Ingame/Faces/default.mat");
            Add("data/gui/Ingame/Faces/default_sm.mat");
            Add("Data/GUI/FrontEnd/avi01.mat");
            Add("Data/GUI/FrontEnd/back01.mat");
            Add("Data/GUI/FrontEnd/controls.mat");
            Add("Data/GUI/FrontEnd/controls.xml");
            Add("Data/GUI/FrontEnd/CountryDependent");
            Add("loading.mat");
            Add("Data/GUI/FrontEnd/MainMenu/back01.mat");
            Add("Data/GUI/FrontEnd/ngmenuskins.mat");
            Add("Data/GUI/FrontEnd/ngmenuskins.xml");
            Add("missionfail.mat");
            Add("missionwin.mat");
            Add("Data/GUI/Ingame/Icons/Units");
            Add("Data/GUI/Ingame/default.mat");
            Add("Data/GUI/Ingame/Icons/");
            Add("WeaponStatic/barrel_default.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/grenade_default.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/shell_default.mat");

            Add("Data/GUI/Ingame/Icons/Magazines/AP.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/HE.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/HEAT.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/APC.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/APCBC.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/APCR.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/APDS.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/HEI.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/APHEBC.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/APBC.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/HEAC.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/API.mat");
            Add("Data/GUI/Ingame/Icons/Magazines/APHE.mat");

            Add("WeaponItem/throw.mat");
            Add("Data/GUI/Ingame/Indicators");
            Add("Data/GUI/Ingame/main.mat");
            Add("Data/GUI/ingame/main.xml");
            Add("Data/AI/messages.xml");
            Add("data/GUI/MenuSystem/Credits.mat");
            Add("data/GUI/MenuSystem/Ingame/MessagePanel/panel.mat");
            Add("minimap.mat");
            Add("Data/GUI/Ingame/Hole.mat");
            Add("Data/Settings/gui.ini");
            Add("ui_click");
            Add("data/GUI/win95/cursors.mat");
            Add("data/GUI/win95/cursorss.mat");
            Add("data/GUI/win95/elements.mat");
            Add("data/GUI/win95/elementss.mat");
            Add("Data/gui/skins/ordinary");
        }

        public static bool IsItem(string name)
        {
            if (!name.StartsWith(_itemPrefix))
            {
                return false;
            }
            return true;
        }

        public sfsDir SearchForFile(sfsDir startDir, string name)
        {
            sfsDir found = null;
            if (startDir == null)
                startDir = Config.Root;

            found = _SearchForFile(startDir, name);
            return found;
        }

        protected sfsDir _SearchForFile(sfsDir parent, string name)
        {
            sfsDir found = null;
            string path = string.Empty;
            parent.GetFullPath(ref path);
            bool res = IsFile(path + "\\" + name);

            if (res == true)
            {
                return parent;
            }
            else
            {
                if (parent.Dirs != null)
                {
                    foreach (sfsDir dir in parent.Dirs)
                    {
                        found = _SearchForFile(dir, name);
                        if (found != null) return found;
                    }

                }

            }
            return null;
        }
    }
}
