using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.AdminForms
{
    public partial class WorkStationView : Form
    {
        private string workstationID;
        private DatabaseHelper dbHelper;

        public WorkStationView(string workstationId)
        {
            InitializeComponent();
            CustomizeForm();
            this.workstationID = workstationId;
            dbHelper = new DatabaseHelper();
        }

        private void WorkStationView_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(workstationID))
            {
                LoadWorkstationDetails();
            }

            // Ensure okButton is focused when the form loads
            okButton.Focus();
            // Set okButton as the default button when the Enter key is pressed
            this.AcceptButton = okButton;
        }

        private void CustomizeForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent; 
        }

        private void LoadWorkstationDetails()
        {
            string query = @"
        SELECT workstations.workstation_id, 
               workstations.system_unit AS systemUnitId, 
               system_units.operating_system AS operatingSystem, 
               system_units.hostname AS hostname, 
               workstations.room, 
               workstations.added_by AS addedBy, 
               workstations.date_added AS dateAdded, 
               workstations.date_updated AS dateUpdated, 
               workstations.updated_by AS updatedBy, 
               CONCAT(monitors.brand, ' | ', IFNULL(monitors.description, 'N/A')) AS monitor, 
               CONCAT(keyboards.brand, ' | ', IFNULL(keyboards.description, 'N/A')) AS keyboard, 
               CONCAT(mouse.brand, ' | ', IFNULL(mouse.description, 'N/A')) AS mouse, 
               CONCAT(avr.brand, ' | ', IFNULL(avr.description, 'N/A')) AS avr, 
               CONCAT(motherboards.brand, ' | ', IFNULL(motherboards.description, 'N/A')) AS motherboard, 
               CONCAT(cpu.brand, ' | ', IFNULL(cpu.description, 'N/A')) AS cpu, 
               CONCAT(ram.brand, ' | ', IFNULL(ram.description, 'N/A')) AS ram, 
               CONCAT(gpu.brand, ' | ', IFNULL(gpu.description, 'N/A')) AS gpu, 
               CONCAT(psu.brand, ' | ', IFNULL(psu.description, 'N/A')) AS psu, 
               CONCAT(system_unit_case.brand, ' | ', IFNULL(system_unit_case.description, 'N/A')) AS systemCase
        FROM workstations
        LEFT JOIN monitors ON workstations.monitor = monitors.monitor_id
        LEFT JOIN keyboards ON workstations.keyboard = keyboards.keyboard_id
        LEFT JOIN mouse ON workstations.mouse = mouse.mouse_id
        LEFT JOIN avr ON workstations.avr = avr.avr_id
        LEFT JOIN system_units ON workstations.system_unit = system_units.system_unit_id
        LEFT JOIN motherboards ON system_units.motherboard = motherboards.motherboard_id
        LEFT JOIN cpu ON system_units.cpu = cpu.cpu_id
        LEFT JOIN ram ON system_units.ram = ram.ram_id
        LEFT JOIN gpu ON system_units.gpu = gpu.gpu_id
        LEFT JOIN psu ON system_units.psu = psu.psu_id
        LEFT JOIN system_unit_case ON system_units.system_unit_case = system_unit_case.system_unit_case_id
        WHERE workstations.workstation_id = @WorkstationId";

            try
            {
                var parameters = new[] { new MySqlParameter("@WorkstationId", workstationID) };
                DataTable dt = dbHelper.GetData(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    // Assign values to labels with "N/A" if the data is null, empty, or an empty string
                    label1.Text = !string.IsNullOrEmpty(row["workstation_id"].ToString()) ? row["workstation_id"].ToString() : "N/A";
                    systemUnitId.Text = !string.IsNullOrEmpty(row["systemUnitId"].ToString()) ? row["systemUnitId"].ToString() : "N/A";
                    operatingSystem.Text = !string.IsNullOrEmpty(row["operatingSystem"].ToString()) ? row["operatingSystem"].ToString() : "N/A";
                    hostname.Text = !string.IsNullOrEmpty(row["hostname"].ToString()) ? row["hostname"].ToString() : "N/A";
                    room.Text = !string.IsNullOrEmpty(row["room"].ToString()) ? row["room"].ToString() : "N/A";                  
                    dateAdded.Text = row["dateAdded"] != DBNull.Value ? Convert.ToDateTime(row["dateAdded"]).ToString("yyyy-MM-dd") : "N/A";
                    addedBy.Text = !string.IsNullOrEmpty(row["addedBy"].ToString()) ? row["addedBy"].ToString() : "N/A";
                    dateUpdated.Text = row["dateUpdated"] != DBNull.Value ? Convert.ToDateTime(row["dateUpdated"]).ToString("yyyy-MM-dd") : "N/A";
                    updatedBy.Text = !string.IsNullOrEmpty(row["updatedBy"].ToString()) ? row["updatedBy"].ToString() : "N/A";

                    // Components section (labels are in the componentsPage)
                    workstation.Text = !string.IsNullOrEmpty(row["workstation_id"].ToString()) ? row["workstation_id"].ToString() : "N/A";
                    monitor.Text = !string.IsNullOrEmpty(row["monitor"].ToString()) ? row["monitor"].ToString() : "N/A";
                    keyboard.Text = !string.IsNullOrEmpty(row["keyboard"].ToString()) ? row["keyboard"].ToString() : "N/A";
                    mouse.Text = !string.IsNullOrEmpty(row["mouse"].ToString()) ? row["mouse"].ToString() : "N/A";
                    avr.Text = !string.IsNullOrEmpty(row["avr"].ToString()) ? row["avr"].ToString() : "N/A";
                    motherboard.Text = !string.IsNullOrEmpty(row["motherboard"].ToString()) ? row["motherboard"].ToString() : "N/A";
                    cpu.Text = !string.IsNullOrEmpty(row["cpu"].ToString()) ? row["cpu"].ToString() : "N/A";
                    ram.Text = !string.IsNullOrEmpty(row["ram"].ToString()) ? row["ram"].ToString() : "N/A";
                    gpu.Text = !string.IsNullOrEmpty(row["gpu"].ToString()) ? row["gpu"].ToString() : "N/A";
                    psu.Text = !string.IsNullOrEmpty(row["psu"].ToString()) ? row["psu"].ToString() : "N/A";
                    sucase.Text = !string.IsNullOrEmpty(row["systemCase"].ToString()) ? row["systemCase"].ToString() : "N/A";
                }
                else
                {
                    MessageBox.Show("No details found for the selected workstation.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        // Event handler for okButton click - closes the form
        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Event handler for cancelButton click - closes the form
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
