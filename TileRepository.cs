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
using System.Drawing;
using System.IO;
using System.Collections;

namespace Cobble {
    public class TileRepository {
        public Dictionary<int, bool> isSolid = new Dictionary<int, bool>();
        private Dictionary<int, Image> tileRepository = new Dictionary<int, Image>();
        private TileSet tileset;

        public TileRepository(string filename) {
            isSolid[0] = false;

            LoadingScreenForm loadingScreen = new LoadingScreenForm();
            loadingScreen.setSubject("tileset");
            loadingScreen.setMax(100);
            loadingScreen.Show();
            loadingScreen.Refresh();

            if (!File.Exists(filename)) throw new ArgumentException("Tile file not found");

            String basePath = System.IO.Path.GetDirectoryName(filename);

            tileset = new TileSet();
            tileset.Parse(filename);

            // append a TileGroup (misc) with all tiles that are not yet in a TileGroup
            TileGroup miscTiles = new TileGroup();
            foreach (Tile tile in tileset.Tiles) {
                if (tile == null) continue;
                bool found = false;
                foreach (TileGroup tileGroup in tileset.TileGroups) {
                    if (tileGroup.Tiles.Contains(tile.ID)) found = true;
                }
                if (found) continue;
                miscTiles.Name = "(misc)";
                miscTiles.Tiles.Add(tile.ID);
            }
            tileset.TileGroups.Add(miscTiles);

            loadingScreen.setMax(tileset.Tiles.Count);

            int progress = 0;
            foreach (Tile tile in tileset.Tiles) {
                progress++;
                if (tile == null) continue;

                addTile(tile, basePath);

                loadingScreen.setProgress(progress);
            }

            loadingScreen.Close();
        }

        public ArrayList getTileGroups() {
            return tileset.TileGroups;
        }

        public Image getTile(int id) {
            if (!tileRepository.ContainsKey(id)) return null;
            return tileRepository[id];
        }

        private void addTile(Tile tile, string basePath) {
            if (tile.ID == 0) return;

            Rectangle srcRect = new Rectangle(0, 0, 32, 32);
            Bitmap srcImg = null;

            if (tile.EditorImage != null) {
                string fname = tile.EditorImage;
                fname = System.IO.Path.Combine(basePath, fname.Replace('/', System.IO.Path.DirectorySeparatorChar));
                srcImg = new Bitmap(fname);
            } else if ((tile.Images != null) && (tile.Images.Count >= 1)) {
                ImageRegion ir = (ImageRegion)tile.Images[0];
                string fname = ir.ImageFile;
                fname = System.IO.Path.Combine(basePath, fname.Replace('/', System.IO.Path.DirectorySeparatorChar));
                srcImg = new Bitmap(fname);

                if (ir.Region != Rectangle.Empty) {
                    srcRect = ir.Region;
                } else if (ir.RelativeRegion != RectangleF.Empty) {
                    int x = (int)(ir.RelativeRegion.X * srcImg.Width);
                    int y = (int)(ir.RelativeRegion.Y * srcImg.Height);
                    int w = (int)(ir.RelativeRegion.Width * srcImg.Width);
                    int h = (int)(ir.RelativeRegion.Height * srcImg.Height);
                    srcRect = new Rectangle(x, y, w, h);
                }
            } else throw new ArgumentException("No usable images found for tile #"+tile.ID);


            Bitmap dstImg = new Bitmap(32, 32);
            Graphics gr = Graphics.FromImage(dstImg);
            gr.DrawImage(srcImg, new Rectangle(0, 0, 32, 32), srcRect, GraphicsUnit.Pixel);
            tileRepository[tile.ID] = dstImg;
            isSolid[tile.ID] = tile.Solid;
        }

    }
}
