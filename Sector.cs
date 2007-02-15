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
using Lisp;

namespace Cobble {
    public class Sector {
        public string name;
        public string music;
        public float gravity;
        public List<Tilemap> tilemaps;
        public Background background;
        public List<GameObject> gameObjects;
        //FIXME: More to come

        public Sector(Parser parser) {
            this.tilemaps = new List<Tilemap>();
            this.gameObjects = new List<GameObject>();

            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d) {
                if (parser.Depth == d + 1) {
                    if (parser.Type != Parser.LispType.SYMBOL)
                        throw new Exception("expected SYMBOL");
                    string symbol = parser.SymbolValue;
                    parser.Parse();
                    switch (symbol) {
                        case "name":
                            this.name = parser.StringValue;
                            break;
                        case "music":
                            this.music = parser.StringValue;
                            break;
                        case "gravity":
                            this.gravity = parser.FloatValue;
                            break;
                        case "tilemap":
                            this.tilemaps.Add(new Tilemap(parser));
                            break;
                        case "background":
                            this.background = new Background(parser);
                            break;
                        default:
                            //this.gameObjects.Add(new GameObject(symbol, parser));
                            this.gameObjects.Add(GameObject.Parse(symbol, parser));
                            //Console.WriteLine("WARNING: Unknown tile element " + symbol + ", skipping");
                            //SkipList(parser);
                            break;
                    }
                }
            }
        }

        public Sector(string name) {
            this.name = name;
            this.music = "music/chipdisko.ogg";
            this.gravity = 10.0F;
            this.tilemaps = new List<Tilemap>();
            this.tilemaps.Add(new Tilemap(-100, false, 310, 19));
            this.tilemaps.Add(new Tilemap(0, true, 310, 19));
            this.tilemaps.Add(new Tilemap(100, false, 310, 19));
            this.background = new Background("images/background/arctis.jpg", 0.5F);
            this.gameObjects = new List<GameObject>();
            this.gameObjects.Add(new Spawnpoint("main", 100, 100));
        }

        public void Write(LispWriter writer) {
            writer.StartList("sector");
            writer.Write("name", this.name);
            writer.Write("music", this.music);
            writer.Write("gravity", this.gravity);
            foreach (Tilemap tilemap in this.tilemaps) {
                tilemap.Write(writer);
            }
            background.Write(writer);
            writer.StartList("camera");
            writer.Write("mode", "normal");
            writer.EndList("camera");
            foreach (GameObject gameObject in this.gameObjects) {
                gameObject.Write(writer);
            }
            writer.EndList("sector");
        }


        public override string ToString() {
            return this.name;
        }

        private static void SkipList(Lisp.Parser parser) {
            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d)
                ;
        }

    }
}
