using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RGB.NET.Core.Tests.Ids;

[TestClass]
public class IdGeneratorTest
{
    #region Helper

    // Called through reflection from a reloaded copy of this assembly, so that copy is the calling assembly
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string MakeUniqueAndReset(string id)
    {
        string uniqueId = IdGenerator.MakeUnique(id);
        IdGenerator.ResetCounter();
        return uniqueId;
    }

    private static string MakeUniqueFromReloadedAssembly(string id)
    {
        AssemblyLoadContext context = new(nameof(IdGeneratorTest), true);
        try
        {
            using MemoryStream stream = new(File.ReadAllBytes(typeof(IdGeneratorTest).Assembly.Location));
            Assembly assembly = context.LoadFromStream(stream);
            MethodInfo method = assembly.GetType(typeof(IdGeneratorTest).FullName!)!.GetMethod(nameof(MakeUniqueAndReset))!;
            return (string)method.Invoke(null, [id])!;
        }
        finally
        {
            context.Unload();
        }
    }

    #endregion

    #region Tests

    [TestMethod]
    public void ReloadedAssemblyGetsTheSameIdAfterReset()
    {
        string id = $"Manufacturer Model {Guid.NewGuid()}";

        string firstLoad = MakeUniqueFromReloadedAssembly(id);
        string secondLoad = MakeUniqueFromReloadedAssembly(id);

        Assert.AreEqual(id, firstLoad);
        Assert.AreEqual(firstLoad, secondLoad);
    }

    [TestMethod]
    public void DuplicateIdsWithinAnAssemblyAreStillUnique()
    {
        string id = $"Manufacturer Model {Guid.NewGuid()}";

        Assert.AreEqual(id, IdGenerator.MakeUnique(id));
        Assert.AreEqual($"{id} (2)", IdGenerator.MakeUnique(id));
    }

    #endregion
}
