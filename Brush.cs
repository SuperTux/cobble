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

namespace Cobble {
    class Brush {
        protected TileRepository tileRepository;
        protected List<int[,]> id_matrices = new List<int[,]>();
        protected List<bool[,]> solid_matrices = new List<bool[,]>();

        //protected int[,] getIdMatrix(int tx, int ty) {
        //    return new int[3, 3] {
        //               { getTileAt(tx-1,ty-1), getTileAt(tx+0,ty-1), getTileAt(tx+1,ty-1) },
        //               { getTileAt(tx-1,ty+0), getTileAt(tx+0,ty+0), getTileAt(tx+1,ty+0) },
        //               { getTileAt(tx-1,ty+1), getTileAt(tx+0,ty+1), getTileAt(tx+1,ty+1) }
        //            };
        //}

        public Brush(TileRepository tileRepository) {
            this.tileRepository = tileRepository;
        }

        public int Length {
            get {
                return id_matrices.Count;
            }
        }

        public void saveToFile(string fname) {
            FileStream fs = new FileStream(fname, FileMode.Create);
            TextWriter tw = new StreamWriter(fs);

            foreach (int[,] m1 in id_matrices) {
                tw.WriteLine("" + m1[0, 0] + "," + m1[0, 1] + "," + m1[0, 2] + "," + m1[1, 0] + "," + m1[1, 1] + "," + m1[1, 2] + "," + m1[2, 0] + "," + m1[2, 1] + "," + m1[2, 2] + "");
            }

            tw.Close();
            fs.Close();

        }

        public static Brush loadFromFile(TileRepository tileRepository, string fname) {
            TileRepository tr = tileRepository;

            Brush brush = new Brush(tileRepository);

            FileStream fs = new FileStream(fname, FileMode.Open);
            TextReader trd = new StreamReader(fs);

            try {
                string s;
                while ((s = trd.ReadLine()) != null) {
                    string[] v = s.Split(',');
                    if (v.Length < 9) continue;
                    int[,] ids = new int[3, 3] { 
                        {int.Parse(v[0]), int.Parse(v[1]), int.Parse(v[2])},
                        {int.Parse(v[3]), int.Parse(v[4]), int.Parse(v[5])},
                        {int.Parse(v[6]), int.Parse(v[7]), int.Parse(v[8])}
                    };
                    bool[,] sols = new bool[3, 3] {
                        {tr.isSolid[int.Parse(v[0])], tr.isSolid[int.Parse(v[1])], tr.isSolid[int.Parse(v[2])]},
                        {tr.isSolid[int.Parse(v[3])], tr.isSolid[int.Parse(v[4])], tr.isSolid[int.Parse(v[5])]},
                        {tr.isSolid[int.Parse(v[6])], tr.isSolid[int.Parse(v[7])], tr.isSolid[int.Parse(v[8])]}
                    };
                    brush.id_matrices.Add(ids);
                    brush.solid_matrices.Add(sols);
                }
            }
            catch (IOException) {
            }

            trd.Close();
            fs.Close();

            return brush;

        }

        protected bool equal(int[,] m1, int[,] m2) {
            for (int px = 0; px < 3; px++) {
                for (int py = 0; py < 3; py++) {
                    if (m1[px, py] != m2[px, py]) return false;
                }
            }
            return true;
        }

        protected bool matrixExists(int[,] m1) {
            foreach (int[,] m2 in id_matrices) {
                if (equal(m1, m2)) return true;
            }
            return false;
        }


        //if (!careful) {
        //    if (!matrixExists(new int[3, 3] {
        //       { tm.getTileAt(tx-2,ty-1), m1[0, 0], m1[0, 1] },
        //       { getTileAt(tx-2,ty+0), m1[1, 0], m1[1, 1] },
        //       { getTileAt(tx-2,ty+1), m1[2, 0], m1[2, 1] }
        //       })) thisSimilarity -= 100;
        //    if (!matrixExists(new int[3, 3] {
        //       { m1[0, 1], m1[0, 1], getTileAt(tx+2,ty-1)},
        //       { m1[1, 1], m1[1, 2], getTileAt(tx+2,ty+0)},
        //       { m1[2, 1], m1[2, 2], getTileAt(tx+2,ty+1)}
        //       })) thisSimilarity -= 100;


        //}

        public void learn(Tilemap tilemap, int tx, int ty) {
            Tilemap tm = tilemap;
            TileRepository tr = tileRepository;

            int[,] m1 = new int[3, 3] {
                           { tm.getTileAt(tx-1,ty-1), tm.getTileAt(tx+0,ty-1), tm.getTileAt(tx+1,ty-1) },
                           { tm.getTileAt(tx-1,ty+0), tm.getTileAt(tx+0,ty+0), tm.getTileAt(tx+1,ty+0) },
                           { tm.getTileAt(tx-1,ty+1), tm.getTileAt(tx+0,ty+1), tm.getTileAt(tx+1,ty+1) }
                        };
            bool[,] m2 = new bool[3, 3] {
                           { tr.isSolid[tm.getTileAt(tx-1,ty-1)], tr.isSolid[tm.getTileAt(tx+0,ty-1)], tr.isSolid[tm.getTileAt(tx+1,ty-1)] },
                           { tr.isSolid[tm.getTileAt(tx-1,ty+0)], tr.isSolid[tm.getTileAt(tx+0,ty+0)], tr.isSolid[tm.getTileAt(tx+1,ty+0)] },
                           { tr.isSolid[tm.getTileAt(tx-1,ty+1)], tr.isSolid[tm.getTileAt(tx+0,ty+1)], tr.isSolid[tm.getTileAt(tx+1,ty+1)] }
                        };

            if (!matrixExists(m1)) {
                id_matrices.Add(m1);
                solid_matrices.Add(m2);
            }
        }

