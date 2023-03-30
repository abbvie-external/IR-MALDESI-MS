using System.ComponentModel;
using System.Windows.Forms;

namespace IR_MALDESI.Browser.Forms
{
    partial class BatchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchForm));
            this.pathLabel = new System.Windows.Forms.Label();
            this.setPath = new System.Windows.Forms.Button();
            this.load = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.restore = new System.Windows.Forms.Button();
            this.batchExport = new System.Windows.Forms.Button();
            this.list = new System.Windows.Forms.ListBox();
            this.openFolder = new System.Windows.Forms.Button();
            this.batchCombine = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathLabel.Location = new System.Drawing.Point(42, 9);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(42, 20);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "Path";
            // 
            // setPath
            // 
            this.setPath.Location = new System.Drawing.Point(12, 6);
            this.setPath.Name = "setPath";
            this.setPath.Size = new System.Drawing.Size(32, 23);
            this.setPath.TabIndex = 1;
            this.setPath.Text = "...";
            this.setPath.UseVisualStyleBackColor = true;
            this.setPath.Click += new System.EventHandler(this.setPath_Click);
            // 
            // load
            // 
            this.load.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.load.Location = new System.Drawing.Point(12, 165);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(125, 32);
            this.load.TabIndex = 2;
            this.load.Text = "Load File";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // remove
            // 
            this.remove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.remove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remove.Location = new System.Drawing.Point(143, 165);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(125, 32);
            this.remove.TabIndex = 3;
            this.remove.Text = "Remove File";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // restore
            // 
            this.restore.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.restore.Location = new System.Drawing.Point(274, 165);
            this.restore.Name = "restore";
            this.restore.Size = new System.Drawing.Size(125, 32);
            this.restore.TabIndex = 4;
            this.restore.Text = "Restore List";
            this.restore.UseVisualStyleBackColor = true;
            this.restore.Click += new System.EventHandler(this.restore_Click);
            // 
            // batchExport
            // 
            this.batchExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batchExport.Location = new System.Drawing.Point(575, 165);
            this.batchExport.Name = "batchExport";
            this.batchExport.Size = new System.Drawing.Size(96, 32);
            this.batchExport.TabIndex = 5;
            this.batchExport.Text = "Export";
            this.batchExport.UseVisualStyleBackColor = true;
            this.batchExport.Click += new System.EventHandler(this.batchExport_Click);
            // 
            // list
            // 
            this.list.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.list.FormattingEnabled = true;
            this.list.HorizontalScrollbar = true;
            this.list.ItemHeight = 20;
            this.list.Location = new System.Drawing.Point(12, 35);
            this.list.Name = "list";
            this.list.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.list.Size = new System.Drawing.Size(660, 124);
            this.list.TabIndex = 6;
            // 
            // openFolder
            // 
            this.openFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openFolder.Location = new System.Drawing.Point(554, 2);
            this.openFolder.Name = "openFolder";
            this.openFolder.Size = new System.Drawing.Size(118, 30);
            this.openFolder.TabIndex = 7;
            this.openFolder.Text = "Open folder";
            this.openFolder.UseVisualStyleBackColor = true;
            this.openFolder.Click += new System.EventHandler(this.openFolder_Click);
            // 
            // batchCombine
            // 
            this.batchCombine.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batchCombine.Location = new System.Drawing.Point(472, 165);
            this.batchCombine.Name = "batchCombine";
            this.batchCombine.Size = new System.Drawing.Size(96, 32);
            this.batchCombine.TabIndex = 8;
            this.batchCombine.Text = "Combine";
            this.batchCombine.UseVisualStyleBackColor = true;
            this.batchCombine.Click += new System.EventHandler(this.batchCombine_Click);
            // 
            // BatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(684, 204);
            this.Controls.Add(this.batchCombine);
            this.Controls.Add(this.openFolder);
            this.Controls.Add(this.list);
            this.Controls.Add(this.batchExport);
            this.Controls.Add(this.restore);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.load);
            this.Controls.Add(this.setPath);
            this.Controls.Add(this.pathLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BatchForm";
            this.Text = "batchForm";
            this.Load += new System.EventHandler(this.batchForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label pathLabel;
        private Button setPath;
        private Button load;
        private Button remove;
        private Button restore;
        private Button batchExport;
        private ListBox list;
        private Button openFolder;
        private Button batchCombine;
    }
}