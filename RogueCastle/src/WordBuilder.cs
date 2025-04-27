using RogueCastle.GameStructs;

namespace RogueCastle;

public static class WordBuilder {
    public static string BuildDungeonNameLocID(GameTypes.LevelType levelType) {
        return levelType switch {
            GameTypes.LevelType.Castle  => "LOC_ID_DUNGEON_NAME_1",
            GameTypes.LevelType.Dungeon => "LOC_ID_DUNGEON_NAME_2",
            GameTypes.LevelType.Garden  => "LOC_ID_DUNGEON_NAME_3",
            GameTypes.LevelType.Tower   => "LOC_ID_DUNGEON_NAME_4",
            _                           => "",
        };
    }
}
