using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;

namespace RogueCastle;

public static class XMLCompiler {
    public static void CompileEnemies(List<EnemyEditorData> enemyDataList, string filePath) {
        var settings = new XmlWriterSettings {
            Indent = true,
            ConformanceLevel = ConformanceLevel.Fragment,
        };
        var writer = XmlWriter.Create(Path.Combine(filePath, "EnemyList.xml"), settings);

        writer.WriteStartElement("xml");
        foreach (var enemyDataObj in enemyDataList) {
            writer.WriteStartElement("EnemyObj");
            writer.WriteAttributeString("Type", $"{enemyDataObj.Type}");
            writer.WriteAttributeString("SpriteName", enemyDataObj.SpriteName);

            writer.WriteAttributeString("BasicScaleX", $"{enemyDataObj.BasicScale.X}");
            writer.WriteAttributeString("BasicScaleY", $"{enemyDataObj.BasicScale.Y}");
            writer.WriteAttributeString("AdvancedScaleX", $"{enemyDataObj.AdvancedScale.X}");
            writer.WriteAttributeString("AdvancedScaleY", $"{enemyDataObj.AdvancedScale.Y}");
            writer.WriteAttributeString("ExpertScaleX", $"{enemyDataObj.ExpertScale.X}");
            writer.WriteAttributeString("ExpertScaleY", $"{enemyDataObj.ExpertScale.Y}");
            writer.WriteAttributeString("MinibossScaleX", $"{enemyDataObj.MinibossScale.X}");
            writer.WriteAttributeString("MinibossScaleY", $"{enemyDataObj.MinibossScale.Y}");

            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.Flush();
        writer.Close();
    }
}

public struct EnemyEditorData {
    public readonly byte Type;
    public readonly string SpriteName;
    public Vector2 BasicScale;
    public Vector2 AdvancedScale;
    public Vector2 ExpertScale;
    public Vector2 MinibossScale;

    public EnemyEditorData(byte enemyType) {
        var enemyBasic = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.Basic);
        var enemyAdvanced = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.Advanced);
        var enemyExpert = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.Expert);
        var enemyMiniboss = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.Miniboss);

        Type = enemyType;
        SpriteName = enemyBasic.SpriteName;
        BasicScale = enemyBasic.Scale;
        AdvancedScale = enemyAdvanced.Scale;
        ExpertScale = enemyExpert.Scale;
        MinibossScale = enemyMiniboss.Scale;
    }
}
