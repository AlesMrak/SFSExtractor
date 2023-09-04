namespace Editor.Configuration.WW2Project
{
    using System;

    public class WW2Options_Path 
    {
        //private StringOption m_MapStaticsFile;
        public readonly string WW2_DATA;

        //internal WW2Options_Path(OptionsData parent, string prefix)
        internal WW2Options_Path(object parent, string prefix)
        {
            this.WW2_DATA = @"..\";
            //this.m_MapStaticsFile = base.GetStringOption("MapStaticsFile", "actors.static");
        }

        public string MapStaticsFile
        {
            get
            {
                return "actors.static";
            }
        }
    }

    public class WW2Options 
    {
        private static WW2Options m_Instance;
        private WW2Options_Path m_Path;

        private WW2Options()
        {
            this.m_Path = new WW2Options_Path(this, "Path");
            //base.SaveIfModified();
        }

        public static WW2Options Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new WW2Options();
                }
                return m_Instance;
            }
        }

        public WW2Options_Path Path
        {
            get
            {
                return this.m_Path;
            }
        }
    }

}

