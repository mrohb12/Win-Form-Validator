using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Validation_Contorls
{
    [ProvideProperty("ValidationType", typeof(Control))]
    public partial class FormValidator : Component, IExtenderProvider
    {
        private Hashtable properties;

        public enum ValidationTypes
        {
            RequiredFieldValidator,
            RangeValidator,
            CompareValidator,
            RegularExpressionValidator,
            CustomValidator
        }

        private class Properties
        {
            private ValidationTypes? vldtype;

            public ValidationTypes? ValidationTypee
            {
                get { return vldtype; }
                set { vldtype = value; }
            }

            public Properties()
            {
                vldtype = null;
            }
        }

        private Properties EnsurePropertiesExists(object key)
        {
            Properties p = (Properties) properties[key];

            if (p == null)
            {
                p = new Properties();

                properties[key] = p;
            }

            return p;
        }

        [DefaultValue("")]
        [Category("Design")]
        [Description("This is used by some code somewhere to do something")]
        public ValidationTypes? GetValidationType(Control ctrl)
        {
            return EnsurePropertiesExists(ctrl).ValidationTypee;
        }

        public void SetValidationType(Control ctrl, ValidationTypes? value)
        {
            EnsurePropertiesExists(ctrl).ValidationTypee = value;
         
            if (value != null)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox) ctrl).Validating += (sender, e) => {
                        TextValid(((TextBox) ctrl), e);
                    }
                    ;

                }
               
            }
        }

        private void TextValid(TextBox txt,CancelEventArgs e)
        {
            if (txt.Text == "")
            {
                errorProvider1.SetError(txt, txt.AccessibleName + " Is Empty");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txt,"");
                e.Cancel = false;
            }
        }

        private void PanelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen penBorder = new Pen(Color.Firebrick, 5);
            Rectangle rectBorder = new Rectangle(((TextBox) (sender)).Location, ((TextBox) (sender)).Size);
            g.DrawRectangle(penBorder, rectBorder);
        }


        private Control ctrltovalidate;
        private Control ctrltocompare;

        private Control ControlToCompare
        {
            get { return ctrltocompare; }
            set { ctrltocompare = value; }
        }


        public Control ControlToValidate
        {
            get { return ctrltovalidate; }
            set { ctrltovalidate = value; }
        }


        public FormValidator()
        {
            InitializeComponent();

            properties = new Hashtable();
        }

        public FormValidator(IContainer container) : this()
        {
            InitializeComponent();

            container.Add(this);
        }

        public bool CanExtend(object o)
        {
            if (o is TextBoxBase || o is ButtonBase || o is ListBox || o is ComboBox || o is DateTimePicker ||
                o is MonthCalendar)
                return true;
            else
                return false;
        }
    }
}