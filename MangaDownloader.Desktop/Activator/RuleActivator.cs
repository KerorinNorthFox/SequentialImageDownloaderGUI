using MangaDownloader.Rule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MangaDownloader.Desktop.Activator
{
    public class RuleActivator
    {
        public static List<IRule> LoadRules(string pluginDir)
        {
            List<IRule> rules = new List<IRule>();

            if (!Directory.Exists(pluginDir))
            {
                return rules;
            }

            foreach (var dllPath in Directory.GetFiles(pluginDir, "*.dll"))
            {
                Assembly asm;
                try
                {
                    asm = Assembly.LoadFrom(dllPath);
                    Debug.WriteLine($"{asm.GetName().Name} : assembly loaded.");
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"{e.Message} >> on Assembly.LoadFrom in LoadRules.");
                    continue;
                }

                foreach (var type in asm.GetTypes())
                {
                    if (type == null)
                    {
                        continue;
                    }

                    if (type.GetInterfaces().Contains(typeof(IRule)))
                    {
                        try
                        {
                            var rule = (IRule)System.Activator.CreateInstance(type)!;
                            rules.Add(rule);
                            Debug.WriteLine($"{rule.Selector.Domain} : rule loaded.");
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine($"{e.Message} >> on Activator.CreateInstance in LoadRules");
                            continue;
                        }
                    }
                }
            }

            return rules;
        }
    }
}
