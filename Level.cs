//  $Id: foo.cpp 2979 2006-01-10 00:00:04Z sommer $
//
//  Cobble - A simple SuperTux level editor
//  Copyright (C) 2006 Christoph Sommer <supertux@2006.expires.deltadevelopment.de>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Lisp;

namespace Cobble {
    public class Level {
        public int version;
        public string name;
        public string author;
        public List<Sector> sectors;

        public Level(string filename) {
            this.sectors = new List<Sector>();

            FileStream fs = new FileStream(filename, FileMode.Open);
            StreamReader stream = new StreamReader(fs);

            Lisp.Parser parser = new Lisp.Parser(stream);
            parser.Parse();
            if (parser.Type != Parser.LispType.START_LIST)
                throw new Exception("Expected START_LIST");
            parser.Parse();
            if (parser.Type != Parser.LispType.SYMBOL)
                throw new Exception("Expected symbol");
            if (parser.SymbolValue != "supertux-level")
                throw new Exception("not a supertux level file. (" + parser.SymbolValue + ")");

            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d) {
                if (parser.Depth == d && parser.Type != Parser.LispType.START_LIST) {
                    Console.WriteLine("non-cons type in list...");
                    continue;
                }

                if (parser.Depth == d + 1) {
                    if (parser.Type != Parser.LispType.SYMBOL) throw new Exception("Expected symbol in list element");
                    switch (parser.SymbolValue) {
                        case "version":
                            parser.Parse();
                            this.version = parser.IntegerValue;
                            break;
                        case "name":
                            parser.Parse();
                            parser.Parse();
                            parser.Parse();
                            this.name = parser.StringValue;
                            parser.Parse();
                            break;
                        case "author":
                            parser.Parse();
                            this.author = parser.StringValue;
                            break;
                        case "extro":
                            SkipList(parser);
                            break;
                        case "sector":
                            this.sectors.Add(new Sector(parser));
                            break;
                        default:
                            throw new Exception("Unexpected entry in level list: " + parser.SymbolValue);
                    }
                }
            }

            stream.Close();
            fs.Close();
        }

        public Level() {
            this.version = 2;
            this.name = "Unnamed Level";
            this.author = "Anonymous";
            this.sectors = new List<Sector>();
            this.sectors.Add(new Sector("main"));
        }

        public void Write(string filename) {
            FileStream fs = new FileStream(filename, FileMode.Create);

            TextWriter tw = new StreamWriter(fs);
            LispWriter writer = new LispWriter(tw);

            writer.WriteComment("Created with Cobble");
            writer.StartList("supertux-level");
            writer.Write("version", this.version);
            writer.StartList("name"); writer.Write("_", this.name); writer.EndList("name");
            writer.Write("author", this.author);
            foreach (Sector sector in this.sectors) {
                sector.Write(writer);
            }
            writer.EndList("supertux-level");
            tw.Close();
            fs.Close();
        }

        private static void SkipList(Lisp.Parser parser) {
            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d)
                ;
        }

    }
}