        public void learn(Tilemap tilemap) {
            for (int tx = 0; tx < tilemap.width; tx++) {
                for (int ty = 0; ty < tilemap.height; ty++) {
                    learn(tilemap, tx, ty);
                }
            }
        }

        public void forget(Tilemap tilemap, int tx, int ty) {
            Tilemap tm = tilemap;
            TileRepository tr = tileRepository;

            int[,] m1 = new int[3, 3] {
                           { tm.getTileAt(tx-1,ty-1), tm.getTileAt(tx+0,ty-1), tm.getTileAt(tx+1,ty-1) },
                           { tm.getTileAt(tx-1,ty+0), tm.getTileAt(tx+0,ty+0), tm.getTileAt(tx+1,ty+0) },
                           { tm.getTileAt(tx-1,ty+1), tm.getTileAt(tx+0,ty+1), tm.getTileAt(tx+1,ty+1) }
                        };
            bool[,] m2 = new bool[3, 3] {
                           { tr.isSolid[tm.getTileAt(tx-1,ty-1)], tr.isSolid[tm.getTileAt(tx+0,ty-1)], tr.isSolid[tm.getTileAt(tx+1,ty-1)] },
                           { tr.isSolid[tm.getTileAt(tx-1,ty+0)], tr.isSolid[tm.getTileAt(tx+0,ty+0)], tr.isSolid[tm.getTileAt(tx+1,ty+0)] },
                           { tr.isSolid[tm.getTileAt(tx-1,ty+1)], tr.isSolid[tm.getTileAt(tx+0,ty+1)], tr.isSolid[tm.getTileAt(tx+1,ty+1)] }
                        };

            for (int i = 0; i < id_matrices.Count; i++) {
                if (equal(m1, id_matrices[i])) {
                    id_matrices.RemoveAt(i);
                    solid_matrices.RemoveAt(i);
                }
            }
        }

        protected void draw(Tilemap tilemap, int tx, int ty, bool erase) {
            Tilemap tm = tilemap;
            TileRepository tr = tileRepository;

            int[,] desiredIdPattern = new int[3, 3] {
                       { tm.getTileAt(tx-1,ty-1), tm.getTileAt(tx+0,ty-1), tm.getTileAt(tx+1,ty-1) },
                       { tm.getTileAt(tx-1,ty+0), tm.getTileAt(tx+0,ty+0), tm.getTileAt(tx+1,ty+0) },
                       { tm.getTileAt(tx-1,ty+1), tm.getTileAt(tx+0,ty+1), tm.getTileAt(tx+1,ty+1) }
                    };

            bool[,] desiredSolPattern = new bool[3, 3] {
                       { tr.isSolid[tm.getTileAt(tx-1,ty-1)], tr.isSolid[tm.getTileAt(tx+0,ty-1)], tr.isSolid[tm.getTileAt(tx+1,ty-1)] },
                       { tr.isSolid[tm.getTileAt(tx-1,ty+0)], !erase                             , tr.isSolid[tm.getTileAt(tx+1,ty+0)] },
                       { tr.isSolid[tm.getTileAt(tx-1,ty+1)], tr.isSolid[tm.getTileAt(tx+0,ty+1)], tr.isSolid[tm.getTileAt(tx+1,ty+1)] }
            };

            float bestSimilarity = 0;
            int[,] bestPattern = new int[3, 3] {
                       { tm.getTileAt(tx-1,ty-1), tm.getTileAt(tx+0,ty-1), tm.getTileAt(tx+1,ty-1) },
                       { tm.getTileAt(tx-1,ty+0), tm.getTileAt(tx+0,ty+0), tm.getTileAt(tx+1,ty+0) },
                       { tm.getTileAt(tx-1,ty+1), tm.getTileAt(tx+0,ty+1), tm.getTileAt(tx+1,ty+1) }
                    };

            for (int i = 0; i < id_matrices.Count; i++) {
                int[,] m1 = id_matrices[i];
                int[,] m2 = desiredIdPattern;

                bool[,] m3 = solid_matrices[i];
                bool[,] m4 = desiredSolPattern;

                float thisSimilarity = 0;
                for (int px = 0; px < 3; px++) {
                    for (int py = 0; py < 3; py++) {
                        if ((px == 1) && (py == 1)) {
                            if (tr.isSolid[m1[px, py]] == (!erase)) thisSimilarity += 1000;
                            continue;
                        }
                        if (m1[px, py] == m2[px, py]) thisSimilarity += 1;
                        if (m3[px, py] == m4[px, py]) thisSimilarity += 100;

                    }
                }

                if (thisSimilarity > bestSimilarity) { bestSimilarity = thisSimilarity; bestPattern = m1; }

            }

            if (bestSimilarity > 0) {
                int[,] m1 = bestPattern;

                tm.setTileAt(tx - 1, ty - 1, m1[0, 0]);
                tm.setTileAt(tx + 0, ty - 1, m1[0, 1]);
                tm.setTileAt(tx + 1, ty - 1, m1[0, 2]);
                tm.setTileAt(tx - 1, ty + 0, m1[1, 0]);
                tm.setTileAt(tx + 0, ty + 0, m1[1, 1]);
                tm.setTileAt(tx + 1, ty + 0, m1[1, 2]);
                tm.setTileAt(tx - 1, ty + 1, m1[2, 0]);
                tm.setTileAt(tx + 0, ty + 1, m1[2, 1]);
                tm.setTileAt(tx + 1, ty + 1, m1[2, 2]);
            }


        }

        public void draw(Tilemap tilemap, int tx, int ty) {
            draw(tilemap, tx, ty, false);
        }

        public void erase(Tilemap tilemap, int tx, int ty) {
            draw(tilemap, tx, ty, true);
        }

    }
}
