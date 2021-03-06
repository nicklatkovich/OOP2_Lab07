﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;

namespace OOP2.Lab07 {
    public partial class MainForm : Form {
        public MainForm( ) {
            InitializeComponent( );
            Load += MainForm_Load;
            ZooPresenter.Columns.Add("Animal", "Животное");
            ZooPresenter.Columns[0].Visible = false;
            ZooPresenter.Columns.Add("Name", "Название");
            ZooPresenter.Columns.Add("Age", "Возраст");
            ZooPresenter.Columns.Add("RedBook", "Красная книга");
            ZooPresenter.Columns.Add("Type", "Тип");
            ZooPresenter.Columns.Add("Class", "Класс");
            ZooPresenter.Columns.Add("Detachment", "Отряд");
            ZooPresenter.Columns.Add("HabitatArea", "Ареал");
        }

        public List<Animal> Animals = new List<Animal>( );

        private EditAnimalForm AddAnimalForm;

        private void MainForm_Load(Object sender, EventArgs e) {
        }

        private void ButtonAddAnimal_Click(Object sender, EventArgs e) {
            if (AddAnimalForm == null || AddAnimalForm.IsDisposed) {
                AddAnimalForm = new EditAnimalForm(null);
            }
            AddAnimalForm.Show( );
            AddAnimalForm.Focus( );
            AddAnimalForm.LabelClass.Text = AddAnimalForm.LabelType.Text = AddAnimalForm.LabelDetachment.Text = "";
        }

        private void groupBox2_Enter(Object sender, EventArgs e) {

        }

        private void ButtonConfirmAnimalAddition_Click(Object sender, EventArgs e) {

        }

        private void BtnRefresh_Click(Object sender, EventArgs e) {
            RefreshZooPresenter( );
        }

        public void RefreshZooPresenter( ) {
            ZooPresenter.Rows.Clear( );
            foreach (var a in Animals) {
                ZooPresenter.Rows.Add(new object[ ] {
                    a,
                    a.Name,
                    a.Age,
                    a.IsInRedBook,
                    a.Type,
                    a.Class,
                    a.Detachment,
                    a.GetCommonHabitatsArea( ),
                });
            }
            ZooPresenter.ClearSelection( );
        }

        private void BtnSave_Click(Object sender, EventArgs e) {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Animal[ ]));
            SaveFileDialog sfd = new SaveFileDialog( );
            sfd.Filter = "Zoo Data Base (*zdb)|*.zdb";
            if (sfd.ShowDialog( ) == DialogResult.OK) {
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.OpenOrCreate)) {
                    jsonFormatter.WriteObject(fs, Animals.ToArray( ));
                }
            }
        }

        private void BtnLoad_Click(Object sender, EventArgs e) {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Animal[ ]));
            OpenFileDialog ofd = new OpenFileDialog( );
            ofd.Filter = "Zoo Data Base (*zdb)|*.zdb";
            if (ofd.ShowDialog( ) == DialogResult.OK) {
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate)) {
                    Animals = new List<Animal>((Animal[ ])jsonFormatter.ReadObject(fs));
                }
            }
            RefreshZooPresenter( );
        }

        private void ZooPresenter_MouseDoubleClick(Object sender, MouseEventArgs e) {
        }

        private void ZooPresenter_Sorted(Object sender, EventArgs e) {
            ZooPresenter.ClearSelection( );
        }

        private void ZooPresenter_SelectionChanged(Object sender, EventArgs e) {
            BtnEdit.Visible = ZooPresenter.SelectedRows.Count == 1;
        }

        private void BtnEdit_Click(Object sender, EventArgs e) {
            Animal animal = ZooPresenter.SelectedRows[0].Cells[0].Value as Animal;
            if (AddAnimalForm == null || AddAnimalForm.IsDisposed) {
                AddAnimalForm = new EditAnimalForm(animal);
            }
            ZooPresenter.Enabled = false;
            AddAnimalForm.Show( );
            AddAnimalForm.Focus( );
            //AddAnimalForm.LabelClass.Text = AddAnimalForm.LabelType.Text = AddAnimalForm.LabelDetachment.Text = "";
        }
    }
}
