﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace EscPosPrinter.Builder
{
    public static class Transpilator
    {
        private static IList<Command> commands;

        static Transpilator()
        {
            commands = new List<Command>();
        }

        public static IList<Command> TranspileElements(XPathNodeIterator elements, string super = null)
        {
            foreach (XPathNavigator child in elements)
            {
                if (string.IsNullOrEmpty(child.Name))
                    commands.Add(new Command
                    {
                        Tag = super,
                        Value = string.Join("", Regex.Split(child.Value, @"(?:\r\n|\n|\r|)")).TrimStart().TrimEnd()
                    });
                else
                {
                    if (!string.IsNullOrEmpty(child.Value))
                    {
                        TranspileElements(child.SelectChildren(XPathNodeType.All), child.Name);
                    }
                    else
                    {
                        commands.Add(new Command
                        {
                            Tag = child.Name
                        });
                    }

                    commands.Add(new Command
                    {
                        Tag = $"/{child.Name}"
                    });
                }
            }

            return commands;
        }
    }
}