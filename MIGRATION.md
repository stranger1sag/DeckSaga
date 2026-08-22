# Unity Migration Notes

This copy was migrated from Tuanjie Engine 1.8.2 / 2022.3.62t4 to Unity 2022.3.48f1c1.

## Changes

- Converted Tuanjie YAML tags and 56-character metadata IDs to Unity-compatible values.
- Restored the serialized GUIDs used by scenes, prefabs, scripts, card data, enemy data, images, and fonts.
- Renamed `Assets/Scenes/SampleScene.scene` to `Assets/Scenes/SampleScene.unity`.
- Added `Packages/manifest.json` and `Packages/packages-lock.json` with 2D Sprite, UGUI, TextMesh Pro, and Splines.
- Removed the optional `SerializeReferenceEditor.SR` inspector attribute while retaining Unity's `SerializeReference` serialization.
- Added an editor-only migration validator and registered `SampleScene.unity` in Build Settings.

## Validation

Unity 2022.3.48f1c1 completed an incremental batch compilation with no C# errors or warnings.
`MigrationValidator.Validate` loaded the main scene and scanned all project prefabs and data assets without finding missing scripts or broken object references.

## Initial Deck Mapping

The source metadata did not contain a reversible mapping for three card asset GUIDs. The migrated starter deck preserves the original 4/4/1 quantities as four Fireball cards, four Arrow Rain cards, and one Draw card. Verify this intended composition before changing card balance.
