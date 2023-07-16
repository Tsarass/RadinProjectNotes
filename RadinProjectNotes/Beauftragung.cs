using ProtoBuf.Meta;
using RadinProjectNotes.ProjectServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public partial class Beauftragung : Form
    {
        public Beauftragung()
        {
            InitializeComponent();
        }

        private void Beauftragung_Load(object sender, EventArgs e)
        {
            InitializeServiceCheckboxes();
        }

        private void InitializeServiceCheckboxes()
        {
            Font headingFont = new Font("Microsoft Sans Serif", 9.5f, FontStyle.Bold);
            Font serviceFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);

            RadinProjectServices services = ProjectServicesController.TryLoadProjectServices();
            panel.ColumnCount = services.getCategoriesCount();
            panel.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 25f);
            for (int i = 1; i < panel.ColumnCount; i++)
            {
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            }

            int columnId = 0;
            int maxRows = 0;
            foreach (var categoryTitle in services.getCategoryTitles())
            {
                int rowsInCategory = services.getServicesCount(categoryTitle);
                if (rowsInCategory > maxRows)
                {
                    maxRows = rowsInCategory;
                }

                // Add label as a heading to each.
                panel.Controls.Add(new Label() { Font = headingFont, Text = categoryTitle }, columnId, 0);

                // Start from the second row (first is the heading)
                int rowId = 1;
                foreach (var service in services.getServicesForCategory(categoryTitle))
                {
                    panel.Controls.Add(new CheckBox { Font = serviceFont, Text = service, AutoSize = true }, columnId, rowId);

                    rowId++;
                }

                columnId++;
            }

            // Add an empty label in an extra row to align everything above.
            // panel.Controls.Add(new Label(), 0, maxRows + 1);
        }
    }
}
