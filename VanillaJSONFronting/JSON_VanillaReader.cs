using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MysteriousAlchemy.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ID.ContentSamples.CreativeHelper;
using MysteriousAlchemy.Tiles;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Core;

namespace MysteriousAlchemy.VanillaJSONFronting
{
    public class JSON_VanillaReader
    {
        private static JSON_VanillaReader _instance;

        public static JSON_VanillaReader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JSON_VanillaReader();
                }
                return _instance;
            }
        }
        public JSON_VanillaReader()
        {
            _instance = this;
        }
        /// <summary>
        /// ��ȡJSON����
        /// </summary>
        /// <typeparam name="T">�Զ���Ľṹ������</typeparam>
        /// <param name="path">��JSON�ļ���λ��</param>
        /// <returns></returns>
        public List<T> GetJsonList<T>(string path)
        {
            List<T> Output = new List<T>();
            //��ȡStream
            Stream stream = ModLoader.GetMod("MysteriousAlchemy").GetFileStream(path + ".json");
            StreamReader streamReader = new StreamReader(stream);
            //ת��ΪJArray
            JArray jArray = JArray.Parse(streamReader.ReadToEnd());
            //�����л�
            Output = JsonConvert.DeserializeObject<List<T>>(jArray.ToString());
            stream.Close();
            return Output;
        }

        public void GeneVanilaStruct<T>(T structs, string path)
        {
            List<T> Output = new List<T>();
            //��ȡStream
            Stream stream = ModLoader.GetMod("MysteriousAlchemy").GetFileStream(path + ".json");
            StreamWriter streamWriter = new StreamWriter(stream);
            string outDate = JsonConvert.SerializeObject(structs, Formatting.Indented);
            streamWriter.Write(outDate);
            stream.Close();
        }
    }
}