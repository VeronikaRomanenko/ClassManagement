using ClassManagement.Admin;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClassManagement {
	public partial class MainFormAdmin : Form {
		/*	 1 : принят запрос
		 *	 0 : на рассмотрении
		 *	-1 : запрос отклонен	*/

		Color busy = Color.Salmon;      //занятые аудитории помечены красным цветом
		Color free = Color.Green;       //свободные аудитории помечены зеленым цветом
		Color pretend = Color.Yellow;   //аудитории, на которые претендуют преподователи, помечены желтым цветом

		public MainFormAdmin() {
			InitializeComponent();
			dataGridView1.RowCount = 15;
			for (int i = 0; i < dataGridView1.RowCount - 1; i++) {
				dataGridView1[0, i].Value = $"{i + 1} ауд.";
			}
			dataGridView1[0, dataGridView1.RowCount - 1].Value = "Конф. зал";
			for (int i = 1; i < dataGridView1.ColumnCount; i++) {
				for (int j = 0; j < dataGridView1.RowCount; j++) {
					dataGridView1[i, j].Style.BackColor = free;
				}
			}
			RefreshDataGrid();
			pictureBox1.Image = Image.FromFile(@"gmail_false.png");
		}

		private void RefreshDataGrid() { //Обновляет содержимое dataGridView 
			if (dataGridView1.InvokeRequired) {
				BeginInvoke(new Action(RefreshDataGrid));
			}
			else {
				using (StepSchedulerEntities db = new StepSchedulerEntities()) {
					IQueryable<ReservedRooms> Querry = from t1 in db.ReservedRooms //достаем зарезервированные аудитории где дата проведения равна дате в dateTimePicker1 
													   join t2 in db.Requests on t1.RequestId equals t2.RequestId
													   where (t2.Status != -1)
													   select t1;
					foreach (ReservedRooms item in Querry) {

						string ShortFormatItem = item.Requests.ClassDate.ToShortDateString();
						string SelectedShortFormat = dateTimePicker1.Value.ToShortDateString();
						if (ShortFormatItem == SelectedShortFormat) {
							if (item.Requests.Status == 1) {
								dataGridView1[item.Requests.LessonNumber, item.Requests.ClassRooms.Number - 1].Style.BackColor = busy;
								if (item.EventType == 0) {
									dataGridView1[item.Requests.LessonNumber, item.Requests.ClassRooms.Number - 1].Value = "З";
								}
								if (item.EventType == 1) {
									dataGridView1[item.Requests.LessonNumber, item.Requests.ClassRooms.Number - 1].Value = "К";
								}
								if (item.EventType == 2) {
									dataGridView1[item.Requests.LessonNumber, item.Requests.ClassRooms.Number - 1].Value = "М";
								}
							}
							else {
								dataGridView1[item.Requests.LessonNumber, item.Requests.ClassRooms.Number - 1].Style.BackColor = pretend;
								if (item.EventType == 0) {
									dataGridView1[item.Requests.LessonNumber, item.Requests.ClassRooms.Number - 1].Value = "З";
								}
								if (item.EventType == 1) {
									dataGridView1[item.Requests.LessonNumber, item.Requests.ClassRooms.Number - 1].Value = "К";
								}
								if (item.EventType == 2) {
									dataGridView1[item.Requests.LessonNumber, item.Requests.ClassRooms.Number].Value = "М";
								}
							}
						}
					}
				}
			}
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {

		}

		private void просмотрToolStripMenuItem_Click(object sender, EventArgs e) {
			FormView form = new FormView();
			form.Show();
		}

		private void добавлениеToolStripMenuItem_Click(object sender, EventArgs e) {
			LessonForm form = new LessonForm();
			form.Show();
		}

		private void просмотрToolStripMenuItem1_Click(object sender, EventArgs e) {
			FormView form = new FormView();
			form.Show();
		}

		private void добавлениеToolStripMenuItem1_Click(object sender, EventArgs e) {
			//TeacherInfoForm form = new TeacherInfoForm();
			//form.Show();
		}

		private void просмотрToolStripMenuItem2_Click(object sender, EventArgs e) {
			FormView form = new FormView();
			form.Show();
		}

		private void добавлениеToolStripMenuItem2_Click(object sender, EventArgs e) {
			AudienceForm form = new AudienceForm();
			form.Show();
		}

		private void pictureBox1_Click(object sender, EventArgs e) {
			ConfirmationForm form = new ConfirmationForm();
			form.Show();
		}
	}
}
