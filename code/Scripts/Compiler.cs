using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace GameEngine.Scripts
{
    public class Compiler
    {
        public static Type? CompileScript(string filepath)
        {
            if (!File.Exists(filepath))
            {
                Console.WriteLine("file doesnt exist");
                return null;
            }
            string sourcecode = File.ReadAllText(filepath);
            SyntaxTree syntaxtree = CSharpSyntaxTree.ParseText(sourcecode);
            // Automatically gather references from all currently loaded assemblies
            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .Cast<MetadataReference>()
                .ToList();

            //create compliation
            string assemblyname = Path.GetRandomFileName();
            var compilation = CSharpCompilation.Create(
                assemblyname,
                new[] { syntaxtree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            //compile to mem
            using var pestream = new MemoryStream();
            var emitResult = compilation.Emit(pestream);
            if (!emitResult.Success)
            {
                Console.WriteLine("compilation error");
                return null;
            }

            pestream.Seek(0, SeekOrigin.Begin);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(pestream);
            
            Type? scripttype = assembly.GetTypes()
                .FirstOrDefault(t => typeof(Script).IsAssignableFrom(t) && !t.IsAbstract);

            if (scripttype == null)
            {
                Console.WriteLine("compilation error");
                return null;
            }
            return scripttype;
        }
    }
}
