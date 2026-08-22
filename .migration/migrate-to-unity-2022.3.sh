#!/usr/bin/env bash
set -euo pipefail

target=${1:?Usage: migrate-to-unity-2022.3.sh TARGET_PROJECT}
map_file=$(dirname "$0")/meta-guid-map.tsv

while IFS=$'\t' read -r asset guid; do
    meta="$target/$asset.meta"
    if [[ ! -f "$meta" ]]; then
        printf 'Missing mapped meta: %s\n' "$meta" >&2
        exit 1
    fi
    sed -i "s|^guid:.*|guid: $guid|" "$meta"
done < "$map_file"

while IFS= read -r -d '' meta; do
    current=$(awk '/^guid:/{print $2; exit}' "$meta")
    if [[ ! "$current" =~ ^[0-9a-f]{32}$ ]]; then
        relative=${meta#"$target/"}
        guid=$(printf '%s' "$relative" | md5sum | awk '{print $1}')
        sed -i "s|^guid:.*|guid: $guid|" "$meta"
    fi
done < <(find "$target/Assets" -name '*.meta' -type f -print0)

find "$target/Assets" "$target/ProjectSettings" -type f \
    \( -name '*.asset' -o -name '*.prefab' -o -name '*.scene' \) -print0 \
    | xargs -0 sed -i 's|tag:yousandi.cn,2023:|tag:unity3d.com,2011:|'

sed -i \
    -e 's/^m_EditorVersion:.*/m_EditorVersion: 2022.3.48f1c1/' \
    -e 's/^m_EditorVersionWithRevision:.*/m_EditorVersionWithRevision: 2022.3.48f1c1 (076abfb8be7c)/' \
    -e '/^m_TuanjieEditorVersion:/d' \
    "$target/ProjectSettings/ProjectVersion.txt"

if [[ -f "$target/Assets/Scenes/SampleScene.scene" ]]; then
    mv "$target/Assets/Scenes/SampleScene.scene" "$target/Assets/Scenes/SampleScene.unity"
    mv "$target/Assets/Scenes/SampleScene.scene.meta" "$target/Assets/Scenes/SampleScene.unity.meta"
fi

sed -i 's#Assets/Scenes/SampleScene\.scene#Assets/Scenes/SampleScene.unity#g' \
    "$target/ProjectSettings/ProjectSettings.asset" \
    "$target/README.md"
sed -i \
    -e '/\[Pp\]ackages/d' \
    -e '/\[Pp\]ackage-lock/d' \
    -e '/^ProjectSettings\/EditorBuildSettings\.asset$/d' \
    "$target/.gitignore"

sed -i '/^using SerializeReferenceEditor;$/d' \
    "$target/Assets/Scripts/Data/CardData.cs" \
    "$target/Assets/Scripts/Data/PerkData.cs" \
    "$target/Assets/Scripts/Models/AutoTargetEffect.cs"
sed -i 's/\[field: SerializeReference, SR\]/[field: SerializeReference]/g' \
    "$target/Assets/Scripts/Data/CardData.cs" \
    "$target/Assets/Scripts/Data/PerkData.cs" \
    "$target/Assets/Scripts/Models/AutoTargetEffect.cs"

manifest="$target/Packages/manifest.json"
jq '.dependencies += {
    "com.unity.2d.sprite": "1.0.0",
    "com.unity.splines": "2.6.1",
    "com.unity.textmeshpro": "3.0.7",
    "com.unity.ugui": "1.0.0"
}' "$manifest" > "$manifest.tmp"
mv "$manifest.tmp" "$manifest"

printf 'Migration metadata prepared for %s\n' "$target"
