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
    public class Background {
        public string image;
        public float speed;

        public Background(Parser parser) {
            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d) {
                if (parser.Depth == d + 1) {
                    if (parser.Type != Parser.LispType.SYMBOL)
                        throw new Exception("expected SYMBOL");
                    string symbol = parser.SymbolValue;
                    parser.Parse();
                    switch (symbol) {
                        case "image":
                            this.image = parser.StringValue;
                            break;
                        case "speed":
                            this.speed = parser.FloatValue;
                            break;
                        default:
                            throw new Exception("Unexpected entry in background list: " + parser.SymbolValue);
                    }
                }
            }
        }

        public Background(string image, float speed) {
            this.image = image;
            this.speed = speed;
        }

        public void Write(LispWriter writer) {
            writer.StartList("background");
            writer.Write("image", this.image);
            writer.Write("speed", this.speed);
            writer.EndList("background");
        }

    
    }
}
