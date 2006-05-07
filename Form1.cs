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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Cobble {
    public partial class Form1 : Form {
        Level level = null;
        Sector currentSector = null;
        Tilemap currentTilemap = null;

        int currentTileId = 0;
        TileGroup currentTilegroup = null;

        TileRepository tileRepository = null;

        string currentLevelFilename = null;

        Rectangle currentSelection = new Rectangle(0, 0, 0, 0);
        Rectangle currentFloatingTilemapPos = new Rectangle(0, 0, 0, 0);
        int[,] currentFloatingTilemap = null;
        Brush currentBrush = null;

        enum CanvasAction { none, drawingTiles, selecting, movingFloatingTilemap, movingObject, drawingBrush };
        CanvasAction currentCanvasAction = CanvasAction.none;
        Point canvasActionStart;

        SpatialGameObject currentGameObject;

        public Form1() {
            InitializeComponent();

            if (!File.Exists("data\\images\\tiles.strf")) {
                MessageBox.Show("Cobble was unable to find data\\images\\tiles.strf.\n\nMake sure your current working directory is the SuperTux main directory.\n\nAborting.", "Cobble - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException("Abort");
            }

            tileRepository = new TileRepository("data\\images\\tiles.strf");
            currentBrush = new Brush(tileRepository);
            foreach (TileGroup tileGroup in tileRepository.getTileGroups()) {
                cbTilegroup.Items.Add(tileGroup);
            }
            if (cbTilegroup.Items.Count >= 1) cbTilegroup.SelectedIndex = 0;

            foreach (GameObject gameObject in GameObject.getExponents()) {
                lbGameObjects.Items.Add(gameObject);
            }
            if (lbGameObjects.Items.Count >= 1) lbGameObjects.SelectedIndex = 0;

            btnModeTileAdd_Click(null, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        
        private void pbCanvas_Paint(object sender, PaintEventArgs e) {
            Graphics gr = e.Graphics;

            if (currentSector == null) return;
            if (tileRepository == null) return;

            foreach (Tilemap tilemap in currentSector.tilemaps) {
                if (!clTilemaps.CheckedItems.Contains(tilemap)) continue;

                for (int ty = 0; ty < tilemap.height; ty++) {
                    for (int tx = 0; tx < tilemap.width; tx++) {
                        int px = tx * 32;
                        int py = ty * 32;

                        if (px + 32 < e.ClipRectangle.Left) continue;
                        if (px > e.ClipRectangle.Right) continue;
                        if (py + 32 < e.ClipRectangle.Top) continue;
                        if (py > e.ClipRectangle.Bottom) continue;

                        int id = tilemap.getTileAt(tx, ty);
                        Image tile = tileRepository.getTile(id);

                        if (tile == null) continue;
                        gr.DrawImageUnscaled(tile, px, py);
                    }
                }

                if (currentTilemap == tilemap) {
                    if (currentSelection.Width > 0) {
                        gr.DrawRectangle(Pens.Red, new Rectangle(currentSelection.X * 32, currentSelection.Y * 32, currentSelection.Width * 32, currentSelection.Height * 32));
                    }
                    if (currentFloatingTilemap != null) {
                        for (int ty = 0; ty < currentFloatingTilemap.GetLength(1); ty++) {
                            for (int tx = 0; tx < currentFloatingTilemap.GetLength(0); tx++) {
                                int px = (currentFloatingTilemapPos.X + tx) * 32;
                                int py = (currentFloatingTilemapPos.Y + ty) * 32;

                                if (px + 32 < e.ClipRectangle.Left) continue;
                                if (px > e.ClipRectangle.Right) continue;
                                if (py + 32 < e.ClipRectangle.Top) continue;
                                if (py > e.ClipRectangle.Bottom) continue;

                                int id = currentFloatingTilemap[tx, ty];
                                Image tile = tileRepository.getTile(id);

                                if (tile == null) continue;
                                gr.DrawImageUnscaled(tile, px, py);
                            }
                        }

                        gr.DrawRectangle(Pens.Lime, new Rectangle(currentFloatingTilemapPos.X * 32, currentFloatingTilemapPos.Y * 32, currentFloatingTilemapPos.Width * 32, currentFloatingTilemapPos.Height * 32));
                    }
                }
            }

            foreach (GameObject gameObject in currentSector.gameObjects) {
                if (!(gameObject is SpatialGameObject)) continue;
                SpatialGameObject spatialGameObject = (SpatialGameObject)gameObject;

                int px = (int)spatialGameObject.X;
                int py = (int)spatialGameObject.Y;

                gr.DrawEllipse(Pens.Blue, new Rectangle(px - 16, py - 16, 32, 32));
                gr.DrawString(gameObject.DescriptiveText(), new Font(FontFamily.GenericSansSerif, 8), Brushes.Blue, px + 12, py - 18);
            }

        }

        private void anchorFloatingTilemap() {
            if (currentFloatingTilemap == null) return;

            for (int y = currentFloatingTilemapPos.Top; y < currentFloatingTilemapPos.Bottom; y++) {
                for (int x = currentFloatingTilemapPos.Left; x < currentFloatingTilemapPos.Right; x++) {
                    currentTilemap.setTileAt(x, y, currentFloatingTilemap[x - currentFloatingTilemapPos.Left, y - currentFloatingTilemapPos.Top]);
                }
            }
            currentFloatingTilemap = null;
            pbCanvas.Invalidate();
        }

        private void createFloatingTilemapFromSelection(bool viaCut) {
            currentFloatingTilemap = new int[currentSelection.Width, currentSelection.Height];
            for (int y = currentSelection.Top; y < currentSelection.Bottom; y++) {
                for (int x = currentSelection.Left; x < currentSelection.Right; x++) {
                    currentFloatingTilemap[x - currentSelection.Left, y - currentSelection.Top] = currentTilemap.getTileAt(x, y);
                    if (viaCut) currentTilemap.setTileAt(x, y, 0);
                }
            }
            currentFloatingTilemapPos = currentSelection;
            currentSelection.Width = 0;
            pbCanvas.Invalidate();
        }

        private void eraseSelection() {
            for (int y = currentSelection.Top; y < currentSelection.Bottom; y++) {
                for (int x = currentSelection.Left; x < currentSelection.Right; x++) {
                    currentTilemap.setTileAt(x, y, 0);
                }
            }
        }

        private SpatialGameObject getNearestGameObject(int x, int y, double maxDistance) {
            SpatialGameObject nearestObject = null;
            foreach (GameObject gameObject in currentSector.gameObjects) {
                if (!(gameObject is SpatialGameObject)) continue;
                SpatialGameObject spatialGameObject = (SpatialGameObject)gameObject;

                double dst = Math.Sqrt(Math.Pow((x - spatialGameObject.X), 2) + Math.Pow((y - spatialGameObject.Y), 2));
                if (dst <= maxDistance) {
                    maxDistance = dst;
                    nearestObject = spatialGameObject;
                }
            }
            return nearestObject;
        }

        private void pbCanvas_MouseDown(object sender, MouseEventArgs e) {
            int tx = e.X / 32;
            int ty = e.Y / 32;

            if (currentSector == null) return;

            if ((e.Button == MouseButtons.Left) && (tcToolSettings.SelectedTab == tpTiles)) {
                currentCanvasAction = CanvasAction.drawingTiles;
            }

            if ((e.Button == MouseButtons.Left) && (tcToolSettings.SelectedTab == tpTileManip)) {
                if ((currentFloatingTilemap != null) && (currentFloatingTilemapPos.Contains(tx,ty))) {
                    currentCanvasAction = CanvasAction.movingFloatingTilemap;
                } else
                if (currentSelection.Contains(tx, ty)) {
                    createFloatingTilemapFromSelection(true);
                    currentCanvasAction = CanvasAction.movingFloatingTilemap;
                    pbCanvas.Invalidate();
                } else {
                    anchorFloatingTilemap();
                    pbCanvas.Invalidate();
                    currentCanvasAction = CanvasAction.selecting;
                    currentSelection = new Rectangle(tx, ty, 0, 0);
                }
            }
            if ((e.Button == MouseButtons.Left) && (tcToolSettings.SelectedTab == tpObjects)) {
                GameObject selectedObject = (GameObject)lbGameObjects.SelectedItem;
                if ((selectedObject != null) && (selectedObject is SpatialGameObject)) {
                    SpatialGameObject spatialGameObject = (SpatialGameObject)selectedObject.Clone();
                    spatialGameObject.X = e.X;
                    spatialGameObject.Y = e.Y;
                    currentSector.gameObjects.Add(spatialGameObject);
                    pbCanvas.Invalidate();
                }
            }
            if ((e.Button == MouseButtons.Left) && (tcToolSettings.SelectedTab == tpGameObjectManip)) {
                currentGameObject = getNearestGameObject(e.X, e.Y, 32);
                if (currentGameObject != null) {
                    currentCanvasAction = CanvasAction.movingObject;
                    pbCanvas.Invalidate();
                }
            }
            if ((e.Button == MouseButtons.Left) && (tcToolSettings.SelectedTab == tpBrushes)) {
                currentCanvasAction = CanvasAction.drawingBrush;
            }


            canvasActionStart.X = e.X;
            canvasActionStart.Y = e.Y;
            pbCanvas_MouseMove(sender, e);
        }

        private void pbCanvas_MouseMove(object sender, MouseEventArgs e) {
            if (currentTilemap == null) return;
            if (!clTilemaps.CheckedItems.Contains(currentTilemap)) return;

            int tx = e.X / 32;
            int ty = e.Y / 32;
            slCoordinates.Text = tx + "," + ty;
            slTileId.Text = currentTilemap.getTileAt(tx, ty).ToString();
            if (currentCanvasAction == CanvasAction.drawingTiles) {
                if (ModifierKeys == Keys.Shift) {
                    currentTilemap.setTileAt(tx, ty, 0);
                } else {
                    currentTilemap.setTileAt(tx, ty, currentTileId);
                }
                pbCanvas.Invalidate(new Rectangle(tx * 32, ty * 32, 32, 32));
            }
            if (currentCanvasAction == CanvasAction.selecting) {
                currentSelection.Width = tx - currentSelection.X + 1;
                currentSelection.Height = ty - currentSelection.Y + 1;
                pbCanvas.Invalidate();
            }
            if (currentCanvasAction == CanvasAction.movingFloatingTilemap) {
                int dx = tx - (canvasActionStart.X / 32);
                int dy = ty - (canvasActionStart.Y / 32);
                currentFloatingTilemapPos.Offset(dx, dy);
                canvasActionStart.X = e.X;
                canvasActionStart.Y = e.Y;
                pbCanvas.Invalidate();
            }
            if (currentCanvasAction == CanvasAction.movingObject) {
                if (ModifierKeys == Keys.Shift) {
                    currentSector.gameObjects.Remove(currentGameObject);
                    pbCanvas.Invalidate();
                    currentCanvasAction = CanvasAction.none;
                } else {
                    int dx = e.X - canvasActionStart.X;
                    int dy = e.Y - canvasActionStart.Y;
                    currentGameObject.X += dx;
                    currentGameObject.Y += dy;
                    canvasActionStart.X = e.X;
                    canvasActionStart.Y = e.Y;
                    pbCanvas.Invalidate();
                }
            }
            if (currentCanvasAction == CanvasAction.drawingBrush) {
                if (ModifierKeys == Keys.Shift) {
                    if (currentBrush != null) currentBrush.erase(currentTilemap, tx, ty);
                } else if (ModifierKeys == Keys.Control) {
                    if (currentBrush != null) {
                        currentBrush.learn(currentTilemap, tx, ty);
                        laBrushSize.Text = currentBrush.Length + " Patterns";
                    }
                } else if (ModifierKeys == (Keys.Control | Keys.Shift)) {
                    if (currentBrush != null) {
                        currentBrush.forget(currentTilemap, tx, ty);
                        laBrushSize.Text = currentBrush.Length + " Patterns";
                    }
                } else {
                    if (currentBrush != null) currentBrush.draw(currentTilemap, tx, ty);
                }
                pbCanvas.Invalidate(new Rectangle((tx - 1) * 32, (ty - 1) * 32, 32 * 3, 32 * 3));
            }


        }

        private void pbCanvas_MouseUp(object sender, MouseEventArgs e) {
            currentCanvasAction = CanvasAction.none;
        }

        private void cbTilegroup_SelectedIndexChanged(object sender, EventArgs e) {
            currentTilegroup = (TileGroup)cbTilegroup.SelectedItem;

            pbTiles.Invalidate();
        }

        private void pbTiles_Paint(object sender, PaintEventArgs e) {
            if (currentTilegroup == null) return;

            //int tilesPerRow = (pnTilesContainer.ClientSize.Width / 32);
            int tilesPerRow = ((pnTilesContainer.Width - 16) / 32); //FIXME: hack, can we get scrollbar width?
            if (tilesPerRow < 1) tilesPerRow = 1;

            pbTiles.Width = tilesPerRow * 32;
            pbTiles.Height = (int)Math.Ceiling((float)currentTilegroup.Tiles.Count / (float)tilesPerRow) * 32;

            Graphics gr = e.Graphics;

            int tx = 0;
            int ty = 0;

            int currentTilePx = -1;
            int currentTilePy = -1;

            foreach (int id in currentTilegroup.Tiles) {
                int px = tx * 32;
                int py = ty * 32;

                if (currentTileId == id) {
                    currentTilePx = px;
                    currentTilePy = py;
                }


                Image tile = tileRepository.getTile(id);

                if (tile == null) continue;
                gr.DrawImageUnscaled(pbTileBackground.Image, px, py);
                gr.DrawImageUnscaled(tile, px, py);

                tx += 1; if (tx >= tilesPerRow) { tx = 0; ty += 1; }                
            }

            if ((currentTilePx >= 0) && (currentTilePy >= 0)) {
                gr.DrawRectangle(Pens.Red, currentTilePx-1, currentTilePy-1, 33, 33);
            }

        }

        private void pbTiles_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) return;

            if (currentTilegroup == null) return;

            int tilesPerRow = (pnTilesContainer.Width / 32);
            if (tilesPerRow < 1) tilesPerRow = 1;

            pbTiles.Width = tilesPerRow * 32;
            pbTiles.Height = (int)Math.Ceiling((float)currentTilegroup.Tiles.Count / (float)tilesPerRow) * 32;

            int tx = 0;
            int ty = 0;
            foreach (int id in currentTilegroup.Tiles) {
                int px = tx * 32;
                int py = ty * 32;
                Image tile = tileRepository.getTile(id);

                if (tile == null) continue;

                if ((e.X >= px) && (e.X < px + 32) && (e.Y >= py) && (e.Y <= py + 32)) {
                    currentTileId = id;
                }

                tx += 1; if (tx >= tilesPerRow) { tx = 0; ty += 1; }
            }

            pbTiles.Invalidate();
        }

        private void newLevel() {
            level = new Level();
            currentLevelFilename = null;
            onLevelChanged();
        }

        private void loadLevel(string filename) {
            if (!File.Exists(filename)) throw new ArgumentException("Level file not found");

            level = new Level(filename);
            currentLevelFilename = filename;
            onLevelChanged();
        }

        private void saveLevel(string filename) {
            if (level == null) return;
            level.Write(filename);
        }

        private void onLevelChanged() {
        	sectorToolStripMenuItem.Enabled = true;
			propertiesToolStripMenuItem.Enabled = true;
			saveToolStripMenuItem.Enabled = true;
        	saveasToolStripMenuItem.Enabled = true;
            cbSector.Items.Clear();
            foreach (Sector sector in level.sectors) {
                cbSector.Items.Add(sector);
            }

            if (cbSector.Items.Count >= 1) cbSector.SelectedIndex = 0;
        }

        private void clTilemaps_ItemCheck(object sender, ItemCheckEventArgs e) {
            pbCanvas.Invalidate();
        }

        private void clTilemaps_SelectedIndexChanged(object sender, EventArgs e) {
            currentTilemap = (Tilemap)clTilemaps.SelectedItem;
            pbCanvas.Invalidate();
        }

        private void cbSector_SelectedIndexChanged(object sender, EventArgs e) {
            currentSector = (Sector)cbSector.SelectedItem;

            if (currentSector == null) return;

            clTilemaps.Items.Clear();

            int maxWidth = 1;
            int maxHeight = 1;

            foreach (Tilemap tilemap in currentSector.tilemaps) {
                clTilemaps.Items.Add(tilemap, true);
                maxWidth = Math.Max(maxWidth, tilemap.width);
                maxHeight = Math.Max(maxHeight, tilemap.height);
            }

            pbCanvas.Width = maxWidth * 32;
            pbCanvas.Height = maxHeight * 32;

            if (clTilemaps.Items.Count >= 2) clTilemaps.SelectedIndex = 1;

            pbCanvas.Invalidate();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e) {
            dgOpenLevel.InitialDirectory = "data\\levels\\";
            if (dgOpenLevel.ShowDialog() == DialogResult.OK) {
                loadLevel(dgOpenLevel.FileName);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            newLevel();
        }

        private void saveasToolStripMenuItem_Click(object sender, EventArgs e) {
            dgSaveLevel.InitialDirectory = "data\\levels\\";
            if (dgSaveLevel.ShowDialog() == DialogResult.OK) {
                saveLevel(dgSaveLevel.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentLevelFilename == null) {
                saveasToolStripMenuItem_Click(sender, e);
            } else {
                saveLevel(currentLevelFilename);
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            LevelPropertiesForm form = new LevelPropertiesForm(level);
            form.ShowDialog();
        }

        private void tcToolSettings_SelectedIndexChanged(object sender, EventArgs e) {
            anchorFloatingTilemap();
            currentSelection.Width = 0;
            pbCanvas.Invalidate();
            updateToolButtons();
        }

        private void btnDuplicateSelection_Click(object sender, EventArgs e) {
            anchorFloatingTilemap();
            createFloatingTilemapFromSelection(false);
        }

        private void resizeTilemapsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentSector == null) return;

            int currentSizeX = 0;
            int currentSizeY = 0;
            foreach (Tilemap tilemap in currentSector.tilemaps) {
                if (tilemap.width > currentSizeX) currentSizeX = tilemap.width;
                if (tilemap.height > currentSizeY) currentSizeY = tilemap.height;
            }

            SectorResizeDialog srd = new SectorResizeDialog(currentSizeX, currentSizeY);

            if (srd.ShowDialog() == DialogResult.OK) {
                foreach (Tilemap tilemap in currentSector.tilemaps) {
                    tilemap.OffsetBy(srd.OffsetX, srd.OffsetY);
                    tilemap.ResizeTo(srd.SectorWidth, srd.SectorHeight);
                }
                foreach (GameObject go in currentSector.gameObjects) {
                    if (!(go is SpatialGameObject)) continue;
                    SpatialGameObject sgo = (SpatialGameObject)go;
                    sgo.X += 32*srd.OffsetX;
                    sgo.Y += 32*srd.OffsetY;
                }
                cbSector_SelectedIndexChanged(sender, e);
            }
        }

        private void updateToolButtons() {
            btnModeTileAdd.Checked = (tcToolSettings.SelectedTab == tpTiles);
            btnModeTileMove.Checked = (tcToolSettings.SelectedTab == tpTileManip);
            btnModeObjAdd.Checked = (tcToolSettings.SelectedTab == tpObjects);
            btnModeObjMove.Checked = (tcToolSettings.SelectedTab == tpGameObjectManip);
            btnModeBrushes.Checked = (tcToolSettings.SelectedTab == tpBrushes);
        }

        private void btnModeTileAdd_Click(object sender, EventArgs e) {
            tcToolSettings.SelectedTab = tpTiles;
            updateToolButtons();
            slQuickHelp.Text = "LMB adds selected tile, Shift+LMB removes tiles";
        }

        private void btnModeTileMove_Click(object sender, EventArgs e) {
            tcToolSettings.SelectedTab = tpTileManip;
            updateToolButtons();
            slQuickHelp.Text = "LMB-drag to select and move tiles";
        }

        private void btnModeObjAdd_Click(object sender, EventArgs e) {
            tcToolSettings.SelectedTab = tpObjects;
            updateToolButtons();
            slQuickHelp.Text = "LMB adds selected object";
        }

        private void btnModeObjMove_Click(object sender, EventArgs e) {
            tcToolSettings.SelectedTab = tpGameObjectManip;
            updateToolButtons();
            slQuickHelp.Text = "LMB-drag to move objects, Shift+LMB to delete objects";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutDialog abd = new AboutDialog();
            abd.ShowDialog();
        }

        private void btnModeBrushes_Click(object sender, EventArgs e) {
            tcToolSettings.SelectedTab = tpBrushes;
            updateToolButtons();
            slQuickHelp.Text = "LMB to draw with loaded brush, Shift+LMB to delete tiles, Ctrl+LMB to learn pattern, Ctrl+Shift+LMB to forget pattern";
        }

        private void btnLoadBrush_Click(object sender, EventArgs e) {
            if (dgOpenBrush.ShowDialog() == DialogResult.OK) {
                currentBrush = Brush.loadFromFile(tileRepository, dgOpenBrush.FileName);
                laBrushSize.Text = currentBrush.Length + " Patterns";
            }
        }

        private void btnSaveBrush_Click(object sender, EventArgs e) {
            if (currentBrush == null) return;
            if (dgSaveBrush.ShowDialog() == DialogResult.OK) {
                currentBrush.saveToFile(dgSaveBrush.FileName);
                laBrushSize.Text = currentBrush.Length + " Patterns";
            }

        }

        private void btnNewBrush_Click(object sender, EventArgs e) {
            currentBrush = new Brush(tileRepository);
            laBrushSize.Text = currentBrush.Length + " Patterns";
        }

        private void btnBrushLearnTM_Click(object sender, EventArgs e) {
            if (currentBrush == null) return;
            if (currentTilemap == null) return;
            currentBrush.learn(currentTilemap);
            laBrushSize.Text = currentBrush.Length + " Patterns";
        }

        private void searchAndReplaceTilesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentSector == null) return;
            int oldId;
            try {
                oldId = int.Parse(slTileId.Text);
            } 
            catch(Exception) {
                oldId = 0;
            }
            int newId = currentTileId;

            TileSearchDialog tsd = new TileSearchDialog(oldId, newId);

            if (tsd.ShowDialog() == DialogResult.OK) {
                foreach (Tilemap tilemap in currentSector.tilemaps) {
                    tilemap.Replace(tsd.OldId, tsd.NewId);
                }
                cbSector_SelectedIndexChanged(sender, e);
            }
        }
    }
}
