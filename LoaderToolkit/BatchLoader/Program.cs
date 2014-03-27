using LoaderLibrary.CommandLineParser;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<LoaderRegistry>();
            });

            List<Type> verbs = new List<Type>() 
            {
                typeof(Verbs.Create),
                typeof(Verbs.Start),
            };

            Verb v = null;

            try
            {
                PrintHeader();
                v = (Verb)ArgumentParser.Parse(args, verbs);
            }
            catch (ArgumentParserException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine();

                ArgumentParser.PrintUsage(verbs, Console.Out);
            }

            if (v != null)
            {
                v.Run();
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine(
            @"Batch loader utility
            (c) 2012-2014 László Dobos dobos@complex.elte.hu, János Szalai-Gindl szalai@complex.elte.hu
            Department of Physics of Complex Systems, Eötvös Loránd University

            ");
        }
    }
}
