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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cobble {
    public partial class SectorResizeDialog : Form {
        public SectorResizeDialog(int currentWidth, int currentHeight) {
            InitializeComponent();
            udOffsetX.Value = 0;
            udOffsetY.Value = 0;
            udWidth.Value = currentWidth;
            udHeight.Value = currentHeight;
        }

        public int OffsetX {
            get {
                return (int)udOffsetX.Value;
            }
        }

        public int OffsetY {
            get {
                return (int)udOffsetY.Value;
            }
        }

        public int SectorWidth {
            get {
                return (int)udWidth.Value;
            }
        }

        public int SectorHeight {
            get {
                return (int)udHeight.Value;
            }
        }
    }
}
