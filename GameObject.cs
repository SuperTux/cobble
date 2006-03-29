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

    public class GameObject : IComparable {
        private string typename;
        private static Dictionary<string, GameObject> exponents = new Dictionary<string,GameObject>(); /**< table of all known typename-to-GameObject mappings */

        public GameObject(string name) {
            this.typename = name;
        }

        static GameObject() {
            exponents["spawnpoint"] = new Spawnpoint("main", -1, -1);
            exponents["bell"] = new SpatialGameObject("bell", -1, -1);
            exponents["powerup"] = new PowerUp(-1, -1);
            foreach (string name in new string[] { "angrystone", "bouncingsnowball", "dispenser", "fish", "flame", "flyingsnowball", "jumpy", "kugelblitz", "mrbomb", "mriceblock", "mrrocket", "mrtree", "plant", "poisonivy", "skullyhop", "snowball", "snowsnail", "spidermite", "spiky", "sspiky", "stalactite", "yeti", "yeti_stalactite", "zeekling" }) {
                exponents[name] = new Badguy(name, -1, -1);
            }
        }

        public static List<GameObject> getExponents() {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (GameObject gameObject in exponents.Values) gameObjects.Add(gameObject);
            return gameObjects;
        }

        public static GameObject Parse(string name, Parser parser) {
            foreach (GameObject gameObject in exponents.Values) {
                if (gameObject.Type == name) {
                    return gameObject.ParseAsThis(name, parser);
                }
            }

            return new SpatialGameObject(name, parser);
        }

        public virtual GameObject ParseAsThis(string name, Parser parser) {
            return new GameObject(name, parser);
        }

        public GameObject(string name, Parser parser) {
            this.typename = name;
            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d) {
                if (parser.Depth == d + 1) {
                    if (parser.Type != Parser.LispType.SYMBOL)
                        throw new Exception("expected SYMBOL");
                    string symbol = parser.SymbolValue;
                    parser.Parse();
                    if (!tryParse(symbol, parser)) {
                        Console.WriteLine("WARNING: Unknown object property: \"" + symbol + "\", skipping");
                        SkipList(parser);
                    }
                }
            }
        }

        public virtual GameObject Clone() {
            GameObject clone = new GameObject(this.typename);
            return clone;
        }

        public virtual string DescriptiveText() {
            return "Game Object \"" + this.Type + "\"";
        }

        /**
         * <returns>true if token was successfully consumed</returns>
         */
        protected virtual bool tryParse(string symbol, Parser parser) {
            return false;
        }

        public void Write(LispWriter writer) {
            writer.StartList(this.typename);
            this.WriteProperties(writer);
            writer.EndList(this.typename);
        }

        protected virtual void WriteProperties(LispWriter writer) {
        }

        private static void SkipList(Lisp.Parser parser) {
            int d = parser.Depth;
            while (parser.Parse() && parser.Depth >= d)
                ;
        }

        public string Type {
            get {
                return typename;
            }
        }

        public override string ToString() {
            return typename;
        }

        public int CompareTo(object o) {
            if (!(o is GameObject)) throw new ArgumentException("Cannot compare GameObject to something else");
            GameObject go = (GameObject)o;
            return this.typename.CompareTo(go.typename);
        }

    }

    public class SpatialGameObject : GameObject {
        private float x = -1;
        private float y = -1;

        public SpatialGameObject(string name, float x, float y) : base(name) {
            this.x = x;
            this.y = y;
        }

        public SpatialGameObject(string name, Parser parser)
            : base(name, parser) {
        }

        public override GameObject ParseAsThis(string name, Parser parser) {
            return new SpatialGameObject(name, parser);
        }

        public override GameObject Clone() {
            SpatialGameObject clone = new SpatialGameObject(Type, X, Y);
            return clone;
        }

        protected override bool tryParse(string symbol, Parser parser) {
            switch (symbol) {
                case "x":
                    this.x = parser.FloatValue;
                    return true;
                case "y":
                    this.y = parser.FloatValue;
                    return true;
                default:
                    return base.tryParse(symbol, parser);
            }
        }

        protected override void WriteProperties(LispWriter writer) {
            base.WriteProperties(writer);
            if (x != -1) writer.Write("x", this.x);
            if (y != -1) writer.Write("y", this.y);
        }

        public float X {
            get {
                return x;
            }
            set {
                this.x = value;
            }
        }

        public float Y {
            get {
                return y;
            }
            set {
                this.y = value;
            }
        }

        public override string DescriptiveText() {
            return "Spatial Game Object \"" + this.Type + "\"";
        }
    
    }

    public class Badguy : SpatialGameObject {
        //FIXME: "stay-on-platform" flag
        //FIXME: direction?
        private bool stayonplatform = false;

        public Badguy(string name, float x, float y)
            : base(name, x, y) {
        }

        public Badguy(string name, Parser parser)
            : base(name, parser) {
        }

        public override GameObject ParseAsThis(string name, Parser parser) {
            return new Badguy(name, parser);
        }

        public override GameObject Clone() {
            Badguy clone = new Badguy(Type, X, Y);
            return clone;
        }

        public override string DescriptiveText() {
            return "Badguy \"" + this.Type + "\"";
        }

        protected override bool tryParse(string symbol, Parser parser) {
            switch (symbol) {
                case "stay-on-platform":
                    this.stayonplatform = parser.BoolValue;
                    return true;
                default:
                    return base.tryParse(symbol, parser);
            }
        }

        protected override void WriteProperties(LispWriter writer) {
            base.WriteProperties(writer);
            if (stayonplatform) writer.Write("stay-on-platform", this.stayonplatform);
        }

        public bool Stayonplatform {
            get {
                return stayonplatform;
            }
            set {
                this.stayonplatform = value;
            }
        }

    }

    public class Spawnpoint : SpatialGameObject {
        private string name = "main";

        /**
         * <param name="spawnname">Name of the spawnpoint, use "main" for default spawnpoint</param> 
         */
        public Spawnpoint(string name, float x, float y)
            : base("spawnpoint", x, y) {
            this.name = name;
        }

        public Spawnpoint(string name, Parser parser)
            : base(name, parser) {
        }

        public override GameObject ParseAsThis(string name, Parser parser) {
            return new Spawnpoint(name, parser);
        }

        public override GameObject Clone() {
            Spawnpoint clone = new Spawnpoint(Name, X, Y);
            return clone;
        }

        public override string DescriptiveText() {
            return "Spawnpoint \""+this.name+"\"";
        }

        protected override bool tryParse(string symbol, Parser parser) {
            switch (symbol) {
                case "name":
                    this.name = parser.StringValue;
                    return true;
                default:
                    return base.tryParse(symbol, parser);
            }
        }

        protected override void WriteProperties(LispWriter writer) {
            base.WriteProperties(writer);
            writer.Write("name", this.name);
        }

        public string Name {
            get {
                return name;
            }
            set {
                this.name = value;
            }
        }


    }

    public class PowerUp : SpatialGameObject {
        private string sprite = "images/powerups/potions/red-potion.sprite";
        private string script = "levelflip();";

        public PowerUp(float x, float y)
            : base("powerup", x, y) {
        }

        public PowerUp(string name, Parser parser)
            : base(name, parser) {
        }

        public override GameObject ParseAsThis(string name, Parser parser) {
            return new PowerUp(name, parser);
        }

        public override GameObject Clone() {
            PowerUp clone = new PowerUp(X, Y);
            return clone;
        }

        public override string DescriptiveText() {
            return "PowerUp \"" + this.sprite + "\", \"" + this.script + "\"";
        }

        protected override bool tryParse(string symbol, Parser parser) {
            switch (symbol) {
                case "sprite":
                    this.sprite = parser.StringValue;
                    return true;
                case "script":
                    this.script = parser.StringValue;
                    return true;
                default:
                    return base.tryParse(symbol, parser);
            }
        }

        protected override void WriteProperties(LispWriter writer) {
            base.WriteProperties(writer);
            writer.Write("sprite", this.sprite);
            writer.Write("script", this.script);
        }

        public string Sprite {
            get {
                return sprite;
            }
            set {
                this.sprite = value;
            }
        }

        public string Script {
            get {
                return script;
            }
            set {
                this.script = value;
            }
        }


    }

}
