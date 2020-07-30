using PrintControl.Model;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace PrintControl.Utils
{
    public static class FileUtils
    {
        public static Setting GetSetting()
        {
            Setting setting = null;
            try
            {
                FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "/data.bin", FileMode.OpenOrCreate);

                if (fs.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    setting = bf.Deserialize(fs) as Setting;
                }
                fs.Close();
            }
            catch
            {

            }
            return setting ?? new Setting();
        }

        public static Boolean SaveSetting(Setting model)
        {
            try
            {
                var file = Directory.GetCurrentDirectory() + "/data.bin";
                FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "/data.bin", FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, model);
                fs.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}
