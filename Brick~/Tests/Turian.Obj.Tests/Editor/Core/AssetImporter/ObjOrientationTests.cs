using Turian.Editor.Obj;

namespace Turian.Tests;

/// <summary>Checks imported OBJ geometry against the camera's world-up convention.</summary>
public sealed class ObjOrientationTests
{
    /// <summary>An OBJ authored above its origin remains above it in both geometry and normals.</summary>
    [Fact]
    public void ObjPreservesUpwardGeometryAndNormals()
    {
        var path = Path.Combine(Path.GetTempPath(), $"turian-up-{Guid.NewGuid():N}.obj");
        try
        {
            File.WriteAllText(path, """
                v 0 1 0
                v 1 1 0
                v 0 2 0
                vt 0 0
                vt 1 0
                vt 0 1
                vn 0 1 0
                f 1/1/1 2/2/1 3/3/1
                """);
            var directory = Directory.CreateTempSubdirectory("turian-orientation-");
            ModelBuilder builder;
            try
            {
                var importer = new ObjModelImporter();
                var artifact = Assert.Single(importer.ImportToCache(importer.CreateAsset(path), path, directory.FullName));
                builder = GltfModelReader.Load(Path.Combine(directory.FullName, artifact));
            }
            finally { directory.Delete(recursive: true); }
            Assert.All(builder.Vertices, vertex =>
            {
                Assert.True(vertex.Position.Y > 0);
                Assert.Equal(Vector3.UnitY, vertex.Normal);
            });
            var camera = new EditorCamera { Position = new Vector3(0, 0, -5) };
            Assert.All(builder.Vertices, vertex => Assert.True(camera.Project(vertex.Position).Y < 0));
            Assert.Equal(4, new ObjModelImporter().Version);
        }
        finally { File.Delete(path); }
    }
}
