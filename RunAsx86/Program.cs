using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RunAsx86
{
	class Program
	{
		static int Main(string[] args)
		{
			try
			{
				if (!args.Any())
				{
					Console.WriteLine("Please, go to application directory, provide path and arguments");
					return -1;
				}
				var folder = Path.GetDirectoryName(args[0]);
				var target = Prepare(
					string.IsNullOrEmpty(folder) ? Path.Combine(Environment.CurrentDirectory, args[0]) : args[0]);

				Console.WriteLine("Executing: " + target);
				AppDomain.CurrentDomain.AssemblyResolve += (o,e) => LoadFromSameFolder(o,e,folder);
				var assembly = Assembly.LoadFile(target);

				var type = assembly.GetTypes().FirstOrDefault(t => GetMethod(t) != null);
				if (type == null)
				{
					Console.WriteLine("Can't find method Main");
					return -1;
				}
				GetMethod(type).Invoke(type, args.Skip(1).Cast<object>().ToArray());
				Console.WriteLine("Done");
				return 1;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return -1;
			}
		}

		private static string Prepare(string path)
		{
			if (File.Exists(path)) return path;
			if (File.Exists(path + ".exe")) return path + ".exe";
			throw new ArgumentException("Nothing to execute: '{0}'", path);
		}

		static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args, string folder)
		{
			string assemblyPath;
			if (!string.IsNullOrEmpty(folder))
			{
				assemblyPath = Path.Combine(folder, new AssemblyName(args.Name).Name + ".dll");
				if (File.Exists(assemblyPath)) 
					return Assembly.LoadFrom(assemblyPath);
			}
			assemblyPath = Path.Combine(Environment.CurrentDirectory, new AssemblyName(args.Name).Name + ".dll");
			return File.Exists(assemblyPath) ? 
				Assembly.LoadFrom(assemblyPath) : 
				null;
		}

		private static MethodInfo GetMethod(Type t)
		{
			return t.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
		}
	}
}
