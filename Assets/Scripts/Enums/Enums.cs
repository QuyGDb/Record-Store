public enum PhysicalCDAlbum
{
    Gieo,
    SDDBP
}
public enum MusicGenres
{
    Blues,
    Jazz,
    Rock,
    Pop,
    Rap,
    RnB,
    CityPop,
}


public enum AnchorType
{
    None,
    Wall,
    VinylShowCase,
    Portal,
}
public enum PlaneEdge
{
    Left,
    Right,
    Top,
    Bottom
}
public enum ApplicationState
{
    Start,
    CreateMapMode,
    LoadingMapMode,
    Instruction,
    Anchor,
    CloudAnchorInCreateMap,
    WallManager,
    ObjectParent,
    ObjectManager,
    TestMap,
    // load map scene
    ListMap,
    CloudAnchorInLoadMap,
    View,
}