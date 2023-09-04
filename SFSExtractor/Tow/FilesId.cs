namespace Editor.Utils
{
    using Editor.JavaClasses;
    using Editor.SFS;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public sealed class FilesID : IEnumerable
    {
        private static readonly string _ammoUnitsPrefix = JavaPath.Prepare("data/ammo/");
        private static readonly string _dataUnitsPrefix = JavaPath.Prepare("data/units/");
        private static readonly string _mapsPrefix = JavaPath.Prepare("maps/");
        private static readonly string _soundsAmbientPresetPrefix = JavaPath.Prepare("presets/sounds/ambience/");
        private static readonly string _soundsPresetPostfix = ".prs";
        private readonly IList idToNames = new ArrayList();
        private static FilesID instance;
        private const int MAX_ID_VALUE = 0x7fff;
        private const int MIN_ID_VALUE = 0;
        private readonly Dictionary<string, int> namesToId = new Dictionary<string, int>();

        private FilesID()
        {
            this.Load();
        }

        private bool Add(int id, string filename)
        {
            if ((id < 0) || (id > 0x7fff))
            {
                throw new ArgumentOutOfRangeException("id");
            }
            string key = JavaPath.Prepare(filename);
            if (!this.namesToId.ContainsKey(key))
            {
                while (this.idToNames.Count <= id)
                {
                    this.idToNames.Add(null);
                }
                this.idToNames[id] = key;
                this.namesToId[key] = id;
                return true;
            }
            return true;
        }

        public void Clear()
        {
            this.idToNames.Clear();
            this.namesToId.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return this.idToNames.GetEnumerator();
        }

        public int GetId(string name)
        {
            try
            {
                return this.namesToId[name];
            }
            catch (KeyNotFoundException)
            {
                return -1;
            }
        }

        public static FilesID getInstance()
        {
            if (instance == null)
            {
                instance = new FilesID();
            }
            return instance;
        }

        public string GetString(int id)
        {
            if (id < 0)
            {
                return null;
            }
            return (string)this.idToNames[id];
        }

        public static bool IsMapDir(string name)
        {
            if (!name.StartsWith(_mapsPrefix))
            {
                return false;
            }
            string text = Path.Combine(name, JavaPaths.MISSION_FILENAMELOADINI_WO_DELIMITER);
            return SFSStream.Exists(Path.Combine(@"..\", text));
        }

        public static bool IsSoundAmbientPreset(string name)
        {
            if (name.StartsWith(_soundsAmbientPresetPrefix) && name.EndsWith(_soundsPresetPostfix))
            {
                return SFSStream.Exists(Path.Combine(@"..\", name));
            }
            return false;
        }

        public static bool IsUnitDir(string name)
        {
            if (!name.StartsWith(_dataUnitsPrefix))
            {
                return false;
            }
            string text = Path.Combine(name, JavaPaths.ENTITY_UNIT_WO_DELIMITER);
            return SFSStream.Exists(Path.Combine(@"..\", text));
        }

        public static bool IsAmmoDir(string name)
        {
            if (!name.StartsWith(_ammoUnitsPrefix))
            {
                return false;
            }
            return true;
        }

        public void Load()
        {
            this.Clear();
            using (SFSReader reader = new SFSReader(Path.Combine(@"..\", "data/settings/filesid.ini")))
            {
                char[] separator = new char[] { '\t' };
                while (true)
                {
                    string text2 = reader.ReadLine();
                    if (text2 == null)
                    {
                        return;
                    }
                    string[] textArray = text2.Split(separator, 2);
                    this.Add(int.Parse(textArray[0]), textArray[1]);
                }
            }
        }

        public int Count
        {
            get
            {
                return this.idToNames.Count;
            }
        }
    }
}

