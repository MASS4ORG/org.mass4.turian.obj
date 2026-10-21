# Wavefront OBJ importer

Editor-only import of `.obj` sources into `primary.glb` through the editor's shared Assimp converter. It provides no runtime assemblies or separate OBJ parser dependency; exported games read glTF 2.0 geometry.

Licensed under MIT. See `package.json` for version and engine compatibility.

## Install

```sh
turian-cli brick add org.mass4.turian.obj
```

Or enable `org.mass4.turian.obj` from the Bricks panel. Reopen the project after changing its bricks.

Keep existing `.obj.meta` files: their asset type, IDs and import settings stay valid. Importer version 4 invalidates older model caches and creates GLB geometry from the source. Rebuild exported games after reimporting. Without this brick, the editor reports an unsupported OBJ import with installation/conversion guidance.

To migrate away from OBJ, export the source to glTF/GLB or FBX, move its `.meta` alongside the replacement source, and retain its asset ID. Verify the reimport and scene references before deleting the old source.

## Scope

The importer preserves vertex colors, source Y coordinates, UV conversion and vertex deduplication. Faces must contain normals and texture coordinates, and authored faces should be triangulated. Material libraries and object hierarchies are not imported.

## Source and baked packages

The regular brick contains editable code under `Runtime/` and/or `Editor/`; its assembly definitions let any Turian client compile it without an engine checkout. Build and test projects live under the opaque `Brick~/` roles. Keep committed `.meta` identities when editing or publishing.

`turian-cli brick pack . --precast --twin --out .bricks` compiles the code, imports the assets and writes the regular brick plus its `.bake` twin. The source archive omits generated payloads; the twin includes the assemblies and verified import cache, with no C# source. `brick publish . --store <store> --key <key> --precast --twin` performs the same producer steps before upload; protected production CI signs and uploads the qualified archive bytes.

Consumers can enable `brick twins on --project <project>`, then `brick swap org.mass4.turian.obj source --project <project>` to download and embed writable originals at the same version. Swapping back to `twin` preserves the edited fork in the project trash. GLSL edits in HZB require rebuilding its shader payload with the build project before packing.
