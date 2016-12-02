using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using CsharpHueAssignment.HueInterface;

namespace CsharpHueAssignment
{
    class DataManager
    {
        public static async Task<T> ReadObjectFromXmlFileAsync<T>(string filename)
        {
            // this reads XML content from a file ("filename") and returns an object  from the XML
            T objectFromXml = default(T);
            var serializer = new XmlSerializer(typeof(T));
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(filename);
            Stream stream = await file.OpenStreamForReadAsync();
            objectFromXml = (T)serializer.Deserialize(stream);
            stream.Dispose();
            return objectFromXml;
        }

        public static async Task SaveObjectToXml<T>(T objectToSave, string filename)
        {
            // stores an object in XML format in file called 'filename'
            var serializer = new XmlSerializer(typeof(T));
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            Stream stream = await file.OpenStreamForWriteAsync();

            using (stream)
            {
                serializer.Serialize(stream, objectToSave);
            }
        }

        public static async void LoadData(ObservableCollection<Bridge> bridges)
        {
            //creating/looking for folder
            var folder = ApplicationData.Current.LocalFolder;
            var files = await folder.GetFilesAsync();
            foreach (var file in files)
            {
                if (file.FileType == ".xml" && file.Name.Contains("bridge_"))
                {
                    var bridge = await DataManager.ReadObjectFromXmlFileAsync<Bridge>(file.Name);
                    bridges.Add(bridge);
                }
            }
        }

        public static async void DeleteFileAsync(string filename)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(filename);
                await file.DeleteAsync();
            }
            catch (Exception exception)
            {
                // This will error when trying to remove the two default bridges
                Debug.WriteLine(exception);       
            }
        }
    }
}
