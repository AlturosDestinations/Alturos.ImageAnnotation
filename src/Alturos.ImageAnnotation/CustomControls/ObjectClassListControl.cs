﻿using Alturos.ImageAnnotation.Model;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class ObjectClassListControl : UserControl
    {
        public ObjectClassListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }

        public ObjectClass[] GetAll()
        {
            var items = new List<ObjectClass>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var item = row.DataBoundItem as ObjectClass;
                items.Add(item);
            }

            return items.ToArray();
        }

        public ObjectClass[] GetSelected()
        {
            var items = new List<ObjectClass>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var item = row.DataBoundItem as ObjectClass;
                if (item.Selected)
                {
                    items.Add(item);
                }
            }

            return items.ToArray();
        }

        public void SetObjectClasses(List<ObjectClass> objectClasses)
        {
            this.dataGridView1.DataSource = objectClasses;
        }
    }
}
