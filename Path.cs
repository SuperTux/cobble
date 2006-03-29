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

namespace Cobble {

    public class PathNode {
        public float x;
        public float y;
        public float time;

        public PathNode(float x, float y, float time) {
            this.x = x;
            this.y = y;
            this.time = time;
        }

        public PathNode(Lisp.Parser parser) {
            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d) {
                if (parser.Depth == d + 1) {
                    if (parser.Type != Lisp.Parser.LispType.SYMBOL) throw new Exception("expected SYMBOL");
                    string symbol = parser.SymbolValue;
                    parser.Parse();
                    switch (symbol) {
                        case "x":
                            this.x = parser.FloatValue;
                            break;
                        case "y":
                            this.x = parser.FloatValue;
                            break;
                        case "time":
                            this.x = parser.FloatValue;
                            break;
                        default:
                            throw new Exception("Unknown Token in Path Node");
                    }
                }
            }
        }

        public void Write(LispWriter writer) {
            writer.StartList("node");
            writer.Write("x", x);
            writer.Write("y", y);
            writer.Write("time", time);
            writer.EndList("node");
        }


    }

    public class Path : GameObject {
        private string name;
        public List<PathNode> pathNodes;

        public Path(string name)
            : base(name) {
        }

        public Path(string name, List<PathNode> pathNodes)
            : base(name) {
            this.pathNodes.AddRange(pathNodes);
        }

        public Path(string name, Lisp.Parser parser)
            : base(name, parser) {
        }

        public override GameObject ParseAsThis(string name, Lisp.Parser parser) {
            return new SpatialGameObject(name, parser);
        }

        public override GameObject Clone() {
            Path clone = new Path(name);
            return clone;
        }

        protected override bool tryParse(string symbol, Lisp.Parser parser) {
            switch (symbol) {
                case "name":
                    this.name = parser.StringValue;
                    return true;
                case "node":
                    this.pathNodes.Add(new PathNode(parser));
                    return true;
                default:
                    return base.tryParse(symbol, parser);
            }
        }

        protected override void WriteProperties(LispWriter writer) {
            base.WriteProperties(writer);
            writer.Write("name", this.name);
            foreach (PathNode pn in pathNodes) pn.Write(writer);
        }

        public string Name {
            get {
                return name;
            }
            set {
                this.name = value;
            }
        }

        public override string DescriptiveText() {
            return "Path \"" + this.name + "\"";
        }

    }

}
