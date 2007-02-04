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
    public class Tilemap {
        public int layer;
        public bool solid;
        public float speed;
        public int width;
        public int height;
        public List<int> tiles;

        public Tilemap(Parser parser) {
            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d) {
                if (parser.Depth == d + 1) {
                    if (parser.Type != Parser.LispType.SYMBOL)
                        throw new Exception("expected SYMBOL");
                    string symbol = parser.SymbolValue;
                    parser.Parse();
                    switch (symbol) {
                        case "z-pos":
                            this.layer = parser.IntegerValue;
                            break;
                        case "solid":
                            this.solid = parser.BoolValue;
                            break;
                        case "speed":
                            this.speed = parser.FloatValue;
                            break;
                        case "width":
                            this.width = parser.IntegerValue;
                            break;
                        case "height":
                            this.height = parser.IntegerValue;
                            break;
                        case "tiles":
                            ParseTiles(parser);
                            break;
                        default:
                            throw new Exception("Unexpected entry in tilemap list: " + parser.SymbolValue);
                    }
                }
            }
        }

        public Tilemap(int layer, bool solid, int width, int height) {
            this.layer = layer;
            this.solid = solid;
            this.speed = 1.0F;
            this.width = width;
            this.height = height;
            this.tiles = new List<int>(width * height);
            for (int i = 0; i < width * height; i++) this.tiles.Add(0);
        }

        public void Write(LispWriter writer) {
            writer.StartList("tilemap");
            writer.Write("layer", this.layer);
            writer.Write("solid", this.solid);
            writer.Write("speed", this.speed);
            writer.Write("width", this.width);
            writer.Write("height", this.height);
            writer.Write("tiles", this.tiles);
            writer.EndList("tilemap");
        }

        public override string ToString() {
        	switch (this.layer)
        	{
        		case -100:
        			return "Background (-100)";
        		case 0:
        			return "Interactive (0)";
        		case 100:
        			return "Foreground (100)";
        		default:
        			return "Layer " + this.layer.ToString();
        	}
        }

        private void ParseTiles(Parser parser) {
            this.tiles = new List<int>();

            if (parser.Type == Parser.LispType.END_LIST) return;

            int d = parser.Depth;
            do {
                if (parser.Type != Parser.LispType.INTEGER) throw new Exception("unexpected lisp data: " + parser.Type);
                this.tiles.Add(parser.IntegerValue);
            } while (parser.Parse() && parser.Depth >= d);
        }

        public int getTileAt(int px, int py) {
            if (px < 0) return 0;
            if (py < 0) return 0;
            if (px >= width) return 0;
            if (py >= height) return 0;
            return tiles[(width * py) + px];
        }

        public void setTileAt(int px, int py, int id) {
            if (px < 0) return;
            if (py < 0) return;
            if (px >= width) return;
            if (py >= height) return;
            tiles[(width * py) + px] = id;
        }

        public void OffsetBy(int offsetX, int offsetY) {
            List<int> newTiles = new List<int>();
            for (int i = 0; i < width * height; i++) newTiles.Add(0);
            for (int ty = 0; ty < height; ty++) {
                for (int tx = 0; tx < width; tx++) {
                    newTiles[(width * ty) + tx] = getTileAt((tx+2*width-offsetX)%width, (ty+2*height-offsetY)%height);
                }
            }
            this.tiles = newTiles;
        }

        public void ResizeTo(int newWidth, int newHeight) {
            List<int> newTiles = new List<int>();
            for (int i = 0; i < newWidth * newHeight; i++) newTiles.Add(0);
            for (int ty = 0; ty < newHeight; ty++) {
                for (int tx = 0; tx < newWidth; tx++) {
                    newTiles[(newWidth * ty) + tx] = getTileAt(tx, ty);
                }
            }
            this.width = newWidth;
            this.height = newHeight;
            this.tiles = newTiles;
        }

        public void Replace(int oldId, int newId) {
            for (int ty = 0; ty < height; ty++) {
                for (int tx = 0; tx < width; tx++) {
                    if (getTileAt(tx, ty) == oldId) setTileAt(tx, ty, newId);
                }
            }            
        }
    }
}
