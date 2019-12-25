using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClassManagement {
	public partial class BookingForm : Form {
        StepSchedulerEntities db;

		public BookingForm(DateTime date, int timeIndex, List<string> audits, int auditIndex) {
			InitializeComponent();
            db = new StepSchedulerEntities();
            cmbTime.Items.Add("9:00");          //заполнение данных на форме
            cmbTime.Items.Add("10:30");
            cmbTime.Items.Add("12:00");
            cmbTime.Items.Add("13:30");
            cmbTime.Items.Add("15:00");
            cmbTime.Items.Add("16:30");
            cmbTime.Items.Add("18:00");
            cmbTime.Items.Add("19:30");
            cmbTime.SelectedIndex = timeIndex - 1;
            cmbAudit.Items.AddRange(audits.ToArray());
            cmbAudit.SelectedIndex = auditIndex;
            cmbTip.Items.Add("Консультация");
            cmbTip.Items.Add("Мероприятие");
            cmbTip.SelectedIndex = 0;
            dateTimePicker1.Value = date;
		}

		private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value < DateTime.Now)           //проверки
            {
                MessageBox.Show("Нельзя бронировать на прошедшее время");
                return;
            }
            if (txbTeacher.Text == "")
            {
                MessageBox.Show("Поле \"Преподаватель\" не может быть пустым");
                return;
            }
            Requests request = new Requests();                  //создание нового запроса на бронирование соответственно данным с формы
            request.ClassRoomId = cmbAudit.SelectedIndex;
            var teacher = db.Users.Where(x => x.Login == txbTeacher.Text);
            if (teacher.Count(x => true) != 1)
            {
                MessageBox.Show("Некорректный логин");
                return;
            }
            request.UserId = teacher.Single().UserId;
            request.ClassDate = dateTimePicker1.Value;
            request.LessonNumber = cmbTime.SelectedIndex;
            if (numericUpDown1.Value < 1)
            {
                MessageBox.Show("");
                return;
            }
            request.CountOfVisitors = (int)numericUpDown1.Value;
            request.EventDescription = txbKomment.Text;
            db.Requests.Add(request);           //сохранение запроса в базе
            db.SaveChanges();
            MessageBox.Show("Заявка на бронирование успешно отправлена");
			Close();
		}
	}
}
