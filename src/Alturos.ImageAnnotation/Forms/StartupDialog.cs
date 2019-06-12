﻿using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Helper;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class StartupDialog : Form
    {
        public IAnnotationPackageProvider AnnotationPackageProvider { get; private set; }
        public List<ObjectClass> ObjectClasses { get; private set; }

        private CheckBox[] _checkBoxes;

        public StartupDialog()
        {
            this.InitializeComponent();

            // Set package providers
            var packageProviders = InterfaceHelper.GetImplementations<IAnnotationPackageProvider>();
            this.comboBoxAnnotationPackageProvider.DataSource = packageProviders;

            // Create check boxes
            var objectClasses = ConfigurationManager.AppSettings["objectClasses"].Split(',');
            this._checkBoxes = new CheckBox[objectClasses.Length];

            var checkBoxBasePos = this.checkBoxTemplate.Location;
            var checkBoxSize = this.checkBoxTemplate.Size;

            this.checkBoxTemplate.Dispose();

            for (var i = 0; i < this._checkBoxes.Length; i++)
            {
                this._checkBoxes[i] = new CheckBox
                {
                    Location = new Point(checkBoxBasePos.X + (i % 3) * checkBoxSize.Width, checkBoxBasePos.Y + (i / 3) * checkBoxSize.Height),
                    Name = $"checkBoxObject{(i+1).ToString()}",
                    Size = checkBoxSize,
                    TabIndex = 4,
                    Text = objectClasses[i],
                    UseVisualStyleBackColor = true,
                    Checked = (i == 0)
                };
                this._checkBoxes[i].CheckedChanged += new EventHandler(this.checkBoxObject_CheckedChanged);
                this.groupBoxObjectClasses.Controls.Add(this._checkBoxes[i]);
            }
        }

        private void checkBoxObject_CheckedChanged(object sender, EventArgs e)
        {
            var checkedBoxCount = this.groupBoxObjectClasses.Controls.OfType<CheckBox>().Count(o => o.Checked);
            this.buttonConfirm.Enabled = checkedBoxCount > 0;
        }

        private void comboBoxAnnotationPackageProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var obj = ((NameValueObject)this.comboBoxAnnotationPackageProvider.SelectedItem).Value;
            this.AnnotationPackageProvider = (IAnnotationPackageProvider)Activator.CreateInstance((Type)obj);
        }

        private void buttonConfirm_Click(object sender, System.EventArgs e)
        {
            this.ObjectClasses = new List<ObjectClass>();

            var checkBoxCount = this.groupBoxObjectClasses.Controls.OfType<CheckBox>().Count();

            var fieldName = $"_checkBoxes";
            var fieldInfo = this.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            var checkBoxArray = fieldInfo.GetValue(this) as Array;

            for (var i = 0; i < checkBoxCount; i++)
            {
                var checkBox = checkBoxArray.GetValue(i) as CheckBox;

                if (checkBox != null && checkBox.Checked)
                {
                    this.ObjectClasses.Add(new ObjectClass
                    {
                        Id = i,
                        Name = checkBox.Text,
                    });
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
