using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Walterlv.MarkdownMail.Annotations;

namespace Walterlv.MarkdownMail
{
    public class JsonSettings
    {
        public static async Task<T> ReadAsync<T>(string settingFileName) where T : class
        {
            var storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(settingFileName);
            if (storageItem?.IsOfType(StorageItemTypes.File) == true)
            {
                var file = (StorageFile) storageItem;
                var json = JsonSerializer.Create();
                using (TextReader reader = new StreamReader(await file.OpenStreamForReadAsync()))
                {
                    return json.Deserialize<T>(new JsonTextReader(reader));
                }
            }
            return default(T);
        }

        public static async Task StoreAsync<T>(string settingFileName, T @object)
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                settingFileName, CreationCollisionOption.ReplaceExisting);
            var json = JsonSerializer.Create();
            using (TextWriter writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
            {
                json.Serialize(writer, @object);
            }
        }
    }
}
