﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;

namespace RogueCastle
{
    public class XMLCompiler
    {
        public static void CompileEnemies(List<EnemyEditorData> enemyDataList, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlWriter writer = XmlWriter.Create(System.IO.Path.Combine(filePath, "EnemyList.xml"), settings);

            string xmlStringtest = "<xml>";
            writer.WriteStartElement("xml");

            foreach (EnemyEditorData enemyDataObj in enemyDataList)
            {
                xmlStringtest += "<EnemyObj>\n";
                writer.WriteStartElement("EnemyObj");
                writer.WriteAttributeString("Type", enemyDataObj.Type.ToString());
                writer.WriteAttributeString("SpriteName", enemyDataObj.SpriteName);

                writer.WriteAttributeString("BasicScaleX", enemyDataObj.BasicScale.X.ToString());
                writer.WriteAttributeString("BasicScaleY", enemyDataObj.BasicScale.Y.ToString());

                writer.WriteAttributeString("AdvancedScaleX", enemyDataObj.AdvancedScale.X.ToString());
                writer.WriteAttributeString("AdvancedScaleY", enemyDataObj.AdvancedScale.Y.ToString());

                writer.WriteAttributeString("ExpertScaleX", enemyDataObj.ExpertScale.X.ToString());
                writer.WriteAttributeString("ExpertScaleY", enemyDataObj.ExpertScale.Y.ToString());

                writer.WriteAttributeString("MinibossScaleX", enemyDataObj.MinibossScale.X.ToString());
                writer.WriteAttributeString("MinibossScaleY", enemyDataObj.MinibossScale.Y.ToString());

                writer.WriteEndElement();
                xmlStringtest += "</EnemyObj>\n";
            }
            writer.WriteEndElement();
            // Console.WriteLine(xmlStringtest);
            writer.Flush();
            writer.Close();

        }
    }

    public struct EnemyEditorData
    {
        public byte Type;
        public string SpriteName;
        public Vector2 BasicScale;
        public Vector2 AdvancedScale;
        public Vector2 ExpertScale;
        public Vector2 MinibossScale;

        public EnemyEditorData(byte enemyType)
        {
            EnemyObj enemyBasic = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.Basic);
            EnemyObj enemyAdvanced = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.Advanced);
            EnemyObj enemyExpert = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.Expert);
            EnemyObj enemyMiniboss = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.Miniboss);

            Type = enemyType;
            SpriteName =     enemyBasic.SpriteName;
            BasicScale =     enemyBasic.Scale;
            AdvancedScale =  enemyAdvanced.Scale;
            ExpertScale =    enemyExpert.Scale;
            MinibossScale =  enemyMiniboss.Scale;
        }
    }

}
