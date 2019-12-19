using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClassManagement {
	public partial class FormView : Form {
		StepSchedulerEntities bd = null;
		Users user = null;

		public FormView() {
			InitializeComponent();
			toolStripButton4.Image = Image.FromFile(@"search.png");
			user = new Users();
		}

		public void InitUser() {
			bd = new StepSchedulerEntities();
			bd.Users.Load(); //подключаемся к базе к таб.User
			dataGridView.DataSource = null;
			dataGridView.DataSource = bd.Users.Local.ToBindingList();//выводим данные в dataGrid
			dataGridView.Columns["Requests"].Visible = false;//убираем колонки таблицы Requests
			dataGridView.Columns["UserId"].Visible = false;
			dataGridView.Columns["Photo"].Visible = false;
			dataGridView.Columns["Login"].Visible = false;
			dataGridView.Columns["Password"].Visible = false;
			dataGridView.Columns["IsAdmin"].Visible = false;
			dataGridView.Columns["IsBlocked"].Visible = false;
		}
		public void AddUser() {
			int index = dataGridView.SelectedRows[0].Index;
			int Id;
			bool converted = Int32.TryParse(dataGridView[0, index].Value.ToString(), out Id);
			if (converted == false)
				return;
			user = bd.Users.Find(Id);//находим по индексу значение
			TeacherInfoForm f = new TeacherInfoForm(user);//открываем доп. форму

			if (f.ShowDialog() == DialogResult.OK) {
				bd.SaveChanges();//сохраняем изменения
				dataGridView.Update();//обновляем отредактированые данные
				dataGridView.Refresh();
				MessageBox.Show("Данные отредактированные!");
			}
			else {
				MessageBox.Show("Не удалось отредактировать данные!");
			}
		}
		public void BlockUser() {
			if (dataGridView.SelectedRows.Count == 1) {
				int index = dataGridView.SelectedRows[0].Index;
				int Id;
				bool converted = Int32.TryParse(dataGridView[0, index].Value.ToString(), out Id);
				if (converted == false)
					return;
				user = bd.Users.Find(Id);
				user.IsBlocked = true;
				bd.SaveChanges();
				//выделяем пользователя красным цветом
				dataGridView.SelectedRows[0].DefaultCellStyle.BackColor = Color.Red;

				dataGridView.Update();
				dataGridView.Refresh();
				MessageBox.Show("Пользователь заблокирован!");
			}
			else {
				MessageBox.Show("Не удалось заблокировать данные!");
			}
		}
		public void DelUser() {
			if (dataGridView.SelectedRows.Count == 1) {
				int index = dataGridView.SelectedRows[0].Index;
				int Id;
				bool converted = Int32.TryParse(dataGridView[0, index].Value.ToString(), out Id);
				if (converted == false)
					return;
				user = bd.Users.Find(Id);
				bd.Users.Remove(user);
				bd.SaveChanges();
				dataGridView.Update();
				dataGridView.Refresh();
				MessageBox.Show("Пользователь удален!");
			}
			else {
				MessageBox.Show("Не удалось заблокировать удален!");
			}
		}

		private void toolStripButton4_Click(object sender, EventArgs e) {
			if (toolStripButton4.Text != " ") {
				using (bd = new StepSchedulerEntities()) {
					bd.Users.Where(x => x.Name.Contains(toolStripButton4.Text) || x.Surname.Contains(toolStripButton4.Text) || x.Login.Contains(toolStripButton4.Text)
					|| x.PhoneNumber.Contains(toolStripButton4.Text) || x.E_Mail.Contains(toolStripButton4.Text)).Load();
					dataGridView.DataSource = bd.Users.Local.ToBindingList();
					dataGridView.Columns["Requests"].Visible = false;
				}
			}
			else {
				MessageBox.Show("Введите текст!");
			}
		}

		private void toolStripButton5_Click(object sender, EventArgs e) {
			bd = new StepSchedulerEntities();
			bd.Users.Load(); //подключаемся к базе к таб.User
			dataGridView.DataSource = null;
			dataGridView.DataSource = bd.Users.Local.ToBindingList();//выводим данные в dataGrid
			dataGridView.Columns["Requests"].Visible = false;//убираем колонки таблицы Requests           
			dataGridView.Columns["Photo"].Visible = false;
		}

		private void checkBoxBlock_CheckedChanged(object sender, EventArgs e) {
			if (checkBoxBlock.Checked) {
				bd = new StepSchedulerEntities();
				bd.Users.Load();
				var query = bd.Users.Local.Where(x => x.IsBlocked == true).ToList();
				dataGridView.DataSource = null;
				dataGridView.DataSource = query;
				dataGridView.Columns["Requests"].Visible = false;//убираем колонки таблицы Requests
				dataGridView.Columns["UserId"].Visible = false;
				dataGridView.Columns["Photo"].Visible = false;
				dataGridView.Columns["BirthDate"].Visible = false;
			}
		}
	}
}
