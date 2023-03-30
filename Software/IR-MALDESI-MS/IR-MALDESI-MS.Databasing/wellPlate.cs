using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IR_MALDESI.Databasing
{
    public class wellPlate
    {
        // Lock object
        private static object lockObject = new object();

        // Clicking
        public bool _mousePressed;
        public bool _ctrlPressed;
        public int firstRowClicked;
        public int firstColumnClicked;

        public List<wellPlateStatus> _wellPlateStatus = new List<wellPlateStatus>();

        public class wellPlateStatus
        {
            public int row { get; set; }
            public int column { get; set; }
            public bool clicked { get; set; }
        }

        public void SetWellColor(int row, int column, Color color, DataGridView dgv)
        {
            if (row < dgv.Rows.Count && column < dgv.Rows[0].Cells.Count) dgv.Rows[row].Cells[column].Style.BackColor = color;
        }

        public void InitializeDataGrid(DataGridView dgv, int numRows, int numCols)
        {
            // Initialize data grid
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            _mousePressed = false;
            _ctrlPressed = false;

            // Cell size
            var cellSize = 32;
            var headerHeight = 32;

            // Add Columns
            for (var i = 0; i < numCols; i++)
            {
                dgv.Columns.Add($"column{i}", "");
                switch (numCols)
                {
                    case 24://384
                        cellSize = 16;
                        headerHeight = 16;
                        break;

                    case 48:
                        cellSize = 8;
                        headerHeight = 16;
                        break;

                    default:// 96
                        cellSize = 31;
                        headerHeight = 22;

                        break;
                }
                dgv.Columns[i].Width = cellSize;

                dgv.Columns[i].HeaderCell.Value = (i + 1).ToString(); ;
                dgv.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgv.ColumnHeadersHeight = headerHeight;

            // Add Rows
            for (var i = 0; i < numRows; i++)
            {
                string[] row = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                dgv.Rows.Add(row);
                dgv.Rows[i].Height = cellSize;
                dgv.Rows[i].HeaderCell.Value = ((char)(i + 65)).ToString();
                dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            // Add status tracking
            _wellPlateStatus = new List<wellPlateStatus>();
            for (var i = 0; i < numRows; i++)
            {
                for (var j = 0; j < numCols; j++)
                {
                    _wellPlateStatus.Add(new wellPlateStatus { row = i, column = j, clicked = false });
                }
            }
        }

        public void setWellDrawingInitialState(List<wellPlateStatus> wells, DataGridView dgv)
        {
            foreach (var w in _wellPlateStatus)
            {
                // New wellplate drawing
                var temp = wells.Where(x => x.row == w.row && x.column == w.column).First();

                if (temp.clicked && !w.clicked)// If new is clicked, but old isn't, then make clicked
                {
                    SetWellColor(temp.row, temp.column, Color.Red, dgv);
                    w.clicked = true;
                }
                else if (!temp.clicked && w.clicked)// If new isn't clicked, but old is, then make not clicked
                {
                    SetWellColor(temp.row, temp.column, Color.White, dgv);
                    w.clicked = false;
                }
                else if (temp.clicked && w.clicked)// If both are clicked, just make sure it's red
                {
                    SetWellColor(temp.row, temp.column, Color.Red, dgv);
                }
            }
        }

        public void MousePressed(int row, int column)
        {
            _mousePressed = true;
            firstRowClicked = row;
            firstColumnClicked = column;
        }

        public void MouseReleased(int row, int column, DataGridView dgv, bool CTRLpressed)
        {
            _mousePressed = false;

            if (row == -1)
            {
                if (!CTRLpressed) foreach (var w in _wellPlateStatus) w.clicked = false;
                var welllist = _wellPlateStatus.Where(x => x.column == column).ToList();
                foreach (var w in welllist)
                {
                    w.clicked = !w.clicked;

                    //    if (w.clicked) SetWellColor(w.row, w.column, Color.Red, dgv);
                    //     else SetWellColor(w.row, w.column, Color.White, dgv);
                }
                RecolorDataGridView(dgv);
            }
            else if (column == -1)
            {
                if (!CTRLpressed) foreach (var w in _wellPlateStatus) w.clicked = false;
                var welllist = _wellPlateStatus.Where(x => x.row == row).ToList();
                foreach (var w in welllist)
                {
                    w.clicked = !w.clicked;

                    //   if (w.clicked) SetWellColor(w.row, w.column, Color.Red, dgv);
                    //  else SetWellColor(w.row, w.column, Color.White, dgv);
                }
                RecolorDataGridView(dgv);
            }
            else if (row == firstRowClicked && column == firstColumnClicked)    // Click single well

            {
                if (!CTRLpressed) foreach (var w in _wellPlateStatus) w.clicked = false;

                var well = _wellPlateStatus.Where(x => x.row == row && x.column == column).ToList().FirstOrDefault();
                well.clicked = !well.clicked;
                RecolorDataGridView(dgv);

                //  if (well.clicked) SetWellColor(row, column, Color.Red, dgv);
                //   else SetWellColor(row, column, Color.White, dgv);
            }
        }

        private void RecolorDataGridView(DataGridView dgv)
        {
            foreach (var w in _wellPlateStatus)
            {
                if (w.clicked) SetWellColor(w.row, w.column, Color.Red, dgv);
                else SetWellColor(w.row, w.column, Color.White, dgv);
            }
        }

        public void MovingHighlight(int row, int column, DataGridView dgv, bool CTRLpressed)
        {
            if (_mousePressed)
            {
                // Don't do anything if row of column clicked
                if (firstRowClicked == -1 || firstColumnClicked == -1) return;

                var t = Task.Run(() =>
            {
                lock (lockObject)
                {
                    int rows = _wellPlateStatus.Select(x => x.row).Max() + 1;
                    int columns = _wellPlateStatus.Select(x => x.column).Max() + 1;

                    // populate transfer plate
                    var wells = new List<wellPlateStatus>();
                    for (var i = 0; i < rows; i++)
                        for (var j = 0; j < columns; j++)
                            wells.Add(new wellPlateStatus { row = i, column = j, clicked = false });

                    // Find the well enclosed by drag
                    for (int i = firstRowClicked; i <= row; i++)
                        for (int j = firstColumnClicked; j <= column; j++)
                            wells.Where(x => x.row == i && x.column == j).First().clicked = true;

                    // If ctrl pressed - transfer priors as well
                    if (CTRLpressed)
                    {
                        for (var i = 0; i < wells.Count; i++)
                        {
                            wells[i].clicked = wells[i].clicked || _wellPlateStatus[i].clicked;
                        }
                    }

                    // Look through and see if they need to change color
                    setWellDrawingInitialState(wells, dgv);
                }
            });
            }
        }
    }
}